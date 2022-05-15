using System;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using System.Linq;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    internal class RequestMailStateCustomerDBM : MailStateCustomerDBM
    {
        internal RequestMailStateCustomerDBM():base()
        {
            SelectCommandText = "dbo.RequestMailState_sp";
        }
    }

    internal class RequestMailCustomerDBM: MailCustomerDBM
    { //Список email юр лиц заявки
        internal RequestMailCustomerDBM():base()
        {
            SelectCommandText = "dbo.RequestMail_sp";
        }
    }

    internal class RequestMailState: MailState
    {
        internal RequestMailState(lib.DomainBaseClass model, int mailstateid):base(model,new RequestMailStateCustomerDBM(),new RequestMailCustomerDBM(), mailstateid) { }

        internal override string CreateBody(MailTemplate temp, MailStateCustomer item)
        {
            return FillWildcards( temp, item, temp.Body);
        }
        internal override string CreateSubject(MailTemplate temp, MailStateCustomer item)
        {
            return FillWildcards(temp, item, temp.Subject);
        }
        private string FillWildcards(MailTemplate temp, MailStateCustomer item, string template)
        {
            if (template.IndexOf('{') > -1)
            {
                Request req = mydbm.DomainObject as Request;
                template = template.Replace("{Вес,Д}", (req.OfficialWeight ?? 0M).ToString("N0"));
                DateTime? arrdate = DateTime.Now.AddDays(temp.Delay ?? 0);
                if (arrdate.Value.DayOfWeek == DayOfWeek.Saturday)
                    arrdate = arrdate.Value.AddDays(-1);
                else if (arrdate.Value.DayOfWeek == DayOfWeek.Sunday)
                    arrdate = arrdate.Value.AddDays(-2);
                template = template.Replace("{Дата+}", arrdate.Value.ToString("dd.MM.yy"));
                arrdate = null;
                switch (req.Status.Id)
                {
                    case 1:
                        arrdate = req.RequestDate;
                        break;
                    case 30:
                        arrdate = req.StoreDate;
                        break;
                    case 50:
                        arrdate = req.Parcel?.ShipPlanDate;
                        break;
                    case 60:
                        arrdate = req.Parcel?.ShipDate;
                        break;
                    case 70:
                        arrdate = req.Parcel?.Prepared;
                        break;
                    case 80:
                        arrdate = req.Parcel?.CrossedBorder;
                        break;
                    case 90:
                        arrdate = req.Parcel?.TerminalIn;
                        break;
                    case 100:
                        arrdate = req.Parcel?.TerminalOut;
                        break;
                }
                template = template.Replace("{Дата статуса}", arrdate.HasValue ? arrdate.Value.ToString("dd.MM.yyyy") : string.Empty);
                template = template.Replace("{Импортер}", req?.Importer.Name ?? string.Empty);
                template = template.Replace("{Кол-во мест}", (req.CellNumber ?? 0).ToString("N0"));
                template = template.Replace("{Марка}", req.BrandNames ?? string.Empty);
                template = template.Replace("{Номер заявки}", req.Id.ToString());
                template = template.Replace("{Номер заявки}", req.Id.ToString());
                template = template.Replace("{Объем}", (req.Volume ?? 0).ToString("N3"));
                template = template.Replace("{Поставщик}", CustomBrokerWpf.References.AgentNames.FindFirstItem("Id", req.AgentId ?? 0)?.Name ?? string.Empty);
                template = template.Replace("{Поставщик(Вес, Д) кг.}", (CustomBrokerWpf.References.AgentNames.FindFirstItem("Id", req.AgentId ?? 0)?.Name ?? string.Empty) + ", " + (req.OfficialWeight ?? 0M).ToString("N0") + " кг.");
                template = template.Replace("{Поставщик(Вес, Д) кг., Объем м3.}", (CustomBrokerWpf.References.AgentNames.FindFirstItem("Id", req.AgentId ?? 0)?.Name ?? string.Empty) + ", " + (req.OfficialWeight ?? 0M).ToString("N0") + " кг., " + (req.Volume ?? 0).ToString("N3") + " м3.");
                template = template.Replace("{Производитель}", string.Empty);
                template = template.Replace("{Страна}", req.Country?.Name ?? string.Empty);
                template = template.Replace("{Сумма}", req.CustomerLegals.FirstOrDefault((RequestCustomerLegal legal) => { return legal.CustomerLegal.Id == item?.CustomerId; })?.InvoiceDiscount?.ToString("N2") + " " + req.CurrencyName ?? string.Empty);
                template = template.Replace("{Характеристика товара}", req.Cargo ?? string.Empty);
                template = template.Replace("{Юр лицо}", req.CustomerLegals.FirstOrDefault((RequestCustomerLegal legal) => { return legal.CustomerLegal.Id == item.CustomerId; })?.CustomerLegal.Name ?? string.Empty);
            }
            return template;
        }
    }
}
