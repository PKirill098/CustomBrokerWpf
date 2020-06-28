using System.Data.SqlClient;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Specification
{
    internal class RequestItemNote : lib.DomainBaseClass
    {
        private RequestItemNote(int id, string notecode, string note, lib.DomainObjectState state):base(id, state)
        {
            mynotecode = notecode;
            mynote = note;
        }
        internal RequestItemNote(int id, int requestitemid, string notecode, string note, lib.DomainObjectState state):this(id, notecode, note, state)
        {
            myrequestitemid = requestitemid;
        }
        internal RequestItemNote(Domain.RequestItem requestitem, string notecode, string note) : this(lib.NewObjectId.NewId, notecode, note, lib.DomainObjectState.Added)
        { myrequestitem = requestitem; }

        private int myrequestitemid;
        internal int RequestItemId {get{return myrequestitemid; } }
        private Domain.RequestItem myrequestitem;
        public Domain.RequestItem RequestItem
        {
            set
            {
                if (!object.Equals(myrequestitem, value))
                {
                    myrequestitem = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                }
            }
            get
            {
                return myrequestitem;
            }
        }
        private string mynotecode;
        public string NoteCode
        {
            set
            {
                if (!string.Equals(mynotecode, value))
                {
                    mynotecode = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                }
            }
            get { return mynotecode; }
        }
        private string mynote;
        public string Note
        {
            set
            {
                if (!string.Equals(mynote, value))
                {
                    mynote = value;
                    if (base.mydomainstate == lib.DomainObjectState.Unchanged) base.mydomainstate = lib.DomainObjectState.Modified;
                }
            }
            get { return mynote; }
        }
    }

    internal class RequestItemNoteDBM : lib.DBManager<RequestItemNote>
    {
        internal RequestItemNoteDBM(int requestitemid):base()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;
            SelectProcedure = false;
            SelectCommandText = "SELECT id,requestitemid,notecode,note FROM dbo.RequestItemNote_tb WHERE requestitemid=@requestitemid";
            SelectParams = new SqlParameter[] { new SqlParameter("@requestitemid", System.Data.SqlDbType.Int) };
            SelectParams[0].Value = requestitemid;

            SqlParameter paridout = new SqlParameter("@id", System.Data.SqlDbType.Int); paridout.Direction = System.Data.ParameterDirection.Output;
            SqlParameter parid = new SqlParameter("@id", System.Data.SqlDbType.Int);

            myinsertparams = new SqlParameter[] { paridout, new SqlParameter("@requestitemid", System.Data.SqlDbType.Int) };
            myupdateparams = new SqlParameter[] { parid };
            myinsertupdateparams = new SqlParameter[]
            {
                new SqlParameter("@notecode", System.Data.SqlDbType.Char,5),
                new SqlParameter("@note", System.Data.SqlDbType.NVarChar,100),
            };
            mydeleteparams = new SqlParameter[] { parid };

            InsertProcedure = false;
            myinsertcommandtext = "INSERT INTO dbo.RequestItemNote_tb (requestitemid,notecode,note) VALUES(@requestitemid,@notecode,@note) SET @id=SCOPE_IDENTITY();";
            UpdateProcedure = false;
            myupdatecommandtext = "UPDATE dbo.RequestItemNote_tb SET notecode=@notecode,note=@note WHERE id=@id";
            DeleteProcedure = false;
            mydeletecommandtext = "DELETE FROM dbo.RequestItemNote_tb WHERE id=@id";

        }
        internal RequestItemNoteDBM() : this(0) { }

        internal int RequestItemId
        {
            get
            {
                return (int)SelectParams[0].Value;
            }
            set
            {
                SelectParams[0].Value = value;
            }
        }

        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
        }
        protected override RequestItemNote CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new RequestItemNote(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3),lib.DomainObjectState.Unchanged);
        }
        protected override void GetOutputParametersValue(RequestItemNote item)
        {
            if (item.DomainState == lib.DomainObjectState.Added)
                item.Id = (int)myinsertparams[0].Value;
        }
        protected override void ItemAcceptChanches(RequestItemNote item)
        {
            item.AcceptChanches();
        }
        protected override bool SaveChildObjects(RequestItemNote item)
        {
           return true;
        }
        protected override bool SaveIncludedObject(RequestItemNote item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetParametersValue(RequestItemNote item)
        {
            myinsertparams[1].Value = item.RequestItemId==0? item.RequestItem.Id: item.RequestItemId;
            myupdateparams[0].Value = item.Id;
            myinsertupdateparams[0].Value = item.NoteCode;
            myinsertupdateparams[1].Value = item.Note;
            return true;
        }
        protected override bool LoadObjects()
        { return true; }
    }
}
