using System;
using System.Data.SqlClient;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class Reminder : lib.DomainBaseStamp
    {
        public Reminder(int id, Int64 stamp, DateTime? updated, string updater, lib.DomainObjectState domainstate
            ,string remtype, int objectid, string note, DateTime? delay, bool stop
            ) : base(id, stamp, updated, updater, domainstate)
        {
            myremtype = remtype;
            myobjectid = objectid;
            mynote = note;
            mydelay = delay;
            mystop = stop;
        }
        public Reminder() : this(lib.NewObjectId.NewId, 0, null, null, lib.DomainObjectState.Added, null, 0, null, null, false) { }
        private string myremtype;
        public string RemType { set { SetProperty<string>(ref myremtype, value); } get { return myremtype; } }
        private int myobjectid;
        public int ObjectId { set { SetProperty<int>(ref myobjectid, value); } get { return myobjectid; } }
        private string mynote;
        public string Note
        {
            set { SetProperty<string>(ref mynote, value); }
            get { return mynote; }
        }
        private DateTime? mydelay;
        public DateTime? Delay { set { SetProperty<DateTime?>(ref mydelay, value); } get { return mydelay; } }
        private bool mystop;
        public bool Stop { set { SetProperty<bool>(ref mystop, value); } get { return mystop; } }

        protected override void RejectProperty(string property, object value)
        {
            throw new NotImplementedException();
        }
        protected override void PropertiesUpdate(lib.DomainBaseUpdate sample)
        {
            throw new NotImplementedException();
        }
    }

    public class RemiderDBM : lib.DBManagerStamp<Reminder,Reminder>
    {
        public RemiderDBM():base()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;

            InsertProcedure = true;
            myinsertcommandtext = "dbo.ReminderADD_sp";
            UpdateProcedure = true;
            myupdatecommandtext = "dbo.ReminderUpd_sp";

            myinsertparams = new SqlParameter[]
            {
                myinsertparams[0],myinsertparams[1]
                , new SqlParameter("@remtype",System.Data.SqlDbType.Char,5)
                ,new SqlParameter("@objectid",System.Data.SqlDbType.Int)
            };
            myinsertupdateparams = new SqlParameter[]
            {
                new SqlParameter("@note",System.Data.SqlDbType.NVarChar,50)
                , new SqlParameter("@delay",System.Data.SqlDbType.DateTime2,0)
                ,new SqlParameter("@stop",System.Data.SqlDbType.Bit)
            };
        }

        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
        }
		protected override Reminder CreateRecord(SqlDataReader reader)
		{
            return new Reminder(reader.GetInt32(0), reader.GetInt64(1), null,null,lib.DomainObjectState.Unchanged,reader.GetString(4), reader.GetInt32(5), (reader.IsDBNull(6) ? null : reader.GetString(6)),(reader.IsDBNull(7) ? (DateTime?)null: reader.GetDateTime(7)),reader.GetBoolean(8));
		}
        protected override Reminder CreateModel(Reminder reader,SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
        {
			return reader;
        }
		protected override void LoadRecord(SqlDataReader reader, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
		{
			base.TakeItem(CreateModel(this.CreateRecord(reader), addcon, canceltasktoken));
		}
		protected override bool GetModels(System.Threading.CancellationToken canceltasktoken=default,Func<bool> reading=null)
		{
			return true;
		}
        protected override bool SaveChildObjects(Reminder item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(Reminder item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetParametersValue(Reminder item)
        {
            base.SetParametersValue(item);
            myupdateparams[1].Value = item.RemType;
            myupdateparams[2].Value = item.ObjectId;
            myinsertupdateparams[0].Value = item.Note;
            myinsertupdateparams[1].Value = item.Delay;
            myinsertupdateparams[2].Value = item.Stop;
            return true;
        }
    }
}
