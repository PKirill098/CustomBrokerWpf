using KirillPolyanskiy.DataModelClassLibrary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Universal window for dictionary
    /// </summary>
    public partial class DictionaryWin : Window
    {
        private lib.BindingDischarger mybinddisp;
        private ReferenceCollectionSimpleItemVM mycmd;
        public DictionaryWin()
        {
            InitializeComponent();
        }

        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //    DataLoad();
        //}
        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            mybinddisp = new lib.BindingDischarger(this, new DataGrid[] { this.mainDataGrid });
            mycmd = e.NewValue as ReferenceCollectionSimpleItemVM;
            mycmd.EndEdit = mybinddisp.EndEdit;
            mycmd.CancelEdit = mybinddisp.CancelEdit;
        }
        //private void DataLoad()
        //{
        //    try
        //    {
        //        this.mainDataGrid.ItemsSource = CustomBrokerWpf.References.GoodsTypesParcel;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex is System.Data.SqlClient.SqlException)
        //        {
        //            System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
        //            System.Text.StringBuilder errs = new System.Text.StringBuilder();
        //            foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
        //            {
        //                errs.Append(sqlerr.Message + "\n");
        //            }
        //            MessageBox.Show(errs.ToString(), "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //        else
        //        {
        //            MessageBox.Show(ex.Message + "\n" + ex.Source, "Загрузка данных", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //        if (MessageBox.Show("Повторить загрузку данных?", "Загрузка данных", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
        //        {
        //            DataLoad();
        //        }
        //    }
        //}

        //private bool SaveChanges()
        //{
        //    bool isSuccess = false;
        //    try
        //    {
        //        if (mainDataGrid.CommitEdit(DataGridEditingUnit.Cell, true) && mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true))
        //        {
        //            //ReferenceDS itemDS = this.FindResource("keyReferenceDS") as ReferenceDS;
        //            //KirillPolyanskiy.CustomBrokerWpf.ReferenceDSTableAdapters.GoodsTypeAdapter adapter = new ReferenceDSTableAdapters.GoodsTypeAdapter();
        //            //adapter.Update(itemDS.tableGoodsType);
        //            CustomBrokerWpf.References.GoodsTypesParcel.SaveDataChanges();
        //            isSuccess = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex is System.Data.SqlClient.SqlException)
        //        {
        //            System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
        //            if (err.Number > 49999) MessageBox.Show(err.Message, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
        //            else
        //            {
        //                System.Text.StringBuilder errs = new System.Text.StringBuilder();
        //                foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
        //                {
        //                    errs.Append(sqlerr.Message + "\n");
        //                }
        //                MessageBox.Show(errs.ToString(), "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show(ex.Message + "\n" + ex.Source, "Сохранение изменений", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //        if (MessageBox.Show("Повторить попытку сохранения?", "Сохранение изменений", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
        //        {
        //            isSuccess = SaveChanges();
        //        }
        //    }
        //    return isSuccess;
        //}
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (mybinddisp.EndEdit())
            {
                bool isdirty = false;
                foreach (ReferenceSimpleItem item in mycmd.Items.SourceCollection) isdirty = isdirty | item.IsDirty;
                if (isdirty)
                {
                    if (MessageBox.Show("Сохранить изменения?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        if (!mycmd.SaveDataChanges())
                        {
                            this.Activate();
                            if (MessageBox.Show("\nИзменения в ДС не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                            {
                                e.Cancel = true;
                            }
                        }
                    }
                }
            }
            else
            {
                this.Activate();
                if (MessageBox.Show("\nИзменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
            if (!e.Cancel)
            {
                (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
                (App.Current.MainWindow as MainWindow).Activate();
            }
        }

        private void CommandBindingDel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mainDataGrid.SelectedItems.Count > 0 & mycmd.Delete.CanExecute(mainDataGrid.SelectedItems);
            e.Handled = true;
        }

        private void CommandBindingDel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mycmd.Delete.Execute(mainDataGrid.SelectedItems);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
