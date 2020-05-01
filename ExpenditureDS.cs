using System;
namespace KirillPolyanskiy.CustomBrokerWpf
{
    public partial class ExpenditureDS
    {
        partial class tableExpenditureDataTable
        {
            bool ContractorNameChanged;
            public override void EndInit()
            {
                base.EndInit();
                ContractorNameChanged = false;
                //this.TableNewRow += tableExpenditureDataTable_TableNewRow;
                //this.ColumnChanging += tableExpenditureDataTable_ColumnChanging;
                this.ColumnChanged += TableExpenditureDataTable_ColumnChanged;
                //this.tableExpenditureRowChanging += tableExpenditureDataTable_tableExpenditureRowChanging;
                this.tableExpenditureRowChanged += TableExpenditureDataTable_tableExpenditureRowChanged;
            }

            private void TableExpenditureDataTable_tableExpenditureRowChanged(object sender, tableExpenditureRowChangeEvent e)
            {
                if (e.Action == System.Data.DataRowAction.Change)
                {
                    if (ContractorNameChanged)
                    {
                        ContractorNameChanged = false; // иначе зациклится
                        Domain.References.Contractor cond = References.Contractors.FindFirstItem("Id", e.Row.recipientEx);
                        if (cond != null) (e.Row as tableExpenditureRow).contractor = cond.Name;
                    }
                }
            }

            private void TableExpenditureDataTable_ColumnChanged(object sender, System.Data.DataColumnChangeEventArgs e)
            {
                if (e.Column.ColumnName == "recipientEx")
                {
                    ContractorNameChanged = true;
                }
            }

            void tableExpenditureDataTable_tableExpenditureRowChanging(object sender, tableExpenditureRowChangeEvent e)
            {
            }

            void tableExpenditureDataTable_ColumnChanging(object sender, System.Data.DataColumnChangeEventArgs e)
            {

            }

            void tableExpenditureDataTable_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
            {
                //(e.Row as ExpenditureDS.tableExpenditureRow).dateEx=System.DateTime.Today;
            }

            internal void CountDetailUpdate()
            {
                foreach (System.Data.DataRow row in this.Rows)
                {
                    (row as tableExpenditureRow).CountDetailUpdate();
                }
            }

        }
        partial class tableExpenditureRow
        {
            public string CheckRow()
            {
                string errmsg = string.Empty;
                if (this.sumEx == 0 & (this.sumPayCurr != 0 | this.sumPayRub != 0))
                {
                    errmsg = "Не указана сумма затраты";
                }
                else if (this.sumPayCurr == 0 & this.sumPayRub == 0 && (!this.IsdateExNull() | !(this.IsopertypeNull() || this.opertype == 0) | !(this.IslegalAccountIdNull() || this.legalAccountId == 0) | !(this.IsrecipientExNull() || this.recipientEx == 0)))
                    errmsg = "Не указана сумма оплаты";
                else if ((this.IslegalAccountIdNull() || this.legalAccountId == 0) && (this.sumPayCurr != 0 | this.sumPayRub != 0 | !this.IsdateExNull() | !(this.IsopertypeNull() || this.opertype == 0)))
                    errmsg = "Не указан источник";
                else if ((this.IsrecipientExNull() || this.recipientEx == 0) && (this.sumPayCurr != 0 | this.sumPayRub != 0 | !this.IsdateExNull() | !(this.IsopertypeNull() || this.opertype == 0)))
                    errmsg = "Не указан контрагент";
                else if (this.IsopertypeNull() && (this.sumPayCurr != 0 | this.sumPayRub != 0 | !this.IsdateExNull() | !(this.IslegalAccountIdNull() || this.legalAccountId == 0)))
                    errmsg = "Не указан тип операции";
                else if (this.IsdateExNull() && (this.sumPayCurr != 0 | this.sumPayRub != 0 | !this.IsdateExNull() | !(this.IslegalAccountIdNull() || this.legalAccountId == 0)))
                    errmsg = "Не указана дата операции";
                else if (this.IsparcelIDNull() & !(this.IsopertypeNull() || this.opertype == 0))
                    errmsg = "Не указана отправка";
                else if (this.sumEx < this.sumPayCurr)
                    errmsg = "Сумма оплаты превышает сумму затраты";
                else if (this.IsperiodEndNull() != this.IsperiodStartNull())
                    errmsg = "Для периода затраты необходимо указать дату начала и дату окончания";
                else if (!this.IsperiodEndNull() & !this.IsperiodStartNull() && this.periodStart > this.periodEnd)
                    errmsg = "Дата окончания периода не может быть меньше даты начала.";
                return errmsg;
            }

            internal void CountDetailUpdate()
            {
                if (this.countDetReal > 0)
                {
                    System.Data.DataRowState state = this.RowState;
                    this.countDet = this.countDetReal;
                    this.EndEdit();
                    if (state == System.Data.DataRowState.Unchanged) this.AcceptChanges();
                }
            }
        }
    }
}