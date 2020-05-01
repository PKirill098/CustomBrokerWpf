using System;
using System.Windows;
using System.Windows.Controls;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для LegalEntityWin.xaml
    /// </summary>
    public partial class LegalEntityWin : Window
    {
        public LegalEntityWin()
        {
            InitializeComponent();
        }

        private void winLegalEntity_Loaded(object sender, RoutedEventArgs e)
        {
            DataLoad();
        }

        private void winLegalEntity_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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

        private void DataLoad()
        {
            try
            {
                ReferenceDS thisDS = this.FindResource("keyReferenceDS") as ReferenceDS;
                thisDS.LegalEntityRefresh();
                System.Data.DataView view= new System.Data.DataView(thisDS.tableLegalEntity);
                view.RowFilter="namelegal<>''";
                view.Sort="namelegal";
                this.mainDataGrid.ItemsSource = view;
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
                mainDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
                mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                ReferenceDS thisDS = this.FindResource("keyReferenceDS") as ReferenceDS;
                CustomBrokerWpf.ReferenceDSTableAdapters.LegalEntityAdapter adapter = new ReferenceDSTableAdapters.LegalEntityAdapter();
                adapter.Update(thisDS.tableLegalEntity);
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

        private void MoveCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in OwnedWindows)
            {
                if (item.Name == "winCustomerMoveLegal") ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                ObjectWin = new CustomerMoveLegalWin();
                ObjectWin.Owner=this;
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }

        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
