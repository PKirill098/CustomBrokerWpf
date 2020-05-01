using System;
using System.Collections.Generic;
using System.Data;
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

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для ExpenditureTypeWin.xaml
    /// </summary>
    public partial class ExpenditureTypeWin : Window
    {
        public ExpenditureTypeWin()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataLoad();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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

        private void DataGrid_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                if (e.Error.Exception == null)
                    MessageBox.Show(e.Error.ErrorContent.ToString(), "Некорректное значение");
                else
                    MessageBox.Show(e.Error.Exception.Message, "Некорректное значение");
            }
        }
        private void detailDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            try
            {
                BindingListCollectionView view = CollectionViewSource.GetDefaultView(mainDataGrid.ItemsSource) as BindingListCollectionView;
                if (view.IsAddingNew) view.CommitNew();
            }
            catch (NoNullAllowedException)
            {
                MessageBox.Show("Одно из обязательных для заполнения полей оставлено пустым. \n Введите значение в поле.", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DataLoad()
        {
            try
            {
                ReferenceDS itemDS = ((ReferenceDS)(this.FindResource("keyReferenceDS")));
                KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.ExpenditureItemAdapter eiаdapter = new KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.ExpenditureItemAdapter();
                eiаdapter.ClearBeforeFill = false;
                eiаdapter.Fill(itemDS.tableExpenditureItem);
                CollectionViewSource eiDS = this.FindResource("keyExpenditureItemVS") as CollectionViewSource;
                eiDS.Source = itemDS.tableExpenditureItem;
                eiDS.SortDescriptions.Add(new System.ComponentModel.SortDescription("nameEI", System.ComponentModel.ListSortDirection.Ascending));
                KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.ExpenditureTypeAdapter etаdapter = new KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.ExpenditureTypeAdapter();
                etаdapter.ClearBeforeFill = false;
                etаdapter.Fill(itemDS.tableExpenditureType);
                KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.ExpenditureDetailNameListAdapter dnаdapter = new KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.ExpenditureDetailNameListAdapter();
                dnаdapter.ClearBeforeFill = false;
                dnаdapter.Fill(itemDS.tableExpenditureDetailNameList);
                mainDataGrid.ItemsSource = new DataView(itemDS.tableExpenditureType);
           }
            catch (Exception ex)
            {
                if (ex is System.Data.SqlClient.SqlException)
                {
                    System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
                    System.Text.StringBuilder errs = new System.Text.StringBuilder();
                    foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
                    {
                        errs.Append(sqlerr.Message + "\n");
                    }
                    MessageBox.Show(errs.ToString(), "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message + "\n" + ex.Source, "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (MessageBox.Show("Повторить загрузку данных?", "Загрузка данных", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    DataLoad();
                }
            }
        }
        private bool SaveChanges()
        {
            bool isSuccess = false;
            try
            {
                detailDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
                detailDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                mainDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
                mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                KirillPolyanskiy.CustomBrokerWpf.ReferenceDS itemDS = ((KirillPolyanskiy.CustomBrokerWpf.ReferenceDS)(this.FindResource("keyReferenceDS")));
                KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.ExpenditureTypeAdapter etadapter = new KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.ExpenditureTypeAdapter();
                etadapter.Update(itemDS.tableExpenditureType);
                KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.ExpenditureDetailNameListAdapter dnadapter = new KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.ExpenditureDetailNameListAdapter();
                dnadapter.Update(itemDS.tableExpenditureDetailNameList);
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
    }
}
