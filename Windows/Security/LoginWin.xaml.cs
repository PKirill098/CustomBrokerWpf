using KirillPolyanskiy.DataModelClassLibrary;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace KirillPolyanskiy.HotelWpf
{
    /// <summary>
    /// Логика взаимодействия для LoginWin.xaml
    /// </summary>
    public partial class LoginWin : Window
    {
        internal string myresult1;
        internal string Result1 { get { return myresult1; } }
        internal string myresult2;
        internal string Result2 { get { return myresult2; } }
        internal string myresult3;
        internal string Result3 { get { return myresult3; } }

        public LoginWin()
        {
            InitializeComponent();
            mylogins = new System.Collections.ObjectModel.ObservableCollection<Classes.LoginVM>();
            FillLogins();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text.Length==0)
            {
                MsgTextBlock.Text = "Не указан логин.";
                return;
            }
            if(passwordBox.SecurePassword.Length==0 & newpasswordBox.SecurePassword.Length == 0 & new2passwordBox.SecurePassword.Length == 0)
            {
                ChangeTB.IsChecked = true;
                newpasswordBox.Visibility = Visibility.Visible;
                new2passwordBox.Visibility = Visibility.Visible;
                newpasswordBlock.Visibility = Visibility.Visible;
                new2passwordBlock.Visibility = Visibility.Visible;
                return;
            }
            if(!string.Equals(newpasswordBox.Password,new2passwordBox.Password))
            {
                MsgTextBlock.Text = "Введенные значения нового пароля не совпадают.";
                return;
            }
            string mystr = CustomBrokerWpf.References.ConnectionString;
            using (SqlConnection conn = new SqlConnection(mystr))
            {
                ExceptionHandler myexhandler=new ExceptionHandler("Подключение");
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.PasswordCheck_sp";
                    cmd.Parameters.Add(new SqlParameter("@param1", textBox1.Text));
                    cmd.Parameters.Add(new SqlParameter("@param2", passwordBox.Password));
                    cmd.Parameters.Add(new SqlParameter("@param3", newpasswordBox.Password));
                    cmd.Parameters.Add(new SqlParameter("@param4", SqlDbType.NVarChar,15));
                    cmd.Parameters.Add(new SqlParameter("@param5", SqlDbType.NVarChar, 15));
                    cmd.Parameters[3].Direction = ParameterDirection.Output;
                    cmd.Parameters[4].Direction = ParameterDirection.Output;
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    if(cmd.Parameters[3].Value==DBNull.Value)
                    {
                        MsgTextBlock.Text = "Неверный логин или пароль.";
                    }
                    else
                    {
                        myresult1 = (string)cmd.Parameters[3].Value;
                        myresult2 = string.IsNullOrEmpty(newpasswordBox.Password) ? passwordBox.Password : newpasswordBox.Password;
                        myresult3 = textBox1.Text;
                        this.DialogResult = true;
                        this.Close();
                    }
                }
                catch (Exception ex)
                { myexhandler.Handle(ex); MsgTextBlock.Text = myexhandler.Message; }
            }
        }

        private void ChangeTB_Click(object sender, RoutedEventArgs e)
        {
            Visibility v;
            if (ChangeTB.IsChecked.Value) v = Visibility.Visible; else v = Visibility.Collapsed;
            newpasswordBox.Visibility = v;
            new2passwordBox.Visibility = v;
            newpasswordBlock.Visibility = v;
            new2passwordBlock.Visibility = v;
        }

        private System.Collections.ObjectModel.ObservableCollection<Classes.LoginVM> mylogins;
        public System.Collections.ObjectModel.ObservableCollection<Classes.LoginVM> Participants
        {
            get
            {
                return mylogins;
            }
        }
        private void FillLogins()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(CustomBrokerWpf.References.ConnectionString))
                {
                    SqlDataReader reader = GetReader(conn);
                    while (reader.Read())
                    {
                        mylogins.Add(new Classes.LoginVM(reader.GetString(0), reader.GetBoolean(1)));
                    }
                    reader.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private SqlDataReader GetReader(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM dbo.Logins_vw";
            cmd.Connection = conn;
            conn.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection & CommandBehavior.SingleResult);
        }
    }
}
