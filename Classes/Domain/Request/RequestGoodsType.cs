using KirillPolyanskiy.DataModelClassLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class RequestCargoDBM:lib.DBManager<lib.ReferenceSimpleItem,lib.ReferenceSimpleItem>
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
        protected override ReferenceSimpleItem CreateRecord(SqlDataReader reader)
        {
            lib.ReferenceSimpleItem reference = CustomBrokerWpf.References.GoodsTypesParcel.FindFirstItem(nameof(lib.ReferenceSimpleItem.Id),reader.GetInt32(0));
            lib.ReferenceSimpleItem cargo = new lib.ReferenceSimpleItem()
            {
                Id = reference.Id,
                Name = reference.Name,
                IsActual = reference.IsActual,
                IsDefault = reference.IsDefault,
                DomainState = lib.DomainObjectState.Unchanged
            };
            return cargo;
        }
        protected override ReferenceSimpleItem CreateModel(ReferenceSimpleItem record, SqlConnection addcon, CancellationToken canceltasktoken = default)
        {
            return record;
        }
        protected override bool SetParametersValue(ReferenceSimpleItem item)
        {
            myinsertparams[0].Value= this.Request.Id;
            myinsertparams[1].Value= item.Id;
            mydeleteparams[0].Value= this.Request.Id;
            mydeleteparams[1].Value= item.Id;
            return true;
        }
        protected override void GetOutputParametersValue(ReferenceSimpleItem item)
        {
        }
    }
}
