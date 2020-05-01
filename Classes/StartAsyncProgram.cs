using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System.Threading.Tasks;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes
{
    internal class StartAsyncProgram
    {
        private delegate void OneArgDelegate(GoodsDBM arg);

        internal async Task StartAsync()
        {
            await Task.Run(() =>
            {
                CertEndDateNotify();
            });
        }

        private void CertEndDateNotify()
        {
            GoodsDBM gdbm = new GoodsDBM
            {
                Ending = true
            };
            gdbm.Fill();
            if(gdbm.Collection.Count>0)
            {
                App.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle, new OneArgDelegate(OpenCertEndDateNotifyWindow), gdbm);
            }
        }
        private  void OpenCertEndDateNotifyWindow(GoodsDBM goodsdbm)
        {
            GoodsDBM gdbm = new GoodsDBM();
            gdbm.Ending = true;
            gdbm.Collection = new System.Collections.ObjectModel.ObservableCollection<Goods>();
            foreach (Goods item in goodsdbm.Collection) gdbm.Collection.Add(item);
            GoodsViewCommand cmd = new GoodsViewCommand(gdbm);
            GoodsEndingWin win = new GoodsEndingWin();
            win.DataContext = cmd;
            win.Show();
            (App.Current.MainWindow as MainWindow).ListChildWindow.Add(win);
        }
    }
}
