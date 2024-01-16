using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using System.Linq;
using System.Threading;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Algorithm
{
    internal struct AlgorithmValuesRequestRecord
    {
        internal int id;
        internal long stamp;
        internal FormulaRecord formula;
        internal decimal? value1;
        internal decimal? value2;
        internal decimal? value1user;
        internal decimal? value2user;
        internal long afstamp;
    }

    public class AlgorithmValuesRequest: AlgorithmValues, IDisposable
    {
        public AlgorithmValuesRequest(int id, long stamp, lib.DomainObjectState state, Algorithm algorithm, Formula formula, decimal? value1, decimal? value2, decimal? value1user, decimal? value2user, long afstamp, Request request) : base(id, stamp, state, algorithm, formula, value1user ?? value1, value2user ?? value2)
        {
            mydbvalue1 = value1;
            mydbvalue2 = value2;
            myvalue1user = value1user;
            myvalue2user = value2user;
            myafstamp = afstamp;
            myrequest = request;
            RequestSync1();
            RequestSync2();
            mygwdbm = new AlgorithmGroupWeightDBM();
            mygwdbm.Request = myrequest;
            switch(this.Formula.Code)
            {
                case "Р1":
                    if (myrequest.ServiceType == "ТЭО")
                        this.Formula.Formula1 = "Р5";
                    else
                    {
                        this.Formula.Formula1 = "{Курс ЦБ +2%}";
                        this.FuncValue1 = (string eer) =>
                        {
                            decimal p1 =
                                myrequest.CustomerLegals.Where((RequestCustomerLegal legal) => { return legal.Selected; }).Sum((RequestCustomerLegal legal) =>
                                {
                                    return legal.Prepays.Count();
                                });
                            if (p1 == 1M)
                                p1 =
                                myrequest.CustomerLegals.Where((RequestCustomerLegal legal) => { return legal.Selected; }).Sum((RequestCustomerLegal legal) =>
                                {
                                    return legal.Prepays.Sum((Account.PrepayCustomerRequest prepay) =>
                                    {
                                        return prepay.Prepay.CBRatep2p ?? 0M;
                                    });
                                });
                            else if (p1 > 1M)
                            {
                                p1 =
                                myrequest.CustomerLegals.Where((RequestCustomerLegal legal) => { return legal.Selected; }).Sum((RequestCustomerLegal legal) =>
                                {
                                    return legal.Prepays.Sum((Account.PrepayCustomerRequest prepay) =>
                                    {
                                        return prepay.DTSum == 0 ? prepay.EuroSum : prepay.DTSum;
                                    });
                                });
                                p1 =
                               myrequest.CustomerLegals.Where((RequestCustomerLegal legal) => { return legal.Selected; }).Sum((RequestCustomerLegal legal) =>
                               {
                                   return legal.Prepays.Sum((Account.PrepayCustomerRequest prepay) =>
                                   {
                                       return (prepay.Prepay.CBRatep2p ?? 0M) * (prepay.DTSum == 0 ? prepay.EuroSum : prepay.DTSum);
                                   });
                               })
                               / p1;
                            }
                            return p1;
                        };
                    }
                    break;
                case "Р2":
                    if (myrequest.ServiceType == "ТЭО")
                        this.Formula.Formula1 = "Р5";
                    else
                    {
                        this.Formula.Formula1 = "{Курс покупки}";
                        this.FuncValue1 = (string eer) =>
                    {
                        decimal p1 =
                            myrequest.CustomerLegals.Where((RequestCustomerLegal legal) => { return legal.Selected; }).Sum((RequestCustomerLegal legal) =>
                            {
                                return legal.Prepays.Count;
                            });
                        if (p1 == 1M)
                            p1 =
                            myrequest.CustomerLegals.Where((RequestCustomerLegal legal) => { return legal.Selected; }).Sum((RequestCustomerLegal legal) =>
                            {
                                return
                                    legal.Prepays.Sum((Account.PrepayCustomerRequest prepay) =>
                                    {
                                        return prepay.Prepay.CurrencyBuyRate ?? 0M;
                                    });
                            });
                        else if (p1 > 1M)
                        {
                            p1 =
                            myrequest.CustomerLegals.Where((RequestCustomerLegal legal) => { return legal.Selected; }).Sum((RequestCustomerLegal legal) =>
                            {
                                return legal.Prepays.Sum((Account.PrepayCustomerRequest prepay) =>
                                {
                                    return prepay.DTSum == 0 ? prepay.EuroSum : prepay.DTSum;
                                });
                            });
                            p1 =
                           myrequest.CustomerLegals.Where((RequestCustomerLegal legal) => { return legal.Selected; }).Sum((RequestCustomerLegal legal) =>
                           {
                               return legal.Prepays.Sum((Account.PrepayCustomerRequest prepay) =>
                               {
                                   return (prepay.Prepay.CurrencyBuyRate ?? 0M) * (prepay.DTSum == 0 ? prepay.EuroSum : prepay.DTSum);
                               });
                           })
                           / p1;
                        }
                        return p1;
                    };
                    }
                    break;
                case "Р3":
                    this.FuncValue1 = (string eer) =>
                    {
                        return myrequest.Specification?.Declaration?.VAT ?? 0;
                    };
                    break;
                case "Р4":
                    this.FuncValue1 = (string eer) =>
                    {
                        return (myrequest.Specification?.Declaration?.Fee ?? 0) + (myrequest.Specification?.Declaration?.Tax ?? 0);
                    };
                    break;
                case "Р5":
                    this.FuncValue1 = (string eer) =>
                    {
                        return myrequest.Specification?.Declaration?.CBRate??0;
                    };
                    break;
            }
            //this.PropertyChanged += AlgorithmValuesRequest_PropertyChanged;
        }
        public AlgorithmValuesRequest(Algorithm algorithm, Formula formula, Request request) : this(lib.NewObjectId.NewId, 0, lib.DomainObjectState.Added, algorithm, formula, null, null, null, null, 0, request) { }
        public AlgorithmValuesRequest(int id, long stamp, lib.DomainObjectState state, Algorithm algorithm, Formula formula, decimal? value1, decimal? value2, decimal? value1user, decimal? value2user, long afstamp)
            : base(id, stamp, state, algorithm, formula, value1user ?? value1, value2user ?? value2) // для консалидации
        { myafstamp = afstamp; }

        protected Request myrequest;
        private AlgorithmGroupWeightDBM mygwdbm;
        protected decimal? mydbvalue1;
        protected decimal? myvalue1user;
        internal decimal? Value1User
        { get { return myvalue1user; } }
        public override decimal? Value1
        {
            set
            {
                if (this.Formula.Code != "П9" || myrequest.ValidateProperty(nameof(Request.InvoiceDiscount), this.Value1User, out myvalue1err, out _))
                    SetProperty<decimal?>(ref myvalue1user, value, () =>
                    {
                        this.PropertyChangedNotification("Value1Err");
                        RequestSync1();
                    });
            }
            get { return myvalue1user ?? myvalue1; }
        }
        internal decimal? Value1Templ
        { set { SetProperty<decimal?>(ref myvalue1, value, () => { this.PropertyChangedNotification("Value1"); RequestSync1(); }); myvalue1err = string.Empty; this.PropertyChangedNotification("Value1Err"); } get { return myvalue1; } }
        public override bool Value1IsReadOnly
        { get { return false; } }
        public override string Value1Err { set { myvalue1err = value; if (!myvalue1user.HasValue) this.PropertyChangedNotification("Value1Err"); } get { return myvalue1user.HasValue && this.Formula.Code != "П9" ? string.Empty : myvalue1err; } }

        protected decimal? mydbvalue2;
        protected decimal? myvalue2user;
        internal decimal? Value2User
        { get { return myvalue2user; } }
        public override decimal? Value2
        { set { SetProperty<decimal?>(ref myvalue2user, value, () => { this.PropertyChangedNotification("Value2Err"); }); } get { return myvalue2user ?? myvalue2; } }
        internal decimal? Value2Templ
        { get { return myvalue2; } }
        public override bool Value2IsReadOnly
        { get { return false; } }
        public override string Value2Err { set { myvalue2err = value; if (!myvalue2user.HasValue) this.PropertyChangedNotification("Value2Err"); } get { return myvalue2user.HasValue ? string.Empty : myvalue2err; } }

        private long myafstamp;
        internal long AFStamp { get { return myafstamp; } }

        protected virtual void RequestSync1()
        {
            if (myrequest != null)
                switch (this.Formula.Code)
                {
                    case "П9":
                        if (this.Value1User.HasValue && !(myrequest.ParcelGroup.HasValue || myrequest.InvoiceDiscount == this.Value1) && myrequest.ValidateProperty(nameof(Request.InvoiceDiscount), this.Value1User, out myvalue1err, out _))
                            myrequest.InvoiceDiscount = this.Value1User;
                        if (!string.IsNullOrEmpty(myrequest.Consolidate) & this.Value1.HasValue & myrequest.AlgorithmConCMD?.Algorithm != null)
                        {
                            int i = 0, maxi = 6;
                            decimal? cx12 = null, cx17 = null, cx18 = null, cx19 = null, cx20 = null,r3 = null,r4 = null;
                            foreach (AlgorithmValuesRequest values in myrequest.AlgorithmConCMD.Algorithm.Formulas)
                            {
                                if (values.Formula.Code == "X1")
                                {
                                    cx12 = values.Value1;
                                    i++;
                                    if (i > maxi) break;
                                }
                                if (values.Formula.Code == "X2")
                                {
                                    cx17 = values.Value1;
                                    i++;
                                    if (i > maxi) break;
                                }
                                if (values.Formula.Code == "X3")
                                {
                                    cx18 = values.Value1;
                                    i++;
                                    if (i > maxi) break;
                                }
                                if (values.Formula.Code == "X4")
                                {
                                    cx19 = values.Value1;
                                    i++;
                                    if (i > maxi) break;
                                }
                                if (values.Formula.Code == "X5")
                                {
                                    cx20 = values.Value1;
                                    i++;
                                    if (i > maxi) break;
                                }
                                if (values.Formula.Code == "R3")
                                {
                                    r3 = values.Value1;
                                    i++;
                                    if (i > maxi) break;
                                }
                                if (values.Formula.Code == "R4")
                                {
                                    r4 = values.Value1;
                                    i++;
                                    if (i > maxi) break;
                                }
                            }
                            i = 0;
                            foreach (AlgorithmValuesRequest values in this.Algorithm.Formulas)
                            {
                                if (values.Formula.Code == "П12")
                                {
                                    if (cx12.HasValue & !values.Value1User.HasValue)
                                        values.Value1Templ = this.Value1.Value * cx12.Value;
                                    i++;
                                    if (i > maxi) break;
                                }
                                if (values.Formula.Code == "П17")
                                {
                                    if (cx17.HasValue & !values.Value1User.HasValue)
                                        values.Value1Templ = this.Value1.Value * cx17.Value;
                                    i++;
                                    if (i > maxi) break;
                                }
                                if (values.Formula.Code == "П18")
                                {
                                    if (cx18.HasValue & !values.Value1User.HasValue)
                                        values.Value1Templ = this.Value1.Value * cx18.Value;
                                    i++;
                                    if (i > maxi) break;
                                }
                                if (values.Formula.Code == "П19")
                                {
                                    if (cx19.HasValue & !values.Value1User.HasValue)
                                        values.Value1Templ = this.Value1.Value * cx19.Value;
                                    i++;
                                    if (i > maxi) break;
                                }
                                if (values.Formula.Code == "П20")
                                {
                                    if (cx20.HasValue & !values.Value1User.HasValue)
                                        values.Value1Templ = this.Value1.Value * cx20.Value;
                                    i++;
                                    if (i > maxi) break;
                                }
                                if (values.Formula.Code == "Р3")
                                {
                                    if (r3.HasValue & !values.Value1User.HasValue)
                                        values.Value1Templ = this.Value1.Value * r3.Value;
                                    i++;
                                    if (i > maxi) break;
                                }
                                if (values.Formula.Code == "Р4")
                                {
                                    if (r4.HasValue & !values.Value1User.HasValue)
                                        values.Value1Templ = this.Value1.Value * r4.Value;
                                    i++;
                                    if (i > maxi) break;
                                }
                            }
                        }
                        break;
                    case "П10":
                        if (this.Value1User.HasValue && !(myrequest.ParcelGroup.HasValue || myrequest.OfficialWeight == this.Value1))
                            myrequest.OfficialWeight = this.Value1;
                        if (!string.IsNullOrEmpty(myrequest.Consolidate) & this.Value1.HasValue & myrequest.AlgorithmConCMD?.Algorithm != null)
                        {
                            int i = 0;
                            decimal? cx13 = null, cx22 = null;
                            foreach (AlgorithmValuesRequest values in myrequest.AlgorithmConCMD.Algorithm.Formulas)
                            {
                                if (values.Formula.Code == "W13")
                                {
                                    cx13 = values.Value1;
                                    i++;
                                    if (i > 1) break;
                                }
                                if (values.Formula.Code == "W22")
                                {
                                    cx22 = values.Value1;
                                    i++;
                                    if (i > 1) break;
                                }
                            }
                            if (cx13.HasValue)
                            {
                                foreach (AlgorithmValuesRequest values in this.Algorithm.Formulas)
                                    if (values.Formula.Code == "П13" & !values.Value1User.HasValue)
                                    {
                                        values.Value1Templ = this.Value1.Value * cx13.Value;
                                        break;
                                    }
                            }
                            if (cx22.HasValue)
                            {
                                foreach (AlgorithmValuesRequest values in this.Algorithm.Formulas)
                                    if (values.Formula.Code == "П22" & !values.Value1User.HasValue)
                                    {
                                        values.Value1Templ = this.Value1.Value * cx22.Value;
                                        break;
                                    }
                            }
                        }
                        break;
                    case "П12":
                        if (myrequest.CustomsCost != this.Value1) myrequest.CustomsCost = this.Value1;
                        break;
                    case "П13":
                        if (myrequest.ServiceType == "ТЭО" && myrequest.BrokerCost != this.Value1) myrequest.BrokerCost = this.Value1;
                        break;
                    case "П14": // если не заполнен Транспорт на машину считаем по старому
                        //if(myrequest.Importer == null || (myrequest.Importer.Id == 1 ? myrequest.Parcel?.TransportTUn == null: myrequest.Parcel?.TransportDUn == null))
                        myrequest.DeliveryCost = this.Value1;
                        break;
                    case "П15":
                        myrequest.InsuranceCost = this.Value1;
                        break;
                    case "П16":
                        myrequest.BringCost = this.Value1;
                        break;
                    case "П17":
                        if (myrequest.FreightCost != this.Value1) myrequest.FreightCost = this.Value1;
                        break;
                    case "П18":
                        if (myrequest.SertificatCost != this.Value1) myrequest.SertificatCost = this.Value1;
                        break;
                    case "П19":
                        if (myrequest.PreparatnCost != this.Value1) myrequest.PreparatnCost = this.Value1;
                        break;
                    case "П20":
                        if (myrequest.AdditionalCost != this.Value1) myrequest.AdditionalCost = this.Value1;
                        break;
                    case "П21":
                        if (myrequest.ServiceType == "ТЭО") myrequest.TotalCost = this.Value1; else myrequest.TotalCost = null;
                        break;
                    case "П22":
                        if (myrequest.BrokerPay != this.Value1) myrequest.BrokerPay = this.Value1;
                        break;
                    case "П23":
                        myrequest.DeliveryPay = this.Value1;
                        break;
                    case "П24":
                        myrequest.InsurancePay = this.Value1;
                        break;
                    case "П25":
                        myrequest.FreightPay = this.Value1;
                        break;
                    case "П26":
                        myrequest.SertificatPay = this.Value1;
                        break;
                    case "П27":
                        myrequest.PreparatnPay = this.Value1;//if (myrequest.ServiceType == "ТЭО") 
                        break;
                    case "П28":
                        myrequest.AdditionalPay = this.Value1;//if (myrequest.ServiceType == "ТЭО") 
                        break;
                    case "П29":
                        myrequest.BringPay = this.Value1;
                        break;
                    case "П30":
                        if (myrequest.ServiceType == "ТЭО")
                        {
                            myrequest.CorrCost = this.Value1;
                        }
                        break;
                    case "П31":
                        if (myrequest.ServiceType == "ТЭО")
                        {
                            myrequest.TotalPay = this.Value1;
                        }
                        break;
                    case "П33":
                        if (myrequest.ServiceType == "ТЭО")
                            myrequest.IncomePay = this.Value1;
                        break;
                    case "П34":
                        if (myrequest.ServiceType == "ТЭО")
                            myrequest.IncomeM3 = this.Value1;
                        break;
                    case "П35":
                        if (myrequest.ServiceType == "ТД")
                        {
                            myrequest.TDPay = this.Value1;
                        }
                        else
                        {
                            myrequest.TDPay = null;
                        }
                        break;
                    case "П39":
                        if (myrequest.ServiceType == "ТД") myrequest.CorrCost = this.Value1;
                        break;
                    case "П40":
                        if (myrequest.ServiceType == "ТД")
                        {
                            myrequest.TotalPay = this.Value1;
                        }
                        break;
                    case "П47":
                        if (myrequest.ServiceType == "ТД")
                            myrequest.IncomePay = this.Value1;
                        break;
                    case "П48":
                        if (myrequest.ServiceType == "ТД")
                            myrequest.IncomeM3 = this.Value1;
                        break;
                    case "П49":
                        myrequest.LogisticsCost = this.Value1;
                        break;
                    case "П50":
                        myrequest.LogisticsPay = this.Value1;
                        break;
                    case "П51":
                        if(myrequest.AlgorithmCMD?.RequestProperties!=null)
                            myrequest.AlgorithmCMD.RequestProperties.CMR = this.Value1;
                        break;
                    case "П52":
                        if (myrequest.AlgorithmCMD?.RequestProperties != null)
                            myrequest.AlgorithmCMD.RequestProperties.CBX = this.Value1;
                        break;
                    case "П53":
                        if (myrequest.AlgorithmCMD?.RequestProperties != null)
                            myrequest.AlgorithmCMD.RequestProperties.EX1T1 = this.Value1;
                        break;
                }
        }
        protected virtual void RequestSync2()
        {
            if (myrequest != null)
                switch (this.Formula.Code)
                {
                    case "П12":
                        if (this.Value2.HasValue)
                            myrequest.CustomsPayInvoice = decimal.Divide(this.Value2.Value, 100M);
                        else
                            myrequest.CustomsPayInvoice = this.Value2;
                        break;
                    case "П31":
                        if (myrequest.ServiceType == "ТЭО")
                        {
                            if (!string.IsNullOrEmpty(myrequest.Consolidate) & this.isValid1 & this.isValid2 & this.Value1.HasValue & this.Value2.HasValue && myrequest.AlgorithmCMD != null & myrequest.AlgorithmConCMD != null)
                            {
                                decimal? p40 = null;
                                foreach (AlgorithmValuesRequestCon values in myrequest.AlgorithmConCMD.Algorithm.Formulas)
                                {
                                    if (values.Formula.Code == "П31")
                                    {
                                        if (values.Value2.HasValue)
                                            p40 = this.Value1.Value * decimal.Divide(values.Value2.Value, this.Value2.Value) - this.Value1.Value;
                                        break;
                                    }
                                }
                                if (p40.HasValue)
                                    foreach (AlgorithmValuesRequest values in myrequest.AlgorithmCMD.Algorithm.Formulas)
                                    {
                                        if (values.Formula.Code == "П30" & !values.Value1User.HasValue)
                                        {
                                            values.Value1Templ = (values.Value1Templ ?? 0M) + p40.Value;
                                            break;
                                        }
                                    }
                            }
                            if (this.Value2.HasValue)
                                myrequest.TotalPayInvoicePer = decimal.Divide(this.Value2.Value, 100M);
                            else
                                myrequest.TotalPayInvoicePer = this.Value2;
                        }
                        break;
                    case "П33":
                        if (myrequest.ServiceType == "ТЭО")
                        {
                            if (this.Value2.HasValue)
                                myrequest.Income = decimal.Divide(this.Value2.Value, 100M);
                            else
                                myrequest.Income = this.Value2;
                        }
                        break;
                    case "П40":
                        if (myrequest.ServiceType == "ТД")
                        {
                            if (!string.IsNullOrEmpty(myrequest.Consolidate) & this.isValid1 & this.isValid2 & this.Value1.HasValue & this.Value2.HasValue && myrequest.AlgorithmCMD != null & myrequest.AlgorithmConCMD != null)
                            {
                                decimal? p40 = null;
                                foreach (AlgorithmValuesRequestCon values in myrequest.AlgorithmConCMD.Algorithm.Formulas)
                                {
                                    if (values.Formula.Code == "П40" && values.isValid2)
                                    {
                                        if (values.Value2.HasValue)
                                            p40 = this.Value1.Value * decimal.Divide(values.Value2.Value, this.Value2.Value) - this.Value1.Value;
                                        break;
                                    }
                                }
                                if (p40.HasValue)
                                    foreach (AlgorithmValuesRequest values in myrequest.AlgorithmCMD.Algorithm.Formulas)
                                    {
                                        if (values.Formula.Code == "П39" & !values.Value1User.HasValue)
                                        {
                                            values.Value1Templ = (values.Value1Templ ?? 0M) + p40.Value;
                                            break;
                                        }
                                    }
                            }
                            if (this.Value2.HasValue)
                                myrequest.TotalPayInvoicePer = decimal.Divide(this.Value2.Value, 100M);
                            else
                                myrequest.TotalPayInvoicePer = this.Value2;
                        }
                        break;
                    case "П47":
                        if (myrequest.ServiceType == "ТД")
                        {
                            if (this.Value2.HasValue)
                                myrequest.Income = decimal.Divide(this.Value2.Value, 100M);
                            else
                                myrequest.Income = this.Value2;
                        }
                        break;
                    case "П50":
                        if (this.Value2.HasValue)
                            myrequest.Log = decimal.Divide(this.Value2.Value, 100M);
                        else
                            myrequest.Log = this.Value2;
                        break;
                    case "П51":
                        if (myrequest.AlgorithmCMD?.RequestProperties != null)
                            myrequest.AlgorithmCMD.RequestProperties.CMR2 = this.Value2;
                        break;
                    case "П52":
                        if (myrequest.AlgorithmCMD?.RequestProperties != null)
                            myrequest.AlgorithmCMD.RequestProperties.CBX2 = this.Value2;
                        break;
                    case "П53":
                        if (myrequest.AlgorithmCMD?.RequestProperties != null)
                            myrequest.AlgorithmCMD.RequestProperties.EX1T12 = this.Value2;
                        break;
                }
        }

        internal override bool SetValue1()
        {
            decimal? oldvalue = myvalue1;
            bool issuccess = base.SetValue1();
            if (oldvalue.HasValue != myvalue1.HasValue || (myvalue1.HasValue && !decimal.Equals(decimal.Round(oldvalue.Value, 4), decimal.Round(myvalue1.Value, 4))))
                RequestSync1();
            if (issuccess // при расчетах Value1 не меняется и это не заполняется, нужно доп поле с отслеживанием ручного ввода, сохранения и обновления св-в
                && !myvalue1user.HasValue // контролируем только расчетное значение
                && this.DomainState == lib.DomainObjectState.Unchanged
                && (mydbvalue1.HasValue != myvalue1.HasValue || (myvalue1.HasValue && !decimal.Equals(decimal.Round(mydbvalue1.Value, 4), decimal.Round(myvalue1.Value, 4)))))
                this.DomainState = lib.DomainObjectState.Modified;
            return issuccess;
        }
        internal override bool SetValue2()
        {
            decimal? oldvalue = myvalue2;
            bool issuccess = base.SetValue2();
            if (oldvalue.HasValue != myvalue2.HasValue || (myvalue2.HasValue && !decimal.Equals(decimal.Round(oldvalue.Value, 4), decimal.Round(myvalue2.Value, 4))))
                RequestSync2();
            if (issuccess
                && !myvalue2user.HasValue
                && this.DomainState == lib.DomainObjectState.Unchanged
                && (mydbvalue2.HasValue != myvalue2.HasValue || (myvalue2.HasValue && !decimal.Equals(decimal.Round(mydbvalue2.Value, 4), decimal.Round(myvalue2.Value, 4)))))
                this.DomainState = lib.DomainObjectState.Modified;
            return issuccess;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Value1":
                    myvalue1user = (decimal?)value;
                    this.PropertyChangedNotification("Value1Err");
                    break;
                case "Value2":
                    myvalue1user = (decimal?)value;
                    this.PropertyChangedNotification("Value2Err");
                    break;
            }
        }
        public override void AcceptChanches()
        {
            base.AcceptChanches();
            mydbvalue1 = this.Value1;
            mydbvalue2 = this.Value2;
        }
        //public override 
        internal void UpdateProperties(AlgorithmValuesRequest newitem)
        {
            this.UpdatingSample = true;
            if (!base.Value1IsReadOnly) this.Value1 = newitem.Value1User;
            if (!base.Value2IsReadOnly) this.Value2 = newitem.Value2User;
            if (this.DomainState == lib.DomainObjectState.Unchanged // если наши расчетные значения отличны от в базе
                && ((!myvalue1user.HasValue
                        && (mydbvalue1.HasValue != newitem.Value1.HasValue || (mydbvalue1.HasValue && !decimal.Equals(decimal.Round(mydbvalue1.Value, 4), decimal.Round(newitem.Value1.Value, 4)))))
                    || (!myvalue2user.HasValue
                        && (mydbvalue2.HasValue != newitem.Value2.HasValue || (mydbvalue2.HasValue && !decimal.Equals(decimal.Round(mydbvalue2.Value, 4), decimal.Round(newitem.Value2.Value, 4)))))
                ))
                this.DomainState = lib.DomainObjectState.Modified;
            mydbvalue1 = newitem.Value1;
            mydbvalue2 = newitem.Value2;
            this.UpdatingSample = false;
        }

        public void Dispose()
        {
            this.DomainState = lib.DomainObjectState.Destroyed;
            if (myrequest != null)
            {
                myrequest = null;
            }
            if (mygwdbm != null)
            {
                mygwdbm.Request = null;
                mygwdbm = null;
            }
        }
        #region Blocking
        //private void AlgorithmValuesRequest_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (myisloaded && e.PropertyName == "DomainState")
        //    {
        //        if (this.DomainStatePrevious == lib.DomainObjectState.Unchanged & (this.DomainState == lib.DomainObjectState.Modified | this.DomainState == lib.DomainObjectState.Deleted))
        //        {
        //            myrequest.Blocking();
        //        }
        //        else if (this.DomainStatePrevious == lib.DomainObjectState.Modified | this.DomainStatePrevious == lib.DomainObjectState.Deleted)
        //            myrequest.UnBlocking();
        //    }
        //}
        #endregion
    }

    internal class AlgorithmValuesRequestDBM : lib.DBManagerStamp<AlgorithmValuesRequestRecord,AlgorithmValuesRequest>
    {
        public AlgorithmValuesRequestDBM(Algorithm algorithm, ObservableCollection<Formula> formulas, AlgorithmValuesStorage storage, Request request) : base()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectProcedure = true;
            InsertProcedure = true;
            UpdateProcedure = true;
            DeleteProcedure = true;
            SelectCommandText = "[dbo].[RequestAlgorithmValues_sp]";
            InsertCommandText = "[dbo].[RequestAlgorithmValuesAdd_sp]";
            UpdateCommandText = "[dbo].[RequestAlgorithmValuesUpd_sp]";
            DeleteCommandText = "[dbo].[RequestAlgorithmValuesDel_sp]";

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@requestid", System.Data.SqlDbType.Int),
                new SqlParameter("@algorithmid", System.Data.SqlDbType.Int),
                new SqlParameter("@group", System.Data.SqlDbType.Int),
            };
            InsertParams = new SqlParameter[]
            {
                myinsertparams[0],myinsertparams[1],
                new SqlParameter("@requestid", System.Data.SqlDbType.Int),
                new SqlParameter("@group", System.Data.SqlDbType.Int),
                new SqlParameter("@ordinal", System.Data.SqlDbType.Int),
            };
            InsertUpdateParams = new SqlParameter[]
            {
                new SqlParameter("@formulaid", System.Data.SqlDbType.Int),
                new SqlParameter("@code", System.Data.SqlDbType.NVarChar,3),
                new SqlParameter("@name", System.Data.SqlDbType.NVarChar,50),
                new SqlParameter("@type", System.Data.SqlDbType.TinyInt),
                new SqlParameter("@formula1", System.Data.SqlDbType.NVarChar,50),
                new SqlParameter("@formula2", System.Data.SqlDbType.NVarChar,50),
                new SqlParameter("@value1", System.Data.SqlDbType.Decimal){Precision=18,Scale=8 },
                new SqlParameter("@value2", System.Data.SqlDbType.Decimal){Precision=18,Scale=8 },
                new SqlParameter("@isuser1", System.Data.SqlDbType.Bit),
                new SqlParameter("@isuser2", System.Data.SqlDbType.Bit),
                new SqlParameter("@afstamp", System.Data.SqlDbType.BigInt)
            };
            myalgorithm = algorithm;
            myformulas = formulas;
            mystorage = storage;
            myrequest = request;
        }

        private Algorithm myalgorithm;
        internal Algorithm Algorithm
        {
            set
            {
                myalgorithm = value;
            }
            get { return myalgorithm; }
        }
        private ObservableCollection<Formula> myformulas;
        internal ObservableCollection<Formula> Formulas
        { set { myformulas = value; } }
        private AlgorithmValuesStorage mystorage;
        private Request myrequest;

		protected override AlgorithmValuesRequestRecord CreateRecord(SqlDataReader reader)
		{
            AlgorithmValuesRequestRecord item = new AlgorithmValuesRequestRecord()
            {
                id = reader.IsDBNull(0) ? lib.NewObjectId.NewId : reader.GetInt32(0)
                ,stamp = reader.IsDBNull(1) ? 0 : reader.GetInt64(1)
                , value1 = reader.IsDBNull(this.Fields["value1"]) ? (decimal?)null : reader.GetDecimal(this.Fields["value1"])
                , value2 = reader.IsDBNull(this.Fields["value2"]) ? (decimal?)null : reader.GetDecimal(this.Fields["value2"])
                , value1user = reader.IsDBNull(this.Fields["value1user"]) ? (decimal?)null : reader.GetDecimal(this.Fields["value1user"])
                , value2user =  reader.IsDBNull(this.Fields["value2user"]) ? (decimal?)null : reader.GetDecimal(this.Fields["value2user"])
                , afstamp = reader.IsDBNull(this.Fields["afstamp"]) ? 0 : reader.GetInt64(this.Fields["afstamp"])
            };
            item.formula.id = reader.GetInt32(this.Fields["formulaid"]);
            item.formula.code = reader.GetString(this.Fields["code"]);
            item.formula.name = reader.GetString(this.Fields["name"]);
            item.formula.type = reader.GetByte(this.Fields["type"]);
            item.formula.formula1 = reader.GetString(this.Fields["formula1"]);
            item.formula.formula2 = reader.GetString(this.Fields["formula2"]);
            item.formula.ordinal = reader.GetInt32(this.Fields["ordinal"]);
            return item;
		}
        protected override AlgorithmValuesRequest CreateModel(AlgorithmValuesRequestRecord record, SqlConnection addcon, CancellationToken canceltasktoken = default)
        {
            Formula formula = null;
            //if (myrequest.Status.Id < 70)
            //{
            //    foreach (Formula frm in myformulas)
            //        if (frm.Id == frmid)
            //        {
            //            formula = frm;
            //            break;
            //        }
            //}
            //else
            //{
                formula = new Formula(record.formula.id, 0, lib.DomainObjectState.Unchanged
                    ,record.formula.code
                    ,record.formula.name
                    ,record.formula.type
                    ,record.formula.formula1
                    ,record.formula.formula2
                    ,record.formula.ordinal);
                myformulas.Add(formula);
            //}
            AlgorithmValuesRequest newitem = new AlgorithmValuesRequest(record.id, record.stamp, (myrequest.Status.Id < 500 ? (record.id<0 ? lib.DomainObjectState.Added : lib.DomainObjectState.Unchanged) : lib.DomainObjectState.Sealed)
                , myalgorithm, formula
                , record.value1
                , record.value2
                , record.value1user
                , record.value2user
                , record.afstamp
                , myrequest);
            return newitem; //mystorage.UpdateItem(newitem) as AlgorithmValuesRequest
        }
        protected override void GetOutputSpecificParametersValue(AlgorithmValuesRequest item)
        {
        }
        protected override bool SaveChildObjects(AlgorithmValuesRequest item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(AlgorithmValuesRequest item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            this.SelectParams[0].Value = myrequest.Id;
            if (myrequest.Status.Id < 500) this.SelectParams[1].Value = myalgorithm.Id; else this.SelectParams[1].Value = null;
            this.SelectParams[2].Value = myrequest.ParcelGroup;
        }
        protected override bool SetSpecificParametersValue(AlgorithmValuesRequest item)
        {
            foreach(SqlParameter par in myinsertparams)
                switch(par.ParameterName)
                {
                    case "@requestid":
                        par.Value = myrequest.Id;
                        break;
                    case "@group":
                        par.Value = myrequest.ParcelGroup;
                        break;
                    case "@ordinal":
                        par.Value = item.Formula.Order;
                        break;
                }
            foreach (SqlParameter par in myinsertupdateparams)
                switch (par.ParameterName)
                {
                    case "@formulaid":
                        par.Value = item.Formula.Id;
                        break;
                    case "@code":
                        par.Value = item.Formula.Code;
                        break;
                    case "@name":
                        par.Value = item.Formula.Name;
                        break;
                    case "@type":
                        par.Value = item.Formula.FormulaType;
                        break;
                    case "@formula1":
                        par.Value = item.Formula.Formula1;
                        break;
                    case "@formula2":
                        par.Value = item.Formula.Formula2;
                        break;
                    case "@value1":
                        par.Value = item.Value1;
                        break;
                    case "@value2":
                        par.Value = item.Value2;
                        break;
                    case "@isuser1":
                        par.Value = item.Value1User.HasValue;
                        break;
                    case "@isuser2":
                        par.Value = item.Value2User.HasValue;
                        break;
                    case "@afstamp":
                        par.Value = item.AFStamp;
                        break;
                }
            return true;
        }
    }

    public class AlgorithmGroupWeightDBM : lib.DBMExec
    {
        internal AlgorithmGroupWeightDBM() : base()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectProcedure = true;
            SelectCommandText = "dbo.AlgorithmGroup_sp";
            SelectParams = new SqlParameter[] {
                new SqlParameter("@group", System.Data.SqlDbType.Int),
                new SqlParameter("@requestid", System.Data.SqlDbType.Int),
                new SqlParameter("@weight", System.Data.SqlDbType.Money),
                new SqlParameter("@customspay", System.Data.SqlDbType.Money),
                new SqlParameter("@invoicediscount", System.Data.SqlDbType.Money),
                new SqlParameter("@volume", System.Data.SqlDbType.Money)
            };
            SelectParams[2].Direction = System.Data.ParameterDirection.Output;
            SelectParams[3].Direction = System.Data.ParameterDirection.Output;
            SelectParams[4].Direction = System.Data.ParameterDirection.Output;
            SelectParams[5].Direction = System.Data.ParameterDirection.Output;
        }

        private int? mygroup;
        internal int? ParcelGroup
        {
            set { mygroup = value; base.Execute(); }
            get { return mygroup; }
        }
        private Request myrequest;
        internal Request Request
        {
            set { myrequest = value; }
            get { return myrequest; }
        }
        public decimal? Weight
        { get { return DBNull.Value == this.SelectParams[2].Value ? (myrequest == null ? (decimal?)null : 0M) : (decimal)this.SelectParams[2].Value; } }
        public decimal? CustomsPay
        { get { return DBNull.Value == this.SelectParams[3].Value ? (myrequest == null ? (decimal?)null : 0M) : (decimal)this.SelectParams[3].Value; } }
        public decimal? InvoiceDiscount
        { get { return DBNull.Value == this.SelectParams[4].Value ? (myrequest == null ? (decimal?)null : 0M) : (decimal)this.SelectParams[4].Value; } }
        public decimal? Volume
        { get { return DBNull.Value == this.SelectParams[5].Value ? (myrequest == null ? (decimal?)null : 0M) : (decimal?)this.SelectParams[5].Value; } }

        protected override void PrepareFill(SqlConnection addcon)
        {
            this.SelectParams[0].Value = mygroup;
            this.SelectParams[1].Value = myrequest?.Id;
        }
    }

    public class AlgorithmRequestDBM : lib.DBManager<Algorithm,Algorithm>
    {
        internal AlgorithmRequestDBM(Request request) : base()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            myrequest = request;
            SelectProcedure = true;
            UpdateProcedure = true;
            SelectCommandText = "RequestAlgorithm_sp";
            UpdateCommandText = "RequestAlgorithmUpd_sp";
            SelectParams = new SqlParameter[] { new SqlParameter("@param1", System.Data.SqlDbType.Int) };
            UpdateParams = new SqlParameter[] { new SqlParameter("@param1", System.Data.SqlDbType.Int), new SqlParameter("@param2", System.Data.SqlDbType.NVarChar, 20) };
        }

        private Request myrequest;
        internal Request Request
        {
            set { myrequest = value; }
            get { return myrequest; }
        }

        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            this.SelectParams[0].Value = myrequest.Id;
        }
		protected override Algorithm CreateRecord(SqlDataReader reader)
		{
            return new Algorithm(0, lib.DomainObjectState.Sealed, reader.GetString(0), 0);
		}
        protected override Algorithm CreateModel(Algorithm reader, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
        {
			return reader;
        }
		protected override void LoadRecord(SqlDataReader reader, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
		{
			base.TakeItem(CreateModel(this.CreateRecord(reader), addcon, canceltasktoken));
		}
		protected override bool GetModels(System.Threading.CancellationToken canceltasktoken=default,Func<bool> reading=null)
		{
			return true;
		}
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override bool SaveIncludedObject(Algorithm item)
        {
            return true;
        }
        protected override bool SaveChildObjects(Algorithm item)
        {
            return true;
        }
        protected override bool SetParametersValue(Algorithm item)
        {
            myupdateparams[0].Value = myrequest.Id;
            myupdateparams[1].Value = item.Name;
            return true;
        }
        protected override void GetOutputParametersValue(Algorithm item)
        {
        }
        protected override void ItemAcceptChanches(Algorithm item)
        {
            item.AcceptChanches();
        }
    }

    public class AlgorithmValuesRequestVM : AlgorithmValuesVM
    {
        public AlgorithmValuesRequestVM(AlgorithmValuesRequest model) : base(model)
        {
        }

        public override Brush Value1Background
        {
            get
            {
                Brush brush = null;
                if ((this.DomainObject as AlgorithmValuesRequest).Value1Templ.HasValue && this.DomainObject.Value1 != (this.DomainObject as AlgorithmValuesRequest).Value1Templ)
                    brush = new System.Windows.Media.SolidColorBrush(lib.Common.MsOfficeHelper.StringToColor("#FFFDCFCF"));
                else
                    brush = base.Value1Background;
                return brush;
            }
        }
        public override Brush Value2Background
        {
            get
            {
                Brush brush = null;
                if ((this.DomainObject as AlgorithmValuesRequest).Value2Templ.HasValue && this.DomainObject.Value2 != (this.DomainObject as AlgorithmValuesRequest).Value2Templ)
                    brush = new System.Windows.Media.SolidColorBrush(lib.Common.MsOfficeHelper.StringToColor("#FFFDCFCF"));
                else
                    brush = base.Value2Background;
                return brush;
            }
        }

        protected override void RejectProperty(string property, object value)
        {
            if (property == "DependentOld")
            {
                lib.DomainObjectState state = this.DomainObject.DomainState;
                this.DomainObject.RejectChanges();
                if (state == lib.DomainObjectState.Added & this.DomainObject.DomainState == lib.DomainObjectState.Destroyed)
                    this.DomainObject.DomainState = state;
                this.PropertyChangedNotification("Value1Background");
                this.PropertyChangedNotification("Value2Background");
            }
        }
    }

    public class AlgorithmFormulaRequestCommand : AlgorithmFormulaCommand
    {
        internal AlgorithmFormulaRequestCommand(Request request) : base(true)
        {
            myrequest = request;
            App.Current.Dispatcher.Invoke(() =>
            {
                myadbm = new AlgorithmDBM();
                myformulasynchronizer = new FormulaSynchronizer();
                myvaluesstorage = new AlgorithmValuesStorage();
                myfdbm = new FormulaDBM();
                mywdbm = new AlgorithmWeightDBM();
                mygwdbm = new AlgorithmGroupWeightDBM();
                mygwdbm.Request = myrequest;
                myardbm = new AlgorithmRequestDBM(myrequest);
                myardbm.Request = myrequest;
                myalgorithms = new ObservableCollection<Algorithm>();
                myalgorithmformulas = new ObservableCollection<AlgorithmFormula>();
                myvdbm = new AlgorithmValuesRequestDBM(null, null, this.ValuesStorage, myrequest);
                mypdbm = new AlgorithmPropertyDBM();

                myview1 = new ListCollectionView(myalgorithmformulas);
                myview1.SortDescriptions.Add(new System.ComponentModel.SortDescription("Formula.Order", System.ComponentModel.ListSortDirection.Ascending));
                myview1.Filter = (object item) => { FormulaVM formula = (item as AlgorithmFormula).Formula; return formula.DomainObject.FormulaType < 100; };
                myview2 = new ListCollectionView(myalgorithmformulas);
                myview2.SortDescriptions.Add(new System.ComponentModel.SortDescription("Formula.Order", System.ComponentModel.ListSortDirection.Ascending));
                myview2.Filter = (object item) => { FormulaVM formula = (item as AlgorithmFormula).Formula; return formula.DomainObject.FormulaType > 100; };
                this.LoadData();
                myrequest.PropertyChanged += Request_PropertyChanged;
            });
        }

        private Request myrequest;
        internal Request Request { get { return myrequest; } }
        private AlgorithmRequestDBM myardbm;
        private AlgorithmGroupWeightDBM mygwdbm;
        internal AlgorithmGroupWeightDBM GroupWeightDBM
        { get { return mygwdbm; } }
        private AlgorithmWeightDBM mywdbm;
        private AlgorithmValuesRequestDBM myvdbm;
        private Algorithm myalgorithm;
        public Algorithm Algorithm
        {
            get { return myalgorithm; }
        }
        private bool myisreadonly;
        public override bool IsReadOnly
        {
            set { myisreadonly = value; PropertyChangedNotification("IsReadOnly"); }
            get { return myrequest.Status.Id > 499 | myisreadonly; }
        }
        public override bool FormulaIsReadOnly
        {
            get { return true; }
        }
        public override bool AlgorithmIsReadOnly
        {
            get { return true; }
        }
        public override Visibility WriterMenuVisible
        {
            get { return Visibility.Collapsed; }
        }
        public override Visibility SaveMenuVisible
        {
            get { return Visibility.Visible; }
        }
        #region Properties
        private decimal? mycbx;
        public decimal? CBX
        {
            set
            {
                if (mycbx.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mycbx.Value, value.Value)))
                {
                    mycbx = value;
                    foreach (AlgorithmValuesRequest item in this.Algorithm.Formulas)
                        if (item.Formula.Code == "П52" && !(item.Value1 == value))
                        {
                            item.Value1 = value;
                            break;
                        }
                    this.PropertyChangedNotification("CBX");
                }
            }
            get { return mycbx; }
        }
        private decimal? mycmr;
        public decimal? CMR
        {
            set
            {
                if (mycmr.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(mycmr.Value, value.Value)))
                {
                    mycmr = value;
                    foreach (AlgorithmValuesRequest item in this.Algorithm.Formulas)
                        if (item.Formula.Code == "П51" && !(item.Value1 == value))
                        {
                            item.Value1 = value;
                            break;
                        }
                    this.PropertyChangedNotification("CMR");
                }
            }
            get { return mycmr; }
        }
        private decimal? myex1t1;
        public decimal? EX1T1
        {
            set
            {
                if (myex1t1.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(myex1t1.Value, value.Value)))
                {
                    myex1t1 = value;
                    foreach (AlgorithmValuesRequest item in this.Algorithm.Formulas)
                        if (item.Formula.Code == "П53" && !(item.Value1 == value))
                        {
                            item.Value1 = value;
                            break;
                        }
                    this.PropertyChangedNotification("EX1T1");
                }
            }
            get { return myex1t1; }
        }
        #endregion
        #region RequestProperties
        private AlgorithmPropertyDBM mypdbm;
        private AlgorithmProperty myproperties;
        public AlgorithmProperty RequestProperties { get { return myproperties; } }
        #endregion

        public override bool SaveDataChanges()
        {
            bool isSuccess = true;
            if (myrequest.Status.Id < 500)
            {
                System.Text.StringBuilder err = new System.Text.StringBuilder();
                err.AppendLine("Изменения не сохранены");
                myvdbm.Errors.Clear();
                if (!myvdbm.SaveCollectionChanches())
                {
                    isSuccess = false;
                    err.AppendLine(myvdbm.ErrorMessage);
                }
                myalgorithm.DomainState = lib.DomainObjectState.Modified;
                myardbm.Errors.Clear();
                if (!myardbm.SaveItemChanches(myalgorithm))
                {
                    isSuccess = false;
                    err.AppendLine(myardbm.ErrorMessage);
                }
                //if (myproperties != null)
                //{
                //    mypdbm.CMD = this;
                //    mypdbm.Errors.Clear();
                //    if (!mypdbm.SaveItemChanches(myproperties))
                //    {
                //        isSuccess = false;
                //        err.AppendLine(mypdbm.ErrorMessage);
                //    }
                //    if (!isSuccess)
                //        this.PopupText = err.ToString();
                //}
                if (!isSuccess)
                    this.PopupText = err.ToString();
            }
            return isSuccess;
        }
        protected override bool CanSaveDataChanges()
        {
            return !this.IsReadOnly && myrequest.Status.Id < 500;
        }
        protected override void AddData(object parametr)
        {
            throw new NotImplementedException();
        }
        protected override bool CanAddData(object parametr)
        {
            return false;
        }
        protected override bool CanDeleteData(object parametr)
        {
            return false;
        }
        protected override void DeleteData(object parametr)
        {
            throw new NotImplementedException();
        }
        protected override void RefreshData(object parametr)
        {
            this.LoadData();
            if (this.PopupText == "Изменения сохранены") this.PopupText = string.Empty;
        }
        protected override bool CanRefreshData()
        {
            return true;
        }
        protected override bool CanRejectChanges()
        {
            return !this.IsReadOnly;
        }
        protected override void RejectChanges(object parametr)
        {
            base.RejectChanges(parametr);
            myproperties.RejectChanges();
        }

        protected new AlgorithmValuesRequest AlgorithmValuesCreate(Algorithm algorithm, Formula formula)
        {
            AlgorithmValuesRequest values = new AlgorithmValuesRequest(algorithm, formula, myrequest);
            myvdbm.Collection.Add(values);
            myvaluesstorage.UpdateItem(values);
            return values;
        }
        private void AlgorithmValuesPlus()
        {
            //AlgorithmValuesRequest[] valuess = new AlgorithmValuesRequest[] {
            //new AlgorithmValuesRequest(myalgorithm, new Formula(0, 0, lib.DomainObjectState.Sealed, "P1", "", 200, "П14+П13+П15+П17+П19+П20+П16", null), myrequest),
            //new AlgorithmValuesRequest(myalgorithm, new Formula(0, 0, lib.DomainObjectState.Sealed, "P2","",200, "П23+П22+П24+П25+П27+П28+П29",null), myrequest),
            //new AlgorithmValuesRequest(myalgorithm, new Formula(0, 0, lib.DomainObjectState.Sealed, "P3", "", 200, "П23+П22+П24+П35+П25+П27+П28+П29", null), myrequest),
            //new AlgorithmValuesRequest(myalgorithm, new Formula(0, 0, lib.DomainObjectState.Sealed, "P4", "", 200, "P2/П9", null), myrequest),
            //new AlgorithmValuesRequest(myalgorithm, new Formula(0, 0, lib.DomainObjectState.Sealed, "P5", "", 200, "P3/П9", null), myrequest)
            //};
            //foreach(AlgorithmValuesRequest item in valuess)
            //    item.FormulaInit();
        }
        protected void Request_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            System.Text.StringBuilder err = new System.Text.StringBuilder();
            switch (e.PropertyName)
            {
                case "AdditionalCost":
                    foreach (AlgorithmValuesRequest item in this.Algorithm.Formulas)
                        if (item.Formula.Code == "П20")
                        {
                            if (item.Value1 != myrequest.AdditionalCost)
                                item.Value1 = myrequest.AdditionalCost;
                            break;
                        }
                    break;
                case "CorrCost":
                    foreach (AlgorithmValuesRequest item in this.Algorithm.Formulas)
                        if (((myrequest.ServiceType == "ТЭО" && item.Formula.Code == "П30") || (myrequest.ServiceType == "ТД" && item.Formula.Code == "П39")))
                        {
                            if (item.Value1 != myrequest.CorrCost)
                                item.Value1 = myrequest.CorrCost;
                            break;
                        }
                    break;
                case "CustomsCost":
                    foreach (AlgorithmValuesRequest item in this.Algorithm.Formulas)
                        if (item.Formula.Code == "П12")
                        {
                            if (item.Value1 != myrequest.CustomsCost)
                                item.Value1 = myrequest.CustomsCost;
                            break;
                        }
                    break;
                case "FreightCost":
                    foreach (AlgorithmValuesRequest item in this.Algorithm.Formulas)
                        if (item.Formula.Code == "П17")
                        {
                            if (item.Value1 != myrequest.FreightCost)
                                item.Value1 = myrequest.FreightCost;
                            break;
                        }
                    break;
                case "InvoiceDiscount":
                    mygwdbm.Errors.Clear();
                    foreach (AlgorithmValuesRequest item in this.Algorithm.Formulas)
                        if (item.Formula.Code == "П9")
                        {
                            if (myrequest.ParcelGroup.HasValue)
                            {
                                mygwdbm.ParcelGroup = myrequest.ParcelGroup;
                                if (mygwdbm.Errors.Count > 0)
                                    err.AppendLine(mygwdbm.ErrorMessage);
                                else
                                    item.Value1Templ = (mygwdbm.InvoiceDiscount ?? 0M) + (myrequest.InvoiceDiscount ?? 0M);
                            }
                            else
                                item.Value1Templ = myrequest.InvoiceDiscount;
                            break;
                        }
                    break;
                case "ParcelGroup":
                    this.SaveRefresh.Execute(null);
                    break;
                case "PreparatnCost":
                    foreach (AlgorithmValuesRequest item in this.Algorithm.Formulas)
                        if (item.Formula.Code == "П19")
                        {
                            if (item.Value1 != myrequest.PreparatnCost)
                                item.Value1 = myrequest.PreparatnCost;
                            break;
                        }
                    break;
                case "OfficialWeight":
                    mywdbm.Errors.Clear();
                    if (myrequest.ParcelGroup.HasValue)
                    {
                        mygwdbm.Errors.Clear();
                        mygwdbm.ParcelGroup = myrequest.ParcelGroup;
                        if (mygwdbm.Errors.Count > 0)
                            err.AppendLine(mygwdbm.ErrorMessage);
                        else
                            mywdbm.Weight = mygwdbm.Weight;
                    }
                    else
                        mywdbm.Weight = myrequest.OfficialWeight;
                    if (mywdbm.Errors.Count > 0)
                        err.AppendLine(mywdbm.ErrorMessage);
                    else
                    {
                        if (myalgorithm.Id != mywdbm.AlgorithmId)
                            this.RefreshData(null);
                        else
                            foreach (AlgorithmValuesRequest item in this.Algorithm.Formulas)
                                if (item.Formula.Code == "П10")
                                {
                                    item.Value1Templ = myrequest.OfficialWeight;
                                    break;
                                }
                    }
                    if (err.Length > 0)
                    {
                        err.Insert(0, "Данные не загружены/n");
                        this.OpenPopup(err.ToString(), true);
                    }
                    break;
                case "SertificatCost":
                    foreach (AlgorithmValuesRequest item in this.Algorithm.Formulas)
                        if (item.Formula.Code == "П18")
                        {
                            if (item.Value1 != myrequest.SertificatCost)
                                item.Value1 = myrequest.SertificatCost;
                            break;
                        }
                    break;
                case "ServiceType":
                    foreach (AlgorithmValuesRequest item in this.Algorithm.Formulas)
                        switch (item.Formula.Code)
                        {
                            case "П11":
                                if (myrequest.ServiceType == "ТД")
                                    item.Value1Templ = 3;
                                else
                                    item.Value1Templ = null;
                                break;
                            case "П21":
                                if (myrequest.ServiceType == "ТЭО")
                                    myrequest.TotalCost = item.Value1;
                                else myrequest.TotalCost = null;
                                break;
                            case "П30":
                                if (myrequest.ServiceType == "ТЭО")
                                    myrequest.CorrCost = item.Value1;
                                break;
                            case "П31":
                                if (myrequest.ServiceType == "ТЭО")
                                {
                                    if (!string.IsNullOrEmpty(myrequest.Consolidate) & item.Value1.HasValue & item.Value2.HasValue && myrequest.AlgorithmCMD != null & myrequest.AlgorithmConCMD != null)
                                    {
                                        decimal? p40 = null;
                                        foreach (AlgorithmValuesRequestCon values in myrequest.AlgorithmConCMD.Algorithm.Formulas)
                                        {
                                            if (values.Formula.Code == "П31")
                                            {
                                                if (values.Value2.HasValue)
                                                    p40 = item.Value1.Value * decimal.Divide(values.Value2.Value, item.Value2.Value) - item.Value1.Value;
                                                break;
                                            }
                                        }
                                        if (p40.HasValue)
                                            foreach (AlgorithmValuesRequest values in myrequest.AlgorithmCMD.Algorithm.Formulas)
                                            {
                                                if (values.Formula.Code == "П30" & !values.Value1User.HasValue)
                                                {
                                                    values.Value1Templ = (values.Value1Templ ?? 0M) + p40.Value;
                                                    break;
                                                }
                                            }
                                    }
                                    myrequest.TotalPay = item.Value1;
                                    if (item.Value2.HasValue)
                                        myrequest.TotalPayInvoicePer = decimal.Divide(item.Value2.Value, 100M);
                                    else
                                        myrequest.TotalPayInvoicePer = item.Value2;
                                }
                                break;
                            case "П33":
                                if (myrequest.ServiceType == "ТЭО")
                                {
                                    myrequest.IncomePay = item.Value1;
                                    if (item.Value2.HasValue)
                                        myrequest.Income = decimal.Divide(item.Value2.Value, 100M);
                                    else
                                        myrequest.Income = item.Value2;
                                }
                                break;
                            case "П34":
                                if (myrequest.ServiceType == "ТЭО")
                                    myrequest.IncomeM3 = item.Value1;
                                break;
                            case "П35":
                                if (myrequest.ServiceType == "ТД")
                                {
                                    myrequest.TDPay = item.Value1;
                                }
                                else
                                {
                                    myrequest.TDPay = null;
                                }
                                break;
                            case "П39":
                                if (myrequest.ServiceType == "ТД") myrequest.CorrCost = item.Value1;
                                break;
                            case "П40":
                                if (myrequest.ServiceType == "ТД")
                                {
                                    if (!string.IsNullOrEmpty(myrequest.Consolidate) & item.Value1.HasValue & item.Value2.HasValue && myrequest.AlgorithmCMD != null & myrequest.AlgorithmConCMD != null)
                                    {
                                        decimal? p40 = null;
                                        foreach (AlgorithmValuesRequestCon values in myrequest.AlgorithmConCMD.Algorithm.Formulas)
                                        {
                                            if (values.Formula.Code == "П40")
                                            {
                                                if (values.Value2.HasValue)
                                                    p40 = item.Value1.Value * decimal.Divide(values.Value2.Value, item.Value2.Value) - item.Value1.Value;
                                                break;
                                            }
                                        }
                                        if (p40.HasValue)
                                            foreach (AlgorithmValuesRequest values in myrequest.AlgorithmCMD.Algorithm.Formulas)
                                            {
                                                if (values.Formula.Code == "П39" & !values.Value1User.HasValue)
                                                {
                                                    values.Value1Templ = (values.Value1Templ ?? 0M) + p40.Value;
                                                    break;
                                                }
                                            }
                                    }
                                    myrequest.TotalPay = item.Value1;
                                    if (item.Value2.HasValue)
                                        myrequest.TotalPayInvoicePer = decimal.Divide(item.Value2.Value, 100M);
                                    else
                                        myrequest.TotalPayInvoicePer = item.Value2;
                                }
                                break;
                            case "П47":
                                if (myrequest.ServiceType == "ТД")
                                    myrequest.IncomePay = item.Value1;
                                if (item.Value2.HasValue)
                                    myrequest.Income = decimal.Divide(item.Value2.Value, 100M);
                                else
                                    myrequest.Income = item.Value2;
                                break;
                            case "П48":
                                if (myrequest.ServiceType == "ТД")
                                    myrequest.IncomeM3 = item.Value1;
                                break;
                        }
                    break;
            }
        }
        private void LoadData()
        {
            AlgorithmValuesRequest values49 = null; // update after RequestProperties init
            System.Text.StringBuilder err = new System.Text.StringBuilder();
            err.AppendLine("Данные не загружены");
            // удаление значений и ссылок на Request
            try
            {
                if (myrequest.Status.Id < 500)
                {    // определение веса
                    if (myrequest.ParcelGroup.HasValue)
                    {
                        mygwdbm.ParcelGroup = myrequest.ParcelGroup;
                        if (mygwdbm.Errors.Count > 0) err.AppendLine(mygwdbm.ErrorMessage);
                        mywdbm.Weight = (mygwdbm.Weight ?? 0M) + (myrequest.OfficialWeight ?? 0M);
                    }
                    else
                        mywdbm.Weight = myrequest.OfficialWeight;
                    if (mywdbm.Errors.Count > 0) err.AppendLine(mywdbm.ErrorMessage);
                    // получаем алгоритм
                    myadbm.Errors.Clear();
                    myadbm.ItemId = mywdbm.AlgorithmId;
                    myalgorithm = myadbm.GetFirst();
                    if (myadbm.Errors.Count > 0) err.AppendLine(myadbm.ErrorMessage);
                    //myfdbm.Errors.Clear();
                    //myfdbm.Fill();
                    //if (myfdbm.Errors.Count > 0) err.AppendLine(myfdbm.ErrorMessage);
                    //myformulasynchronizer.DomainCollection = myfdbm.Collection;
                }
                else
                {
                    myalgorithm = myardbm.GetFirst();
                    //myalgorithm.Formulas.Clear();
                    //if (myformulasynchronizer.DomainCollection == null)
                    //    myformulasynchronizer.DomainCollection = new ObservableCollection<Formula>();
                    //else
                    //    myformulasynchronizer.DomainCollection?.Clear();
                }
                if (myformulasynchronizer.DomainCollection == null)
                    myformulasynchronizer.DomainCollection = new ObservableCollection<Formula>();
                else
                    myformulasynchronizer.DomainCollection?.Clear();
                myalgorithms.Clear();
                if (myalgorithm != null) myalgorithms.Add(myalgorithm);
                this.PropertyChangedNotification("Algorithms");
                //Загружаем значения
                myvdbm.Algorithm = myalgorithm;
                myvdbm.Formulas = myformulasynchronizer.DomainCollection;
                myvdbm.Errors.Clear();
                if (myvdbm.Collection?.Count > 0)
                    foreach (AlgorithmValuesRequest vals in myvdbm.Collection) // удаление ссылок на Request
                        vals.Dispose();
                myvdbm.Fill();
                if (myvdbm.Errors.Count > 0) err.AppendLine(myvdbm.ErrorMessage);
                // формируем коллекцию для отображения
                myalgorithmformulas.Clear();
                AlgorithmValuesRequest values = null;
                foreach (FormulaVM frm in myformulasynchronizer.ViewModelCollection)
                {
                    AlgorithmFormula algfrm = new AlgorithmFormula(frm, lib.DomainObjectState.Unchanged);
                    myalgorithmformulas.Add(algfrm);
                    values = null;
                    foreach (AlgorithmValuesRequest vals in myvdbm.Collection)
                    {
                        if (vals.Formula == frm.DomainObject)
                        {
                            values = vals;
                            break;
                        }
                    }
                    if (values == null) values = AlgorithmValuesCreate(myalgorithm, frm.DomainObject);
                    if (myrequest.Status.Id < 500)
                    {
                        switch (frm.Code)
                        {
                            case "П9":
                                if (myrequest.ParcelGroup.HasValue)
                                    values.Value1Templ = (mygwdbm.InvoiceDiscount ?? 0M) + (myrequest.InvoiceDiscount ?? 0M);
                                else
                                    values.Value1Templ = myrequest.InvoiceDiscount;
                                break;
                            case "П10":
                                values.Value1Templ = mywdbm.Weight;
                                break;
                            case "П11":
                                if (myrequest.ServiceType == "ТД")
                                    values.Value1Templ = 3;
                                else
                                    values.Value1Templ = null;
                                break;
                            //case "П12":
                            //    if (!string.IsNullOrEmpty(myrequest.Consolidate) & values.Value1.HasValue & myrequest.AlgorithmConCMD != null)
                            //    {
                            //        decimal? cx = null;
                            //        foreach (AlgorithmValuesRequest convalues in myrequest.AlgorithmConCMD.Algorithm.Formulas)
                            //            if (convalues.Formula.Code == "X1")
                            //            {
                            //                cx = convalues.Value1;
                            //                break;
                            //            }
                            //        if (cx.HasValue)
                            //            foreach (AlgorithmValuesRequest values9 in this.Algorithm.Formulas)
                            //                if (values9.Formula.Code == "П9")
                            //                {
                            //                    if (values9.Value1.HasValue) values.Value1Templ = cx.Value * values9.Value1.Value;
                            //                    break;
                            //                }
                            //    }
                            //    break;
                            case "П14": // если не заполнен Транспорт на машину считаем по старому
                                if (myrequest.Parcel != null && myrequest.Importer != null) // update properties depend on Parcel
                                {
                                    values.InitLater = true;
                                    if (myrequest.Importer.Id == 1)
                                    {
                                        if (myrequest.Parcel.TransportTUn.HasValue) // old algoritm if in Parcel missing Transport
                                        {
                                            values.FuncValue1 = (string error) => {
                                                if (myrequest.Parcel.RequestsIsLoaded)
                                                    return (myrequest.Parcel.TransportTUn ?? 0M) * ((myrequest.ParcelGroup.HasValue ? (mygwdbm.Volume ?? 0M) : 0M) + (myrequest.Volume ?? 0M));
                                                else
                                                { 
                                                    error = "Не все заявки загружены";
                                                    return 0M;
                                                }
                                            };
                                            values.Formula.Formula1 = @"{Трансп. Расходы * Объем}";
                                        }
                                    }
                                    else
                                    {
                                        if (myrequest.Parcel.TransportDUn.HasValue)
                                        {
                                            values.FuncValue1 = (string error) => {
                                                if (myrequest.Parcel.RequestsIsLoaded)
                                                    return (myrequest.Parcel.TransportDUn??0M) * ((myrequest.ParcelGroup.HasValue ? (mygwdbm.Volume ?? 0M) : 0M) + (myrequest.Volume ?? 0M));
                                                else
                                                { 
                                                    error = "Не все заявки загружены";
                                                    return 0M; 
                                                }
                                            };
                                            values.Formula.Formula1 = @"{Трансп. Расходы * Объем}";
                                        }
                                    }
                                    values.InitLater = false;
                                    myrequest.Parcel.PropertyChanged += Parcel_14PropertyChanged;
                                }
                                break;
                            //case "П17":
                            //    if (!string.IsNullOrEmpty(myrequest.Consolidate) & values.Value1.HasValue & myrequest.AlgorithmConCMD != null)
                            //    {
                            //        decimal? cx = null;
                            //        foreach (AlgorithmValuesRequest convalues in myrequest.AlgorithmConCMD.Algorithm.Formulas)
                            //            if (convalues.Formula.Code == "X2")
                            //            {
                            //                cx = convalues.Value1;
                            //                break;
                            //            }
                            //        if (cx.HasValue)
                            //            foreach (AlgorithmValuesRequest values9 in this.Algorithm.Formulas)
                            //                if (values9.Formula.Code == "П9")
                            //                {
                            //                    if (values9.Value1.HasValue) values.Value1Templ = cx.Value * values9.Value1.Value;
                            //                    break;
                            //                }
                            //    }
                            //    break;
                            //case "П18":
                            //    if (!string.IsNullOrEmpty(myrequest.Consolidate) & values.Value1.HasValue & myrequest.AlgorithmConCMD != null)
                            //    {
                            //        decimal? cx = null;
                            //        foreach (AlgorithmValuesRequest convalues in myrequest.AlgorithmConCMD.Algorithm.Formulas)
                            //            if (convalues.Formula.Code == "X3")
                            //            {
                            //                cx = convalues.Value1;
                            //                break;
                            //            }
                            //        if (cx.HasValue)
                            //            foreach (AlgorithmValuesRequest values9 in this.Algorithm.Formulas)
                            //                if (values9.Formula.Code == "П9")
                            //                {
                            //                    if (values9.Value1.HasValue) values.Value1Templ = cx.Value * values9.Value1.Value;
                            //                    break;
                            //                }
                            //    }
                            //    break;
                            //case "П19":
                            //    if (!string.IsNullOrEmpty(myrequest.Consolidate) & values.Value1.HasValue & myrequest.AlgorithmConCMD != null)
                            //    {
                            //        decimal? cx = null;
                            //        foreach (AlgorithmValuesRequest convalues in myrequest.AlgorithmConCMD.Algorithm.Formulas)
                            //            if (convalues.Formula.Code == "X4")
                            //            {
                            //                cx = convalues.Value1;
                            //                break;
                            //            }
                            //        if (cx.HasValue)
                            //            foreach (AlgorithmValuesRequest values9 in this.Algorithm.Formulas)
                            //                if (values9.Formula.Code == "П9")
                            //                {
                            //                    if (values9.Value1.HasValue) values.Value1Templ = cx.Value * values9.Value1.Value;
                            //                    break;
                            //                }
                            //    }
                            //    break;
                            //case "П20":
                            //    if (!string.IsNullOrEmpty(myrequest.Consolidate) & values.Value1.HasValue & myrequest.AlgorithmConCMD != null)
                            //    {
                            //        decimal? cx = null;
                            //        foreach (AlgorithmValuesRequest convalues in myrequest.AlgorithmConCMD.Algorithm.Formulas)
                            //            if (convalues.Formula.Code == "X5")
                            //            {
                            //                cx = convalues.Value1;
                            //                break;
                            //            }
                            //        if (cx.HasValue)
                            //            foreach (AlgorithmValuesRequest values9 in this.Algorithm.Formulas)
                            //                if (values9.Formula.Code == "П9")
                            //                {
                            //                    if (values9.Value1.HasValue) values.Value1Templ = cx.Value * values9.Value1.Value;
                            //                    break;
                            //                }
                            //    }
                            //    break;
                            case "П30":
                                if (values.Value1User == null && myrequest.ServiceType == "ТЭО" && !string.IsNullOrEmpty(myrequest.Consolidate) & myrequest.AlgorithmConCMD != null)
                                {
                                    decimal? p31 = null, p231 = null, c231 = null;
                                    foreach (AlgorithmValuesRequest values31 in this.Algorithm.Formulas)
                                    {
                                        if (values31.Formula.Code == "П31")
                                        {
                                            p31 = values31.Value1;
                                            p231 = values31.Value2;
                                            break;
                                        }
                                    }
                                    if (p31.HasValue & p231.HasValue)
                                    {
                                        foreach (AlgorithmValuesRequestCon values31 in myrequest.AlgorithmConCMD.Algorithm.Formulas)
                                        {
                                            if (values31.Formula.Code == "П31")
                                            {
                                                c231 = values31.Value2;
                                                break;
                                            }
                                        }
                                        if (c231.HasValue)
                                        {
                                            values.Value1Templ = (values.Value1Templ ?? 0M) + p31.Value * decimal.Divide(c231.Value, p231.Value) - p31.Value;
                                        }
                                    }
                                }
                                break;
                            case "П39":
                                if (values.Value1User == null && myrequest.ServiceType == "ТД" && !string.IsNullOrEmpty(myrequest.Consolidate) & myrequest.AlgorithmConCMD != null)
                                {
                                    decimal? p31 = null, p231 = null, c231 = null;
                                    foreach (AlgorithmValuesRequest values31 in this.Algorithm.Formulas)
                                    {
                                        if (values31.Formula.Code == "П40")
                                        {
                                            p31 = values31.Value1;
                                            p231 = values31.Value2;
                                            break;
                                        }
                                    }
                                    if (p31.HasValue & p231.HasValue)
                                    {
                                        foreach (AlgorithmValuesRequestCon values31 in myrequest.AlgorithmConCMD.Algorithm.Formulas)
                                        {
                                            if (values31.Formula.Code == "П40")
                                            {
                                                c231 = values31.Value2;
                                                break;
                                            }
                                        }
                                        if (c231.HasValue)
                                        {
                                            values.Value1Templ = (values.Value1Templ ?? 0M) + p31.Value * decimal.Divide(c231.Value, p231.Value) - p31.Value;
                                        }
                                    }
                                }
                                break;
                            //case "П49": // занести в алгоритм и убрать после развертывания новой версии
                            //    //values49 = values;
                            //    if (values.Formula.Formula1 == "П14+П13+П15+П17+П19+П20+П16+{CMR+СВХ+EX1/T1}")
                            //    {
                            //        values.FuncValue1 = (string error) => { return (this.RequestProperties?.CBX ?? 0M) + (this.RequestProperties?.CMR ?? 0M) + (this.RequestProperties?.EX1T1 ?? 0M); };
                            //    }
                            //    break;
                            //default:
                            //    values.FormulaInit();
                            //    break;
                        }
                        values.FormulaInit();
                    }
                    algfrm.AlgorithmValues.Add(new AlgorithmValuesRequestVM(values));
                }
                // Зависимые от консолидации
                if (myrequest.Status.Id < 500 && !string.IsNullOrEmpty(myrequest.Consolidate) & myrequest.AlgorithmConCMD?.Algorithm != null)
                {
                    int i = 0;
                    decimal? p9 = null, p10 = null, cx = null;
                    foreach (AlgorithmValuesRequest v in this.Algorithm.Formulas)
                    {
                        if (v.Formula.Code == "П9")
                        {
                            p9 = v.Value1;
                            i++;
                        }
                        else if (v.Formula.Code == "П10")
                        {
                            p10 = v.Value1;
                            i++;
                        }
                        if (i > 1) break;
                    }
                    foreach (AlgorithmValuesRequest v in this.Algorithm.Formulas)
                    {
                        if (!v.Value1User.HasValue)
                        {
                            if (p9.HasValue)
                                switch (v.Formula.Code)
                                {
                                    case "П12":
                                        foreach (AlgorithmValuesRequest convalues in myrequest.AlgorithmConCMD.Algorithm.Formulas)
                                        {
                                            if (convalues.Formula.Code == "X1")
                                            { 
                                                cx = convalues.Value1;
                                                break;
                                            }
                                        }
                                        if (cx.HasValue)
                                        {
                                            v.Value1Templ = p9 * cx.Value;
                                            v.isValid1 = true;
                                        }
                                        break;
                                    case "П17":
                                        foreach (AlgorithmValuesRequest convalues in myrequest.AlgorithmConCMD.Algorithm.Formulas)
                                        {
                                            if (convalues.Formula.Code == "X2")
                                            {
                                                cx = convalues.Value1;
                                                break;
                                            }
                                        }
                                        if (cx.HasValue)
                                        {
                                            v.Value1Templ = p9 * cx.Value;
                                            v.isValid1 = true;
                                        }
                                        break;
                                    case "П18":
                                        foreach (AlgorithmValuesRequest convalues in myrequest.AlgorithmConCMD.Algorithm.Formulas)
                                        {
                                            if (convalues.Formula.Code == "X3")
                                            {
                                                cx = convalues.Value1;
                                                break;
                                            }
                                        }
                                        if (cx.HasValue)
                                        {
                                            v.Value1Templ = p9 * cx.Value;
                                            v.isValid1 = true;
                                        }
                                        break;
                                    case "П19":
                                        foreach (AlgorithmValuesRequest convalues in myrequest.AlgorithmConCMD.Algorithm.Formulas)
                                        {
                                            if (convalues.Formula.Code == "X4")
                                            {
                                                cx = convalues.Value1;
                                                break;
                                            }
                                        }
                                        if (cx.HasValue)
                                        {
                                            v.Value1Templ = p9 * cx.Value;
                                            v.isValid1 = true;
                                        }
                                        break;
                                    case "П20":
                                        foreach (AlgorithmValuesRequest convalues in myrequest.AlgorithmConCMD.Algorithm.Formulas)
                                            if (convalues.Formula.Code == "X5")
                                            {
                                                cx = convalues.Value1;
                                                break;
                                            }
                                        if (cx.HasValue)
                                        {
                                            v.Value1Templ = p9 * cx.Value;
                                            v.isValid1 = true;
                                        }
                                        break;
                                    case "Р3":
                                        foreach (AlgorithmValuesRequest convalues in myrequest.AlgorithmConCMD.Algorithm.Formulas)
                                            if (convalues.Formula.Code == "R3")
                                            {
                                                cx = convalues.Value1;
                                                break;
                                            }
                                        if (cx.HasValue)
                                        {
                                            v.Value1Templ = p9 * cx.Value;
                                            v.isValid1 = true;
                                        }
                                        break;
                                    case "Р4":
                                        foreach (AlgorithmValuesRequest convalues in myrequest.AlgorithmConCMD.Algorithm.Formulas)
                                            if (convalues.Formula.Code == "R4")
                                            {
                                                cx = convalues.Value1;
                                                break;
                                            }
                                        if (cx.HasValue)
                                        {
                                            v.Value1Templ = p9 * cx.Value;
                                            v.isValid1 = true;
                                        }
                                        break;
                                }
                            if (p10.HasValue)
                                switch (v.Formula.Code)
                                {
                                    case "П13":
                                        foreach (AlgorithmValuesRequest convalues in myrequest.AlgorithmConCMD.Algorithm.Formulas)
                                        {
                                            if (convalues.Formula.Code == "W13")
                                                cx = convalues.Value1;
                                        }
                                        if (cx.HasValue)
                                        {
                                            v.Value1Templ = p10 * cx.Value;
                                            v.isValid1 = true;
                                        }
                                        break;
                                    case "П22":
                                        foreach (AlgorithmValuesRequest convalues in myrequest.AlgorithmConCMD.Algorithm.Formulas)
                                        {
                                            if (convalues.Formula.Code == "W22")
                                                cx = convalues.Value1;
                                        }
                                        if (cx.HasValue)
                                        {
                                            //values.Formula.Formula1 = string.Empty;
                                            v.Value1Templ = p10 * cx.Value;
                                            v.isValid1 = true;
                                        }
                                        break;
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            { err.AppendLine(ex.Message); }
            //AlgorithmValuesPlus();

            //mypdbm.CMD = this;
            AlgorithmProperty property = null;//mypdbm.GetFirst()
            if (myproperties == null && property == null)
            {
                myproperties = new AlgorithmProperty(this);
                this.PropertyChangedNotification(nameof(this.RequestProperties));
            }
            else if (myproperties == null)
            {
                myproperties = property;
                this.PropertyChangedNotification(nameof(this.RequestProperties));
            }
            else if (property != null)
                myproperties.UpdateProperties(property);
            //values49?.FormulaInit();

            myview1.MoveCurrentToPosition(-1);
            myview2.MoveCurrentToPosition(-1);
            if (err.Length > 22)
                this.PopupText = err.ToString();
            else
                this.Save.Execute(null);
        }

        private void Parcel_14PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TransportTUn":
                case "TransportDUn":
                case "RequestsIsLoaded":
                    foreach (AlgorithmValuesRequest values in this.Algorithm.Formulas)
                        if (values.Formula.Code == "П14")
                        {
                            values.SetValue1();
                            break;
                        }
                    break;
            }
        }
    }

    public class AlgorithmProperty : lib.DomainStampValueChanged
    {
        internal AlgorithmProperty(int id, Int64 stamp, lib.DomainObjectState domainstate
            , AlgorithmFormulaRequestCommand cmd
            , decimal? cbx, bool cbxisuser, decimal? cmr, bool cmrisuser, decimal? ex1t1, bool ex1t1isuser
            ) : base(id, stamp, null, null, domainstate)
        {
            mycmd = cmd;

            mycmr = cmr;
            mycbxuser = cbxisuser;
            mycbx = cbx;
            mycmruser = cmrisuser;
            myex1t1 = ex1t1;
            myex1t1user = ex1t1isuser;

            this.InitProperties();
        }
        internal AlgorithmProperty(AlgorithmFormulaRequestCommand cmd) : this(lib.NewObjectId.NewId, 0, lib.DomainObjectState.Added, cmd, null, false, 20M, false, null, false) { }

        private AlgorithmFormulaRequestCommand mycmd;

        private decimal? mycbx;
        public decimal? CBX
        {
            set { 
                SetProperty<decimal?>(ref mycbx, value, () =>
                {
                    foreach (AlgorithmValuesRequest item in mycmd.Algorithm.Formulas)
                        if (item.Formula.Code == "П52" && !(item.Value1 == value))
                        {
                            item.Value1 = value;
                            break;
                        }
                }
                    ); 
            } get { return mycbx; } }
        private decimal? mycbx2;
        public decimal? CBX2
        {
            set
            {
                SetProperty<decimal?>(ref mycbx2, value
                //    , () =>
                //{
                //    foreach (AlgorithmValuesRequest item in mycmd.Algorithm.Formulas)
                //        if (item.Formula.Code == "П52" && !(item.Value2 == value))
                //        {
                //            item.Value2 = value;
                //            break;
                //        }
                //}
                    );
            }
            get { return mycbx2; }
        }
        private bool mycbxuser;
        public bool CBXUser
        { set {
                SetProperty<bool>(ref mycbxuser, value, () =>
                {

                }
                    ); } get { return mycbxuser; } }
        private decimal? mycmr;
        public decimal? CMR
        { set {
                SetProperty<decimal?>(ref mycmr, value,() =>
                {
                    foreach (AlgorithmValuesRequest item in mycmd.Algorithm.Formulas)
                        if (item.Formula.Code == "П51" && !(item.Value1 == value))
                        {
                            item.Value1 = value;
                            break;
                        }
                    this.SetDeliveryTotal();
                }
                ); } get { return mycmr; } }
        private decimal? mycmr2;
        public decimal? CMR2
        {
            set
            {
                SetProperty<decimal?>(ref mycmr2, value
                //    , () =>
                //{
                //    foreach (AlgorithmValuesRequest item in mycmd.Algorithm.Formulas)
                //        if (item.Formula.Code == "П51" && !(item.Value2 == value))
                //        {
                //            item.Value2 = value;
                //            break;
                //        }
                //    //this.SetDeliveryTotal();
                //}
                );
            }
            get { return mycmr2; }
        }
        private bool mycmruser;
        public bool CMRUser
        { set { SetProperty<bool>(ref mycmruser, value); } get { return mycmruser; } }
        private decimal? myex1t1;
        public decimal? EX1T1
        { set { SetProperty<decimal?>(ref myex1t1, value, () =>
            {
                foreach (AlgorithmValuesRequest item in mycmd.Algorithm.Formulas)
                    if (item.Formula.Code == "П53" && !(item.Value1 == value))
                    {
                        item.Value1 = value;
                        break;
                    }
                this.SetDeliveryTotal();
            }
            ); } get { return myex1t1; } }
        private decimal? myex1t12;
        public decimal? EX1T12
        {
            set
            {
                SetProperty<decimal?>(ref myex1t12, value
                //    , () =>
                //{
                //    foreach (AlgorithmValuesRequest item in mycmd.Algorithm.Formulas)
                //        if (item.Formula.Code == "П53" && !(item.Value2 == value))
                //        {
                //            item.Value2 = value;
                //            break;
                //        }
                //    //this.SetDeliveryTotal();
                //}
            );
            }
            get { return myex1t12; }
        }
        private bool myex1t1user;
        public bool EX1T1User
        { set { SetProperty<bool>(ref myex1t1user, value); } get { return myex1t1user; } }
        private decimal? mydeliverytotal;
        public decimal DeliveryTotal
        { get {
                if (!mydeliverytotal.HasValue)
                    this.SetDeliveryTotal();
                return mydeliverytotal.Value; } }
        internal void SetDeliveryTotal()
        {
            string name = nameof(AlgorithmProperty.DeliveryTotal);
            decimal oldvalue = mydeliverytotal??0M;
            mydeliverytotal = (mycmd.Request.DeliveryCost ?? 0M) + (mycmd.Request.FreightCost ?? 0M) + (this.CMR ?? 0) + (this.EX1T1 ?? 0M) + (mycmd.Request.PreparatnCost ?? 0M) + (mycmd.Request.AdditionalCost ?? 0M);
            this.PropertyChangedNotification(name);
            this.OnValueChanged(name, oldvalue, mydeliverytotal);
        }

        private void InitProperties()
        {
            foreach (AlgorithmValuesRequest item in mycmd.Algorithm?.Formulas)
                switch(item.Formula.Code)
                {
                    case "П51":
                        mycmr = item.Value1;
                        mycmr2 = item.Value2;
                        break;
                    case "П52":
                        mycbx = item.Value1;
                        mycbx2 = item.Value2;
                        break;
                    case "П53":
                        myex1t1 = item.Value1;
                        myex1t12 = item.Value2;
                        break;
                }
        }

        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            AlgorithmProperty temp = (AlgorithmProperty)sample;
            this.CBX = temp.CBX;
            this.CBXUser = temp.CBXUser;
            this.CMR = temp.CMR;
            this.CMRUser = temp.CMRUser;
            this.EX1T1 = temp.EX1T1;
            this.EX1T1User = temp.EX1T1User;
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case nameof(this.CBX):
                    mycbx = (decimal?)value;
                    break;
                case nameof(this.CBXUser):
                    mycbxuser = (bool)value;
                    break;
                case nameof(this.CMR):
                    mycmr = (decimal?)value;
                    break;
                case nameof(this.CMRUser):
                    mycmruser = (bool)value;
                    break;
                case nameof(this.EX1T1):
                    myex1t1 = (decimal?)value;
                    break;
                case nameof(this.EX1T1User):
                    myex1t1user = (bool)value;
                    break;
            }
        }
    }
    internal class AlgorithmPropertyDBM : lib.DBManagerStamp<AlgorithmProperty,AlgorithmProperty>
    {
        private AlgorithmFormulaRequestCommand mycmd;
        internal AlgorithmFormulaRequestCommand CMD
        { set { mycmd = value; } get { return mycmd; } }

        internal AlgorithmPropertyDBM()
        {
            this.NeedAddConnection = false;
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;
            SelectCommandText = "dbo.RequestAlgorithmProperty_sp";
            InsertCommandText = "dbo.RequestAlgorithmPropertyAdd_sp";
            UpdateCommandText = "dbo.RequestAlgorithmPropertyUpd_sp";
            DeleteCommandText = "dbo.RequestAlgorithmPropertyDel_sp";
            SelectParams = new SqlParameter[] { new SqlParameter("@request", System.Data.SqlDbType.Int), new SqlParameter("@group", System.Data.SqlDbType.NVarChar, 5) };
            InsertParams = new SqlParameter[] { InsertParams[0], new SqlParameter("@request", System.Data.SqlDbType.Int), new SqlParameter("@group", System.Data.SqlDbType.NVarChar, 5) };
            UpdateParams = new SqlParameter[] {
                UpdateParams[0],
                new SqlParameter("@cbxupd", System.Data.SqlDbType.Bit),
                new SqlParameter("@cbxisuserupd", System.Data.SqlDbType.Bit),
                new SqlParameter("@cmrupd", System.Data.SqlDbType.Bit),
                new SqlParameter("@cmrisuserupd", System.Data.SqlDbType.Bit),
                new SqlParameter("@ex1t1upd", System.Data.SqlDbType.Bit),
                new SqlParameter("@ex1t1isuserupd", System.Data.SqlDbType.Bit)
            };
            InsertUpdateParams = new SqlParameter[] {
                new SqlParameter("@cbx", System.Data.SqlDbType.Money),
                new SqlParameter("@cbxisuser", System.Data.SqlDbType.Bit),
                new SqlParameter("@cmr", System.Data.SqlDbType.Money),
                new SqlParameter("@cmrisuser", System.Data.SqlDbType.Bit),
                new SqlParameter("@ex1t1", System.Data.SqlDbType.Money),
                new SqlParameter("@ex1t1isuser", System.Data.SqlDbType.Bit)
            };
        }

        protected override bool SetSpecificParametersValue(AlgorithmProperty item)
        {
            foreach (SqlParameter par in myinsertparams)
                switch (par.ParameterName)
                {
                    case "@request":
                        par.Value = mycmd.Request.ParcelGroup.HasValue ? 0 : mycmd.Request.Id;
                        break;
                    case "@group":
                        par.Value = mycmd.Request.ParcelGroup;
                        break;
                }
            foreach (SqlParameter par in myupdateparams)
                switch (par.ParameterName)
                {
                    case "@cbxupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(AlgorithmProperty.CBX));
                        break;
                    case "@cbxisuserupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(AlgorithmProperty.CBXUser));
                        break;
                    case "@cmrupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(AlgorithmProperty.CMR));
                        break;
                    case "@cmrisuserupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(AlgorithmProperty.CMRUser));
                        break;
                    case "@ex1t1upd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(AlgorithmProperty.EX1T1));
                        break;
                    case "@ex1t1isuserupd":
                        par.Value = item.HasPropertyOutdatedValue(nameof(AlgorithmProperty.EX1T1User));
                        break;
                }
            foreach (SqlParameter par in myinsertupdateparams)
                switch (par.ParameterName)
                {
                    case "@cbx":
                        par.Value = item.CBX;
                        break;
                    case "@cbxisuser":
                        par.Value = item.CBXUser;
                        break;
                    case "@cmr":
                        par.Value = item.CMR;
                        break;
                    case "@cmrisuser":
                        par.Value = item.CMRUser;
                        break;
                    case "@ex1t1":
                        par.Value = item.EX1T1;
                        break;
                    case "@ex1t1isuser":
                        par.Value = item.EX1T1User;
                        break;
                }
            return true;
        }
        protected override void GetOutputSpecificParametersValue(AlgorithmProperty item)
        {
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {
            foreach (SqlParameter par in SelectParams)
                switch (par.ParameterName)
                {
                    case "@request":
                        par.Value = mycmd.Request.ParcelGroup.HasValue ? 0 : mycmd.Request.Id;
                        break;
                    case "@group":
                        par.Value = mycmd.Request.ParcelGroup;
                        break;
                }
        }
		protected override AlgorithmProperty CreateRecord(SqlDataReader reader)
		{
            return new AlgorithmProperty(reader.GetInt32(this.Fields["id"]), reader.GetInt64(this.Fields["stamp"]), lib.DomainObjectState.Unchanged
                , mycmd
                , reader.IsDBNull(this.Fields["cbx"]) ? (decimal?)null : reader.GetDecimal(this.Fields["cbx"])
                , reader.GetBoolean(this.Fields["cbxisuser"])
                , reader.IsDBNull(this.Fields["cmr"]) ? (decimal?)null : reader.GetDecimal(this.Fields["cmr"])
                , reader.GetBoolean(this.Fields["cmrisuser"])
                , reader.IsDBNull(this.Fields["ex1t1"]) ? (decimal?)null : reader.GetDecimal(this.Fields["ex1t1"])
                , reader.GetBoolean(this.Fields["ex1t1isuser"])
                );
		}
        protected override AlgorithmProperty CreateModel(AlgorithmProperty reader, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
        {
			return reader;
        }
		protected override void LoadRecord(SqlDataReader reader, SqlConnection addcon, System.Threading.CancellationToken canceltasktoken = default)
		{
			base.TakeItem(CreateModel(this.CreateRecord(reader), addcon, canceltasktoken));
		}
		protected override bool GetModels(System.Threading.CancellationToken canceltasktoken=default,Func<bool> reading=null)
		{
			return true;
		}
    }
}
