using KirillPolyanskiy.DataModelClassLibrary;
using KirillPolyanskiy.DataModelClassLibrary.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class RequestCargo:lib.ReferenceContainer<lib.ReferenceSimpleItem>
    {
        public RequestCargo():base()
        {
        }

        public string Name { get { return this.InnerObject.Name; } }
    }
    public class RequestCargoDBM:lib.DBManager<RequestCargo,RequestCargo>
    {
        internal RequestCargoDBM()
        {
            this.NeedAddConnection = false;
            this.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            this.SelectProcedure = true;
            this.InsertProcedure = true;
            this.DeleteProcedure = true;

            SelectCommandText = "dbo.RequestGoodsType_sp";
            InsertCommandText = "dbo.RequestGoodsTypeAdd_sp";
            DeleteCommandText = "dbo.RequestGoodsTypeDel_sp";

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@request", System.Data.SqlDbType.Int)
            };
            myinsertparams = new SqlParameter[] {
                new SqlParameter("@request", System.Data.SqlDbType.Int)
                ,new SqlParameter("@goods", System.Data.SqlDbType.Int)
            };
            mydeleteparams = new SqlParameter[] {
                new SqlParameter("@request", System.Data.SqlDbType.Int)
                ,new SqlParameter("@goods", System.Data.SqlDbType.Int)
            };
        }

        internal Request Request { set; get; }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            this.SelectParams[0].Value = this.Request.Id;
        }
        protected override RequestCargo CreateRecord(SqlDataReader reader)
        {
            lib.ReferenceSimpleItem cargo = CustomBrokerWpf.References.GoodsTypesParcel.FindFirstItem(nameof(lib.ReferenceSimpleItem.Id),reader.GetInt32(0));
            //lib.ReferenceSimpleItem cargo = new lib.ReferenceSimpleItem()
            //{
            //    Id = reference.Id,
            //    Name = reference.Name,
            //    IsActual = reference.IsActual,
            //    IsDefault = reference.IsDefault,
            //    DomainState = lib.DomainObjectState.Unchanged
            //};

            return new RequestCargo() { InnerObject = cargo, DomainState = lib.DomainObjectState.Unchanged };
        }
        protected override RequestCargo CreateModel(RequestCargo record, SqlConnection addcon, CancellationToken canceltasktoken = default)
        {
            return record;
        }
        protected override bool SetParametersValue(RequestCargo item)
        {
            myinsertparams[0].Value= Request.Id;
            myinsertparams[1].Value= item.Id;
            mydeleteparams[0].Value= Request.Id;
            mydeleteparams[1].Value= item.Id;
            return true;
        }
        protected override void GetOutputParametersValue(RequestCargo item)
        {
        }
    }

    public class RequestCargoVM:ReferenceContainerVM<lib.ReferenceSimpleItem,lib.ReferenceSimpleItem>
    {
        public RequestCargoVM():base() { }
        public string Name { get{return this.InnerObjectViewModel.Name;}}

        protected override lib.ReferenceSimpleItem ConvertToVM(lib.ReferenceSimpleItem model)
        { return model; }
    }

    public class RequestCargoSincronaser : lib.ReferencesCollectionsSynchronizer<lib.ReferenceSimpleItem,lib.ReferenceSimpleItem,RequestCargo, RequestCargoVM>
    {
        protected override bool ReferenceCollectionFilter(lib.ReferenceSimpleItem model)
        {
            return model.IsActual;
        }
    }
}
