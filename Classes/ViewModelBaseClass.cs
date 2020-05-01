using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes
{
	public abstract class ViewModelBaseItem: INotifyPropertyChanged, INotifyDataErrorInfo
	{
		//INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		protected void PropertyChangedNotification(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
		}
		private void DomainObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			PropertyChangedNotification(e.PropertyName);
            // для оповещения представления
        }

        //INotifyDataErrorInfo 
        protected Dictionary<string, List<string>> myerrormessages;
		public bool HasErrors { get { return myerrormessages.Count > 0; } }
		protected void AddErrorMessageForProperty(string propertyName, string errorMessage)
		{
			if (!myerrormessages.ContainsKey(propertyName))
			{
				myerrormessages.Add(propertyName, new List<string> { errorMessage });
				ErrorsChangedNotification(propertyName);
			}
			else if(!myerrormessages[propertyName].Contains(errorMessage))
			{
				myerrormessages[propertyName].Add(errorMessage);
				ErrorsChangedNotification(propertyName);
			}
		}
		protected void ClearErrorMessageForProperty(string propertyName)
		{
			if (myerrormessages.ContainsKey(propertyName))
			{
				myerrormessages.Remove(propertyName);
				ErrorsChangedNotification(propertyName);
                if(myerrormessages.Count==0) ErrorsChangedNotification(string.Empty);
            }
		}
		public IEnumerable GetErrors(string propertyName)
		{
			if (myerrormessages.ContainsKey(propertyName))
				return myerrormessages[propertyName];
			return new string[0];
		}
		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
		protected void ErrorsChangedNotification(string propertyName)
		{
			if (ErrorsChanged != null)
				ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
		}

		private DomainBaseClass mydomainobject;
		public DomainBaseClass DomainObject { get { return mydomainobject; } }

		public ViewModelBaseItem(DomainBaseClass domainobject):base()
		{
			mydomainobject = domainobject;
			mydomainobject.PropertyChanged += DomainObject_PropertyChanged;
            myerrormessages = new Dictionary<string, List<string>>();
            myUnchangedPropertyCollection = new Dictionary<string, object>();
        }

		public DateTime? UpdateWhen { get { return mydomainobject.UpdateWhen; } }
		public string UpdateWho { get { return mydomainobject.UpdateWho; } }

		protected Dictionary<string, object> myUnchangedPropertyCollection;

        internal void RejectChanges()
        {
            myerrormessages.Clear();
            foreach (string propertyname in myUnchangedPropertyCollection.Keys)
            {
                RejectProperty(propertyname, myUnchangedPropertyCollection[propertyname]);
                PropertyChangedNotification(propertyname);
                ErrorsChangedNotification(propertyname);
            }
            ErrorsChangedNotification(string.Empty);
        }
        protected abstract void RejectProperty(string property, object value);
    }

    public abstract class ViewModelBaseList
    {

        public ViewModelBaseList() : base()
        {
            myendedit = () => { return true; };
            mycanceledit = () => { return; };
            mysave = new RelayCommand(SaveExec, SaveCanExec);
            myrefresh = new RelayCommand(RefreshExec, RefreshCanExec);
            myreject = new RelayCommand(RejectExec, RejectCanExec);
            mydelete = new RelayCommand(DeleteExec, DeleteCanExec);
        }

		protected System.Windows.Data.ListCollectionView myview;

        protected List<INotifyDataErrorInfo> myErrorsCollection;
        protected void ItemErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            INotifyDataErrorInfo item = sender as INotifyDataErrorInfo;
            if ((item).HasErrors & !myErrorsCollection.Contains(item))
                myErrorsCollection.Add(item);
            else if (!item.HasErrors & myErrorsCollection.Contains(item))
                myErrorsCollection.Remove(item);
        }

        private Func<bool> myendedit; // вызов для окна закончить редактирование
        internal Func<bool> EndEdit { set { myendedit = value; } }
        private Action mycanceledit; // вызов для окна отменить изменения
        internal Action CancelEdit { set { mycanceledit = value; } }

        private RelayCommand myrefresh;
        public ICommand Refresh
        {
            get { return myrefresh; }
        }
        private void RefreshExec(object parametr)
        {
            if (SaveDataChanges() || MessageBox.Show("Изменения не были сохранены и будут потеряны при обновлении!\nОстановить обновление?", "Обновление данных", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                RefreshData();
            }
        }
        private bool RefreshCanExec(object parametr)
        { return myendedit(); }

        private RelayCommand mysave;
        public ICommand Save { get { return mysave; } }
        private void SaveExec(object parametr)
        {
            myendedit();
            SaveDataChanges();
        }
        private bool SaveCanExec(object parametr)
        {
            return CanSaveDataChanges();
        }

        private RelayCommand myreject;
        public ICommand Reject { get { return myreject; } }
        private void RejectExec(object parametr)
        {
            mycanceledit();
            RejectChanges();
        }
        private bool RejectCanExec(object parametr)
        {
            return CanRejectChanges();
        }

        private RelayCommand mydelete;
        public ICommand Delete { get { return mydelete; } }
        private void DeleteExec(object parametr)
        {
            mycanceledit();
            DeleteData(parametr);
        }
        private bool DeleteCanExec(object parametr)
        {
            return CanDeleteData();
        }

        internal abstract bool SaveDataChanges();
        protected abstract bool CanSaveDataChanges();
        protected abstract void RefreshData();
        protected abstract void RejectChanges();
        protected abstract bool CanRejectChanges();
        protected abstract void DeleteData(object parametr);
        protected abstract bool CanDeleteData();
    }
}
