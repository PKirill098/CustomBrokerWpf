using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для CostWin.xaml
    /// </summary>
    public partial class CostWin : Window, ISQLFiltredWindow
    {
        ParcelCostDS thisDS;
        public CostWin()
        {
            InitializeComponent();
            thisDS = new ParcelCostDS();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReferenceDS refds = this.FindResource("keyReferenceDS") as ReferenceDS;
            if (refds.tableLegalEntity.Count == 0)
            {
                ReferenceDSTableAdapters.LegalEntityAdapter legadapter = new ReferenceDSTableAdapters.LegalEntityAdapter();
                legadapter.Fill(refds.tableLegalEntity);
            }
            (mainDataGrid.FindResource("keyLegalEntity") as CollectionViewSource).Source = refds.tableLegalEntity.DefaultView;
            CustomBrokerWpf.SQLFilter parcelfilter = new SQLFilter("parcel", "AND");
            parcelfilter.ConditionValueAdd(parcelfilter.ConditionAdd(parcelfilter.FilterWhereId, "parcelstatus", "<"), "500", 0);
            ParcelCostDSTableAdapters.ParcelAdapter parceladapter = new ParcelCostDSTableAdapters.ParcelAdapter();
            parceladapter.Fill(thisDS.tableParcel, parcelfilter.FilterWhereId);
            parcelfilter.RemoveCurrentWhere();
            thisDS.tableParcel.DefaultView.Sort = "sortnumber";
            (mainDataGrid.FindResource("keyParcel") as CollectionViewSource).Source = thisDS.tableParcel.DefaultView;
            dataLoad();
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
            if (!e.Cancel)
            {
                (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
                thisfilter.RemoveCurrentWhere();
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
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveChanges() || MessageBox.Show("Изменения не были сохранены и будут потеряны при обновлении!\nОстановить обновление?", "Обновление данных", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                dataLoad();
            }
        }
        private void thisCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private bool SaveChanges()
        {
            bool isSuccess = false;
            try
            {
                this.mainDataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
                this.mainDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                ParcelCostDSTableAdapters.CostAdapter costadapter = new ParcelCostDSTableAdapters.CostAdapter();
                costadapter.Update(thisDS.tableCost);
                isSuccess = true;
            }
            catch(Exception ex)
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

        private void mainDataGrid_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action != ValidationErrorEventAction.Removed)
            {
                if (e.Error.Exception == null)
                    MessageBox.Show(e.Error.ErrorContent.ToString(), "Некорректное значение");
                else
                    MessageBox.Show(e.Error.Exception.Message, "Некорректное значение");
            }
        }

        private void dataLoad()
        {
            this.mainDataGrid.ItemsSource = null;
            ParcelCostDSTableAdapters.CostAdapter adapter = new ParcelCostDSTableAdapters.CostAdapter();
            adapter.Fill(thisDS.tableCost, thisfilter.FilterWhereId);
            thisDS.tableCost.DefaultView.Sort="datetran";
            this.mainDataGrid.ItemsSource = thisDS.tableCost.DefaultView;
            setFilterButtonImage();
        }

        #region Filter
        private CustomBrokerWpf.SQLFilter thisfilter = new SQLFilter("parcelcost", "AND");
        public bool IsShowFilter
        {
            set
            {
                this.FilterButton.IsChecked = value;
            }
            get { return this.FilterButton.IsChecked.Value; }
        }
        public SQLFilter Filter
        {
            get { return thisfilter; }
            set
            {
                if (this.IsLoaded && !SaveChanges())
                    MessageBox.Show("Применение фильтра невозможно. Перевозка содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else
                {
                    thisfilter.RemoveCurrentWhere();
                    thisfilter = value;
                    if (this.IsLoaded) dataLoad();
                }
            }
        }
        public void RunFilter()
        {
            if (!SaveChanges())
                MessageBox.Show("Применение фильтра невозможно. Не удалось сохранить изменения. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
            {
                dataLoad();
            }
        }
        private void setFilterButtonImage()
        {
            string uribitmap;
            if (thisfilter.isEmpty) uribitmap = @"/CustomBrokerWpf;component/Images/funnel.png";
            else uribitmap = @"/CustomBrokerWpf;component/Images/funnel_preferences.png";
            System.Windows.Media.Imaging.BitmapImage bi3 = new System.Windows.Media.Imaging.BitmapImage(new Uri(uribitmap, UriKind.Relative));
            (FilterButton.Content as Image).Source = bi3;
        }
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winCostFilter") ObjectWin = item;
            }
            if (FilterButton.IsChecked.Value)
            {
                if (ObjectWin == null)
                {
                    ObjectWin.Owner = this;
                    ObjectWin.Show();
                }
                else
                {
                    ObjectWin.Activate();
                    if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
                }
            }
            else
            {
                if (ObjectWin != null)
                {
                    ObjectWin.Close();
                }
            }
        }
        #endregion

   }
}
