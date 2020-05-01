using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class EventLog : DomainBaseClass
    {
        public EventLog(bool inform,string what,DateTime when,string who, string description,int? sourceid,string updatewho,DateTime? updatewhen,int id):base()
        {
            myid = id;
            myinform = inform;
            mywhat = what;
            mywhen = when;
            mywho = who;
            mydescr = description;
            mysourceid = sourceid;
            myupdatewhen = updatewhen;
            myupdatewho = updatewho;
            base.mystate = DomainObjectState.Unchanged;
        }
        private int myid;
        internal int Id { get { return myid; } }
        private string mywhat;
        public string What { get { return mywhat; } }
        private DateTime mywhen;
        public DateTime When { get { return mywhen; } }
        private string mywho;
        public string Who { get { return mywho; } }
        private string mydescr;
        public string Description { get { return mydescr; } }
        private int? mysourceid;
        internal int? SourceId { get { return mysourceid; } }
        private bool myinform;
        public bool Inform
        {
            set
            {
                if (!bool.Equals(myinform, value))
                {
                    string name = "Inform";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myinform);
                    myinform = value;
                    if (mystate == DomainObjectState.Unchanged) mystate = DomainObjectState.Modified;
                    PropertyChangedNotification(name);
                }
            }
            get { return myinform; }
        }

        protected override void RejectProperty(string property, object value)
        {
            this.Inform = (bool)value;
        }
    }

    public class EventLogVM
    {
        private ExceptionHandler myexhandler;
        private CustomBrokerWpf.SQLFilter myfilter;

        public EventLogVM() : base()
        {
            myexhandler = new ExceptionHandler();
            myendedit =()=>{return true; };
            mycanceledit = () => { return; };
            mysave = new RelayCommand(SaveExec, SaveCanExec);
            myrefresh = new RelayCommand(RefreshExec, RefreshCanExec);
            myreject = new RelayCommand(RejectExec, RejectCanExec);
            myhide = new RelayCommand(HideExec, HideCanExec);

            myeventlogs = new ObservableCollection<EventLog>();
            myfilter = new SQLFilter("eventlog", "AND");
            DataLoad();
        }

        private Func<bool> myendedit;
        internal Func<bool> EndEdit { set { myendedit = value; } }
        private Action mycanceledit;
        internal Action CancelEdit { set { mycanceledit = value; } }

        private ObservableCollection<EventLog> myeventlogs;
        public ObservableCollection<EventLog> EventLogs
        {
            get
            {
                return myeventlogs;
            }
        }

        internal CustomBrokerWpf.SQLFilter Filter { get { return myfilter; } }

        private RelayCommand myrefresh;
        public ICommand Refresh
        {
            get { return myrefresh; }
        }
        private void RefreshExec(object parametr)
        {
            if (SaveDataChanges() || MessageBox.Show("Изменения не были сохранены и будут потеряны при обновлении!\nОстановить обновление?", "Обновление данных", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                myeventlogs.Clear();
                DataLoad();
            }
        }
        private bool RefreshCanExec(object parametr)
        { return myendedit(); }

        private RelayCommand mysave;
        public ICommand Save { get { return mysave; } }
        private void SaveExec(object parametr)
        {
            SaveDataChanges();
        }
        private bool SaveCanExec(object parametr)
        {
            bool iscan = false;
            if(myendedit())
            foreach (EventLog log in myeventlogs)
            {
                if(log.DomainState!=DomainObjectState.Unchanged)
                {
                    iscan = true;
                    break;
                }
            }
            return iscan;
        }

        private RelayCommand myreject;
        public ICommand Reject { get { return myreject; } }
        private void RejectExec(object parametr)
        {
            mycanceledit();
            foreach (EventLog log in myeventlogs) log.RejectChanges();
        }
        private bool RejectCanExec(object parametr)
        {
            bool iscan = true;
            return iscan;
        }

		private RelayCommand myhide;
		public ICommand Hide { get { return myhide; } }
		private void HideExec(object parametr)
		{
			foreach (EventLog log in (parametr as System.Collections.IList)) log.Inform=true;
		}
		private bool HideCanExec(object parametr)
		{
			return parametr is System.Collections.IList && (parametr as System.Collections.IList).Count>0;
		}


		private void DataLoad()
        {
            myexhandler.Title = "Загрузка данных";
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection())
            {
                connection.ConnectionString = KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString;
                SqlCommand comd = new SqlCommand();
                comd.Connection = connection;
                comd.CommandType = System.Data.CommandType.StoredProcedure;
                comd.CommandText = "dbo.EventLog_sp";
                comd.Parameters.Add(new SqlParameter("@filterId", myfilter.FilterWhereId));
                try
                {
                    connection.Open();
                    SqlDataReader rdr = comd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    while (rdr.Read())
                    {
                        myeventlogs.Add(new EventLog(
                            rdr.GetBoolean(rdr.GetOrdinal("inform")),
                            rdr.GetString(rdr.GetOrdinal("what")),
                            rdr.GetDateTime(rdr.GetOrdinal("when")),
                            rdr.GetString(rdr.GetOrdinal("who")),
                            rdr.GetString(rdr.GetOrdinal("description")),
                            (rdr.IsDBNull(rdr.GetOrdinal("objectid")) ? (int?)null : rdr.GetInt32(rdr.GetOrdinal("objectid"))),
                            (rdr.IsDBNull(rdr.GetOrdinal("updateWho")) ? string.Empty : rdr.GetString(rdr.GetOrdinal("updateWho"))),
                            (rdr.IsDBNull(rdr.GetOrdinal("updateWhen")) ? (DateTime?)null : rdr.GetDateTime(rdr.GetOrdinal("updateWhen"))),
                            rdr.GetInt32(rdr.GetOrdinal("id"))
                           ));
                    }
                    rdr.Close();
                }
                catch (Exception ex)
                { myexhandler.Handle(ex); }
                finally
                { if(connection.State==System.Data.ConnectionState.Open) connection.Close(); }
            }
        }
        internal bool SaveDataChanges()
        {
            bool isSuccess = false;
            if (!myendedit()) return isSuccess;
            myexhandler.Title = "Сохранение изменений";
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection())
            {
                connection.ConnectionString = KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString;
                SqlCommand comd = new SqlCommand();
                comd.Connection = connection;
                comd.CommandType = System.Data.CommandType.StoredProcedure;
                comd.CommandText = "EventLogUpd_sp";
                SqlParameter parinf = new SqlParameter("@inform", System.Data.SqlDbType.Bit);
                SqlParameter parid = new SqlParameter("@id", System.Data.SqlDbType.Int);
                SqlParameter parupdwhen = new SqlParameter("@updatewhen", System.Data.SqlDbType.DateTime2);
                SqlParameter parupdwho = new SqlParameter("@updatewho", System.Data.SqlDbType.NChar,20);
                parupdwhen.Direction = System.Data.ParameterDirection.Output;
                parupdwho.Direction = System.Data.ParameterDirection.Output;
                comd.Parameters.Add(parinf); comd.Parameters.Add(parid); comd.Parameters.Add(parupdwhen); comd.Parameters.Add(parupdwho);
                try
                {
                    connection.Open();
                    foreach (EventLog log in myeventlogs)
                    {
                        if (log.DomainState != DomainObjectState.Unchanged)
                        {
                            parinf.Value = log.Inform;
                            parid.Value = log.Id;
                            comd.ExecuteNonQuery();
                            log.UpdateWhen = DBNull.Value != parupdwhen.Value? (DateTime)parupdwhen.Value:(DateTime?)null;
                            log.UpdateWho = DBNull.Value != parupdwho.Value? parupdwho.Value.ToString():null;
                            log.AcceptChanches();
                        }
                    }
                    isSuccess = true;
                }
                catch (Exception ex)
                { myexhandler.Handle(ex); }
                finally
                { if (connection.State == System.Data.ConnectionState.Open) connection.Close(); }
            }
            return isSuccess;
        }
    }
}
