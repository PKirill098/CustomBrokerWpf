namespace KirillPolyanskiy.CustomBrokerWpf
{
    public partial class InvoiceDS
    {
        partial class tableInvoiceDetailDataTable
        {
            public override void EndInit()
            {
                base.EndInit();
                this.tableInvoiceDetailRowChanged += DetailRowChanging;
            }

            void DetailRowChanging(object sender, tableInvoiceDetailRowChangeEvent e)
            {
                if (e.Action == System.Data.DataRowAction.Change | e.Action == System.Data.DataRowAction.Add)
                {
                    if (!(e.Row.IsdetamountNull() | e.Row.IsdetpriceNull() | e.Row.IsdetsumNull()))
                        return;
                    else if (!e.Row.IsdetamountNull() & !e.Row.IsdetpriceNull() & e.Row.IsdetsumNull())
                        e.Row.detsum = e.Row.detamount * e.Row.detprice;
                    else if (!e.Row.IsdetamountNull() & e.Row.IsdetpriceNull() & !e.Row.IsdetsumNull())
                        e.Row.detprice = decimal.Divide(e.Row.detsum, (decimal)e.Row.detamount);
                    else if (e.Row.IsdetamountNull() & !e.Row.IsdetpriceNull() & !e.Row.IsdetsumNull())
                        e.Row.detamount = (short)decimal.Floor(e.Row.detsum / e.Row.detprice);
                }
            }
        }

        partial class tableInvoiceDataTable
        {
            public override void EndInit()
            {
                base.EndInit();
                this.TableNewRow += tableInvoiceDataTable_TableNewRow;
            }

            void tableInvoiceDataTable_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
            {
                (e.Row as InvoiceDS.tableInvoiceRow).invoicedate = System.DateTime.Today;
            }
        }
        partial class tableInvoiceRow
        {
            internal void Refresh()
            {
                CustomBrokerWpf.SQLFilter filter = new SQLFilter("invoice", "AND");
                filter.SetNumber(filter.FilterWhereId, "id", 0, this.invoiceid.ToString());
                InvoiceDSTableAdapters.InvoiceAdapter adapter = new InvoiceDSTableAdapters.InvoiceAdapter();
                adapter.ClearBeforeFill = false;
                adapter.Fill(this.tabletableInvoice, filter.FilterWhereId);
                filter.RemoveCurrentWhere();
            }
        }
    }
}
