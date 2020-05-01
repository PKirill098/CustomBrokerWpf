namespace KirillPolyanskiy.CustomBrokerWpf
{
    public partial class FreightDS
    {
        partial class tableFreightGoodsDataTable
        {
            public override void EndInit()
            {
                base.EndInit();
                this.TableNewRow += new System.Data.DataTableNewRowEventHandler(tableFreightGoodsDataTable_TableNewRow);
            }
            private void tableFreightGoodsDataTable_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
            {
                if ((this.DataSet as FreightDS).tableFreight.Count > 0)
                    (e.Row as FreightDS.tableFreightGoodsRow).freightid = (this.DataSet as FreightDS).tableFreight[0].freightId;
                else
                    (e.Row as FreightDS.tableFreightGoodsRow).freightid = -1;
            }
        }

        partial class tableFreightDataTable
        {
            public override void EndInit()
            {
                base.EndInit();
                this.TableNewRow += new System.Data.DataTableNewRowEventHandler(Freight_tbDataTable_TableNewRow);
            }

            void Freight_tbDataTable_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
            {
                (e.Row as FreightDS.tableFreightRow).freightDate = System.DateTime.Today;
            }

        }
    }
}