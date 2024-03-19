using System.Data.SqlClient;
using System.Text;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes
{
    internal class MailSMS
    {
        internal int? ClientId { set; get; }
        internal int? LegalId { set; get; }
        internal string Client { set; get; }
        internal string Legal { set; get; }
        internal string What { set; get; }
        internal string Value { set; get; }
    }

    internal class MailSMSDBM : lib.DBMSTake<MailSMS,MailSMS>
    {
        internal MailSMSDBM(int parcel, MailSMSCommand cmd) :base()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;
            SelectProcedure = true;
            SelectCommandText = "dbo.ParcelMailSMS_sp";
            SelectParams = new SqlParameter[] { new SqlParameter("@param1", parcel) };
            mysmcmd = cmd;
        }

        MailSMSCommand mysmcmd;

        protected override void PrepareFill()
        {
        }
		protected override MailSMS CreateRecord(SqlDataReader reader)
		{
            MailSMS newitem = new MailSMS();
            newitem.ClientId = reader.IsDBNull(0) ? (int?)null : reader.GetInt32(0);
            newitem.LegalId = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1);
            newitem.Client = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
            newitem.Legal = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
            //newitem.What = reader.GetString(0);
            newitem.Value = reader.IsDBNull(4)?string.Empty:reader.GetString(4);
            return newitem;
		}
        protected override MailSMS CreateModel(MailSMS reader,SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
        {
			return reader;
        }
		protected override void LoadRecord(SqlDataReader reader, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
		{
			this.TakeItem(CreateModel(this.CreateRecord(reader), addcon, canceltasktoken));
		}
		protected override bool GetModels(System.Threading.CancellationToken canceltasktoken=default,System.Func<bool> reading=null)
		{
			return true;
		}

        private int clientid, legalid,c,m,t;
        protected override void TakeItem(MailSMS item)
        {
            if(!(clientid==(item.ClientId??0) & legalid==(item.LegalId??0)))
            {
                while(t-m>0 | m<c) // небыло контактов
                {
                    mysmcmd.Mail.AppendLine(); m++;
                }
                while (m - t > 0)
                {
                    mysmcmd.SMS.AppendLine(); t++;
                }
                while (m - 1 > 0)
                {
                    mysmcmd.Client.AppendLine();
                    mysmcmd.Legal.AppendLine();
                    m--;
                }
                m = 0; t = 0;c = 1;
                if (clientid != item.ClientId) mysmcmd.Client.AppendLine(item.Client); else mysmcmd.Client.AppendLine();
                clientid = item.ClientId??0; legalid = item.LegalId ?? 0;
                mysmcmd.Legal.AppendLine(item.Legal);
            }
            if (!string.IsNullOrEmpty(item.Value))
            {
                if (item.Value.IndexOf('@') > 0)
                { mysmcmd.Mail.AppendLine(item.Value); m++; }
                else
                { mysmcmd.SMS.AppendLine(item.Value); t++; }
            }
        }
    }

    public class MailSMSCommand : lib.DomainBaseClass
    {
        internal MailSMSCommand(int parcel) : base(0, lib.DomainObjectState.Unchanged)
        {
            Client = new StringBuilder();
            SMS = new StringBuilder();
            Legal = new StringBuilder();
            Mail = new StringBuilder();
            MailSMSDBM dbm = new MailSMSDBM(parcel, this);
            dbm.Load();
        }

        public StringBuilder Client { set; get; }
        public StringBuilder Legal { set; get; }
        public StringBuilder SMS { set; get; }
        public StringBuilder Mail { set; get; }
    }
}
