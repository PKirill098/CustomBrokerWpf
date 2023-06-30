using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
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

    internal class AgentCustomerBalanceRecord
    {
		internal int myagent;
		internal decimal mybalance;
		internal int mycustomer;
	}

	internal class AgentCustomerBalanceDBM : lib.DBMSFill<AgentCustomerBalanceRecord,AgentCustomerBalance>
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

        protected override AgentCustomerBalanceRecord CreateRecord(SqlDataReader reader)
        {
            return new AgentCustomerBalanceRecord()
            {
                myagent = reader.GetInt32(reader.GetOrdinal("agentid")),
                mycustomer = reader.GetInt32(reader.GetOrdinal("customerid")),
                mybalance = reader.GetDecimal(reader.GetOrdinal("balance")),
            };
        }
		protected override AgentCustomerBalance CreateModel(AgentCustomerBalanceRecord record, SqlConnection addcon, CancellationToken mycanceltasktoken = default)
		{
			return new AgentCustomerBalance(
				CustomBrokerWpf.References.AgentStore.GetItemLoad(record.myagent, addcon, out _),
				CustomBrokerWpf.References.CustomerLegalStore.GetItemLoad(record.mycustomer, addcon, out _),
				record.mybalance,
				myimporter
				);
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
