using KirillPolyanskiy.DataModelClassLibrary;
using System;
using System.Collections;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class Brand:lib.DomainBaseReject
    {
        public Brand (int id, lib.DomainObjectState state
            ,string name
            ) :base(id, state)
        {
            myname = name;
        }
        public Brand() : this(lib.NewObjectId.NewId, lib.DomainObjectState.Added,null) { }

        private string myname;
        public string Name
        { set { SetProperty<string>(ref myname, value); } get { return myname; } }

        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.Name):
                    myname = (string)this.GetPropertyOutdatedValue(nameof(this.Name));
                    break;
            }
        }
        protected override void PropertiesUpdate(DomainBaseReject sample)
        {
            Brand templ = (Brand)sample;
            this.Name = templ.Name;
        }
    }

    internal class BrandDBM : lib.DBMSTake<Brand,Brand>
    {
        internal BrandDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;
            base.SelectProcedure = false;
            SelectCommandText = "SELECT brandID,brandName FROM dbo.Brand_tb";
        }

        private IList mycollection;
        internal IList Collection
        { set { mycollection = value; } get { return mycollection; } }

        protected override Brand CreateRecord(SqlDataReader reader)
        {
            return new Brand(reader.GetInt32(0),lib.DomainObjectState.Unchanged,reader.GetString(1));
        }
		protected override Brand CreateModel(Brand record, SqlConnection addcon, CancellationToken mycanceltasktoken = default)
		{
			return record;
		}
		protected override void LoadRecord(SqlDataReader reader, SqlConnection addcon, CancellationToken mycanceltasktoken = default)
		{
			this.TakeItem(this.CreateRecord(reader));
		}
		protected override bool GetModels(System.Threading.CancellationToken canceltasktoken=default,Func<bool> reading=null)
		{
			return true;
		}
		protected override void PrepareFill(SqlConnection addcon)
        {
        }
        protected override void TakeItem(Brand item)
        {
            mycollection.Add(item);
        }
    }
}
