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
            string body = temp.Body;
            if (body.IndexOf('{') > -1)
            {
                Request req = mydbm.DomainObject as Request;
                body = body.Replace("{Вес,Д}", (req.OfficialWeight ?? 0M).ToString("N0"));
                DateTime? arrdate = DateTime.Now.AddDays(temp.Delay ?? 0);
                if (arrdate.Value.DayOfWeek == DayOfWeek.Saturday)
                    arrdate = arrdate.Value.AddDays(-1);
                else if(arrdate.Value.DayOfWeek == DayOfWeek.Sunday)
                    arrdate = arrdate.Value.AddDays(-2);
                body = body.Replace("{Дата+}", arrdate.Value.ToString("dd.MM.yy"));
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
                body = body.Replace("{Дата статуса}", arrdate.HasValue ? arrdate.Value.ToString("dd.MM.yyyy") : string.Empty);
                body = body.Replace("{Импортер}", req?.Importer.Name??string.Empty);
                body = body.Replace("{Кол-во мест}", (req.CellNumber ?? 0).ToString("N0"));
                body = body.Replace("{Марка}", req.BrandNames??string.Empty);
                body = body.Replace("{Номер заявки}", req.Id.ToString());
                body = body.Replace("{Номер заявки}", req.Id.ToString());
                body = body.Replace("{Объем}", (req.Volume ?? 0).ToString("N3"));
                body = body.Replace("{Поставщик}", CustomBrokerWpf.References.AgentNames.FindFirstItem("Id", req.AgentId ?? 0)?.Name ?? string.Empty);
                body = body.Replace("{Поставщик(Вес, Д) кг.}", (CustomBrokerWpf.References.AgentNames.FindFirstItem("Id", req.AgentId ?? 0)?.Name ?? string.Empty)+ ", "+ (req.OfficialWeight ?? 0M).ToString("N0")+ " кг.");
                body = body.Replace("{Поставщик(Вес, Д) кг., Объем м3.}", (CustomBrokerWpf.References.AgentNames.FindFirstItem("Id", req.AgentId ?? 0)?.Name ?? string.Empty) + ", " + (req.OfficialWeight ?? 0M).ToString("N0") + " кг., " + (req.Volume ?? 0).ToString("N3") + " м3.");
                body = body.Replace("{Производитель}", string.Empty);
                body = body.Replace("{Страна}", req.Country?.Name ?? string.Empty);
                body = body.Replace("{Сумма}", req.CustomerLegals.FirstOrDefault((RequestCustomerLegal legal)=> { return legal.CustomerLegal.Id == item?.CustomerId; })?.InvoiceDiscount?.ToString("N2") + " " + req.CurrencyName ?? string.Empty);
                body = body.Replace("{Характеристика товара}", req.Cargo ?? string.Empty);
                body = body.Replace("{Юр лицо}", req.CustomerLegals.FirstOrDefault((RequestCustomerLegal legal) => { return legal.CustomerLegal.Id == item.CustomerId; })?.CustomerLegal.Name ?? string.Empty);
            }
            return body;
        }
    }
}
