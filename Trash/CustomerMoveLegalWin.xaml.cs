using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для CustomerMoveLegalWin.xaml
    /// </summary>
    public partial class CustomerMoveLegalWin : Window
    {
        CustomerMoveLegalDS thisDS;
        CustomerMoveLegalDSTableAdapters.LegalJoinCustomerAdapter JoinLegalAdapter;
        CustomerMoveLegalDSTableAdapters.CustomerJoinLeganAdapter JoinCustomerAdapter;
        public CustomerMoveLegalWin()
        {
            InitializeComponent();
            thisDS = new CustomerMoveLegalDS();
            thisDS.LegalJoinCustomer_tb.DefaultView.Sort = "namelegal";
            thisDS.CustomerJoinLegan_td.DefaultView.Sort = "customerName";
            JoinLegalAdapter = new CustomerMoveLegalDSTableAdapters.LegalJoinCustomerAdapter();
            JoinCustomerAdapter = new CustomerMoveLegalDSTableAdapters.CustomerJoinLeganAdapter();
        }

        private void winCustomerMoveLegal_Loaded(object sender, RoutedEventArgs e)
        {
            ReferenceDS refds = this.FindResource("keyReferenceDS") as ReferenceDS;
            if (refds.tableLegalEntity.Count == 0) refds.LegalEntityRefresh();
            this.newlegalComboBox.ItemsSource = new System.Data.DataView(refds.tableLegalEntity, "idlegalentity<>0 AND isActual=true", "namelegal", System.Data.DataViewRowState.CurrentRows);
            JoinLegalAdapter.Fill(thisDS.LegalJoinCustomer_tb);
            this.oldLegalComboBox.ItemsSource = thisDS.LegalJoinCustomer_tb.DefaultView;
            this.customerComboBox.ItemsSource = thisDS.CustomerJoinLegan_td.DefaultView;
        }

        private void oldLegalComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.oldLegalComboBox.SelectedIndex > -1)
            {
                if (this.oldLegalComboBox.SelectedValue == null)
                    JoinCustomerAdapter.Fill(thisDS.CustomerJoinLegan_td, ((e.AddedItems[0] as DataRowView).Row as CustomerMoveLegalDS.LegalJoinCustomer_tbRow).accountid);
                else
                    JoinCustomerAdapter.Fill(thisDS.CustomerJoinLegan_td, (int)this.oldLegalComboBox.SelectedValue);
                this.customerComboBox.SelectAll();
            }
            else
            { thisDS.CustomerJoinLegan_td.Clear(); }
        }

        private void checkAllButton_Click(object sender, RoutedEventArgs e)
        {
            this.customerComboBox.SelectAll();
        }
        private void uncheckAllComboBox_Click(object sender, RoutedEventArgs e)
        {
            this.customerComboBox.SelectedItems.Clear();
        }
        private void MoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (oldLegalComboBox.SelectedIndex > -1 & newlegalComboBox.SelectedIndex > -1 & this.customerComboBox.SelectedItems.Count > 0) moveCustomer();
        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            int curlegal = -1;
            if (this.oldLegalComboBox.SelectedIndex > -1) curlegal = (int)this.oldLegalComboBox.SelectedValue;
            JoinLegalAdapter.Fill(thisDS.LegalJoinCustomer_tb);
            if (curlegal > -1) this.oldLegalComboBox.SelectedValue = curlegal;
        }

        private void ListBoxCheckBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (!lb.IsKeyboardFocusWithin) lb.Focus();
        }

        private void moveCustomer()
        {
            DataTable table = new DataTable();
            table.Columns.Add("", typeof(int));
            foreach (DataRowView viewrow in this.customerComboBox.SelectedItems) table.Rows.Add(viewrow.Row.Field<int>(1));
            SqlParameter customers = new SqlParameter();
            customers.ParameterName = "@customers";
            customers.SqlDbType = SqlDbType.Structured;
            customers.TypeName = "dbo.ID_TVP";
            customers.Value = table;
            SqlParameter newpayaccount = new SqlParameter("@payaccount", (int)newlegalComboBox.SelectedValue);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.CustomersMoveLegal_sp";
            cmd.Parameters.Add(customers); cmd.Parameters.Add(newpayaccount);
            using (SqlConnection con = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
            {
                cmd.Connection = con;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    if (ex is System.Data.SqlClient.SqlException)
                    {
                        System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                        if (err.Number > 49999) MessageBox.Show(err.Message, "Перенос клиентов", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                        {
                            System.Text.StringBuilder errs = new System.Text.StringBuilder();
                            foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                            {
                                errs.Append(sqlerr.Message + "\n");
                            }
                            MessageBox.Show(errs.ToString(), "Перенос клиентов", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(ex.Message + "\n" + ex.Source, "Перенос клиентов", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            this.JoinLegalAdapter.Fill(thisDS.LegalJoinCustomer_tb);
            this.oldLegalComboBox.SelectedValue = this.newlegalComboBox.SelectedValue;
            this.newlegalComboBox.SelectedIndex = -1;
        }

    }
}
