using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    class RequestStoagePointValidationRule : ValidationRule
    {
        int requestid=0;
        internal int RequestId
        { set { requestid = value; } get { return requestid; } }
        DataGrid currDataGrid;
        internal DataGrid CurrentDataGrid
        {
            set { currDataGrid = value; }
            get { return currDataGrid; }
        }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int n = CheckStoragePoint((string)value);
            if (n > 0)
            {
                if (requestid > 0)
                {
                    if (requestid == n) n = 0;
                }
                if (this.currDataGrid!=null && (this.currDataGrid.CurrentItem is Classes.Domain.RequestVM))
                {
                   if ((this.currDataGrid.CurrentItem as Classes.Domain.RequestVM).Id == n) n = 0;
                }
                if (n > 0) return new ValidationResult(false, "Эта позиция уже используется заявкой № " + n.ToString() + " !");
            }
            else if(n<0)
                return new ValidationResult(false, "Позиция содержит недопустимые символы!");
            return new ValidationResult(true, null);
        }
        private int CheckStoragePoint(string point)
        {
            if (string.IsNullOrEmpty(point)) return 0;
            if (string.IsNullOrWhiteSpace(point)) return -1;
            using (SqlConnection con = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
            {
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "dbo.RequestCheckStoragePoint";
                SqlParameter p = new SqlParameter();
                p.ParameterName = "@storagePoint";
                p.SqlDbType = SqlDbType.NVarChar;
                p.Size = 6;
                p.Value = point;
                SqlParameter id = new SqlParameter();
                id.Direction = ParameterDirection.Output;
                id.ParameterName = "@requestId";
                id.SqlDbType = SqlDbType.Int;
                id.Value = 0;
                com.Parameters.Add(p);
                com.Parameters.Add(id);
                try
                {
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();
                }
                catch
                {
                    con.Close();
                }
                return (int)(id.Value??0);
            }
        }
    }
}
