using System.Data;
namespace KirillPolyanskiy.CustomBrokerWpf
{


    public partial class PaymentDS
    {
        partial class tableDCJoinDataTable
        {
            public override void EndInit()
            {
                base.EndInit();
                this.ColumnChanging += new DataColumnChangeEventHandler(tableTransactionDataTable_ColumnChanging);
            }
            void tableTransactionDataTable_ColumnChanging(object sender, DataColumnChangeEventArgs e)
            {
                if (e.Column.ColumnName == "joinsum")
                {
                    PaymentDS.tableDCJoinRow row = e.Row as PaymentDS.tableDCJoinRow;
                    decimal newvalue = (decimal)e.ProposedValue;
                    decimal freesum = row.freesum;
                    decimal transum = row.joinsum + row.tableTransactionRow.freesum;
                    if (newvalue < 0M) throw new System.Exception("Сумма разноски не может быть меньше ноля.");
                    if (decimal.Round(freesum, 2) == decimal.Round(newvalue, 2) & freesum != newvalue) newvalue = freesum;
                    if (decimal.Round(transum, 2) == decimal.Round(newvalue, 2) & transum != newvalue) newvalue = transum;
                    if (freesum < newvalue) newvalue = freesum;
                    if (transum < newvalue) newvalue = transum;
                    e.ProposedValue = newvalue;
                }
            }
        }

        partial class tablePaymentDataTable
        {
            public override void EndInit()
            {
                base.EndInit();
                this.ColumnChanging += new DataColumnChangeEventHandler(tableTransactionDataTable_ColumnChanging);
                //this.tablePaymentRowChanging += new tablePaymentRowChangeEventHandler(PaymentRowChanging);
                this.tablePaymentRowChanged += new tablePaymentRowChangeEventHandler(PaymentRowChanging);
                //this.TableNewRow += new DataTableNewRowEventHandler(tablePaymentDataTable_TableNewRow);
            }
            void tableTransactionDataTable_ColumnChanging(object sender, DataColumnChangeEventArgs e)
            {
                if (e.Column.ColumnName == "payerid" | e.Column.ColumnName == "accountid" | e.Column.ColumnName == "ppSum")
                {
                    decimal sum = 0M;
                    PaymentDS.tablePaymentRow row = e.Row as PaymentDS.tablePaymentRow;
                    if ((this.DataSet as PaymentDS).tableTransaction.Select("ppid=" + row.ppid.ToString(), string.Empty, DataViewRowState.Deleted | DataViewRowState.CurrentRows).Length > 0 & !row.IspaysumNull())
                        sum = row.paysum;
                    else if (!row.IssumpayNull()) sum = row.sumpay;
                    if (sum > 0)
                        switch (e.Column.ColumnName)
                        {
                            case "payerid":
                                if (row.payerid != (int)e.ProposedValue)
                                {
                                    //e.Row.SetColumnError(e.Column,"В проведенном поручении Плательщик не может быть изменен.!\nПредварительно необходимо удалить проводки.");
                                    throw new System.Data.ConstraintException("В проведенном поручении Плательщик не может быть изменен.!\nПредварительно необходимо удалить проводки.");
                                }
                                break;
                            case "accountid":
                                if (row.accountid != (int)e.ProposedValue) throw new System.Data.ConstraintException("В проведенном поручении Получатель не может быть изменен.!\nПредварительно необходимо удалить проводки.");
                                break;
                            case "ppSum":
                                if (row.ppSum != (decimal)e.ProposedValue)
                                {
                                    //e.ProposedValue = row.ppSum;
                                    throw new System.Data.ConstraintException("В проведенном поручении сумма не может быть изменена.!\nПредварительно необходимо удалить проводки.");
                                }
                                break;
                        }
                }
            }
            void PaymentRowChanging(object sender, tablePaymentRowChangeEvent e)
            {
                //if (e.Action == DataRowAction.Change)
                //{
                //    if (e.Row.GetColumnsInError().Length > 0) throw new System.Data.InRowChangingEventException();
                //}
                if (e.Action == DataRowAction.Add && e.Row.ppid < 0M)
                {
                    try
                    {
                        PaymentDS.tablePaymentRow newrow = e.Row as PaymentDS.tablePaymentRow;
                        //PaymentDS.tablePaymentRow[] rowadd = this.Select(string.Empty, string.Empty, DataViewRowState.Added);
                        foreach (PaymentDS.tablePaymentRow row in this)
                        {
                            if (row.ppid != e.Row.ppid
                                && row.ppSum == newrow.ppSum & row.payerid == newrow.payerid
                                && (((!row.IsppNumberNull() & !newrow.IsppNumberNull() && row.ppNumber == newrow.ppNumber) | row.IsppNumberNull() | newrow.IsppNumberNull()) && ((!row.IsppDateNull() & !newrow.IsppDateNull() && row.ppDate == newrow.ppDate) | row.IsppDateNull() | newrow.IsppDateNull())))
                            {
                                PaymentDoubleWin win = new PaymentDoubleWin();
                                if (!row.IsppDateNull()) win.ppDate.Text = row.ppDate.ToShortDateString();
                                if (!row.IsppNumberNull()) win.ppNumber.Text = row.ppNumber;
                                if (!row.IspayerNameNull()) win.payerName.Text = row.payerName;
                                if (!row.IslegalNameNull()) win.legalName.Text = row.legalName;
                                if (!row.IsdeductedNull()) win.deducted.Text = row.deducted.ToShortDateString();
                                win.ppSum.Text = row.ppSum.ToString("N");
                                win.sumpay.Text = row.sumpay.ToString("N");
                                if (!row.IsnojoinsumNull()) win.nojoinsum.Text = row.nojoinsum.ToString("N");
                                if (!row.IspurposeNull()) win.purpose.Text = row.purpose;
                                if (!row.IsnoteNull()) win.note.Text = row.note;
                                if (!row.IsupdtDateNull()) win.updtDate.Text = row.updtDate.ToShortDateString();
                                if (!row.IsupdtWhoNull()) win.updtWho.Text = row.updtWho;
                                if (win.ShowDialog().HasValue && win.DialogResult.Value) e.Row.RejectChanges();
                                break;
                            }
                        }
                        //if (!isdubl)
                        //{
                        //    using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
                        //    {
                        //        System.Data.SqlClient.SqlCommand comm = new System.Data.SqlClient.SqlCommand();
                        //        comm.CommandType = CommandType.StoredProcedure;
                        //        comm.CommandText = "account.PPCheckDouble_sp";
                        //        if (!newrow.IsppNumberNull())
                        //        {
                        //            System.Data.SqlClient.SqlParameter ppNumber = new System.Data.SqlClient.SqlParameter("@ppNumber", newrow.ppNumber);
                        //            comm.Parameters.Add(ppNumber);
                        //        }
                        //        if (!newrow.IsppDateNull())
                        //        {
                        //            System.Data.SqlClient.SqlParameter ppDate = new System.Data.SqlClient.SqlParameter("@ppDate", newrow.ppDate);
                        //            comm.Parameters.Add(ppDate);
                        //        }
                        //        System.Data.SqlClient.SqlParameter accountid = new System.Data.SqlClient.SqlParameter("@payerid", newrow.payerid);
                        //        comm.Parameters.Add(accountid);
                        //        System.Data.SqlClient.SqlParameter ppSum = new System.Data.SqlClient.SqlParameter("@ppSum", newrow.ppSum);
                        //        comm.Parameters.Add(ppSum);
                        //        con.Open();
                        //        comm.Connection = con;
                        //        System.Data.SqlClient.SqlDataReader reader = comm.ExecuteReader();
                        //        if (reader.Read())
                        //        {
                        //            PaymentDoubleWin win = new PaymentDoubleWin();
                        //            if (!reader.IsDBNull(0)) win.ppDate.Text = reader.GetDateTime(0).ToShortDateString();
                        //            if (!row.IsppNumberNull()) win.ppNumber.Text = row.ppNumber;
                        //            if (!row.IspayerNameNull()) win.payerName.Text = row.payerName;
                        //            if (!row.IslegalNameNull()) win.legalName.Text = row.legalName;
                        //            if (!row.IsdeductedNull()) win.deducted.Text = row.deducted.ToShortDateString();
                        //            win.ppSum.Text = row.ppSum.ToString("N");
                        //            win.sumpay.Text = row.sumpay.ToString("N");
                        //            if (!row.IsnojoinsumNull()) win.nojoinsum.Text = row.nojoinsum.ToString("N");
                        //            if (!row.IspurposeNull()) win.purpose.Text = row.purpose;
                        //            if (!row.IsnoteNull()) win.note.Text = row.note;
                        //            if (!row.IsupdtDateNull()) win.updtDate.Text = row.updtDate.ToShortDateString();
                        //            if (!row.IsupdtWhoNull()) win.updtWho.Text = row.updtWho;
                        //            if (win.ShowDialog().HasValue && win.DialogResult.Value) e.Row.RejectChanges();
                        //        }
                        //        reader.Close();
                        //        con.Close();
                        //    }
                        //}
                    }
                    catch (System.Exception ex)
                    {
                        System.Windows.MessageBox.Show("Не удалось проверить уникальность платежа!/n" + ex.Message, "Проверка уникальности", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    }
                }
            }
            void tablePaymentDataTable_TableNewRow(object sender, DataTableNewRowEventArgs e)
            {
            }
        }

        partial class tablePaymentRow
        {
            internal void Refresh()
            {
                CustomBrokerWpf.SQLFilter filter = new SQLFilter("payment", "AND");
                filter.SetNumber(filter.FilterWhereId, "id", 0, this.ppid.ToString());
                PaymentDSTableAdapters.PaymentAdapter adapter = new PaymentDSTableAdapters.PaymentAdapter();
                adapter.ClearBeforeFill = false;
                adapter.Fill(this.tabletablePayment, filter.FilterWhereId);
                filter.RemoveCurrentWhere();
            }
        }

        partial class tableTransactionDataTable
        {
            public override void EndInit()
            {
                base.EndInit();
                this.RowChanged += new System.Data.DataRowChangeEventHandler(FillNewDCJoinn);
            }
            private void FillNewDCJoinn(object sender, DataRowChangeEventArgs e)
            {
                if (e.Action == DataRowAction.Add)
                {
                    PaymentDS.tableTransactionRow row = e.Row as PaymentDS.tableTransactionRow;
                    if (row.idtran < 0)
                    {
                        PaymentDS ds = this.DataSet as PaymentDS;
                        PaymentDSTableAdapters.DCJoinAdapter dcjadapter = new PaymentDSTableAdapters.DCJoinAdapter();
                        dcjadapter.ClearBeforeFill = false;
                        dcjadapter.Fill(ds.tableDCJoin, row.idtran, row.idC);
                    }
                }
            }
        }
    }
}
