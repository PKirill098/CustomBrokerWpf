﻿using System;
using System.Data.SqlClient;
using System.Threading;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.References
{
    public class Color : lib.DomainBaseNotifyChanged
    {
        public Color(string id, string name) : base(0, lib.DomainObjectState.Unchanged)
        {
            mycode = id;
            myname = name;
        }

        private string mycode;
        public string Code
        {
            get { return mycode; }
        }
        private string myname;
        public string Name
        { get { return myname; } }
    }

    public class ColorDBM : lib.DBMSFill<Color,Color>
    {
        internal ColorDBM() : base()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;
            SelectProcedure = false;
            base.SelectCommandText = "SELECT id,name FROM dbo.Color_tb ORDER BY id";
        }

		protected override Color CreateRecord(SqlDataReader reader)
		{
            return new Color(reader.GetString(0), reader.GetString(1));
		}
		protected override Color CreateModel(Color record, SqlConnection addcon, CancellationToken canceltasktoken = default)
		{
			return record;
		}
		protected override void LoadRecord(SqlDataReader reader, SqlConnection addcon, CancellationToken canceltasktoken = default)
		{
			base.TakeItem(this.CreateRecord(reader));
		}
		protected override bool GetModels(System.Threading.CancellationToken canceltasktoken=default,Func<bool> reading=null)
		{
			return true;
		}
        protected override void PrepareFill()
        {
        }
    }

    public class ColorCollection : lib.ReferenceCollectionDomainBase<Color>
    {
        public ColorCollection() : base(new ColorDBM())
        {
            this.Fill();
            this.Insert(0, new Color("#FFFFFFFF", string.Empty));
        }

        public override Color FindFirstItem(string propertyName, object value)
        {
            Color firstitem = null;
            if (value is string)
                switch (propertyName)
                {
                    case "Id":
                        string id = (string)value;
                        foreach (Color item in this)
                            if (item.Code.ToUpper() == id.ToUpper())
                            { firstitem = item; break; }
                        break;
                    case "Name":
                        string name = ((string)value).ToUpper();
                        foreach (Color item in this)
                            if (item.Name.ToUpper().Equals(name))
                            { firstitem = item; break; }
                        break;
                    default:
                        throw new NotImplementedException("Свойство " + propertyName + " не реализовано");
                }
            return firstitem;
        }
        protected override int CompareReferences(Color item1, Color item2)
        {
            return item1.Code.CompareTo(item2.Code);
        }
        protected override void UpdateItem(Color olditem, Color newitem)
        { }
    }
}
