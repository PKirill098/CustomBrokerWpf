using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для ParcelListWin.xaml
    /// </summary>
    public partial class ParcelListWin : Window
    {
        //ParcelDS parcelDS;
        ParcelViewCommander mycmd;
        private lib.BindingDischarger mybinddisp;
        public ParcelListWin()
        {
            //parcelDS = new ParcelDS();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mybinddisp = new lib.BindingDischarger(this, new DataGrid[] { ParcelDataGrid });
            mycmd = new ParcelViewCommander();
            mycmd.CancelEdit = mybinddisp.CancelEdit;
            mycmd.EndEdit = mybinddisp.EndEdit;
            this.DataContext = mycmd;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mycmd.Save.Execute(null);
            if (!mycmd.LastSaveResult)
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
                mycmd.Filter.RemoveCurrentWhere();
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (mycmd.Add.CanExecute(null))
            { 
                mycmd.Add.Execute(null);
                OpenParcel();
            }
            //((App.Current.MainWindow as MainWindow).ParcelGrid.DataContext as Classes.Domain.ParcelCurItemCommander).Add.Execute(null);
            //App.Current.MainWindow.Activate();
            //(App.Current.MainWindow as MainWindow).MainTabControl.SelectedIndex = 1;
        }
        private void ParcelItem_Click(object sender, RoutedEventArgs e)
        {
            OpenParcel();
            //if (this.parcelDataGrid.CurrentItem != null)
            //{
            //    parcelDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
            //    if (!this.parcelDataGrid.CurrentCell.IsValid) //для обновления Grid
            //    {
            //        if (!this.parcelDataGrid.IsFocused) this.parcelDataGrid.Focus();
            //        this.parcelDataGrid.CurrentCell = new DataGridCellInfo(this.parcelDataGrid.CurrentItem, this.parcelDataGrid.Columns[0]);
            //    }
            //    int id = ((this.parcelDataGrid.CurrentItem as DataRowView).Row as ParcelDS.tableParcelRow).parcelId;
            //    foreach (Classes.Domain.ParcelVM parcel in ((App.Current.MainWindow as MainWindow).ParcelGrid.DataContext as Classes.Domain.ParcelCurItemCommander).Items)
            //        if(parcel.DomainObject.Id==id)
            //        {
            //            App.Current.MainWindow.Activate();
            //            (App.Current.MainWindow as MainWindow).MainTabControl.SelectedIndex = 1;
            //            break;
            //        }
            //}
        }
        private void OpenParcel()
        {
            if (mycmd.Items.CurrentItem is ParcelVM)
            {
                Window parcelwin = null;
                ParcelVM parcel = mycmd.Items.CurrentItem as ParcelVM;
                foreach (Window win in (App.Current.MainWindow as MainWindow).ListChildWindow)
                {
                    if (win.Name == "winParcelItem" && (win.DataContext as ParcelCommander).VModel.DomainObject == parcel.DomainObject)
                    {
                        parcelwin = win;
                        break;
                    }
                } 
                if(parcelwin==null)
                {
                    parcelwin = new Windows.Parcel.ParcelItemWin();
                    parcelwin.DataContext = new ParcelCommander(parcel,mycmd.Items);
                    parcelwin.Show();
                    (App.Current.MainWindow as MainWindow).ListChildWindow.Add(parcelwin);
                }
                else
                {
                    parcelwin.Activate();
                    if (parcelwin.WindowState == WindowState.Minimized) parcelwin.WindowState = WindowState.Normal;
                }
            }
        }
        //private void RejectButton_Click(object sender, RoutedEventArgs e)
        //{
        //    bool isReject = false;
        //    if (this.parcelDataGrid.SelectedItem is DataRowView & this.parcelDataGrid.SelectedItems.Count == 1)
        //    {
        //        if (MessageBox.Show("Отменить несохраненные изменения в перевозке?", "Отмена изменений", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        //        {

        //            if ((this.parcelDataGrid.SelectedItem as DataRowView).IsEdit | (this.parcelDataGrid.SelectedItem as DataRowView).IsNew)
        //            {
        //                this.parcelDataGrid.CancelEdit(DataGridEditingUnit.Cell);
        //                this.parcelDataGrid.CancelEdit(DataGridEditingUnit.Row);
        //            }
        //            else
        //            {
        //                ParcelDS.tableParcelRow prow = (this.parcelDataGrid.SelectedItem as DataRowView).Row as ParcelDS.tableParcelRow;
        //                DataRow[] rrows = parcelDS.tableParcelRequest.Select("parcel=" + prow.parcelId.ToString());
        //                foreach (DataRow rrow in rrows)
        //                    rrow.RejectChanges();
        //                prow.RejectChanges();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (MessageBox.Show("Отменить все несохраненные изменения?", "Отмена изменений", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        //        {
        //            this.parcelDataGrid.CancelEdit(DataGridEditingUnit.Cell);
        //            this.parcelDataGrid.CancelEdit(DataGridEditingUnit.Row);
        //            parcelDS.tableParcel.RejectChanges();
        //            parcelDS.tableParcelRequest.RejectChanges();
        //        }
        //    }
        //    if (isReject)
        //    {
        //        PopupText.Text = "Изменения отменены";
        //        popInf.PlacementTarget = sender as UIElement;
        //        popInf.IsOpen = true;
        //    }
        //}
        //private void SaveButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (SaveChanges())
        //    {
        //        PopupText.Text = "Изменения сохранены";
        //        popInf.IsOpen = true;
        //    }
        //}
        //private void RefreshButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (SaveChanges() || MessageBox.Show("Изменения не были сохранены и будут потеряны при обновлении!\nОстановить обновление?", "Обновление данных", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) mainDataRefresh();
        //}
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void mainDataGrid_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            System.Windows.Input.RoutedCommand com = e.Command as System.Windows.Input.RoutedCommand;
            if (com != null)
            {
                if (com == ApplicationCommands.Delete && this.ParcelDataGrid.SelectedItems.Count > 0)
                {
                    e.Handled = !(MessageBox.Show("Удалить выделенные строки?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes);
                }
            }
        }
        private void mainDataGrid_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                if (e.Error.Exception == null)
                    MessageBox.Show(e.Error.ErrorContent.ToString(), "Некорректное значение");
                else
                    MessageBox.Show(e.Error.Exception.Message, "Некорректное значение");
            }
        }

        #region Filter
        public bool IsShowFilterWindow
        {
            set
            {
                this.FilterButton.IsChecked = value;
            }
            get { return this.FilterButton.IsChecked.Value; }
        }
        //public lib.SQLFilter.SQLFilter Filter
        //{
        //    get { return thisfilter; }
        //    set
        //    {
        //        if (!SaveChanges())
        //            MessageBox.Show("Применение фильтра невозможно. Перевозка содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //        else
        //        {
        //            thisfilter = value;
        //            mainDataRefresh();
        //        }
        //    }
        //}
        //public void RunFilter(lib.Filter.FilterItem[] filters)
        //{
        //    if (!SaveChanges())
        //        MessageBox.Show("Применение фильтра невозможно. Перевозка содержит не сохраненные данные. \n Сохраните данные и повторите попытку.", "Применение фильтра", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //    else
        //    {
        //        mainDataRefresh();
        //    }
        //}

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winParcelFilter") ObjectWin = item;
            }
            if (FilterButton.IsChecked.Value)
            {
                if (ObjectWin == null)
                {
                    ObjectWin = new ParcelFilterWin() { FilterOwner = (lib.Interfaces.IFilterWindowOwner)this.DataContext };
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

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is TextBlock && ParcelDataGrid.CurrentCell.Column?.SortMemberPath == nameof(Parcel.ParcelNumberOrder))
            {
                OpenParcel();
                e.Handled = true;
            }
        }
    }
}
