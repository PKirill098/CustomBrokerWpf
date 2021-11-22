using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для RequestItemWin.xaml
    /// </summary>
    public partial class RequestItemWin : Window
    {
        //private DataModelClassLibrary.Metadata.MetadataDataGrid mymetadatadatagrid;
        //private DataModelClassLibrary.Metadata.MetadataDataGrid mymetadatadatagrid2;

        public RequestItemWin()
        {
            InitializeComponent();
            mycurrencyrate = new Classes.CurrencyRate();
        }
        private void winRequestItem_Loaded(object sender, RoutedEventArgs e)
        {
            //mymetadatadatagrid = new DataModelClassLibrary.Metadata.MetadataDataGrid("RequestItemWinMainDataGrid", "", mainDataGrid);
            //mymetadatadatagrid2 = new DataModelClassLibrary.Metadata.MetadataDataGrid("RequestItemWinMainDataGrid", "", main2DataGrid);
            //mymetadatadatagrid.Set();
            //mymetadatadatagrid2.Set();
        }

        private void HistoryOpen_Click(object sender, RoutedEventArgs e)
        {
            RequestHistoryWin newHistory = new RequestHistoryWin();
            if ((sender as Button).Tag is RequestVM)
            {
                Request request = ((sender as Button).Tag as RequestVM).DomainObject;
                RequestHistoryViewCommand cmd = new RequestHistoryViewCommand(request);
                newHistory.DataContext = cmd;
            }
            newHistory.Owner = this;
            newHistory.Show();
        }
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveChanges();
        }
        private bool SaveChanges()
        {
            bool isSuccess = false;
            try
            {
                BindingListCollectionView view;
                if (this.Owner is RequestWin)
                    view = CollectionViewSource.GetDefaultView((this.Owner as RequestWin).mainDataGrid.ItemsSource) as BindingListCollectionView;
                else
                    view = CollectionViewSource.GetDefaultView((this.Owner as MainWindow).RequestDataGrid.ItemsSource) as BindingListCollectionView;
                IInputElement fcontrol =FocusManager.GetFocusedElement(this);
                if (fcontrol is TextBox & view.CurrentItem != null)
                {
                    BindingExpression be;
                    be = (fcontrol as FrameworkElement).GetBindingExpression(TextBox.TextProperty);
                    if (be != null)
                    {
                        //DataRow row = (view.CurrentItem as DataRowView).Row as DataRow;
                        //decimal d;
                        //DateTime dt;
                        //bool isDirty = false;
                        //switch (be.ParentBinding.Path.Path)
                        //{
                        //    case "cellNumber":
                        //    case "officialWeight":
                        //    case "volume":
                        //    case "actualWeight":
                        //    case "goodValue":
                        //        isDirty = (row.IsNull(be.ParentBinding.Path.Path) & (fcontrol as TextBox).Text.Length > 0) || !decimal.TryParse((fcontrol as TextBox).Text, out d) || row.Field<Decimal>(be.ParentBinding.Path.Path) != d;
                        //        break;
                        //    case "specification":
                        //    case "storageDate":
                        //        isDirty = (row.IsNull(be.ParentBinding.Path.Path) & (fcontrol as TextBox).Text.Length > 0) || !DateTime.TryParse((fcontrol as TextBox).Text, out dt) || row.Field<DateTime>(be.ParentBinding.Path.Path) != dt;
                        //        break;
                        //    case "storagePoint":
                        //    case "storageNote":
                        //    case "managerNote":
                        //        isDirty = (row.IsNull(be.ParentBinding.Path.Path) & (fcontrol as TextBox).Text.Length > 0) || !(fcontrol as TextBox).Text.Equals(row.Field<string>(be.ParentBinding.Path.Path));
                        //        break;
                        //    default:
                        //        isDirty = true;
                        //        MessageBox.Show("Поле не добавлено в обработчик сохранения без потери фокуса!", "Сохранение изменений");
                        //        break;
                        //}
                        if (be.IsDirty) be.UpdateSource();
                        if (be.HasError) return false;
                    }
                }
                
                if (view.IsAddingNew) view.CommitNew();
                if (view.IsEditingItem) view.CommitEdit();

                RequestDS requestDS;
                if (this.Owner is RequestWin)
                    requestDS = ((KirillPolyanskiy.CustomBrokerWpf.RequestDS)(this.Owner.FindResource("requestDS")));
                else
                    requestDS = (this.Owner as MainWindow).RequestGrid.FindResource("requestDS") as RequestDS;
                KirillPolyanskiy.CustomBrokerWpf.RequestDSTableAdapters.adapterRequest requestDSRequest_tbTableAdapter = new KirillPolyanskiy.CustomBrokerWpf.RequestDSTableAdapters.adapterRequest();
                requestDSRequest_tbTableAdapter.Adapter.ContinueUpdateOnError = false;
                requestDSRequest_tbTableAdapter.Update(requestDS.tableRequest);

                isSuccess = true;
                isSuccess = isSuccess & this.mainDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
                isSuccess = isSuccess & this.mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                isSuccess = isSuccess & this.main2DataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
                isSuccess = isSuccess & this.main2DataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                if (isSuccess) RequestItemViewCommand.Save.Execute(null);

            }
            catch (Exception ex)
            {
                if (ex is System.Data.SqlClient.SqlException)
                {
                    System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                    if (err.Number > 49999) MessageBox.Show(err.Message, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        System.Text.StringBuilder errs = new System.Text.StringBuilder();
                        foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                        {
                            errs.Append(sqlerr.Message + "\n");
                        }
                        MessageBox.Show(errs.ToString(), "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (MessageBox.Show("Повторить попытку сохранения?", "Сохранение изменений", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    isSuccess = SaveChanges();
                }
            }
            return isSuccess;
        }

        private void winRequestItem_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveChanges();
            //mymetadatadatagrid.Save();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            RequestDS.tableRequestRow row = (mainGrid.DataContext as DataRowView).Row as RequestDS.tableRequestRow;
            FreightWin winFreight = null;
            foreach (Window frwin in this.OwnedWindows)
            {
                if (frwin.Name == "winFreight")
                {
                    if ((frwin as FreightWin).RequestRow.requestId == row.requestId) winFreight = frwin as FreightWin;
                }
            }
            if (winFreight == null)
            {
                foreach (Window frwin in this.Owner.OwnedWindows)
                {
                    if (frwin.Name == "winFreight")
                    {
                        if ((frwin as FreightWin).RequestRow.requestId == row.requestId) winFreight = frwin as FreightWin;
                    }
                }
            }
            if (winFreight == null)
            {
                winFreight = new FreightWin();
                if (row.isfreight) winFreight.FreightId = row.freight;
                else winFreight.FreightId = 0;
                RequestDS requestDS;
                if (this.Owner is RequestWin)
                    requestDS = ((KirillPolyanskiy.CustomBrokerWpf.RequestDS)(this.Owner.FindResource("requestDS")));
                else
                    requestDS = (this.Owner as MainWindow).RequestGrid.FindResource("requestDS") as RequestDS;
                winFreight.agentComboBox.ItemsSource = new System.Data.DataView(requestDS.tableAgentName, string.Empty, "agentName", System.Data.DataViewRowState.CurrentRows);
                if(!row.IsagentIdNull()) winFreight.agentComboBox.SelectedValue = row.agentId;
                //if(!row.IsgoodValueNull()) winFreight.goodValueTextBox.Text = row.goodValue.ToString("N");
                winFreight.RequestRow = row;
                winFreight.Owner = this;
                winFreight.Show();
            }
            else
            {
                winFreight.Activate();
                if (winFreight.WindowState == WindowState.Minimized) winFreight.WindowState = WindowState.Normal;
            }
            //if (winFreight.FreightId==0) row.SetfreightNull();
            //else row.freight = winFreight.FreightId;
        }

        private void agentButton_Click(object sender, RoutedEventArgs e)
        {
            AgentOpen();
        }
        private void AgentOpen()
        {
            if (this.agentComboBox.Text.Length > 0)
            {
                AgentWin agentWin = new AgentWin();
                agentWin.Show();
                agentWin.AgentNameList.Text = this.agentComboBox.Text;
            }
        }

        private void customerButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerOpen();
        }
        private void CustomerOpen()
        {
            if (this.customerComboBox.Text.Length > 0)
            {
                ClientWin win = new ClientWin();
                win.Show();
                win.CustomerNameList.Text = this.customerComboBox.Text;
            }
        }

        private void thisValidation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                if (e.Error.Exception == null)
                    MessageBox.Show(e.Error.ErrorContent.ToString(), "Некорректное значение");
                else
                    MessageBox.Show(e.Error.Exception.Message, "Некорректное значение");
            }
        }

        public Classes.Domain.RequestItemViewCommand RequestItemViewCommand { set; get; }
        private Classes.CurrencyRate mycurrencyrate;
        public Classes.CurrencyRate CurrencyRate
        {
            get
            {
                return mycurrencyrate;
            }
        }
        private void InfoErrDescription_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Tag is Classes.Domain.RequestItemVM)
            {
                ViewTextWin win = new ViewTextWin();
                win.TextBox.Text = ((sender as Button).Tag as Classes.Domain.RequestItemVM).ErrDescriptions;
                win.Owner = this;
                win.Show();
            }
        }

        private void CommandBindingDelete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RequestItemViewCommand.Delete.Execute((sender as DataGrid)?.SelectedItems);
            e.Handled = true;
        }
        private void CommandBindingDelete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = RequestItemViewCommand.Delete.CanExecute((sender as DataGrid)?.SelectedItems);
            e.Handled = true;
        }

    }
}
