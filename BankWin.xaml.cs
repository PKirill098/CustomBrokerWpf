using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для BankWin.xaml
    /// </summary>
    public partial class BankWin : Window
    {
        private CustomBrokerWpf.BankDS thisDS;
        private CustomBrokerWpf.BankDSTableAdapters.adapterBank bankadapter;
        private CustomBrokerWpf.BankDSTableAdapters.AccountAdapter accountadapter;

        public BankWin()
        {
            InitializeComponent();
        }

        private void winBank_Loaded(object sender, RoutedEventArgs e)
        {
            ReferenceDS thisReferenceDS = (this.FindResource("keyReferenceDS") as ReferenceDS);
            if (thisReferenceDS.tableAccountCurrency.Rows.Count == 0)
            {
                ReferenceDSTableAdapters.AccountCurrencyAdapter currencyadapter = new ReferenceDSTableAdapters.AccountCurrencyAdapter();
                currencyadapter.Fill(thisReferenceDS.tableAccountCurrency);
            }
            CollectionViewSource brandVS = this.accountDataGrid.FindResource("keyCurrencyVS") as CollectionViewSource;
            brandVS.Source = new DataView(thisReferenceDS.tableAccountCurrency, string.Empty, "currdescription", DataViewRowState.CurrentRows);

            thisDS = new BankDS();
            bankadapter = new BankDSTableAdapters.adapterBank();
            bankadapter.Fill(thisDS.tableBank);
            accountadapter = new BankDSTableAdapters.AccountAdapter();
            accountadapter.Fill(thisDS.tableAccount);
            this.BankList.ItemsSource = thisDS.tableBank.DefaultView;
        }

        private void winBank_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!SaveChanges())
            {
                this.Activate();
                if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
            if (!e.Cancel) (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BindingListCollectionView view = CollectionViewSource.GetDefaultView(this.BankList.ItemsSource) as BindingListCollectionView;
                view.AddNew();
            }
            catch (NoNullAllowedException)
            {
                if (string.IsNullOrEmpty(this.nameTextBox.Text)) MessageBox.Show("Поле \"Название\" не заполнено!\n Введите значение в поле.", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Stop);
                if (string.IsNullOrEmpty(this.bikTextBox.Text)) MessageBox.Show("Поле \"БИК\" не заполнено!\nВведите значение в поле.", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить банк?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (this.BankList.SelectedItem != null) (this.BankList.SelectedItem as DataRowView).Delete();
            }
        }
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveChanges())
            {
                PopupText.Text = "Изменения сохранены";
                popInf.IsOpen = true;
            }
        }

        private void BankList_GotFocus(object sender, RoutedEventArgs e)
        {
            accountDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
            accountDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
        }

        private bool SaveChanges()
        {
            bool isSuccess = false, UpdateAccountBinding = false;
            int bankcurrent = 1;
            try
            {
                accountDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
                accountDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                BindingListCollectionView view = CollectionViewSource.GetDefaultView(this.BankList.ItemsSource) as BindingListCollectionView;
                if (view.IsEditingItem) view.CommitEdit();
                if (view.IsAddingNew) view.CommitNew();
                if (this.BankList.SelectedItem is DataRowView)
                {
                    bankcurrent = accountDataGrid.SelectedIndex;
                    UpdateAccountBinding = (this.BankList.SelectedItem as DataRowView).Row.RowState == DataRowState.Added;
                }
                bankadapter.Update(thisDS.tableBank);
                accountadapter.Update(thisDS.tableAccount);
                if (UpdateAccountBinding)
                {
                    BindingExpression accountBinding = accountDataGrid.GetBindingExpression(DataGrid.ItemsSourceProperty);
                    accountBinding.UpdateTarget();
                    accountDataGrid.SelectedIndex = bankcurrent;
                }
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
                    if (string.IsNullOrEmpty(this.nameTextBox.Text)) MessageBox.Show("Поле \"Название\" не заполнено!\nЗаполните поле или удалите банк.", "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
                    if (string.IsNullOrEmpty(this.bikTextBox.Text)) MessageBox.Show("Поле \"БИК\" не заполнено!\nЗаполните поле или удалите банк.", "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void accountDataGrid_Error(object sender, ValidationErrorEventArgs e)
        {
            bool iserr = e.Action == ValidationErrorEventAction.Added;
            AddButton.IsEnabled = !iserr;
            BankList.IsEnabled = !iserr;
            if (iserr) MessageBox.Show(e.Error.ErrorContent.ToString());
        }
        private void accountDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            try
            {
                BindingListCollectionView view = CollectionViewSource.GetDefaultView(BankList.ItemsSource) as BindingListCollectionView;
                if (view.IsAddingNew) view.CommitNew();
            }
            catch (NoNullAllowedException)
            {
                if (string.IsNullOrEmpty(this.nameTextBox.Text)) MessageBox.Show("Поле \"Название\" не заполнено!\n Введите значение в поле.", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Stop);
                if (string.IsNullOrEmpty(this.bikTextBox.Text)) MessageBox.Show("Поле \"БИК\" не заполнено!\nВведите значение в поле.", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    public class currency
    {
        private string curcode;
        private string curdescr;
        public string Code
        { set { curcode = value; } get { return curcode; } }
        public string Description
        { set { curdescr = value; } get { return curdescr; } }
    }
}
