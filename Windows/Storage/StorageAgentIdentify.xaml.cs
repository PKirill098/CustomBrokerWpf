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

namespace KirillPolyanskiy.CustomBrokerWpf
{
	/// <summary>
	/// Логика взаимодействия для StorageAgentIdentify.xaml
	/// </summary>
	public partial class StorageAgentIdentify : Window
	{
		public StorageAgentIdentify()
		{
			InitializeComponent();
		}
		public string AgentName { set; get; }
		public lib.ReferenceSimpleItem Agent { set; get; }
		public List<lib.ReferenceSimpleItem> Agents	{ set; get; }

		private void OK_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}
	}
}
