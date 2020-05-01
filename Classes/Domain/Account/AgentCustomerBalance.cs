using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Account
{
    public class AgentCustomerBalance
    {
        internal AgentCustomerBalance() : base() { }
        internal AgentCustomerBalance(int agentid, int customerid, decimal balance,Importer importer):this()
        {
            myagentid = agentid;
            mycustomerid = customerid;
            mybalance = balance;
            myimporter = importer;
        }
        internal int myagentid;
        internal int mycustomerid;

        private Agent myagent;
        public Agent Agent
        { set { myagent = value; } get { return myagent; } }
        private decimal mybalance;
        public decimal Balance
        { set { mybalance = value; } get { return mybalance; } }
        private CustomerLegal mycustomer;
        public CustomerLegal Customer
        { set { mycustomer = value; } get { return mycustomer; } }
        private Importer myimporter;
        public Importer Importer
        { set { myimporter = value; } get { return myimporter; } }
    }

    internal class AgentCustomerBalanceDBM : lib.DBMSFill<AgentCustomerBalance>
    {
        internal AgentCustomerBalanceDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;
            SelectProcedure = true;
            SelectCommandText = "account.AgentCustomerBalanceDelivered_sp";
            SelectParams = new SqlParameter[] {
                new SqlParameter("@agentid", System.Data.SqlDbType.Int),
                new SqlParameter("@customerid", System.Data.SqlDbType.Int),
                new SqlParameter("@minbalance", System.Data.SqlDbType.Money),
                new SqlParameter("@importerid", System.Data.SqlDbType.Int)};
        }

        private Agent myagent;
        internal Agent Agent
        { set { myagent = value; } get { return myagent; } }
        private CustomerLegal mycustomer;
        internal CustomerLegal Customer
        { set { mycustomer = value; } get { return mycustomer; } }
        private Importer myimporter;
        public Importer Importer
        { set { myimporter = value; } get { return myimporter; } }
        private decimal? myminbalance;
        internal decimal? MinBalance
        { set { myminbalance = value; } get { return myminbalance; } }

        protected override AgentCustomerBalance CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new AgentCustomerBalance(
                reader.GetInt32(reader.GetOrdinal("agentid")),
                reader.GetInt32(reader.GetOrdinal("customerid")),
                reader.GetDecimal(reader.GetOrdinal("balance")),
                myimporter
                );
        }

        protected override bool LoadObjects()
        {
            foreach (AgentCustomerBalance item in this.Collection)
                LoadObjects(item);
            return this.Errors.Count == 0;
        }
        protected override void LoadObjects(AgentCustomerBalance item)
        {
            item.Agent = CustomBrokerWpf.References.AgentStore.GetItemLoad(item.myagentid, this.Command.Connection);
            item.Customer = CustomBrokerWpf.References.CustomerLegalStore.GetItemLoad(item.mycustomerid, this.Command.Connection);
        }

        protected override void SetParametersValue()
        {
            foreach(SqlParameter par in SelectParams)
                switch (par.ParameterName)
                {
                    case "@agentid":
                        par.Value = myagent?.Id;
                        break;
                    case "@customerid":
                        par.Value = mycustomer?.Id;
                        break;
                    case "@minbalance":
                        par.Value = myminbalance;
                        break;
                    case "@importerid":
                        par.Value = myimporter?.Id;
                        break;
                }
        }
    }
}
