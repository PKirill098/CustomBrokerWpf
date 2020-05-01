using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    internal class ParcelCustomerMailState : lib.DomainBaseStamp
    {
        internal ParcelCustomerMailState(int id, long stamp, lib.DomainObjectState mstate
            , int customerid, DateTime? shipdate, DateTime? prepared, DateTime? crossedborder, DateTime? terminalin, DateTime? terminalout, DateTime? unloaded
            ) : base(id, stamp, null, null, mstate)
        {
            mycustomerid = customerid;
            myshipdate = shipdate;
            myprepared = prepared;
            mycrossedborder = crossedborder;
            myterminalin = terminalin;
            myterminalout = terminalout;
            myunloaded = unloaded;
        }

        private int mycustomerid;
        public int CustomerId
        { get { return mycustomerid; } }
        private DateTime? myshipdate;
        public DateTime? ShipDate
        {
            set { SetProperty<DateTime?>(ref myshipdate, value); }
            get { return myshipdate; }
        }
        private DateTime? myprepared;
        public DateTime? Prepared
        {
            set { SetProperty<DateTime?>(ref myprepared, value); }
            get { return myprepared; }
        }
        private DateTime? mycrossedborder;
        public DateTime? CrossedBorder
        {
            set { SetProperty<DateTime?>(ref mycrossedborder, value); }
            get { return mycrossedborder; }
        }
        private DateTime? myterminalin;
        public DateTime? TerminalIn
        {
            set { SetProperty<DateTime?>(ref myterminalin, value); }
            get { return myterminalin; }
        }
        private DateTime? myterminalout;
        public DateTime? TerminalOut
        {
            set { SetProperty<DateTime?>(ref myterminalout, value); }
            get { return myterminalout; }
        }
        private DateTime? myunloaded;
        public DateTime? UnLoaded
        {
            set { SetProperty<DateTime?>(ref myunloaded, value); }
            get { return myunloaded; }
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

    internal class ParcelCustomerMailStateDBM : lib.DBManagerStamp<ParcelCustomerMailState>
    {
        internal ParcelCustomerMailStateDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectCommandText = "dbo.ParcelMailState_sp";
            UpdateCommandText = "dbo.ParcelMailStateUpd_sp";

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@param1", System.Data.SqlDbType.Int),
            };
            myupdateparams = new SqlParameter[]
            {
                myupdateparams[0]
                ,new SqlParameter("@parcelid", System.Data.SqlDbType.Int)
                ,new SqlParameter("@customerid", System.Data.SqlDbType.Int)
                ,new SqlParameter("@shipdateupdated", System.Data.SqlDbType.DateTime2)
                ,new SqlParameter("@shipdatetrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@preparedupdated", System.Data.SqlDbType.DateTime2)
                ,new SqlParameter("@preparedtrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@crossedborderupdated", System.Data.SqlDbType.DateTime2)
                ,new SqlParameter("@crossedbordertrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@terminalinupdated", System.Data.SqlDbType.DateTime2)
                ,new SqlParameter("@terminalintrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@terminaloutupdated", System.Data.SqlDbType.DateTime2)
                ,new SqlParameter("@terminalouttrue", System.Data.SqlDbType.Bit)
                ,new SqlParameter("@unloadedupdated", System.Data.SqlDbType.DateTime2)
                ,new SqlParameter("@unloadedtrue", System.Data.SqlDbType.Bit)
            };
            myupdateparams[0].Direction = System.Data.ParameterDirection.InputOutput;
        }

        private Parcel myparcel;
        internal Parcel Parcel
        {
            set { myparcel = value; }
            get { return myparcel; }
        }

        protected override ParcelCustomerMailState CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new ParcelCustomerMailState(reader.GetInt32(0), reader.GetInt64(1), lib.DomainObjectState.Unchanged
                , reader.IsDBNull(3) ? 0 : reader.GetInt32(3)
                , reader.IsDBNull(reader.GetOrdinal("shipdateupdated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("shipdateupdated"))
                , reader.IsDBNull(reader.GetOrdinal("preparedupdated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("preparedupdated"))
                , reader.IsDBNull(reader.GetOrdinal("crossedborderupdated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("crossedborderupdated"))
                , reader.IsDBNull(reader.GetOrdinal("terminalinupdated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("terminalinupdated"))
                , reader.IsDBNull(reader.GetOrdinal("terminaloutupdated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("terminaloutupdated"))
                , reader.IsDBNull(reader.GetOrdinal("unloadedupdated")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("unloadedupdated")));
        }
        protected override void GetOutputSpecificParametersValue(ParcelCustomerMailState item)
        {
        }
        protected override bool SaveChildObjects(ParcelCustomerMailState item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(ParcelCustomerMailState item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override void SetSelectParametersValue()
        {
            SelectParams[0].Value = myparcel.Id;
        }
        protected override bool SetSpecificParametersValue(ParcelCustomerMailState item)
        {
            myupdateparams[1].Value = myparcel.Id;
            myupdateparams[2].Value = item.CustomerId;
            myupdateparams[3].Value = item.ShipDate;
            myupdateparams[4].Value = item.HasPropertyOutdatedValue("ShipDate");
            myupdateparams[5].Value = item.Prepared;
            myupdateparams[6].Value = item.HasPropertyOutdatedValue("Prepared");
            myupdateparams[7].Value = item.CrossedBorder;
            myupdateparams[8].Value = item.HasPropertyOutdatedValue("CrossedBorder");
            myupdateparams[9].Value = item.TerminalIn;
            myupdateparams[10].Value = item.HasPropertyOutdatedValue("TerminalIn");
            myupdateparams[11].Value = item.TerminalOut;
            myupdateparams[12].Value = item.HasPropertyOutdatedValue("TerminalOut");
            myupdateparams[13].Value = item.UnLoaded;
            myupdateparams[14].Value = item.HasPropertyOutdatedValue("UnLoaded");
            if (myparcel.Id <= 0)
                this.Errors.Add(new lib.DBMError(item, "Загруска не сохранена в БД!", "0"));
            return myparcel.Id > 0;
        }
        protected override void LoadObjects(ParcelCustomerMailState item)
        {
        }
        protected override bool LoadObjects()
        { return true; }
    }

    internal class ParcelCustomerMailDBM : MailCustomerDBM
    {
        internal ParcelCustomerMailDBM()
        {
            SelectCommandText = "dbo.ParcelMail_sp";
        }

        private Parcel myparcel;
        internal Parcel Parcel
        {
            set { myparcel = value; }
            get { return myparcel; }
        }

        protected override KeyValuePair<int, string> CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            return new KeyValuePair<int, string>(reader.GetInt32(0), reader.IsDBNull(1) ? null : reader.GetString(1));
        }
        protected override void SetParametersValue()
        {
            SelectParams[0].Value = myparcel.Id;
        }
    }

    public class ParcelMailState : lib.DomainBaseNotifyChanged
    {
        public ParcelMailState(Parcel parcel) : base(0, lib.DomainObjectState.Sealed)
        {
            mysenderrors = new List<lib.DBMError>();
            mydbm = new ParcelCustomerMailStateDBM();
            mydbm.Parcel = parcel;
            Task task = Load();
        }

        private byte myshipdate;
        public byte ShipDate
        {
            get { return myshipdate; }
        }
        private byte myprepared;
        public byte Prepared
        {
            get { return myprepared; }
        }
        private byte mycrossedborder;
        public byte CrossedBorder
        {
            get { return mycrossedborder; }
        }
        private byte myterminalin;
        public byte TerminalIn
        {
            get { return myterminalin; }
        }
        private byte myterminalout;
        public byte TerminalOut
        {
            get { return myterminalout; }
        }
        private byte myunloaded;
        public byte UnLoaded
        {
            get { return myunloaded; }
        }

        private ParcelCustomerMailStateDBM mydbm;

        private void UpdateState()
        {
            myshipdate = 255; myprepared = 255; mycrossedborder = 255; myterminalin = 255; myterminalout = 255; myunloaded = 255;
            foreach (ParcelCustomerMailState item in mydbm.Collection)
            {
                if (myshipdate == 255)
                {
                    if (item.ShipDate.HasValue)
                        myshipdate = 2;
                    else
                        myshipdate = 0;
                }
                else if ((item.ShipDate.HasValue & myshipdate == 0) || (!item.ShipDate.HasValue & myshipdate == 2))
                    myshipdate = 1;
                if (myprepared == 255)
                {
                    if (item.Prepared.HasValue)
                        myprepared = 2;
                    else
                        myprepared = 0;
                }
                else if ((item.Prepared.HasValue & myprepared == 0) || (!item.Prepared.HasValue & myprepared == 2))
                    myprepared = 1;
                if (mycrossedborder == 255)
                {
                    if (item.CrossedBorder.HasValue)
                        mycrossedborder = 2;
                    else
                        mycrossedborder = 0;
                }
                else if ((item.CrossedBorder.HasValue & mycrossedborder == 0) || (!item.CrossedBorder.HasValue & mycrossedborder == 2))
                    mycrossedborder = 1;
                if (myterminalin == 255)
                {
                    if (item.TerminalIn.HasValue)
                        myterminalin = 2;
                    else
                        myterminalin = 0;
                }
                else if ((item.TerminalIn.HasValue & myterminalin == 0) || (!item.TerminalIn.HasValue & myterminalin == 2))
                    myterminalin = 1;
                if (myterminalout == 255)
                {
                    if (item.TerminalOut.HasValue)
                        myterminalout = 2;
                    else
                        myterminalout = 0;
                }
                else if ((item.TerminalOut.HasValue & myterminalout == 0) || (!item.TerminalOut.HasValue & myterminalout == 2))
                    myterminalout = 1;
                if (myunloaded == 255)
                {
                    if (item.UnLoaded.HasValue)
                        myunloaded = 2;
                    else
                        myunloaded = 0;
                }
                else if ((item.UnLoaded.HasValue & myunloaded == 0) || (!item.UnLoaded.HasValue & myunloaded == 2))
                    myunloaded = 1;
            }
            if (myshipdate == 255) myshipdate = 0;
            if (myprepared == 255) myprepared = 0;
            if (mycrossedborder == 255) mycrossedborder = 0;
            if (myterminalin == 255) myterminalin = 0;
            if (myterminalout == 255) myterminalout = 0;
            if (myunloaded == 255) myunloaded = 0;
            this.PropertyChangedNotification("ShipDate");
            this.PropertyChangedNotification("Prepared");
            this.PropertyChangedNotification("CrossedBorder");
            this.PropertyChangedNotification("TerminalIn");
            this.PropertyChangedNotification("TerminalOut");
            this.PropertyChangedNotification("UnLoaded");
        }
        private async Task Load()
        {
            mydbm.Errors.Clear();
            await mydbm.FillAsync();
            UpdateState();
        }
        internal void Send(int mtype)
        {
            mysenderrors.Clear();
            mydbm.Errors.Clear();
            mydbm.Fill();
            if (mydbm.Errors.Count == 0)
            {
                ParcelCustomerMailDBM mdbm = new ParcelCustomerMailDBM();
                mdbm.Parcel = mydbm.Parcel;
                mdbm.Fill();
                if (mdbm.Errors.Count == 0)
                {
                    MailTemplateDBM tdbm = new MailTemplateDBM();
                    tdbm.Fill();
                    if (tdbm.Errors.Count == 0)
                    {
                        MailTemplate temp = null;
                        foreach (MailTemplate item in tdbm.Collection)
                            if (item.State == mtype)
                            { temp = item; break; }
                        if (temp != null)
                        {
                            if (!string.IsNullOrEmpty(temp.Body))
                            {
                                Mail mailer = new Mail();
                                switch (mtype)
                                {
                                    case 60:
                                        if (this.ShipDate == 2)
                                            mysenderrors.Add(new lib.DBMError(this, "Все сообщения уже отправлены.", "0"));
                                        else
                                        {
                                            foreach (ParcelCustomerMailState item in mydbm.Collection)
                                            {
                                                if (!item.ShipDate.HasValue)
                                                {
                                                    byte sent = 0;
                                                    foreach (KeyValuePair<int, string> mail in mdbm.Collection)
                                                    {
                                                        if (item.CustomerId == mail.Key)
                                                        {
                                                            sent = 1;
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
                                                        item.ShipDate = DateTime.Now;
                                                    else if (sent == 0)
                                                    {
                                                        mysenderrors.Add(new lib.DBMError(this, "Не найден адрес рассылки для " + (CustomBrokerWpf.References.CustomerLegalStore.GetItemLoad(item.CustomerId)?.Name ?? string.Empty), "1"));
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case 70:
                                        if (this.Prepared == 2)
                                            mysenderrors.Add(new lib.DBMError(this, "Все сообщения уже отправлены.", "0"));
                                        else
                                        {
                                            foreach (ParcelCustomerMailState item in mydbm.Collection)
                                            {
                                                if (!item.Prepared.HasValue)
                                                {
                                                    byte sent = 0;
                                                    foreach (KeyValuePair<int, string> mail in mdbm.Collection)
                                                    {
                                                        if (item.CustomerId == mail.Key)
                                                        {
                                                            sent = 1;
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
                                                        item.Prepared = DateTime.Now;
                                                    else if (sent == 0)
                                                    {
                                                        mysenderrors.Add(new lib.DBMError(this, "Не найден адрес рассылки для " + (CustomBrokerWpf.References.CustomerLegalStore.GetItemLoad(item.CustomerId)?.Name ?? string.Empty), "1"));
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case 80:
                                        if (this.CrossedBorder == 2)
                                            mysenderrors.Add(new lib.DBMError(this, "Все сообщения уже отправлены.", "0"));
                                        else
                                        {
                                            foreach (ParcelCustomerMailState item in mydbm.Collection)
                                            {
                                                if (!item.CrossedBorder.HasValue)
                                                {
                                                    byte sent = 0;
                                                    foreach (KeyValuePair<int, string> mail in mdbm.Collection)
                                                    {
                                                        if (item.CustomerId == mail.Key)
                                                        {
                                                            sent = 1;
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
                                                        item.CrossedBorder = DateTime.Now;
                                                    else if (sent == 0)
                                                    {
                                                        mysenderrors.Add(new lib.DBMError(this, "Не найден адрес рассылки для " + (CustomBrokerWpf.References.CustomerLegalStore.GetItemLoad(item.CustomerId)?.Name ?? string.Empty), "1"));
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case 90:
                                        if (this.TerminalIn == 2)
                                            mysenderrors.Add(new lib.DBMError(this, "Все сообщения уже отправлены.", "0"));
                                        else
                                        {
                                            foreach (ParcelCustomerMailState item in mydbm.Collection)
                                            {
                                                if (!item.TerminalIn.HasValue)
                                                {
                                                    byte sent = 0;
                                                    foreach (KeyValuePair<int, string> mail in mdbm.Collection)
                                                    {
                                                        if (item.CustomerId == mail.Key)
                                                        {
                                                            sent = 1;
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
                                                        item.TerminalIn = DateTime.Now;
                                                    else if (sent == 0)
                                                    {
                                                        mysenderrors.Add(new lib.DBMError(this, "Не найден адрес рассылки для " + (CustomBrokerWpf.References.CustomerLegalStore.GetItemLoad(item.CustomerId)?.Name ?? string.Empty), "1"));
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case 100:
                                        if (this.TerminalOut == 2)
                                            mysenderrors.Add(new lib.DBMError(this, "Все сообщения уже отправлены.", "0"));
                                        else
                                        {
                                            foreach (ParcelCustomerMailState item in mydbm.Collection)
                                            {
                                                if (!item.TerminalOut.HasValue)
                                                {
                                                    byte sent = 0;
                                                    foreach (KeyValuePair<int, string> mail in mdbm.Collection)
                                                    {
                                                        if (item.CustomerId == mail.Key)
                                                        {
                                                            sent = 1;
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
                                                        item.TerminalOut = DateTime.Now;
                                                    else if (sent == 0)
                                                    {
                                                        mysenderrors.Add(new lib.DBMError(this, "Не найден адрес рассылки для " + (CustomBrokerWpf.References.CustomerLegalStore.GetItemLoad(item.CustomerId)?.Name ?? string.Empty), "1"));
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case 110:
                                        if (this.UnLoaded == 2)
                                            mysenderrors.Add(new lib.DBMError(this, "Все сообщения уже отправлены.", "0"));
                                        else
                                        {
                                            foreach (ParcelCustomerMailState item in mydbm.Collection)
                                            {
                                                if (!item.UnLoaded.HasValue)
                                                {
                                                    byte sent = 0;
                                                    foreach (KeyValuePair<int, string> mail in mdbm.Collection)
                                                    {
                                                        if (item.CustomerId == mail.Key)
                                                        {
                                                            sent = 1;
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
                                                        item.UnLoaded = DateTime.Now;
                                                    else if (sent == 0)
                                                    {
                                                        mysenderrors.Add(new lib.DBMError(this, "Не найден адрес рассылки для " + (CustomBrokerWpf.References.CustomerLegalStore.GetItemLoad(item.CustomerId)?.Name ?? string.Empty), "1"));
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    default:
                                        mysenderrors.Add(new lib.DBMError(this, "Тип сообщения не определен!", "1"));
                                        break;
                                }
                            }
                            else
                                mysenderrors.Add(new lib.DBMError(this, "Шаблон письма пуст.", "1"));
                        }
                        else
                            mysenderrors.Add(new lib.DBMError(this, "Не найден шаблон письма.", "1"));
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
                    foreach (lib.DBMError err in mdbm.Errors) mysenderrors.Add(err);
            }
            else
                foreach (lib.DBMError err in mydbm.Errors) mysenderrors.Add(err);
            if (mysenderrors.Count == 0)
                mysenderrors.Add(new lib.DBMError(this, "Все сообщения отправлены.", "0"));
        }

        private List<lib.DBMError> mysenderrors;
        internal List<lib.DBMError> SendErrors
        { get { return mysenderrors; } }
        internal virtual string CreateBody(MailTemplate temp, ParcelCustomerMailState item)
        {
            string body = temp.Body;
            if (body.IndexOf('{') > -1)
            {
                short? cellnumber = 0; decimal weight = 0M, volume = 0M;
                bool peragent = body.IndexOf("{Поставщик") > -1;
                List<IntStringDecimal> agents = new List<IntStringDecimal>();
                foreach (Request req in mydbm.Parcel.Requests)
                    if (req.ParcelId.HasValue && req.DomainState < lib.DomainObjectState.Deleted)
                    {
                        bool isis = false;
                        foreach (RequestCustomerLegal lgl in req.CustomerLegals)
                            if (lgl.CustomerLegal.Id == item.CustomerId)
                            { isis = true; break; }
                        if (isis)
                        {
                            cellnumber += req.CellNumber ?? 0;
                            weight += req.OfficialWeight ?? 0M;
                            volume += req.Volume ?? 0M;
                            if (peragent)
                            {
                                bool isnew = true;
                                foreach (IntStringDecimal agent in agents)
                                    if (agent.IntValue == (req.AgentId ?? 0))
                                    {
                                        agent.Weight += req.OfficialWeight ?? 0M;
                                        agent.Volume += req.Volume ?? 0M;
                                        isnew = false;
                                        break;
                                    }
                                if (isnew) agents.Add(new IntStringDecimal((req.AgentId ?? 0), CustomBrokerWpf.References.AgentNames.FindFirstItem("Id", req.AgentId ?? 0)?.Name ?? string.Empty, (req.OfficialWeight ?? 0M), (req.Volume ?? 0M)));
                            }
                        }
                    }
                if (peragent)
                {
                    StringBuilder str = new StringBuilder();
                    str.Append("\n");
                    foreach (IntStringDecimal agent in agents)
                    {
                        str.Append(agent.StringValue);
                    }
                    body = body.Replace("{Поставщик}", str.ToString());
                    //if (agents.Count > 1)
                    str.Clear();
                    str.Append("\n");
                    foreach (IntStringDecimal agent in agents)
                    {
                        str.Append(agent.StringValue);
                        str.Append(" - ");
                        str.Append(agent.Weight.ToString("N0"));
                        //if (agents.Count > 1)
                            str.AppendLine(" кг.");
                        //else
                        //    str.Append(" кг.");
                    }
                    body = body.Replace("{Поставщик(Вес, Д) кг.}", str.ToString());
                    str.Clear();
                    str.Append("\n");
                    foreach (IntStringDecimal agent in agents)
                    {
                        str.Append(agent.StringValue);
                        str.Append(" - ");
                        str.Append(agent.Weight.ToString("N0"));
                        str.Append(" кг., ");
                        str.Append(agent.Volume.ToString("N3"));
                        str.AppendLine(" м3.");
                    }
                    body = body.Replace("{Поставщик(Вес, Д) кг., Объем м3.}", str.ToString());
                }
                body = body.Replace("{Вес,Д}", weight.ToString("N0"));
                body = body.Replace("{Объем}", volume.ToString("N3"));
                body = body.Replace("{Кол-во мест}", cellnumber.ToString());
                DateTime arrdate = DateTime.Now.AddDays(temp.Delay ?? 0);
                if (arrdate.DayOfWeek == DayOfWeek.Saturday)
                    arrdate = arrdate.AddDays(-1);
                else if (arrdate.DayOfWeek == DayOfWeek.Sunday)
                    arrdate = arrdate.AddDays(-2);
                body = body.Replace("{Дата+}", arrdate.ToString("dd.MM.yy"));
            }
            return body;
        }
    }

    internal class IntStringDecimal
    {
        internal IntStringDecimal() { }
        internal IntStringDecimal(int intvalue, string stringvalue, decimal weight, decimal volume) : this()
        {
            myintvalue = intvalue;
            mystringvalue = stringvalue;
            myweight = weight;
            myvolume = volume;
        }

        private int myintvalue;
        internal int IntValue
        {
            set { myintvalue = value; }
            get { return myintvalue; }
        }
        private string mystringvalue;
        internal string StringValue
        {
            set { mystringvalue = value; }
            get { return mystringvalue; }
        }
        private decimal myweight;
        internal decimal Weight
        {
            set { myweight = value; }
            get { return myweight; }
        }
        private decimal myvolume;
        internal decimal Volume
        {
            set { myvolume = value; }
            get { return myvolume; }
        }
    }
}
