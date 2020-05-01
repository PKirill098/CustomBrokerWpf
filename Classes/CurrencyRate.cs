using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes
{
    public class CurrencyRate : INotifyPropertyChanged
    {
        public CurrencyRate(DateTime ratedate)
        {
            myratedate = ratedate;
            AsyncPrepare();
        }
        public CurrencyRate()
        {
            //AsyncPrepare();
        }

        private DateTime? mylastdate;
        private DateTime? myratedate;
        public DateTime? RateDate
        {
            set
            {
                if (!System.Collections.Generic.EqualityComparer<DateTime?>.Default.Equals(myratedate, value))
                {
                    myratedate = value.HasValue ? value.Value.Date : value;
                    GetLatestRateAsync();
                }
            }
            get { return myratedate; }
        }

        private decimal? myusdrate;
        public decimal? USDRate
        {
            private set
            {
                myusdrate = value;
                PropertyChangedNotification("USDRate");
            }
            get { return myusdrate; }
        }
        private decimal? myeurrate;
        public decimal? EURRate
        {
            private set
            {
                myeurrate = value;
                PropertyChangedNotification("EURRate");
            }
            get { return myeurrate; }
        }
        public decimal? USDUERRate
        {
            get { return myeurrate.HasValue & myusdrate.HasValue && myusdrate.Value != 0M ? decimal.Divide(myeurrate.Value, myusdrate.Value) : (decimal?)null; }
        }

        private DateTime? GetLatestDateTime()
        {
            DateTime? value = null;
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(@"http://www.cbr.ru/DailyInfoWebServ/DailyInfo.asmx");
                req.Timeout = 100000;
                req.CookieContainer = new CookieContainer();
                req.AllowAutoRedirect = false;
                req.Method = @"POST";
                req.Host = @"www.cbr.ru";
                req.ContentType = @"text/xml; charset=utf-8";
                //req.Headers.Add("User-Agent");
                req.Headers.Add("SOAPAction", "http://web.cbr.ru/GetLatestDateTime");
                string soapEnv = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                                    <soap:Body >
                                        <GetLatestDateTime xmlns = ""http://web.cbr.ru/"" />
                                    </soap:Body>
                                </soap:Envelope>";
                req.ContentLength = soapEnv.Length;
                StreamWriter streamWriter = new StreamWriter(req.GetRequestStream());
                streamWriter.Write(soapEnv);
                streamWriter.Close();
                WebResponse resp = req.GetResponse();
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Async = true;
                //StreamReader sreader = new StreamReader(resp.GetResponseStream());
                //string answer = sreader.ReadToEnd();
                XmlReader reader = XmlReader.Create(resp.GetResponseStream(), settings);
                reader.ReadToFollowing("GetLatestDateTimeResult");
                value = reader.ReadElementContentAsDateTime();
            }
            catch { }
            return value;
        }
        private decimal? GetLatestRate(string ValutaCode)
        {
            decimal? value = null;
            if (!mylastdate.HasValue || (myratedate.HasValue && myratedate.Value > mylastdate.Value.AddDays(1)))
                mylastdate = GetLatestDateTime();
            if (myratedate.HasValue & mylastdate.HasValue && myratedate.Value <= mylastdate.Value.AddDays(1))
                try
                {
                    value = GetDateRate(ValutaCode, myratedate.Value);
                    while (!value.HasValue)
                    {
                        myratedate = myratedate.Value.AddDays(-1);
                        value = GetDateRate(ValutaCode, myratedate.Value);
                    }
                }
                catch { }
            return value;
        }
        private decimal? GetDateRate(string ValutaCode, DateTime ratedate)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(@"http://www.cbr.ru/DailyInfoWebServ/DailyInfo.asmx");
            req.Timeout = 100000;
            req.CookieContainer = new CookieContainer();
            req.AllowAutoRedirect = false;
            req.Method = @"POST";
            req.Host = @"www.cbr.ru";
            req.ContentType = @"text/xml; charset=utf-8";
            req.Headers.Add("SOAPAction", "http://web.cbr.ru/GetCursDynamicXML");
            string soapEnv = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                <soap:Envelope xmlns:xsi =""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap = ""http://schemas.xmlsoap.org/soap/envelope/"">
                                   <soap:Body>
                                      <GetCursDynamicXML xmlns=""http://web.cbr.ru/"">
                                        <FromDate>" + ratedate.ToString("yyyy-MM-ddT00:00:00+03:00") + @"</FromDate>
                                        <ToDate>" + ratedate.ToString("yyyy-MM-ddTHH:00:00+03:00") + @"</ToDate>
                                        <ValutaCode>" + ValutaCode + @"</ValutaCode>
                                      </GetCursDynamicXML>
                                 </soap:Body>
                                </soap:Envelope>";
            req.ContentLength = soapEnv.Length;
            StreamWriter streamWriter = new StreamWriter(req.GetRequestStream());
            streamWriter.Write(soapEnv);
            streamWriter.Close();
            WebResponse resp = req.GetResponse();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;
            XmlReader reader = XmlReader.Create(resp.GetResponseStream(), settings);
            reader.ReadToFollowing("Vcurs");
            return reader.EOF ? (decimal?)null : reader.ReadElementContentAsDecimal(); ;
        }

        private async void AsyncPrepare()
        {
            await Task.Run(() => { mylastdate = GetLatestDateTime(); if (!myratedate.HasValue) myratedate = mylastdate; myusdrate = GetLatestRate("R01235"); myeurrate = GetLatestRate("R01239"); });
            PropertyChangedNotification("RateDate");
            PropertyChangedNotification("USDRate");
            PropertyChangedNotification("EURRate");
            PropertyChangedNotification("USDUERRate");
        }
        private async void GetLatestRateAsync()
        {
            await Task.Run(() => { myusdrate = GetLatestRate("R01235"); myeurrate = GetLatestRate("R01239"); });
            PropertyChangedNotification("USDRate");
            PropertyChangedNotification("EURRate");
            PropertyChangedNotification("USDUERRate");
        }

        //INotifyPropertyChanged
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void PropertyChangedNotification(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }

    public class CurrencyRateSingleton
    {
        public CurrencyRateSingleton()
        {
            mythreadlock = new object();
            myrates = new Dictionary<DateTime, Dictionary<string, decimal>>();
        }

        private Dictionary<DateTime, Dictionary<string, decimal>> myrates;
        private object mythreadlock;
        private DateTime? mylastdate;

        private DateTime? GetLatestDateTime()
        {
            DateTime? value = null;
            try
            {
                lock (mythreadlock)
                {
                    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(@"http://www.cbr.ru/DailyInfoWebServ/DailyInfo.asmx");
                    req.Timeout = 100000;
                    req.CookieContainer = new CookieContainer();
                    req.AllowAutoRedirect = false;
                    req.Method = @"POST";
                    req.Host = @"www.cbr.ru";
                    req.ContentType = @"text/xml; charset=utf-8";
                    req.Headers.Add("SOAPAction", "http://web.cbr.ru/GetLatestDateTime");
                    string soapEnv = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                                    <soap:Body >
                                        <GetLatestDateTime xmlns = ""http://web.cbr.ru/"" />
                                    </soap:Body>
                                </soap:Envelope>";
                    req.ContentLength = soapEnv.Length;
                    StreamWriter streamWriter = new StreamWriter(req.GetRequestStream());
                    streamWriter.Write(soapEnv);
                    streamWriter.Close();
                    WebResponse resp = req.GetResponse();
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.Async = true;
                    //StreamReader sreader = new StreamReader(resp.GetResponseStream());
                    //string answer = sreader.ReadToEnd();
                    XmlReader reader = XmlReader.Create(resp.GetResponseStream(), settings);
                    reader.ReadToFollowing("GetLatestDateTimeResult");
                    value = reader.EOF ? (DateTime?)null : reader.ReadElementContentAsDateTime();
                }
            }
            catch { }
            return value;
        }
        public decimal? GetLatestRate(string ValutaCode, DateTime ratedate) //перебор дат
        {
            decimal? value = null;
            lock (mythreadlock)
            {
                if (!mylastdate.HasValue || (ratedate > mylastdate.Value))
                    mylastdate = GetLatestDateTime();
            }
            try
            {
                if (mylastdate.HasValue && ratedate <= mylastdate.Value.AddDays(1))
                {
                    DateTime date = ratedate;
                    value = GetDateRate(ValutaCode, date, ratedate);
                    while (!value.HasValue)
                    {
                        date = date.AddDays(-1);
                        value = GetDateRate(ValutaCode, date, ratedate);
                    }
                }
                else if (mylastdate.HasValue)
                    value = GetDateRate(ValutaCode, mylastdate.Value, ratedate);
            }
            catch { }
            return value;
        }
        private decimal? GetDateRate(string ValutaCode, DateTime ratedate, DateTime myquerydate) // rate from cbr
        {
            decimal? value = null;
            try
            {
                lock (mythreadlock) // блокируем чтобы все не добавляли в коллекцию
                {
                    if (myrates.ContainsKey(ratedate) && myrates[ratedate].ContainsKey(ValutaCode))
                    {
                        value = myrates[ratedate][ValutaCode];
                        if (!myrates.ContainsKey(myquerydate))
                            myrates.Add(myquerydate, new Dictionary<string, decimal>());
                        if (!myrates[myquerydate].ContainsKey(ValutaCode))
                            myrates[myquerydate].Add(ValutaCode, value.Value);
                    }
                    else
                    {
                        HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(@"http://www.cbr.ru/DailyInfoWebServ/DailyInfo.asmx");
                        req.Timeout = 100000;
                        req.CookieContainer = new CookieContainer();
                        req.AllowAutoRedirect = false;
                        req.Method = @"POST";
                        req.Host = @"www.cbr.ru";
                        req.ContentType = @"text/xml; charset=utf-8";
                        req.Headers.Add("SOAPAction", "http://web.cbr.ru/GetCursDynamicXML");
                        string soapEnv = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                <soap:Envelope xmlns:xsi =""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap = ""http://schemas.xmlsoap.org/soap/envelope/"">
                                   <soap:Body>
                                      <GetCursDynamicXML xmlns=""http://web.cbr.ru/"">
                                        <FromDate>" + ratedate.ToString("yyyy-MM-ddT00:00:00+03:00") + @"</FromDate>
                                        <ToDate>" + ratedate.ToString("yyyy-MM-ddTHH:00:00+03:00") + @"</ToDate>
                                        <ValutaCode>" + ValutaCode + @"</ValutaCode>
                                      </GetCursDynamicXML>
                                 </soap:Body>
                                </soap:Envelope>";
                        req.ContentLength = soapEnv.Length;
                        StreamWriter streamWriter = new StreamWriter(req.GetRequestStream());
                        streamWriter.Write(soapEnv);
                        streamWriter.Close();
                        WebResponse resp = req.GetResponse();
                        XmlReaderSettings settings = new XmlReaderSettings();
                        settings.Async = true;
                        XmlReader reader = XmlReader.Create(resp.GetResponseStream(), settings);
                        reader.ReadToFollowing("Vcurs");
                        value = reader.EOF ? (decimal?)null : reader.ReadElementContentAsDecimal();
                        if (value.HasValue)
                        {
                            if (!myrates.ContainsKey(ratedate))
                                myrates.Add(ratedate, new Dictionary<string, decimal>());
                            myrates[ratedate].Add(ValutaCode, value.Value);
                            if (!myrates.ContainsKey(myquerydate))
                                myrates.Add(myquerydate, new Dictionary<string, decimal>());
                            if (!myrates[myquerydate].ContainsKey(ValutaCode))
                                myrates[myquerydate].Add(ValutaCode, value.Value);
                        }
                    }
                }
            }
            catch { }
            return value;
        }
    }

    public class CurrencyRateProxy : INotifyPropertyChanged
    {
        public CurrencyRateProxy(CurrencyRateSingleton singleton)
        {
            mysingleton = singleton;
        }

        private CurrencyRateSingleton mysingleton;
        private DateTime? myratedate;
        public DateTime? RateDate
        {
            set
            {
                if (!System.Collections.Generic.EqualityComparer<DateTime?>.Default.Equals(myratedate, value))
                {
                    myratedate = value.HasValue ? value.Value.Date : value;
                    GetLatestRateAsync();
                }
            }
            get { return myratedate; }
        }

        private decimal? myusdrate;
        public decimal? USDRate
        {
            private set
            {
                myusdrate = value;
                PropertyChangedNotification("USDRate");
            }
            get { return myusdrate; }
        }
        private decimal? myeurrate;
        public decimal? EURRate
        {
            private set
            {
                myeurrate = value;
                PropertyChangedNotification("EURRate");
            }
            get { return myeurrate; }
        }
        public decimal? USDUERRate
        {
            get { return myeurrate.HasValue & myusdrate.HasValue && myusdrate.Value != 0M ? decimal.Divide(myeurrate.Value, myusdrate.Value) : (decimal?)null; }
        }

        private async void GetLatestRateAsync()
        {
            await Task.Run(() => { myusdrate = mysingleton.GetLatestRate("R01235", myratedate ?? DateTime.Today); myeurrate = mysingleton.GetLatestRate("R01239", myratedate ?? DateTime.Today); });
            PropertyChangedNotification("USDRate");
            PropertyChangedNotification("EURRate");
            PropertyChangedNotification("USDUERRate");
        }

        //INotifyPropertyChanged
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void PropertyChangedNotification(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}
