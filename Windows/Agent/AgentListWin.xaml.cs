using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using KirillPolyanskiy.DataModelClassLibrary.Metadata;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public partial class AgentListWin : Window
    {
        private MetadataDataGrid mymetadatadatagrid;
        private Classes.Domain.AgentViewCommand mycmd;
        private DataModelClassLibrary.BindingDischarger mybindingdischanger;
        
        public AgentListWin()
        {
            InitializeComponent();
            mybindingdischanger = new DataModelClassLibrary.BindingDischarger(this, new DataGrid[] { MainDataGrid });
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mymetadatadatagrid = new MetadataDataGrid("ClientNewWinMainDataGrid", CustomBrokerWpf.References.CurrentUser, MainDataGrid);
            mymetadatadatagrid.ConnectionString = CustomBrokerWpf.References.ConnectionString;
            mymetadatadatagrid.Set();
            mycmd = new Classes.Domain.AgentViewCommand();
            mycmd.EndEdit = mybindingdischanger.EndEdit;
            mycmd.CancelEdit = mybindingdischanger.CancelEdit;
            this.DataContext = mycmd;
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
            {
                mycmd.Filter.Dispose();
                (App.Current.MainWindow as MainWindow)?.ListChildWindow.Remove(this);
                mymetadatadatagrid.Save();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AgentOpen(null);
        }

        private void MainDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is DataGrid && e.OriginalSource is TextBlock)
            {
                object item = (e.OriginalSource as TextBlock).GetBindingExpression(TextBlock.TextProperty).ResolvedSource;
                string property = (e.OriginalSource as TextBlock).GetBindingExpression(TextBlock.TextProperty).ResolvedSourcePropertyName;
                if (item is Classes.Domain.AgentVM & MainDataGrid.CommitEdit(DataGridEditingUnit.Row, true))
                {
                    AgentOpen(item as Classes.Domain.AgentVM);
                }
            }
        }

        private void AgentOpen(Classes.Domain.AgentVM agent)
        {
            if (agent == null)
            {
                mycmd.Add.Execute(null);
                agent = mycmd.Items.CurrentItem as Classes.Domain.AgentVM;
            }

            Window ObjectWin = null;
            foreach (Window item in this.OwnedWindows)
            {
                if (item.Name == "winAgentItem" && (item.DataContext as Classes.Domain.AgentCommand).VModel.DomainObject == agent.DomainObject) ObjectWin = item;
            }
            if (ObjectWin == null)
            {
                Classes.Domain.AgentCommand cmd = new Classes.Domain.AgentCommand(agent, mycmd.Items);
                ObjectWin = new AgentItemWin();
                ObjectWin.Owner = this;
                ObjectWin.DataContext = cmd;
                ObjectWin.Show();
            }
            else
            {
                ObjectWin.Activate();
                if (ObjectWin.WindowState == WindowState.Minimized) ObjectWin.WindowState = WindowState.Normal;
            }
        }
        #region Filter
        private void NameFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.NameFilter != null && !mycmd.NameFilter.FilterOn) mycmd.NameFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("NameFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void FullNameFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.FullNameFilter != null && !mycmd.FullNameFilter.FilterOn) mycmd.FullNameFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("FullNameFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void DayEntryFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("DayEntryFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void CreaterFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.CreaterFilter != null && !mycmd.CreaterFilter.FilterOn) mycmd.CreaterFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("CreaterFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void RecommendFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            if (mycmd.RecommendFilter != null && !mycmd.RecommendFilter.FilterOn) mycmd.RecommendFilter?.FillAsync();
            Popup ppp = this.MainDataGrid.FindResource("RecommendFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        private void StateFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
            Popup ppp = this.MainDataGrid.FindResource("StateFilterPopup") as Popup;
            ppp.PlacementTarget = (UIElement)sender;
            ppp.IsOpen = true;
            e.Handled = true;
        }
        #endregion
    }
}
