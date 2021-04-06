using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using KirillPolyanskiy.DataModelClassLibrary;
using System;
using System.Threading.Tasks;
using System.Windows;
using lib = KirillPolyanskiy.DataModelClassLibrary;

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

    interface IWarningAsyncItem
    {
        void Run();
    }

    internal class WarningAsync
    {
        private IWarningAsyncItem myitem;

        internal WarningAsync(IWarningAsyncItem item)
        {
            myitem = item;
        }

        internal async Task StartAsync()
        {
            await Task.Run(() =>
            {
                myitem.Run();
            });
        }
    }

    internal abstract class WarningAsyncItem<TWindow,T> : IWarningAsyncItem
        where TWindow : Window, new()
    {
        private lib.IDBMFill<T> mydbm;

        internal WarningAsyncItem(lib.IDBMFill<T> dbm)
        {
            mydbm = dbm;
        }

        public void Run()
        {
            mydbm.Fill();
            if (mydbm.Collection.Count > 0)
            {
                App.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle, new Action(OpenWindow));
            }
        }
        private void OpenWindow()
        {
            TWindow win = new TWindow();
            win.DataContext = GetCMD(mydbm);
            win.Show();
            (App.Current.MainWindow as lib.Interfaces.IMainWindow).ListChildWindow.Add(win);
        }
        internal abstract lib.ViewModelViewCommand GetCMD(lib.IDBMFill<T> dbm);
    }

    internal class WarningAsyncItemGoods: WarningAsyncItem<GoodsEndingWin, Goods>
    {
        internal WarningAsyncItemGoods():base(new GoodsDBM { Ending = true }) { }

        internal override lib.ViewModelViewCommand GetCMD(IDBMFill<Goods> dbm)
        {
            GoodsDBM gdbm = new GoodsDBM();
            gdbm.Ending = true;
            gdbm.Collection = new System.Collections.ObjectModel.ObservableCollection<Goods>();
            foreach (Goods item in dbm.Collection) gdbm.Collection.Add(item);
            return new GoodsViewCommand(gdbm);
        }
    }

    internal class WarningAsyncItemPrepay : WarningAsyncItem<Windows.Account.PrepayExpireWin, Domain.Account.PrepayCustomerRequest>
    {
        internal WarningAsyncItemPrepay() : base(new Domain.Account.PrepayCustomerRequestDBM { SelectCommandText = "account.SPDDateExpire_sp", SelectParams = new System.Data.SqlClient.SqlParameter[0] }) { }
        internal override lib.ViewModelViewCommand GetCMD(IDBMFill<Domain.Account.PrepayCustomerRequest> dbm)
        {
            Domain.Account.PrepayExpire cmd = new Domain.Account.PrepayExpire();
            foreach (Domain.Account.PrepayCustomerRequest item in dbm.Collection) cmd.DomainCollection.Add(item);
            return cmd;
        }
    }
}
