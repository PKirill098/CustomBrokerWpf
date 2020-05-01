namespace KirillPolyanskiy.CustomBrokerWpf
{


    public partial class WithdrawalDS
    {
        public partial class tableExpenditureDataTable
        {
            public override void EndInit()
            {
                base.EndInit();
                //this.ColumnChanging += tableExpenditureDataTable_ColumnChanging;
            }

            void tableExpenditureDataTable_ColumnChanging(object sender, System.Data.DataColumnChangeEventArgs e)
            {
                //throw new System.NotImplementedException();
            }

            internal ExpenditureDS.tableExpenditureRow[] Select<T1>()
            {
                throw new System.NotImplementedException();
            }
        }

        partial class tableWithdrawalDataTable
        {
            public override void EndInit()
            {
                base.EndInit();
                this.TableNewRow += tableExpenditureDataTable_TableNewRow;
                this.ColumnChanged += TableWithdrawalDataTable_ColumnChanged;
            }

            private void TableWithdrawalDataTable_ColumnChanged(object sender, System.Data.DataColumnChangeEventArgs e)
            {
                if (e.Column.ColumnName == "recipient")
                {
                    Domain.References.Contractor cond = References.Contractors.FindFirstItem("Id", e.ProposedValue);
                    if (cond != null) (e.Row as tableWithdrawalRow).contractor = cond.Name;
                }
            }

            private void tableExpenditureDataTable_TableNewRow(object sender, System.Data.DataTableNewRowEventArgs e)
            {
                tableWithdrawalRow wdrow = e.Row as tableWithdrawalRow;
                wdrow.wddate = System.DateTime.Today;
            }
        }
        partial class tableWithdrawalRow
        {
            public tableExpenditureDataTable Expenditures
            {
                get
                {
                    return (this.Table.DataSet as WithdrawalDS).tableExpenditure;
                }
            }
        }
    }
}
