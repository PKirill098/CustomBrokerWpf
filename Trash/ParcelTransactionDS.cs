using System.Data;
namespace KirillPolyanskiy.CustomBrokerWpf.Windows.Parcel
{


    public partial class ParcelTransactionDS
    {
        partial class tableReturnDataTable
        {
            internal void SetStatus()
            {
                foreach (ParcelTransactionDS.tableReturnRow row in this.Rows)
                {
                    if (row.RowState != DataRowState.Unchanged)
                    {
                        if (row.RowState == DataRowState.Deleted)
                        {
                            row.RejectChanges();
                            row.status = row.tableParcelTransactionRow.tableParcelRow.parcelstatus;
                            row.EndEdit();
                            row.Delete();
                        }
                        else
                        {
                            row.status = row.tableParcelTransactionRow.tableParcelRow.parcelstatus;
                            row.EndEdit();
                        }
                    }
                }
            }

        }

        partial class tableOtherDataTable
        {
            internal void SetStatus()
            {
                foreach (ParcelTransactionDS.tableOtherRow row in this.Rows)
                {
                    if (row.RowState != DataRowState.Unchanged)
                    {
                        if (row.RowState == DataRowState.Deleted)
                        {
                            row.RejectChanges();
                            row.status = row.tableParcelTransactionRow.tableParcelRow.parcelstatus;
                            row.EndEdit();
                            row.Delete();
                        }
                        else
                        {
                            row.status = row.tableParcelTransactionRow.tableParcelRow.parcelstatus;
                            row.EndEdit();
                        }
                    }
                }
            }
        }

        partial class tableParcelTransactionDataTable
        {
            internal void SetStatus()
            {
                foreach (ParcelTransactionDS.tableParcelTransactionRow row in this.Rows)
                {
                    if (row.RowState == DataRowState.Modified)
                    {
                        row.status = row.tableParcelRow.parcelstatus;
                        row.EndEdit();
                    }
                }
            }
        }
    }
}
