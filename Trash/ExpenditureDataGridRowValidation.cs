using System.Data;
using System.Windows.Controls;
using System.Windows.Data;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    class DataGridRowValidation:ValidationRule
    {
        public override ValidationResult Validate(object value,System.Globalization.CultureInfo cultureInfo)
        {
            string errmsg;
            ExpenditureDS.tableExpenditureRow row;
            foreach(DataRowView rowview in (value as BindingGroup).Items)
            {
                row = rowview.Row as ExpenditureDS.tableExpenditureRow;
                errmsg = row.CheckRow();
                if (errmsg.Length>0) return new ValidationResult(false, errmsg);
            }
            return ValidationResult.ValidResult;
        }
    }
}
