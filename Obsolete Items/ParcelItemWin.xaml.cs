using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для ParcelItemWin.xaml
    /// </summary>
    public partial class ParcelItemWin : Window
    {
        public ParcelItemWin()
        {
            InitializeComponent();
        }

        private void winParcel_Loaded(object sender, RoutedEventArgs e)
        {
            ReferenceDS refDS = this.FindResource("keyReferenceDS") as ReferenceDS;
            statusComboBox.ItemsSource = new System.Data.DataView(refDS.tableRequestStatus, "rowId>49", "rowId", DataViewRowState.CurrentRows);
            parcelTypeComboBox.ItemsSource = refDS.tableParcelType.DefaultView;
            refDS.tableGoodsType.DefaultView.Sort = "Nameitem";
            goodstypeComboBox.ItemsSource = refDS.tableGoodsType.DefaultView;
        }
        private void winParcelItem_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!SaveChanges())
            {
                this.Activate();
                if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
            if (!e.Cancel)
            {
                //(App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
                //thisfilter.RemoveFilter();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveChanges())
            {
                PopupText.Text = "Изменения сохранены";
                popInf.IsOpen = true;
            }
        }
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            if (ParcelRequestDataGrid.Items.Count > 0)
                MessageBox.Show("Нельзя удалить перевозку пока она содержит заявки!", "Удаление", MessageBoxButton.OK, MessageBoxImage.Stop);
            else if (MessageBox.Show("Удалить перевозку?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                //BindingListCollectionView view = CollectionViewSource.GetDefaultView(parcelDS.tableParcel.DefaultView) as BindingListCollectionView;
                //if (view.CurrentItem != null) (view.CurrentItem as DataRowView).Delete();
            }
        }

        private bool SaveChanges()
        {
            bool isSuccess = false;
            try
            {
                IInputElement fcontrol = FocusManager.GetFocusedElement(this);
                if (fcontrol is TextBox)
                {
                    BindingExpression be;
                    be = (fcontrol as FrameworkElement).GetBindingExpression(TextBox.TextProperty);
                    be.UpdateSource();
                }
                //BindingListCollectionView view = CollectionViewSource.GetDefaultView(parcelDS.tableParcel.DefaultView) as BindingListCollectionView;
                //if (view.IsAddingNew) view.CommitNew();
                //if (view.IsEditingItem) view.CommitEdit();
                //ParcelDSTableAdapters.ParcelAdapter parcelAdapter = new ParcelDSTableAdapters.ParcelAdapter();
                //DataRow[] rows = parcelDS.tableParcel.Select("", "", DataViewRowState.Added);
                //parcelAdapter.Update(rows);
                //rows = parcelDS.tableParcel.Select("", "", DataViewRowState.ModifiedCurrent);
                //parcelAdapter.Update(rows);
                //// обновление заявок
                //ParcelDSTableAdapters.ParcelRequestAdapter requestAdapter = new ParcelDSTableAdapters.ParcelRequestAdapter();
                //requestAdapter.Update(parcelDS.tableParcelRequest);
                //parcelAdapter.Update(parcelDS.tableParcel);

                isSuccess = true;
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
                else if (ex is System.Data.NoNullAllowedException)
                {
                    MessageBox.Show("Не все обязательные поля заполнены!\nЗаполните поля или удалите перевозку.", "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RequestDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RequestAddButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RequestOutButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RequestUpDown_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void RequestItem_Click(object sender, RoutedEventArgs e)
        {
            DataGrid grid = null;
            if (ParcelRequestDataGrid.IsFocused)
                grid = ParcelRequestDataGrid;
            else if (RequestDataGrid.IsFocused)
                grid = RequestDataGrid;
            if (grid!=null && grid.CurrentItem != null & grid.CommitEdit(DataGridEditingUnit.Row, true))
            {

                if (!grid.CurrentCell.IsValid) //для обновления Grid
                {
                    grid.CurrentCell = new DataGridCellInfo(grid.CurrentItem, grid.Columns[4]);
                }
                RequestItemWin newWin = null;
                foreach (Window item in this.OwnedWindows)
                {
                    if (item.Name == "winRequestItem")
                    {
                        if ((item as RequestItemWin).mainGrid.DataContext.Equals(grid.CurrentItem))
                            newWin = item as RequestItemWin;
                    }
                }
                if (newWin == null)
                {
                    newWin = new RequestItemWin();
                    newWin.Owner = this;
                    ReferenceDS refDS = this.FindResource("keyReferenceDS") as ReferenceDS;
                    newWin.statusComboBox.ItemsSource = new System.Data.DataView(refDS.tableRequestStatus, "rowId>0", string.Empty, System.Data.DataViewRowState.CurrentRows);
                    newWin.goodsComboBox.ItemsSource = new System.Data.DataView(refDS.tableGoodsType, "Iditem>0", string.Empty, System.Data.DataViewRowState.CurrentRows);

                    RequestDS requestDS = ((RequestDS)(this.FindResource("requestDS")));
                    newWin.customerComboBox.ItemsSource = new System.Data.DataView(requestDS.tableCustomerName, string.Empty, "customerName", System.Data.DataViewRowState.CurrentRows);
                    newWin.agentComboBox.ItemsSource = new System.Data.DataView(requestDS.tableAgentName, string.Empty, "agentName", System.Data.DataViewRowState.CurrentRows);
                    newWin.mainGrid.DataContext = grid.CurrentItem;
                    newWin.RequestItemViewCommand = new Classes.Domain.RequestItemViewCommand(((grid.CurrentItem as DataRowView).Row as CustomBrokerWpf.RequestDS.tableRequestRow).requestId);
                    newWin.thisStoragePointValidationRule.RequestId = ((grid.CurrentItem as DataRowView).Row as CustomBrokerWpf.RequestDS.tableRequestRow).requestId;
                    newWin.Show();
                }
                else
                {
                    newWin.Activate();
                    if (newWin.WindowState == WindowState.Minimized) newWin.WindowState = WindowState.Normal;
                }
            }
        }
    }
}
