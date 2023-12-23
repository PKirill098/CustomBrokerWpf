using System;
using System.Windows;
using System.Windows.Controls;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для ContactPointTypeWin.xaml
    /// </summary>
    public partial class ContactPointTypeWin : Window
    {
        public ContactPointTypeWin()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataLoad();
        }
        private void DataLoad()
        {
            try
            {
                ReferenceDS thisDS = this.FindResource("keyReferenceDS") as ReferenceDS;
                KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.ContactPointTypeAdapter keyBrandDSContactPointTypeAdapter = new KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.ContactPointTypeAdapter();
                keyBrandDSContactPointTypeAdapter.ClearBeforeFill = false;
                keyBrandDSContactPointTypeAdapter.Fill(thisDS.ContactPointTypeTb);
                ReferenceDSTableAdapters.adapterContactPointTemplate templateAdapter = new ReferenceDSTableAdapters.adapterContactPointTemplate();
                templateAdapter.Fill(thisDS.tableContactPointTemplate);
                System.Windows.Data.CollectionViewSource contactPointTypeTbViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("contactPointTypeTbViewSource")));
                contactPointTypeTbViewSource.View.MoveCurrentToFirst();
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
        private bool SaveChanges()
        {
            bool isSuccess = false;
            try
            {
                if (mainDataGrid.CommitEdit(DataGridEditingUnit.Cell, true) && mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true))
                {
                    CustomBrokerWpf.ReferenceDS thisDS = ((CustomBrokerWpf.ReferenceDS)(this.FindResource("keyReferenceDS")));
                    CustomBrokerWpf.ReferenceDSTableAdapters.ContactPointTypeAdapter thisAdapter = new CustomBrokerWpf.ReferenceDSTableAdapters.ContactPointTypeAdapter();
                    thisAdapter.Update(thisDS.ContactPointTypeTb);
                    isSuccess = true;
                }
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
