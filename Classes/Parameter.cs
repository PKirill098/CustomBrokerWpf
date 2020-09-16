using System.Data.SqlClient;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes
{
    internal class Parameter : lib.DomainBaseClass
    {
        public Parameter(int id, string refid, string owner, string name, string value, lib.DomainObjectState state) : base(id, state)
        {
            myrefid = refid;
            myowner = owner;
            myname = name;
            myvalue = value;
        }

        private string myrefid;
        internal string RefId
        {
            set { SetProperty<string>(ref myrefid, value); }
            get { return myrefid; }
        }
        private string myowner;
        internal string Owner
        {
            set { SetProperty<string>(ref myowner, value); }
            get { return myowner; }
        }
        private string myname;
        internal string Name
        {
            set { SetProperty<string>(ref myname, value); }
            get { return myname; }
        }
        private string myvalue;
        internal string Value
        {
            set { SetProperty<string>(ref myvalue, value); }
            get { return myvalue; }
        }
    }

    internal class ParametrDBM : lib.DBManager<Parameter>
    {
        internal ParametrDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectProcedure = false;
            InsertProcedure = false;
            UpdateProcedure = false;
            DeleteProcedure = false;

            SelectCommandText = "SELECT id,refid,owner,refname,refvalue FROM [dbo].[Reference_tb] WHERE refid=@refid AND owner=SYSTEM_USER";
            InsertCommandText = "INSERT INTO [dbo].[Reference_tb] (refid,refname,refvalue) VALUES (@refid,@refname,@refvalue) SET @id=SCOPE_IDENTITY()";
            UpdateCommandText = "UPDATE [dbo].[Reference_tb] SET refvalue=@refvalue WHERE id=@id";

            this.SelectParams = new SqlParameter[] { new SqlParameter("@refid", System.Data.SqlDbType.Char, 5) };
            this.InsertParams = new SqlParameter[] { new SqlParameter("@id", System.Data.SqlDbType.Char, 5), new SqlParameter("@refid", System.Data.SqlDbType.Char, 5), new SqlParameter("@refname", System.Data.SqlDbType.NVarChar, 50) };
            this.UpdateParams = new SqlParameter[] { new SqlParameter("@id", System.Data.SqlDbType.Char, 5) };
            this.InsertUpdateParams = new SqlParameter[] { new SqlParameter("@refvalue", System.Data.SqlDbType.NVarChar, 50) };
            this.InsertParams[0].Direction = System.Data.ParameterDirection.Output;
        }

        internal string Id
        {
            set { this.SelectParams[0].Value = value; }
            get { return (string)this.SelectParams[0].Value; }
        }

        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
        }
        protected override Parameter CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new Parameter(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4),lib.DomainObjectState.Unchanged);
        }
        protected override void GetOutputParametersValue(Parameter item)
        {
        }
        protected override void ItemAcceptChanches(Parameter item)
        {
            item.AcceptChanches();
        }
        protected override bool SaveChildObjects(Parameter item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(Parameter item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetParametersValue(Parameter item)
        {
            this.InsertParams[1].Value = item.RefId;
            this.InsertParams[2].Value = item.Name;
            this.UpdateParams[0].Value = item.Id;
            this.InsertUpdateParams[0].Value = item.Value;
            return true;
        }
        protected override void CancelLoad()
        { }
    }
}
