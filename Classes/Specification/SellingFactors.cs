using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Specification
{
    internal class SellingFactors : lib.DomainBaseClass
    {
        internal SellingFactors(CustomerLegal customer, Request request
            ,decimal? cbrate,decimal? dtrate, decimal? persent, string service) : base(customer.Id, lib.DomainObjectState.Sealed)
        {
            mycustomer = customer;
            myrequest = request;
            mycbrate = cbrate;
            mydtrate = dtrate;
            mypersent = persent;
            myservice = service;
        }

        private CustomerLegal mycustomer;
        internal CustomerLegal Customer
        { set { SetProperty(ref mycustomer, value); } get { return mycustomer; } }
        private Request myrequest;
        internal Request Request
        { set { SetProperty<Request>(ref myrequest, value); } get { return myrequest; } }
        private decimal? mycbrate;
        internal decimal? CBRate
        { set { SetProperty(ref mycbrate, value); } get { return mycbrate; } }
        private decimal? mydtrate;
        internal decimal? DTRate
        { set { SetProperty(ref mydtrate, value); } get { return mydtrate; } }
        private decimal? mypersent;
        internal decimal? Persent
        { set { SetProperty(ref mypersent, value); } get { return mypersent; } }
        private string myservice;
        internal string Service
        { set { SetProperty<string>(ref myservice, value); } get { return myservice; } }
    }

    internal class SellingFactorsDBM : lib.DBMSTake<SellingFactors>
    {
        internal SellingFactorsDBM()
        {
            this.ConnectionString= ConnectionString = CustomBrokerWpf.References.ConnectionString;
            this.NeedAddConnection = false;
            this.SelectCommandText = "spec.SellingFactors_sp";
            this.SelectParams = new SqlParameter[] { new SqlParameter("@specification", System.Data.SqlDbType.Int) };
            this.SelectProcedure = true;
            this.SellingFactors = new List<SellingFactors>();
        }

        internal Specification Specification { set; get; }
        internal List<SellingFactors> SellingFactors { set; get; }
        protected override void CancelLoad()
        {
        }
        protected override SellingFactors CreateItem(SqlDataReader reader, SqlConnection addcon)
        {
            return new SellingFactors(
                CustomBrokerWpf.References.CustomerLegalStore.GetItem(reader.GetInt32(this.Fields["customer"])),
                reader.IsDBNull(this.Fields["request"]) ? null : CustomBrokerWpf.References.RequestStore.GetItem(reader.GetInt32(this.Fields["request"])),
                reader.IsDBNull(this.Fields["cbrate"]) ? (decimal?)null : reader.GetDecimal(this.Fields["cbrate"]),
                reader.IsDBNull(this.Fields["dtrate"]) ? (decimal?)null : reader.GetDecimal(this.Fields["dtrate"]),
                reader.IsDBNull(this.Fields["persent"]) ? (decimal?)null : reader.GetDecimal(this.Fields["persent"]),
                reader.IsDBNull(this.Fields["service"]) ? null : reader.GetString(this.Fields["service"]));
        }
        protected override void PrepareFill(SqlConnection addcon)
        {
            this.SelectParams[0].Value=this.Specification.Id;
        }
        protected override void TakeItem(SellingFactors item)
        {
            this.SellingFactors.Add(item);
        }
    }
}
