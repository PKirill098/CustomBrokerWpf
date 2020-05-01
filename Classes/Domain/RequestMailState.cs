using System;
using lib = KirillPolyanskiy.DataModelClassLibrary;

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
    {
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
                body = body.Replace("{Поставщик(Вес, Д) кг.}", (CustomBrokerWpf.References.AgentNames.FindFirstItem("Id", req.AgentId ?? 0)?.Name ?? string.Empty)+ ", "+ (req.OfficialWeight ?? 0M).ToString("N0")+ " кг.");
                body = body.Replace("{Поставщик(Вес, Д) кг., Объем м3.}", (CustomBrokerWpf.References.AgentNames.FindFirstItem("Id", req.AgentId ?? 0)?.Name ?? string.Empty) + ", " + (req.OfficialWeight ?? 0M).ToString("N0") + " кг., " + (req.Volume ?? 0).ToString("N3") + " м3.");
                body = body.Replace("{Поставщик}", CustomBrokerWpf.References.AgentNames.FindFirstItem("Id", req.AgentId ?? 0)?.Name ?? string.Empty);
                body = body.Replace("{Вес,Д}", (req.OfficialWeight ?? 0M).ToString("N0"));
                body = body.Replace("{Кол-во мест}", (req.CellNumber ?? 0).ToString("N0"));
                body = body.Replace("{Объем}", (req.Volume ?? 0).ToString("N3"));
                DateTime arrdate = DateTime.Now.AddDays(temp.Delay ?? 0);
                if (arrdate.DayOfWeek == DayOfWeek.Saturday)
                    arrdate = arrdate.AddDays(-1);
                else if(arrdate.DayOfWeek == DayOfWeek.Sunday)
                    arrdate = arrdate.AddDays(-2);
                body = body.Replace("{Дата+}", arrdate.ToString("dd.MM.yy"));
            }
            return body;
        }
    }
}
