using System;
using System.Text;
namespace KirillPolyanskiy.CustomBrokerWpf
{


    public partial class ParcelDS
    {
        partial class tableParcelRequestDataTable
        {
            public override void EndInit()
            {
                base.EndInit();
                this.ColumnChanging += new System.Data.DataColumnChangeEventHandler(tableRequestDataTable_ColumnChanging);
            }
            void tableRequestDataTable_ColumnChanging(object sender, System.Data.DataColumnChangeEventArgs e)
            {
                if (e.Column.ColumnName == "parcelgroup")
                {
                    //System.Data.DataRow[] grow = this.Select("parcelgroup=" + e.ProposedValue + " AND customerId<>" + (e.Row as RequestDS.tableRequestRow).customerId);
                    //if (grow.Length > 0) throw new System.Data.ConstraintException("В группу(спецификацию) могут быть включены только заявки одного клиента.!");
                }
            }
        }

        partial class tableParcelDataTable
        {
            public override void EndInit()
            {
                base.EndInit();
                this.ColumnChanging += new System.Data.DataColumnChangeEventHandler(TelColumnChangeEvent);
            }

            private void TelColumnChangeEvent(object sender, System.Data.DataColumnChangeEventArgs e)
            {
                if (e.Column.ColumnName == "carriertel" | e.Column.ColumnName == "truckertel")
                {
                    e.ProposedValue = CustomBrokerWpf.TelephoneNumberFormat.ConvertValue(e.ProposedValue.ToString());
                }
            }
        }
        public partial class tableParcelRow : global::System.Data.DataRow
        {
            public string FullNumberCurrent
            { get { return this.parcelnumber + "-" + this.lorry + "-" + this.shipplandate.ToString("yy"); } }
        }
    }
}
