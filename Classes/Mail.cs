using MailKit.Net.Smtp;
using MailKit.Net.Imap;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using MailKit;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes
{
    internal class Mail
    {
        internal void Send(string to, string tomail, string subject, string body)
        {
            string mailbox, smtphost, imaphost, user,password;
#if DEBUG
            mailbox = "pk73@mail.ru";
            smtphost = "smtp.mail.ru";
            imaphost = "imap.mail.ru";
            user = "pk73@mail.ru";
            password = "1QAZxsw2";
#else
            mailbox = "order@art-delivery.ru";
            smtphost = "mail.nic.ru";
            imaphost = "mail.nic.ru";
            user="order@art-delivery.ru";
            password = "GHJiop67*";
#endif
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("АРТ ДЕЛИВЕРИ", mailbox));
            message.To.Add(new MailboxAddress(to, tomail));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(smtphost, 465, true); 
                // Note: only needed if the SMTP server requires authentication  
                client.Authenticate(user, password);
                client.Send(message);
                client.Disconnect(true);
            }
            using (var client = new ImapClient())
			{
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(imaphost, 993);
                client.Authenticate(user, password);
                IMailFolder folder = client.GetFolder(MailKit.SpecialFolder.Sent);
                folder.Append(message, MessageFlags.None);
            }
        }
    }

    internal class MailStateCustomer : lib.DomainBaseStamp
    {
        internal MailStateCustomer(int id, long stamp, lib.DomainObjectState mstate
            , int objectid, int customerid, int typeid, DateTime? updated
            ) : base(id, stamp, null, null, mstate)
        {
            mycustomerid = customerid;
            myobjectid = objectid;
            mytypeid = typeid;
            myupdated = updated;
        }

        private int mycustomerid;
        public int CustomerId
        { get { return mycustomerid; } }
        private int myobjectid;
        public int ObjectId
        {
            set { SetProperty<int>(ref myobjectid, value); }
            get { return myobjectid; }
        }
        private int mytypeid;
        public int TypeId
        {
            set { SetProperty<int>(ref mytypeid, value); }
            get { return mytypeid; }
        }
        private DateTime? myupdated;
        public DateTime? Updated
        {
            set { SetProperty<DateTime?>(ref myupdated, value); }
            get { return myupdated; }
        }

        protected override void RejectProperty(string property, object value)
        {
            throw new NotImplementedException();
        }
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            throw new NotImplementedException();
        }
    }

    internal class MailStateCustomerDBM : lib.DBManagerStamp<MailStateCustomer>
    {
        internal MailStateCustomerDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "dbo.MailState_sp";
            UpdateCommandText = "dbo.MailStateUpd_sp";

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@param1", System.Data.SqlDbType.Int),
                new SqlParameter("@param2", System.Data.SqlDbType.Int)
            };
            myupdateparams = new SqlParameter[]
            {
                myupdateparams[0]
                ,new SqlParameter("@objectid", System.Data.SqlDbType.Int)
                ,new SqlParameter("@customerid", System.Data.SqlDbType.Int)
                ,new SqlParameter("@typeid", System.Data.SqlDbType.Int)
                ,new SqlParameter("@updated", System.Data.SqlDbType.DateTime2)
                ,new SqlParameter("@updatedtrue", System.Data.SqlDbType.Bit)
            };
            myupdateparams[0].Direction = System.Data.ParameterDirection.InputOutput;
        }

        private lib.DomainBaseClass myobject;
        internal lib.DomainBaseClass DomainObject
        {
            set { myobject = value; }
            get { return myobject; }
        }
        private int mytype;
        internal int StateType
        {
            set { mytype = value; }
            get { return mytype; }
        }

        protected override MailStateCustomer CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new MailStateCustomer(reader.GetInt32(0), reader.GetInt64(1), lib.DomainObjectState.Unchanged
                , reader.GetInt32(reader.GetOrdinal("objectid")), reader.GetInt32(reader.GetOrdinal("customerid")), reader.GetInt32(4)
                , reader.IsDBNull(reader.GetOrdinal("updated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("updated")));
        }
        protected override void GetOutputSpecificParametersValue(MailStateCustomer item)
        {
        }
        protected override bool SaveChildObjects(MailStateCustomer item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(MailStateCustomer item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            SelectParams[0].Value = mytype;
            SelectParams[1].Value = myobject.Id;
        }
        protected override bool SetSpecificParametersValue(MailStateCustomer item)
        {
            myupdateparams[1].Value = myobject.Id;
            myupdateparams[2].Value = item.CustomerId;
            myupdateparams[3].Value = item.TypeId;
            myupdateparams[4].Value = item.Updated;
            myupdateparams[5].Value = item.HasPropertyOutdatedValue("Updated");
            if (myobject.Id <= 0)
                this.Errors.Add(new lib.DBMError(item, "Объект рассылки не сохранен в БД!", "0"));
            return myobject.Id > 0;
        }
        protected override void CancelLoad()
        { }
    }

    internal class MailCustomerDBM : lib.DBMSFill<KeyValuePair<int, string>>
    {
        internal MailCustomerDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;
            SelectProcedure = true;
            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@param1", System.Data.SqlDbType.Int)
            };
        }

        private lib.DomainBaseClass myobject;
        internal lib.DomainBaseClass DomainObject
        {
            set { myobject = value; }
            get { return myobject; }
        }

        protected override KeyValuePair<int, string> CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new KeyValuePair<int, string>(reader.GetInt32(0), reader.IsDBNull(1) ? null : reader.GetString(1));
        }
        protected override void PrepareFill(SqlConnection addcon)
        {
            SelectParams[0].Value = myobject.Id;
        }
        protected override void CancelLoad()
        { }
    }

    internal class MailState : lib.DomainBaseNotifyChanged
    {
        internal MailState(lib.DomainBaseClass model, MailStateCustomerDBM sdbm, MailCustomerDBM mdbm, int mailstateid) : base(0, lib.DomainObjectState.Sealed)
        {
            mysenderrors = new List<lib.DBMError>();
            mydbm = sdbm;
            mydbm.DomainObject = model;
            mydbm.StateType = mailstateid;
            task = Load();
            mymdbm = mdbm;
            mymdbm.DomainObject = model;
        }

        Task task;
        private byte mystate;
        public byte State
        {
            get { return mystate; }
        }
        internal System.Collections.ObjectModel.ObservableCollection<MailStateCustomer> CustomersState
        { get { return mydbm.Collection; } }
        private List<KeyValuePair<int, string>> mymails;
        internal List<KeyValuePair<int, string>> Mails
        { set { mymails = value; } }
        protected MailStateCustomerDBM mydbm;
        private MailCustomerDBM mymdbm;
        internal int MailStateId
        { set { mydbm.StateType = value; } get { return mydbm.StateType; } }

        internal void Update()
        {
            if (task.IsCompleted)
            {
                mydbm.Fill();
                UpdateState();
            }
        }
        private void UpdateState()
        {
            mystate = 255;
            foreach (MailStateCustomer item in mydbm.Collection)
            {
                if (mystate == 255)
                {
                    if (item.Updated.HasValue)
                        mystate = 2;
                    else
                        mystate = 0;
                }
                else if ((item.Updated.HasValue & mystate == 0) || (!item.Updated.HasValue & mystate == 2))
                    mystate = 1;
            }
            if (mystate == 255) mystate = 0;
            if (mystate == 0)
            {
                mymdbm.Fill();
                if (mymdbm.Errors.Count == 0)
                {
                    byte sent = 0;
                    foreach (MailStateCustomer item in mydbm.Collection)
                    {
                        sent = 0;
                        foreach (KeyValuePair<int, string> mail in mymdbm.Collection)
                        {
                            if (item.CustomerId == mail.Key)
                            {
                                sent = 1;
                                break;
                            }
                        }
                    }
                    if (sent == 0)
                        mystate = 1;
                }
            }
            this.PropertyChangedNotification("State");
        }
        private async Task Load()
        {
            mydbm.Errors.Clear();
            await mydbm.FillAsync();
            UpdateState();
        }
        internal void Send()
        {
            mysenderrors.Clear();
            mydbm.Errors.Clear();
            mydbm.Fill();
            if (mydbm.Errors.Count == 0)
            {
                mymdbm.Fill();
                if (mymdbm.Errors.Count == 0)
                {
                    Domain.MailTemplateDBM tdbm = new Domain.MailTemplateDBM();
                    tdbm.State = mydbm.StateType;
                    tdbm.Fill();
                    if (tdbm.Errors.Count == 0)
                    {
                        if (this.State == 2)
                            mysenderrors.Add(new lib.DBMError(this, "Все сообщения уже отправлены.", "0"));
                        else
                        {
                            if (tdbm.Collection.Count > 0)
                            {
                                Mail mailer = new Mail();
                                foreach (MailStateCustomer item in mydbm.Collection)
                                {
                                    if (!item.Updated.HasValue)
                                    {
                                        byte sent = 0;
                                        foreach (KeyValuePair<int, string> mail in mymdbm.Collection)
                                        {
                                            if (item.CustomerId == mail.Key)
                                            {
                                                sent = 1;
                                                foreach (Domain.MailTemplate temp in tdbm.Collection)
                                                {
                                                    string body = CreateBody(temp, item);
                                                    try
                                                    {
                                                        mailer.Send(string.Empty, mail.Value, temp.Subject, body);
                                                        sent = 2;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        mysenderrors.Add(new lib.DBMError(this, "Ошибка от почтового сервера: " + ex.Message, "1"));
                                                    }
                                                }
                                            }
                                            if (sent == 2)
                                                item.Updated = DateTime.Now;
                                            else if (sent == 0)
                                            {
                                                //ReferenceDS refds = App.Current.FindResource("keyReferenceDS") as ReferenceDS;
                                                //if (refds.tableCustomerName.Count == 0) refds.CustomerNameRefresh();
                                                mysenderrors.Add(new lib.DBMError(this, "Не найден адрес рассылки для " + CustomBrokerWpf.References.CustomerLegalStore.GetItemLoad(item.CustomerId,out _)?.Name ?? string.Empty, "1"));
                                            }
                                        }
                                    }
                                }
                            }
                            else
                                mysenderrors.Add(new lib.DBMError(this, "Не найден шаблон письма.", "1"));
                        }
                        mydbm.Errors.Clear();
                        mydbm.SaveCollectionChanches();
                        if (mydbm.Errors.Count > 0)
                            foreach (lib.DBMError err in mydbm.Errors) mysenderrors.Add(err);
                        else
                            this.UpdateState();
                        this.PropertyChangedNotification("SendErrors");
                    }
                    else
                        foreach (lib.DBMError err in tdbm.Errors) mysenderrors.Add(err);
                }
                else
                    foreach (lib.DBMError err in mymdbm.Errors) mysenderrors.Add(err);
            }
            else
                foreach (lib.DBMError err in mydbm.Errors) mysenderrors.Add(err);
            if (mysenderrors.Count == 0)
                mysenderrors.Add(new lib.DBMError(this, "Все сообщения отправлены.", "0"));
        }
        private List<lib.DBMError> mysenderrors;
        internal List<lib.DBMError> SendErrors
        { get { return mysenderrors; } }
        internal virtual string CreateBody(Domain.MailTemplate temp, MailStateCustomer item)
        {
            return temp.Body;
        }
    }

}
