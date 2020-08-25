using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Text;
using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System.Windows.Input;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для AgentWin.xaml
    /// </summary>
    public partial class AgentWin : Window, IFiltredWindow
    {
        public AgentWin()
        {
            InitializeComponent();
            mybindingdischanger = new DataModelClassLibrary.BindingDischarger(this, new DataGrid[] { AliasDataGrid });// AgentBrandDataGrid, AgentAddressDataGrid, AgentContactDataGrid, ContactPointDataGrid
        }

        private AgentItemViewCommander mycmd;
        private DataModelClassLibrary.BindingDischarger mybindingdischanger;
        internal DataModelClassLibrary.BindingDischarger BindingDischarger
        { get { return mybindingdischanger; } }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mycmd = new AgentItemViewCommander();
            mycmd.EndEdit = mybindingdischanger.EndEdit;
            mycmd.CancelEdit = mybindingdischanger.CancelEdit;
            this.DataContext = mycmd;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!(mybindingdischanger.EndEdit() && mycmd.SaveDataChanges()))
            {
                this.Activate();
                if (MessageBox.Show("Изменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
            if (!e.Cancel)
            { (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this); App.Current.MainWindow.Activate(); }
        }

        private void AgentNameList_GotFocus(object sender, RoutedEventArgs e)
        {
            mybindingdischanger.EndEdit();
        }




        public bool IsShowFilter
        {
            set
            {
                this.FilterButton.IsChecked = value;
            }
            get { return this.FilterButton.IsChecked.Value; }
        }
        public ItemFilter[] Filter
        {
            get { return mythisfilter; }
            set
            {
                mythisfilter = value;
                BindingListCollectionView view = (this.FindResource("keyAgentVS") as CollectionViewSource).View as BindingListCollectionView;
                if (mythisfilter.Length > 0)
                {
                    StringBuilder newfilter = new StringBuilder();
                    foreach (ItemFilter item in mythisfilter)
                    {
                        if (!(item is ItemFilter)) continue;
                        switch (item.PropertyName)
                        {
                            case "brandID":
                                //AgentDS ods = this.FindResource("keyAgentDS") as AgentDS;
                                //System.Data.DataRow[] rows = ods.tableAgentBrand.Select("brandID In (" + item.Value+")", "agentID");
                                //int curid = 0;
                                //StringBuilder filterBuilder = new StringBuilder();
                                //foreach (AgentDS.tableAgentBrandRow rowview in rows)
                                //{
                                //    if (curid != rowview.agentID)
                                //    {
                                //        curid = rowview.agentID;
                                //        filterBuilder.Append("," + curid.ToString());
                                //    }
                                //}
                                //if (filterBuilder.Length > 0) filterBuilder.Remove(0, 1); else filterBuilder.Append("0");
                                //newfilter.Append("agentID In (" + filterBuilder.ToString() + ")");
                                break;
                        }
                        //newfilter.Append(" AND "+item.PropertyName+ " "+item.Operation+" "+item.Value);
                    }
                    //newfilter.Remove(0,5);
                    view.CustomFilter = newfilter.ToString();
                    view.MoveCurrentToFirst();
                    string uribitmap;
                    if (newfilter.Length > 0) uribitmap = "/CustomBrokerWpf;component/Images/funnel_preferences.png";
                    else uribitmap = "/CustomBrokerWpf;component/Images/funnel.png";
                    System.Windows.Media.Imaging.BitmapImage bi3 = new System.Windows.Media.Imaging.BitmapImage(new Uri(uribitmap, UriKind.Relative));
                    (FilterButton.Content as Image).Source = bi3;

                }
                else
                {
                    view.CustomFilter = string.Empty;
                    view.MoveCurrentToFirst();
                    System.Windows.Media.Imaging.BitmapImage bi3 = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"/CustomBrokerWpf;component/Images/funnel.png", UriKind.Relative));
                    (FilterButton.Content as Image).Source = bi3;
                }
            }
        }
        ItemFilter[] mythisfilter = new ItemFilter[0];

        private void AgentBrandDataGrid_Error(object sender, ValidationErrorEventArgs e)
        {
            bool iserr = e.Action == ValidationErrorEventAction.Added;
            //AddButton.IsEnabled = !iserr;
            AgentNameList.IsEnabled = !iserr;
            if (iserr) MessageBox.Show(e.Error.ErrorContent.ToString());
        }
        private void AgentBrandDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            try
            {
                BindingListCollectionView view = CollectionViewSource.GetDefaultView(AgentNameList.ItemsSource) as BindingListCollectionView;
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

        private void ComboBox20_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox obj = (ComboBox)sender;
            if (obj != null)
            {
                var myTextBox = (TextBox)obj.Template.FindName("PART_EditableTextBox", obj);
                if (myTextBox != null)
                {
                    myTextBox.MaxLength = 20;
                }
            }
        }
        private void ComboBox100_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox obj = (ComboBox)sender;
            if (obj != null)
            {
                var myTextBox = (TextBox)obj.Template.FindName("PART_EditableTextBox", obj);
                if (myTextBox != null)
                {
                    myTextBox.MaxLength = 100;
                }
            }
        }
        private void ComboBox50_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox obj = (ComboBox)sender;
            if (obj != null)
            {
                var myTextBox = (TextBox)obj.Template.FindName("PART_EditableTextBox", obj);
                if (myTextBox != null)
                {
                    myTextBox.MaxLength = 50;
                }
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            Window ObjectWin = null;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winAgentFilter") ObjectWin = item;
            }
            if (FilterButton.IsChecked.Value)
            {
                if (ObjectWin == null)
                {
                    ObjectWin = new AgentFilterWin();
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

        private void Aliases_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.AliasDataGrid.CurrentItem is AgentAliasVM;
            e.Handled = true;
        }
        private void Aliases_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show("Удалить псевдоним?", "Удаление псевдонима", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                AgentAliasVM item = this.AliasDataGrid.SelectedItem as AgentAliasVM;
                mycmd.CurrentItem.Aliases.EditItem(item);
                item.DomainState = DataModelClassLibrary.DomainObjectState.Deleted;
                mycmd.CurrentItem.Aliases.CommitEdit();
            }
        }
    }
}
