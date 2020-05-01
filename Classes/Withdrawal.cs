using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public class Withdrawal:System.ComponentModel.INotifyPropertyChanged
    {
        private int _accountid;
        private decimal _sum, _sumjoin;
        private DateTime _operationdate;
        private string _recipient,_descr;
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        private void PropertyChangedNotification(string propertyName) 
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
        
        internal int ID {set;get;}
        internal int AccountEntryID
        {
            set
            {
                _accountid = value;
                PropertyChangedNotification("AccountEntryID");
            }
            get
            { return _accountid; }
        }
        internal string Recipient
        {
            set
            {
                _recipient = value;
                PropertyChangedNotification("Recipient");
            }
            get { return _recipient; }
        }
        internal decimal Sum
        {
            set
            {
                _sum=value;
                PropertyChangedNotification("Sum");
            }
            get { return _sum; }
        }
        internal decimal SumJoin
        {
            set 
            {
                _sumjoin = value;
                PropertyChangedNotification("SumJoin");
            }
            get { return _sumjoin; }
        }
        internal DateTime OperationDate {
            set
            {
                _operationdate=value;
                PropertyChangedNotification("OperationDate");
            }
            get { return _operationdate; }
        }
        internal string Description {
            set
            {
                _descr=value;
                PropertyChangedNotification("Description");
            }
            get { return _descr; }
        }
        //internal System.Collections.ObjectModel.ObservableCollection<Classes.Domain.Expenditure> Expenditure;

    }

}
