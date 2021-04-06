using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;

// Не используется Удалить файл из проекта
namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Specification
{
    public class SpecificationCustomerInvoiceRate
    {
        public int? CustomerId { get; set; }
        public decimal? DTSum { set; get; }
        public bool Equally { set; get; }
        public decimal? Rate { set; get; }
        public decimal? Selling { set; get; }
    }

    public class SpecificationCustomerInvoiceRateDBM : lib.DBMSTake<SpecificationCustomerInvoiceRate>
    {
        internal SpecificationCustomerInvoiceRateDBM()
        {
            ConnectionString = CustomBrokerWpf.References.ConnectionString;
            SelectProcedure = true;
            SelectCommandText = "spec.SpecificationCustomerInvoiceRate_sp";
            SelectParams = new SqlParameter[] { new SqlParameter("@specid", System.Data.SqlDbType.Int) };
        }

        internal Specification Specification { set;get;}

        protected override SpecificationCustomerInvoiceRate CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new SpecificationCustomerInvoiceRate() { CustomerId=reader.IsDBNull(0)?(int?)null: reader.GetInt32(0),Rate= reader.IsDBNull(1) ? (decimal?)null : reader.GetDecimal(1),Equally = reader.IsDBNull(2) ? false : reader.GetBoolean(2) };
        }
        protected override void CancelLoad()
        {
        }
        protected override void PrepareFill(SqlConnection addcon)
        {
            SelectParams[0].Value = Specification.Id;
        }
        protected override void TakeItem(SpecificationCustomerInvoiceRate item)
        {
            //Specification.InvoiceDTRatesAdd(item);
        }
    }
}
