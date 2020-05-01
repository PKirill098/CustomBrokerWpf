using System;
using System.Windows;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception is Exception) e.Handled = true;
            MessageBox.Show(e.Exception.Message+"\n"+(e.Exception.Source??string.Empty) + "\n"+(e.Exception.InnerException?.Message??string.Empty) , "Необработанное исключение");
        }

        private void ListBoxCheckBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ListBox lb = sender as System.Windows.Controls.ListBox;
            if (!lb.IsKeyboardFocusWithin) lb.Focus();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            References.Init();
        }
    }
}
