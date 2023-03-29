using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Marking;
using System.Windows.Controls.Primitives;

namespace KirillPolyanskiy.CustomBrokerWpf
{
	/// <summary>
	/// Логика взаимодействия для Marking_win.xaml
	/// </summary>
	public partial class MarkingWin : Window
	{
		MarkingViewCommader mycmd;
		lib.BindingDischarger mybinddisp;

		public MarkingWin()
		{
			this.InitializeComponent();
			mybinddisp = new lib.BindingDischarger(this, new DataGrid[] { this.MainDataGrid });
		}

		private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != null)
			{
				mycmd = e.NewValue as MarkingViewCommader;
				mycmd.CancelEdit = mybinddisp.CancelEdit;
				mycmd.EndEdit = mybinddisp.EndEdit;
			}
			else
				mycmd = null;
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (mybinddisp.EndEdit())
			{
				bool isdirty = false;
				foreach (MarkingVM item in mycmd.Items.SourceCollection) isdirty = isdirty | item.DomainObject.IsDirty;
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

		private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = mycmd?.Delete.CanExecute(e.Parameter)??false;
			e.Handled = true;
		}
		private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			mycmd?.Delete.Execute(e.Parameter);
		}
		#region Filter
		private void BrandFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{
			if (mycmd.BrandFilter != null && !mycmd.BrandFilter.FilterOn) mycmd.BrandFilter?.FillAsync();
			Popup ppp = this.MainDataGrid.FindResource("BrandFilterPopup") as Popup;
			ppp.PlacementTarget = (UIElement)sender;
			ppp.IsOpen = true;
			e.Handled = true;
		}
		private void ColorFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{
			if (mycmd.ColorFilter != null && !mycmd.ColorFilter.FilterOn) mycmd.ColorFilter?.FillAsync();
			Popup ppp = this.MainDataGrid.FindResource("ColorFilterPopup") as Popup;
			ppp.PlacementTarget = (UIElement)sender;
			ppp.IsOpen = true;
			e.Handled = true;
		}
		private void CountryFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{
			if (mycmd.CountryFilter != null && !mycmd.CountryFilter.FilterOn) mycmd.CountryFilter?.FillAsync();
			Popup ppp = this.MainDataGrid.FindResource("CountryFilterPopup") as Popup;
			ppp.PlacementTarget = (UIElement)sender;
			ppp.IsOpen = true;
			e.Handled = true;
		}
		private void Ean13FilterPopup_Open(object sender, MouseButtonEventArgs e)
		{
			if (mycmd.Ean13Filter != null && !mycmd.Ean13Filter.FilterOn) mycmd.Ean13Filter?.FillAsync();
			Popup ppp = this.MainDataGrid.FindResource("Ean13FilterPopup") as Popup;
			ppp.PlacementTarget = (UIElement)sender;
			ppp.IsOpen = true;
			e.Handled = true;
		}
		private void FileNameFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{
			if (mycmd.FileNameFilter != null && !mycmd.FileNameFilter.FilterOn) mycmd.FileNameFilter?.FillAsync();
			Popup ppp = this.MainDataGrid.FindResource("FileNameFilterPopup") as Popup;
			ppp.PlacementTarget = (UIElement)sender;
			ppp.IsOpen = true;
			e.Handled = true;
		}
		private void GtinFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{
			if (mycmd.GtinFilter != null && !mycmd.GtinFilter.FilterOn) mycmd.GtinFilter?.FillAsync();
			Popup ppp = this.MainDataGrid.FindResource("GtinFilterPopup") as Popup;
			ppp.PlacementTarget = (UIElement)sender;
			ppp.IsOpen = true;
			e.Handled = true;
		}
		private void InnFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{
			if (mycmd.InnFilter != null && !mycmd.InnFilter.FilterOn) mycmd.InnFilter?.FillAsync();
			Popup ppp = this.MainDataGrid.FindResource("InnFilterPopup") as Popup;
			ppp.PlacementTarget = (UIElement)sender;
			ppp.IsOpen = true;
			e.Handled = true;
		}
		private void MaterialDownFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{
			if (mycmd.MaterialDownFilter != null && !mycmd.MaterialDownFilter.FilterOn) mycmd.MaterialDownFilter?.FillAsync();
			Popup ppp = this.MainDataGrid.FindResource("MaterialDownFilterPopup") as Popup;
			ppp.PlacementTarget = (UIElement)sender;
			ppp.IsOpen = true;
			e.Handled = true;
		}
		private void MaterialInFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{
			if (mycmd.MaterialInFilter != null && !mycmd.MaterialInFilter.FilterOn) mycmd.MaterialInFilter?.FillAsync();
			Popup ppp = this.MainDataGrid.FindResource("MaterialInFilterPopup") as Popup;
			ppp.PlacementTarget = (UIElement)sender;
			ppp.IsOpen = true;
			e.Handled = true;
		}
		private void MaterialUpFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{
			if (mycmd.MaterialUpFilter != null && !mycmd.MaterialUpFilter.FilterOn) mycmd.MaterialUpFilter?.FillAsync();
			Popup ppp = this.MainDataGrid.FindResource("MaterialUpFilterPopup") as Popup;
			ppp.PlacementTarget = (UIElement)sender;
			ppp.IsOpen = true;
			e.Handled = true;
		}
		private void ProductNameFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{
			if (mycmd.ProductNameFilter != null && !mycmd.ProductNameFilter.FilterOn) mycmd.ProductNameFilter?.FillAsync();
			Popup ppp = this.MainDataGrid.FindResource("ProductNameFilterPopup") as Popup;
			ppp.PlacementTarget = (UIElement)sender;
			ppp.IsOpen = true;
			e.Handled = true;
		}
		private void ProductTypeFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{
			if (mycmd.ProductTypeFilter != null && !mycmd.ProductTypeFilter.FilterOn) mycmd.ProductTypeFilter?.FillAsync();
			Popup ppp = this.MainDataGrid.FindResource("ProductTypeFilterPopup") as Popup;
			ppp.PlacementTarget = (UIElement)sender;
			ppp.IsOpen = true;
			e.Handled = true;
		}
		private void PublishedFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{
			Popup ppp = this.MainDataGrid.FindResource("PublishedFilterPopup") as Popup;
			ppp.PlacementTarget = (UIElement)sender;
			ppp.IsOpen = true;
			e.Handled = true;
		}
		private void SizeFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{
			if (mycmd.SizeFilter != null && !mycmd.SizeFilter.FilterOn) mycmd.SizeFilter?.FillAsync();
			Popup ppp = this.MainDataGrid.FindResource("SizeFilterPopup") as Popup;
			ppp.PlacementTarget = (UIElement)sender;
			ppp.IsOpen = true;
			e.Handled = true;
		}
		private void TnvedFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{
			if (mycmd.TnvedFilter != null && !mycmd.TnvedFilter.FilterOn) mycmd.TnvedFilter?.FillAsync();
			Popup ppp = this.MainDataGrid.FindResource("TnvedFilterPopup") as Popup;
			ppp.PlacementTarget = (UIElement)sender;
			ppp.IsOpen = true;
			e.Handled = true;
		}
		private void VendorCodeFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{
			if (mycmd.VendorCodeFilter != null && !mycmd.VendorCodeFilter.FilterOn) mycmd.VendorCodeFilter?.FillAsync();
			Popup ppp = this.MainDataGrid.FindResource("VendorCodeFilterPopup") as Popup;
			ppp.PlacementTarget = (UIElement)sender;
			ppp.IsOpen = true;
			e.Handled = true;
		}
		#endregion
	}
}
