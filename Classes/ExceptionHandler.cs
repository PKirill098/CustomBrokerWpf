using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes
{
    internal class ExceptionHandler
    {
		internal ExceptionHandler() { }
		internal ExceptionHandler(string title) : this()
		{ mytitle = title; }

		private string mytitle, mymsg;

		internal string Title
		{
			set { mytitle = value; }
			get { return mytitle; }
		}
		internal string Message { get { return mymsg; } }

		internal void Handle(Exception ex)
		{
			if (ex is System.Data.SqlClient.SqlException)
			{
				System.Data.SqlClient.SqlException err = ex as System.Data.SqlClient.SqlException;
				if (err.Number > 49999) mymsg = err.Message;
				else
				{
					System.Text.StringBuilder errs = new System.Text.StringBuilder();
					foreach (System.Data.SqlClient.SqlError sqlerr in err.Errors)
					{
						errs.Append(sqlerr.Message + "\n");
					}
					mymsg = errs.ToString();
				}
			}
			else if (ex is System.Data.NoNullAllowedException)
			{
				mymsg = "Не все обязательные поля заполнены!\nЗаполните поля или удалите запись.";
			}
			else
			{
				mymsg = ex.Message + "\n" + ex.Source;
			}
		}
		internal void ShowMessage()
		{
			MessageBox.Show(mymsg, mytitle, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}
