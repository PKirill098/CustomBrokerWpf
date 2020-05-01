using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System.Windows;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Interaction logic for ProducerSelectWin.xaml
    /// </summary>
    public partial class ProducerSelectWin : Window
    {
        public ProducerSelectWin()
        {
            Producers = new System.Windows.Data.ListCollectionView(new System.Collections.ObjectModel.ObservableCollection<string>());
            InitializeComponent();
        }

        GetProducerDBM dbm;
        public int Client { set; get; }
        public string SelectProducer { set; get; }
        public System.Windows.Data.ListCollectionView Producers { set; get; }

        private void winProducerSelect_Loaded(object sender, RoutedEventArgs e)
        {
            GetProducerDBM dbm = new GetProducerDBM(Client);
            dbm.Fill();
            foreach (string item in dbm.Collection)
            {
                Producers.AddNewItem(item);
                Producers.CommitNew();
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
