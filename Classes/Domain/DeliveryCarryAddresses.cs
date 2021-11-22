using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class DeliveryCarryAddresses : lib.DomainBaseReject
    {
        internal DeliveryCarryAddresses(int id, lib.DomainObjectState domainstate
            , DeliveryCarry carry, string address) : base(id, domainstate)
        {
            mycarry = carry;
            myaddress = address;
        }

        private DeliveryCarry mycarry;
        public DeliveryCarry Carry
        { set { mycarry = value; } get { return mycarry; } }
        private string myaddress;
        public string Address
        { set { SetProperty<string>(ref myaddress, value); } get { return myaddress; } }

        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Address":
                    this.Address = (string)value;
                    break;
            }
        }
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            throw new NotImplementedException();
        }
    }

    internal class DeliveryCarryAddressesDBM : lib.DBManagerId<DeliveryCarryAddresses>
    {
        internal DeliveryCarryAddressesDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectProcedure = true;
            InsertProcedure = true;
            UpdateProcedure = true;
            DeleteProcedure = true;

            SelectCommandText = "delivery.DeliveryCarryAddresses_sp";
            InsertCommandText = "delivery.DeliveryCarryAddressesAdd_sp";
            UpdateCommandText = "delivery.DeliveryCarryAddressepds_sp";
            DeleteCommandText = "delivery.DeliveryCarryAddressesDel_sp";

            SqlParameter paridout = new SqlParameter("@id", System.Data.SqlDbType.Int); paridout.Direction = System.Data.ParameterDirection.Output;
            SqlParameter parid = new SqlParameter("@id", System.Data.SqlDbType.Int);

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@param1", System.Data.SqlDbType.Int),
                new SqlParameter("@param2", System.Data.SqlDbType.Int)
            };
            InsertParams = new SqlParameter[]
            {
                paridout
                ,new SqlParameter("@delivery", System.Data.SqlDbType.Int)
                ,new SqlParameter("@address", System.Data.SqlDbType.NVarChar,200)
            };
            DeleteParams = new SqlParameter[] { parid };
        }

        private DeliveryCarry mycarry;
        internal DeliveryCarry Carry { get { return mycarry; } set { mycarry = value; } }

        protected override DeliveryCarryAddresses CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new DeliveryCarryAddresses(reader.GetInt32(0),lib.DomainObjectState.Unchanged, mycarry,reader.GetString(2));
        }
        protected override void GetOutputParametersValue(DeliveryCarryAddresses item)
        {
            if(item.DomainState==lib.DomainObjectState.Added)
                item.Id = (int)myinsertparams[0].Value;
        }
        protected override void ItemAcceptChanches(DeliveryCarryAddresses item)
        {
            item.AcceptChanches();
        }
        protected override bool SaveChildObjects(DeliveryCarryAddresses item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(DeliveryCarryAddresses item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SetParametersValue(DeliveryCarryAddresses item)
        {
            myinsertparams[1].Value=this.Carry.Id;
            myinsertparams[2].Value = item.Address;
            mydeleteparams[0].Value = item.Id;
            return this.Carry.Id > 0;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            base.SelectParams[0].Value = this.Carry?.Id;
        }
        protected override void CancelLoad()
        { }
    }
}
