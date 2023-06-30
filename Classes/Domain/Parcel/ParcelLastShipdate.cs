using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    internal class ParcelLastShipdate
    {
        internal ParcelLastShipdate()
        {
            mydbm = new ParcelLastShipdateDBM();
        }

        private ParcelLastShipdateDBM mydbm;
        internal DateTime Shipdate { private set; get; }

        internal void Update()
        {
            this.Shipdate = mydbm.GetFirst();
        }
    }

    internal class ParcelLastShipdateDBM : lib.DBMGetFirst<DateTime,DateTime>
    {
        internal ParcelLastShipdateDBM():base()
        {
            this.ConnectionString = CustomBrokerWpf.References.ConnectionString;
            this.SelectProcedure = false;
            this.SelectCommandText = "SELECT * FROM dbo.ParcelLast_vw";
        }

        protected override DateTime CreateRecord(SqlDataReader reader)
        {
           return reader.GetDateTime(0);
        }
        protected override DateTime CreateModel(DateTime record, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
        {
            return record;
        }
        protected override void PrepareFill(SqlConnection addcon)
        {
        }
    }
}
