using System.Windows;
using System.Windows.Controls;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public partial class MailTemplateWin : Window
    {
        private lib.BindingDischarger mybinddisp;
        private Classes.Domain.MailTemplateCurrentCMD mycmd;
        public MailTemplateWin()
        {
            InitializeComponent();
            mybinddisp = new lib.BindingDischarger(this,new DataGrid[0]);
            mycmd = new Classes.Domain.MailTemplateCurrentCMD();
            mycmd.CancelEdit = mybinddisp.CancelEdit;
            mycmd.EndEdit = mybinddisp.EndEdit;
            mycmd.PropertyChanged += CMD_PropertyChanged;
            this.DataContext = mycmd;
        }

        private void CMD_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           //if(e.PropertyName== "CurrentItem")
           // {
           //     if (mycmd.CurrentItem != null)
           //         mainRTB.Document = mycmd.CurrentItem?.Document;
           //     else
           //         mainRTB.Document = new FlowDocument();
           // }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (mybinddisp.EndEdit())
            {
                bool isdirty = false;
                foreach (Classes.Domain.MailTemplate item in mycmd.Items.SourceCollection) isdirty = isdirty | item.IsDirty;
                if (isdirty)
                {
                    if (MessageBox.Show("Сохранить изменения?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                    {
                        if (!mycmd.SaveDataChanges())
                        {
                            this.Activate();
                            if (MessageBox.Show("\nИзменения не сохранены и будут потеряны при закрытии окна. \n Отменить закрытие окна?", "Закрытие окна", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                            {
                                e.Cancel = true;
                            }
                            else
                                mycmd.Reject.Execute(null);
                        }
                    }
                    else
                    {
                        mycmd.Reject.Execute(null);
                    }
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
                if (!e.Cancel) (App.Current.MainWindow as MainWindow).ListChildWindow.Remove(this);
                (App.Current.MainWindow as MainWindow).Activate();
            }
        }
    }
}
