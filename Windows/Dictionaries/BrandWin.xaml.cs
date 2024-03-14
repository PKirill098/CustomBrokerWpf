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
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для BrandWin.xaml
    /// </summary>
    public partial class BrandWin : Window
    {
		Classes.Domain.BrandViewCMD mycmd;
		lib.BindingDischarger mybinddisp;
        public BrandWin()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mybinddisp = new lib.BindingDischarger(this, new DataGrid[] { this.MainDataGrid });
            mycmd = new Classes.Domain.BrandViewCMD();
            mycmd.CancelEdit = mybinddisp.CancelEdit;
			mycmd.EndEdit = mybinddisp.EndEdit;
            this.DataContext = mycmd;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
			if (mybinddisp.EndEdit())
			{
				bool isdirty = false;
				foreach (Classes.Domain.BrandVM item in mycmd.Items.SourceCollection) isdirty = isdirty | item.DomainObject.IsDirty;
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
							else
								mycmd.Reject.Execute(null);
						}
					}
					else
						mycmd.Reject.Execute(null);
				}
			}
			else
			{
				this.Activate();
				if (MessageBox.Show("\nИзменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
				{
					e.Cancel = true;
				}
				else
				{
					mycmd.Reject.Execute(null);
				}
			}
			if (!e.Cancel)
			{
				mycmd.Dispose();
				if (!e.Cancel) (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
				(App.Current.MainWindow as MainWindow).Activate();
			}
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CommandBindingDel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = MainDataGrid.SelectedItems.Count > 0 & mycmd.Delete.CanExecute(MainDataGrid.SelectedItems);
            e.Handled = true;
        }
        private void CommandBindingDel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mycmd.Delete.Execute(MainDataGrid.SelectedItems);
        }

        private void BrandFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
			if (mycmd.BrandFilter != null && !mycmd.BrandFilter.FilterOn) mycmd.BrandFilter?.FillAsync();
			System.Windows.Controls.Primitives.Popup ppp = this.MainDataGrid.FindResource("BrandFilterPopup") as System.Windows.Controls.Primitives.Popup;
			ppp.PlacementTarget = (UIElement)sender;
			ppp.IsOpen = true;
			e.Handled = true;
        }
        private void ProducerFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
			if (mycmd.ProducerFilter != null && !mycmd.ProducerFilter.FilterOn) mycmd.ProducerFilter?.FillAsync();
			System.Windows.Controls.Primitives.Popup ppp = this.MainDataGrid.FindResource("ProducerFilterPopup") as System.Windows.Controls.Primitives.Popup;
			ppp.PlacementTarget = (UIElement)sender;
			ppp.IsOpen = true;
			e.Handled = true;
        }
        private void HomelandFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
			if (mycmd.HomelandFilter != null && !mycmd.HomelandFilter.FilterOn) mycmd.HomelandFilter?.FillAsync();
			System.Windows.Controls.Primitives.Popup ppp = this.MainDataGrid.FindResource("HomelandFilterPopup") as System.Windows.Controls.Primitives.Popup;
			ppp.PlacementTarget = (UIElement)sender;
			ppp.IsOpen = true;
			e.Handled = true;
        }
        private void SizePlusFilterPopup_Open(object sender, MouseButtonEventArgs e)
        {
			System.Windows.Controls.Primitives.Popup ppp = this.MainDataGrid.FindResource("SizePlusFilterPopup") as System.Windows.Controls.Primitives.Popup;
			ppp.PlacementTarget = (UIElement)sender;
			ppp.IsOpen = true;
			e.Handled = true;
        }
    }
}
