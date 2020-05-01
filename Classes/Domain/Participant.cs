using KirillPolyanskiy.DataModelClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security;
using System.Windows;

namespace KirillPolyanskiy.HotelWpf.Classes
{
    public class Participant : DomainBaseReject
	{
		string myname,mylogin, myrole, mydescription;
        public string Name
        {
            set
            {
                if (!string.Equals(myname, value))
                {
                    string name = "Name";
                    //myhaserror=true; ErrorsChangedNotification(name);
                    if (!myUnchangedPropertyCollection.ContainsKey(myname))
                        this.myUnchangedPropertyCollection.Add(name, mylogin);
                    myname = value;
                    if (this.DomainState == DomainObjectState.Unchanged) this.DomainState = DomainObjectState.Modified;
                    PropertyChangedNotification(myname);
                }
            }
            get { return myname; }
        }
        internal string Login
		{
			set
			{
				if (!string.Equals(mylogin, value))
				{
					mylogin = value;
				}
			}
			get { return mylogin; }
		}
		public string Role
		{
			set
			{
				if (!string.Equals(myrole, value))
				{
					string name = "Role";
					//myhaserror=true; ErrorsChangedNotification(name);
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, myrole);
					myrole = value;
					if (this.DomainState == DomainObjectState.Unchanged) this.DomainState = DomainObjectState.Modified;
					PropertyChangedNotification(name);
				}
			}
			get { return myrole; }
		}
		public string Description
		{
			set
			{
				if (!string.Equals(mydescription, value))
				{
					string name = "Description";
					//myhaserror=true; ErrorsChangedNotification(name);
					if (!myUnchangedPropertyCollection.ContainsKey(name))
						this.myUnchangedPropertyCollection.Add(name, mydescription);
					mydescription = value;
					if (this.DomainState == DomainObjectState.Unchanged) this.DomainState = DomainObjectState.Modified;
					PropertyChangedNotification(name);
				}
			}
			get { return mydescription; }
		}

		public Participant() : this(string.Empty, string.Empty, string.Empty, string.Empty, DomainObjectState.Added) { }
		public Participant(string name,string login,string role,string description, DomainObjectState initstate) :base(0,initstate)
		{
			mylogin=login;
			myrole = role;
			mydescription = description;
            this.DomainState = initstate;
        }

		protected override void RejectProperty(string property, object value)
		{
			switch (property)
			{
				case "Name":
					this.myname = (string)value;
					break;
				case "Role":
					this.myrole = (string)value;
					break;
				case "Description":
					this.mydescription = (string)value;
					break;
			}
			return;
		}
        protected override void PropertiesUpdate(DomainBaseReject sample)
        {
            throw new NotImplementedException();
        }
    }

	internal class ParticipantListVM : ViewModelViewCommand
    {
		private ExceptionHandler myexhandler;
		private System.Collections.ObjectModel.ObservableCollection<Participant> myparticipants;
        private List<KeyValuePair<string, string>> myroles;
        
		internal ParticipantListVM():base()
        {
			myexhandler = new ExceptionHandler();
			myparticipants = new System.Collections.ObjectModel.ObservableCollection<Participant>();
            base.Collection = myparticipants;
            myroles = new List<KeyValuePair<string, string>>(); myroles.Add(new KeyValuePair<string, string>("Manager", "Администратор"));myroles.Add(new KeyValuePair<string, string>("Admin", "Старший администратор"));
            Fill();
		}

		public System.Windows.Data.ListCollectionView Participants
		{ get { return myview; } }
        public List<KeyValuePair<string,string>> Roles
        { get { return myroles; } }



		protected override void RefreshData(object parametr)
		{
			if (SaveDataChanges())
				using (this.myview.DeferRefresh())
				{
					this.myparticipants.Clear();
					this.Fill();
				}
		}
        protected override bool CanSaveDataChanges() { return true; }
		public override bool SaveDataChanges()
		{
			bool isSuccess;
            isSuccess = myendedit();
            if (isSuccess)
            {
                myexhandler.Title = "Сохранение изменений";
                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(CustomBrokerWpf.References.ConnectionString))
                {
                    SqlCommand cmdadd = new SqlCommand();
                    cmdadd.Connection = connection;
                    cmdadd.CommandType = System.Data.CommandType.StoredProcedure;
                    //cmdadd.CommandText = "dbo.GoodsAdd_sp";
                    //SqlCommand cmdupd = new SqlCommand();
                    //cmdupd.Connection = connection;
                    //cmdupd.CommandType = System.Data.CommandType.StoredProcedure;
                    //cmdupd.CommandText = "dbo.GoodsUpd_sp";
                    //SqlCommand cmddel = new SqlCommand();
                    //cmddel.Connection = connection;
                    //cmddel.CommandType = System.Data.CommandType.StoredProcedure;
                    //cmddel.CommandText = "dbo.GoodsDel_sp";

                    SqlParameter paroldlogin = new SqlParameter("@param0", System.Data.SqlDbType.NVarChar, 15);
                    SqlParameter parlogin = new SqlParameter("@param1", System.Data.SqlDbType.NVarChar, 15);
                    SqlParameter parrole = new SqlParameter("@param2", System.Data.SqlDbType.NVarChar, 15);
                    SqlParameter pardescription = new SqlParameter("@param3", System.Data.SqlDbType.NVarChar, 50);
                    SqlParameter[] pars = new SqlParameter[] { paroldlogin, parlogin, parrole, pardescription };
                    //cmdadd.Parameters.AddRange(pars);
                    //cmdupd.Parameters.AddRange(pars);
                    //cmddel.Parameters.AddRange(new SqlParameter[] { parlogin });

                    try
                    {
                        connection.Open();
                        foreach (Participant item in myparticipants)
                        {
                            if (item.DomainState != DomainObjectState.Unchanged)
                            {
                                cmdadd.Parameters.Clear();
                                paroldlogin.Value = item.HasPropertyOutdatedValue("Login")?item.GetPropertyOutdatedValue("Login"):item.Login;
                                parlogin.Value = item.Login;
                                parrole.Value = item.Role;
                                pardescription.Value = item.Description;
                                switch (item.DomainState)
                                {
                                    case DomainObjectState.Added:
                                        cmdadd.CommandText = "dbo.ParticipantAdd_sp";
                                        cmdadd.Parameters.Add(parlogin); cmdadd.Parameters.Add(parrole); cmdadd.Parameters.Add(pardescription);
                                        break;
                                    case DomainObjectState.Modified:
                                        cmdadd.CommandText = "dbo.ParticipantUpd_sp";
                                        cmdadd.Parameters.Add(paroldlogin); cmdadd.Parameters.Add(parlogin); cmdadd.Parameters.Add(parrole); cmdadd.Parameters.Add(pardescription);
                                        break;
                                    case DomainObjectState.Deleted:
                                        cmdadd.CommandText = "dbo.ParticipantDel_sp";
                                        cmdadd.Parameters.Add(paroldlogin);
                                        break;
                                }
                                cmdadd.ExecuteNonQuery();
                                item.AcceptChanches();
                            }
                        }
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    { myexhandler.Handle(ex); myexhandler.ShowMessage(); }
                    finally
                    { if (connection.State == System.Data.ConnectionState.Open) connection.Close(); }
                }
            }
			return isSuccess;
		}
		protected override bool CanRejectChanges() { return true; }
		protected override void RejectChanges(object parametr)
        {
			if (this.myview.IsAddingNew) this.myview.CancelNew();
			if (this.myview.CanCancelEdit) this.myview.CancelEdit();
            for(int i=myparticipants.Count-1;i>-1;i-- )
            {
                DomainBaseReject item = myparticipants[i];
                if (item.DomainState == DomainObjectState.Added)
                    this.myview.Remove(item);
                else
                    item.RejectChanges();
            }
			if (!(this.myview.IsEditingItem | this.myview.IsAddingNew)) this.myview.Refresh();
        }
		protected override void DeleteData(object parametr)
        {
            if (parametr != null && MessageBox.Show("Удалить пользователя?", "Удаление", MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
            {
                (parametr as Participant).DomainState = DomainObjectState.Deleted;
                if (this.myview.IsAddingNew) this.myview.CancelNew();
                if(this.myview.CanCancelEdit) this.myview.CancelEdit();
				if (!(this.myview.IsEditingItem | this.myview.IsAddingNew)) this.myview.Refresh();
            }
        }
		protected override bool CanDeleteData(object parametr) { return parametr is Participant; }

        private SqlDataReader GetReader(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM dbo.ParticipantList_vw";
            cmd.Connection = conn;
            conn.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection & CommandBehavior.SingleResult);
        }
        private void Fill()
        {
            using (SqlConnection conn = new SqlConnection(CustomBrokerWpf.References.ConnectionString))
            {
                SqlDataReader reader = GetReader(conn);
                while (reader.Read())
                {
                    myparticipants.Add(new Participant("",
                        reader.GetString(0)
                        ,reader.GetString(1)
                        , reader.IsDBNull(2) ? null : reader.GetString(2)
                        , DomainObjectState.Unchanged));
                }
                reader.Close();
            }
        }

        protected override void OtherViewRefresh()
        {
            throw new NotImplementedException();
        }

        protected override void SettingView()
        {
            myview.Filter = delegate (object item) { return (item as DomainBaseClass).DomainState != DomainObjectState.Deleted; };
        }

        protected override bool CanRefreshData()
        {
            return true;
        }

        protected override void AddData(object parametr)
        {
            throw new NotImplementedException();
        }

        protected override bool CanAddData(object parametr)
        {
            return false;
        }
    }

    public class LoginVM
    {
        bool myisnew;
        string mylogin;
        SecureString mypas, mynewpas, mynew2pas;
        internal LoginVM():this(string.Empty,true) { }
        internal LoginVM(string login,bool isnew)
        {
            mylogin = login;
            myisnew = isnew;
        }
        public bool IsNew
        {
            set
            {
                if (myisnew != value)
                {
                    myisnew = value;
                }
            }
            get { return myisnew; }
        }
        public string Login
        {
            set
            {
                if (!string.Equals(mylogin, value))
                {
                    mylogin = value;
                }
            }
            get { return mylogin; }
        }
        public SecureString Password
        {
            set
            {
                if (!SecureString.Equals(mypas, value))
                {
                    mypas = value;
                }
            }
        }
        public SecureString PasswordNew
        {
            set
            {
                if (!SecureString.Equals(mynewpas, value))
                {
                    mynewpas = value;
                }
            }
            get { return mynewpas; }
        }
        public SecureString PasswordNew2
        {
            set
            {
                if (!SecureString.Equals(mynew2pas, value))
                {
                    mynew2pas = value;
                }
            }
            get { return mynew2pas; }
        }
    }

}
