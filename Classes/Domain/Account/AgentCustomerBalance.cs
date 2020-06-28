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
        internal AgentCustomerBalance(Agent agent, CustomerLegal customer, decimal balance,Importer importer):this()
        {
            myagent = agent;
            mycustomer = customer;
            mybalance = balance;
            myimporter = importer;
        }

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
            this.NeedAddConnection = true;
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
                CustomBrokerWpf.References.AgentStore.GetItemLoad(reader.GetInt32(reader.GetOrdinal("agentid")), addcon,out _),
                CustomBrokerWpf.References.CustomerLegalStore.GetItemLoad(reader.GetInt32(reader.GetOrdinal("customerid")), addcon, out _),
                reader.GetDecimal(reader.GetOrdinal("balance")),
                myimporter
                );
        }

        protected override bool LoadObjects()
        {
            return this.Errors.Count == 0;
        }

        protected override void PrepareFill(SqlConnection addcon)
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
