using System.Data;
namespace KirillPolyanskiy.CustomBrokerWpf
{


    public partial class RequestDS
    {
        partial class tableRequestDataTable
        {
            public override void EndInit()
            {
                base.EndInit();
                this.TableNewRow += new System.Data.DataTableNewRowEventHandler(Request_tbDataTable_TableNewRow);
                this.ColumnChanging += new System.Data.DataColumnChangeEventHandler(tableRequestDataTable_ColumnChanging);
            }

            void tableRequestDataTable_ColumnChanging(object sender, System.Data.DataColumnChangeEventArgs e)
            {
                if (e.Column.ColumnName == "parcelgroup" & e.ProposedValue != System.DBNull.Value)
                {
                    System.Data.DataRow[] grow = this.Select("parcelgroup=" + e.ProposedValue + " AND customerId<>" + (e.Row as RequestDS.tableRequestRow).customerId);
                    if (grow.Length > 0) throw new System.Data.ConstraintException("В группу(спецификацию) могут быть включены только заявки одного клиента.!");
                }
            }

            void Request_tbDataTable_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
            {
                (e.Row as RequestDS.tableRequestRow).requestDate = System.DateTime.Today;
            }

        }
    }
}
