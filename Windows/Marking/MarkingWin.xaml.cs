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

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{

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

		private void BrandFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{

		}

		private void ColorFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{

		}

		private void CountryFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{

		}

		private void Ean13FilterPopup_Open(object sender, MouseButtonEventArgs e)
		{

		}

		private void FileNameFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{

		}

		private void GtinFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{

		}

		private void InnFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{

		}

		private void MaterialDownFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{

		}

		private void MaterialInFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{

		}

		private void MaterialUpFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{

		}

		private void ProductNameFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{

		}

		private void ProductTypeFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{

		}

		private void PublishedFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{

		}

		private void SizeFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{

		}

		private void VendorCodeFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{

		}

		private void TnvedFilterPopup_Open(object sender, MouseButtonEventArgs e)
		{

		}
	}
}
