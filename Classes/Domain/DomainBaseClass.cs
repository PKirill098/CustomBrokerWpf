using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
	public enum DomainObjectState { Unchanged, Added, Modified,Deleted }
	 
	public abstract class DomainBaseClass : INotifyPropertyChanged,INotifyDataErrorInfo
    {
		//INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		protected void PropertyChangedNotification(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
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
            else if (!myerrormessages[propertyName].Contains(errorMessage))
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
                if (myerrormessages.Count == 0) ErrorsChangedNotification(string.Empty);
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

        protected Dictionary<string, object> myUnchangedPropertyCollection;

		protected DomainObjectState mystate;
        internal  DomainObjectState DomainState
        { set { mystate = value;
                PropertyChangedNotification("DomainState");
            }
            get { return mystate; } }

        protected int? mystamp;
        internal int? Stamp
        {
            set { mystamp = value; }
            get { return mystamp; }
        }
        protected DateTime? myupdatewhen;
        public DateTime? UpdateWhen
        {
            set
            {
                if (!myupdatewhen.Equals(value))
                {
                    string name = "UpdateWhen";
                    myupdatewhen = value;
                    PropertyChangedNotification(name);
                }
            }
            get { return myupdatewhen; }
        }
        protected string myupdatewho;
        public string UpdateWho
        {
            set
            {
                if (!string.Equals(myupdatewho, value))
                {
                    string name = "UpdateWho";
                    myupdatewho = value;
                    PropertyChangedNotification(name);
                }
            }
            get { return myupdatewho; }
        }

		//public string Property
		//{
		//	set
		//	{
		//		if (!string.Equals(myproperty,value))
		//		{
		//			string name = "propertyName";
		//			//myhaserror=true; ErrorsChangedNotification(name);
		//			if (!myUnchangedPropertyCollection.ContainsKey(name))
		//				this.myUnchangedPropertyCollection.Add(name, myproperty);
		//			myproperty = value;
		//			if (mystate == DomainObjectState.Unchanged) mystate = DomainObjectState.Modified;
		//			PropertyChangedNotification(name);
		//		}
		//	}
		//	get { return myproperty; }
		//}

		internal DomainBaseClass()
        {
            mystate = DomainObjectState.Added;
            myUnchangedPropertyCollection = new Dictionary<string, object>();
			myerrormessages = new Dictionary<string, List<string>>();
		}

        internal void AcceptChanches()
        {
            myUnchangedPropertyCollection.Clear();
            mystate = DomainObjectState.Unchanged;
        }
        internal void RejectChanges()
        {
            foreach (string key in myUnchangedPropertyCollection.Keys)
            {
                RejectProperty(key, myUnchangedPropertyCollection[key]);
                PropertyChangedNotification(key);
            }
            AcceptChanches();
        }
        protected abstract void RejectProperty(string property, object value);

        internal bool HasPropertyOutdatedValue(string property)
        {
            return myUnchangedPropertyCollection.ContainsKey(property);
        }
        internal object GetPropertyOutdatedValue(string property)
        {
            object value = null;
            if (myUnchangedPropertyCollection.ContainsKey(property))
                value = myUnchangedPropertyCollection[property];
            return value;
        }
    }
}
