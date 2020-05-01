using System;
namespace KirillPolyanskiy.CustomBrokerWpf
{


    public partial class ParcelCostDS
    {
        partial class tableCostDataTable
        {
            public override void EndInit()
            {
                base.EndInit();
                this.TableNewRow += new System.Data.DataTableNewRowEventHandler(tableCostDataTable_TableNewRow);
            }

            void tableCostDataTable_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
            {
                (e.Row as ParcelCostDS.tableCostRow).datetran = DateTime.Today;
            }
        }
    }
}
