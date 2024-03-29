﻿using System;
using System.Data.SqlClient;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Algorithm
{
    public struct FormulaRecord
    {
        internal int id;
        internal long stamp;
        internal string code;
        internal string name;
        internal byte type;
        internal string formula1;
        internal string formula2;
        internal int ordinal;
    }
    
    public class Formula : lib.DomainBaseStamp
    {
        public Formula(int id, long stamp, lib.DomainObjectState state
            , string code, string name, byte type, string formula1, string formula2, int order
            ) : base(id, stamp, null, null, state)
        {
            mycode = code;
            myformula1 = formula1;
            myformula2 = formula2;
            myname = name;
            myorder = order;
            mytype = type;
        }
        public Formula() : this(lib.NewObjectId.NewId, 0, lib.DomainObjectState.Added, null, null, 0, null, null,0) { }

        private string mycode;
        public string Code
        { set { SetProperty<string>(ref mycode, value); } get { return mycode; } }
        private string myformula1;
        public string Formula1
        { set { SetProperty<string>(ref myformula1, value); } get { return myformula1; } }
        private string myformula2;
        public string Formula2
        { set { SetProperty<string>(ref myformula2, value); } get { return myformula2; } }
        private string myname;
        public string Name
        { set { SetProperty<string>(ref myname, value); } get { return myname; } }
        private byte mytype;
        public byte FormulaType
        { set { SetProperty<byte>(ref mytype, value); } get { return mytype; } }
        private static System.Globalization.CultureInfo myformulaculture;
        public static System.Globalization.CultureInfo FormulaCulture
        { get { if (myformulaculture == null) myformulaculture = new System.Globalization.CultureInfo("ru-RU", false); return myformulaculture; } }
        private int myorder;
        public int Order
        { set { SetProperty<int>(ref myorder, value); } get { return myorder; } }

        protected override void PropertiesUpdate(lib.DomainBaseUpdate sample)
        {
            Formula newitem = (Formula)sample;
            this.Code = newitem.Code;
            this.Formula1 = newitem.Formula1;
            this.Formula2 = newitem.Formula2;
            this.Name = newitem.Name;
        }
        protected override void RejectProperty(string property, object value)
        {
            throw new NotImplementedException();
        }
        #region Validate
        public override bool ValidateProperty(string propertyname, object value, out string errmsg, out byte errmsgkey)
        {
            bool isvalid = true;
            errmsg = null;
            errmsgkey = 0;
            switch(propertyname)
            {
                case "Code":
                    string code = (string)value;
                    if (string.IsNullOrEmpty(code))
                    {
                        errmsg = "Необходимо указать № !";
                        isvalid = false;
                    }
                    else if (this.Formula1 != null && this.Formula1.IndexOf(code) > -1)
                    {
                        errmsg = "Формула 1 ссылается сама на себя !";
                        isvalid = false;
                    }
                    //else if (this.Formula2.IndexOf(value) > -1)
                    //{
                    //    errmsg = "Формула 2 ссылается сама на себя !";
                    //    isvalid = false;
                    //}
                    break;
                case "Formula1":
                    string formula1 = (string)value;
                    if (!(string.IsNullOrEmpty(formula1) || CalculateCheck(formula1, 1, out errmsg)))
                    {
                        isvalid = false;
                    }
                    break;
                case "Formula2":
                    string formula2 = (string)value;
                    if (!(string.IsNullOrEmpty(formula2) || CalculateCheck(formula2, 2, out errmsg)))
                    {
                        isvalid = false;
                    }
                    break;
            }
            return isvalid;
        }
        private bool CalculateCheck(string formula, int n, out string err)
        {
            err = string.Empty;
            int operposition1, operposition2, operposition3 = 0;
            if (CalculateOperandCheck(formula, n, out operposition1, out err) && operposition1 < formula.Length)
            {
                CalculateOperandCheck(formula.Substring(operposition1 + 1), n, out operposition2, out err);
                operposition2 += operposition1 + 1;
            }
            else
                operposition2 = operposition1;
            do
            {
                if (err == string.Empty && operposition2 < formula.Length)
                {
                    CalculateOperandCheck(formula.Substring(operposition2 + 1), n, out operposition3, out err);
                    operposition3 += operposition2 + 1;
                }
                if (err == string.Empty)
                {
                    if (operposition2 < formula.Length)
                    {
                        char oper1 = formula[operposition1], oper2 = formula[operposition2];
                        if ((oper1 == '+' | oper1 == '-') & (oper2 == '*' | oper1 == '/'))
                        {
                            PerformOperationCheck(formula[operposition2], out err);
                            operposition2 = operposition3;
                        }
                        else
                        {
                            PerformOperationCheck(formula[operposition1], out err);
                            operposition1 = operposition2;
                            operposition2 = operposition3;
                        }
                    }
                    else if (operposition1 < formula.Length)
                    {
                        PerformOperationCheck(formula[operposition1], out err);
                        operposition1 = operposition2;
                    }
                }
            } while (string.IsNullOrEmpty(err) && operposition2 < formula.Length);
            if (string.IsNullOrEmpty(err) && operposition1 < formula.Length)
            {
                PerformOperationCheck(formula[operposition1], out err);
            }
            return string.IsNullOrEmpty(err);
        }
        private bool CalculateOperandCheck(string formula, int n, out int operposition, out string err)
        {
            decimal value1 = 0M;
            err = string.Empty;
            operposition = 0;
            if (formula[0] == '(')
            {
                int o = formula.IndexOf('(', 1), c = formula.IndexOf(')');
                while (c > 0 && o > 0 && o < c)
                {
                    o = formula.IndexOf('(', o + 1);
                    c = formula.IndexOf(')', c + 1);
                }
                operposition = c + 1;
                if (operposition < 2)
                {
                    err = @"Ошибка в формуле отсутствует "")""!";
                    return false;
                }
                else
                    CalculateCheck(formula.Substring(1, c - 1), n, out err);
            }
            if (formula[0] == '{')
            {
                int o = formula.IndexOf('{', 1), c = formula.IndexOf('}');
                while (c > 0 && o > 0 && o < c)
                {
                    o = formula.IndexOf('{', o + 1);
                    c = formula.IndexOf('{', c + 1);
                }
                operposition = c + 1;
                if (operposition < 2)
                {
                    err = @"Ошибка в формуле отсутствует ""}""!";
                    return false;
                }
            }
            else if (char.IsDigit(formula[0]))
            {
                int i = 1;
                while (i < formula.Length && (char.IsDigit(formula[i]) || formula[i] == '.' || formula[i] == ','))
                    i++;
                if (!decimal.TryParse(formula.Substring(0, i), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CreateSpecificCulture("ru-RU"), out value1))
                {
                    err = @"Ошибка в формуле, некорректный формат числа " + formula.Substring(0, i) + "!";
                    return false;
                }
                operposition = i;
            }
            else if (formula.Length > 5 && formula.Substring(0, 5) == "СУММ(")
            {
                operposition = formula.IndexOf(')') + 1;
                if (operposition == 0)
                {
                    err = @"Ошибка в формуле, отсутствует СУММ( "")""!";
                    return false;
                }
                else
                {
                    string sum = formula.Substring(5, operposition - 6);
                    while (sum.IndexOf(';') > 0)
                    {
                        if (!SumCheck(sum.Substring(0, sum.IndexOf(';')), n, out err)) return false;
                        sum = sum.Substring(sum.IndexOf(';') + 1);
                    }
                    if (!SumCheck(sum, n, out err)) return false;
                }
            }
            else if (char.IsLetter(formula[0]))
            {
                int i = 1;
                while (i < formula.Length && char.IsDigit(formula[i]))
                    i++;
                if (i == 1)
                {
                    err = @"Ошибка в формуле, некорректная ссылка на № !";
                    return false;
                }
                else
                {
                    operposition = i;
                    string pname = formula.Substring(0, i);
                    if (pname == this?.Code & n == 1)
                    {
                        err = "Формула 1 ссылается сама на себя !";
                        return false;
                    }
                }
            }
            return true;
        }
        private bool PerformOperationCheck(char operation, out string err)
        {
            err = string.Empty;
            bool success = true;
            switch (operation)
            {
                case '*':
                case '/':
                case '+':
                case '-':
                    break;
                default:
                    success = false;
                    err = "Не обрабатываемая или пропущенная операция - " + operation;
                    break;
            }
            return success;
        }
        private bool SumCheck(string arg, int n, out string err)
        {
            err = string.Empty;
            int pos = arg.IndexOf(':'), start, stop;
            if (pos > 0)
            {
                if (arg[0] == arg[pos + 1] & int.TryParse(arg.Substring(1, pos - 1), out start) & int.TryParse(arg.Substring(pos + 2), out stop))
                    for (int i = start; i <= stop; i++)
                        CalculateCheck(arg[0] + i.ToString(), n, out err);
                else
                {
                    err = @"Ошибка в формуле, некорректный аргумент функции СУММ !";
                    return false;
                }
            }
            else
                CalculateCheck(arg, n, out err);
            return true;
        }
        #endregion
    }

    internal class FormulaStorage : lib.DomainStorage<Formula>
    {
        protected override void UpdateProperties(Formula olditem, Formula newitem)
        {
            olditem.UpdateProperties(newitem);
        }
    }

    public class FormulaDBM : lib.DBManagerStamp<FormulaRecord, Formula>
    {
        internal FormulaDBM()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectProcedure = true;
            InsertProcedure = true;
            UpdateProcedure = true;
            DeleteProcedure = true;
            SelectCommandText = "[dbo].[Formula_sp]";
            InsertCommandText = "[dbo].[FormulaAdd_sp]";
            UpdateCommandText = "[dbo].[FormulaUpd_sp]";
            DeleteCommandText = "[dbo].[FormulaDel_sp]";

            SelectParams = new SqlParameter[] { new SqlParameter("@id", System.Data.SqlDbType.Int) };
            UpdateParams = new SqlParameter[]
            {
                myupdateparams[0],
                new SqlParameter("@codetrue", System.Data.SqlDbType.Bit),
                new SqlParameter("@nametrue", System.Data.SqlDbType.Bit),
                new SqlParameter("@typetrue", System.Data.SqlDbType.Bit),
                new SqlParameter("@formula1true", System.Data.SqlDbType.Bit),
                new SqlParameter("@formula2true", System.Data.SqlDbType.Bit),
                new SqlParameter("@ordinaltrue", System.Data.SqlDbType.Bit)
            };
            InsertUpdateParams = new SqlParameter[]
            {
                new SqlParameter("@code", System.Data.SqlDbType.NVarChar,3),
                new SqlParameter("@name", System.Data.SqlDbType.NVarChar,50),
                new SqlParameter("@type", System.Data.SqlDbType.TinyInt),
                new SqlParameter("@formula1", System.Data.SqlDbType.NVarChar,50),
                new SqlParameter("@formula2", System.Data.SqlDbType.NVarChar,50),
                new SqlParameter("@ordinal", System.Data.SqlDbType.Int)
            };
        }

        protected override FormulaRecord CreateRecord(SqlDataReader reader)
        {
            return new FormulaRecord()
            {
                id=reader.GetInt32(0), stamp=reader.GetInt64(1)
                , code=reader.GetString(this.Fields["code"])
                , name=reader.GetString(this.Fields["name"])
                , type=reader.GetByte(this.Fields["type"])
                , formula1=reader.IsDBNull(this.Fields["formula1"]) ? null : reader.GetString(this.Fields["formula1"])
                , formula2=reader.IsDBNull(this.Fields["formula2"]) ? null : reader.GetString(this.Fields["formula2"])
                , ordinal=reader.IsDBNull(this.Fields["ordinal"]) ? 0 : reader.GetInt32(this.Fields["ordinal"])
            };
        }
        protected override Formula CreateModel(FormulaRecord record,SqlConnection addcon, CancellationToken canceltasktoken = default)
        {
            Formula item = new Formula(record.id, record.stamp, lib.DomainObjectState.Unchanged
                , record.code
                , record.name
                , record.type
                , record.formula1
                , record.formula2
                , record.ordinal);
            return CustomBrokerWpf.References.FormulaStorage.UpdateItem(item);
        }
        protected override bool SaveChildObjects(Formula item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(Formula item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override void SetSelectParametersValue()
        {
        }
        protected override bool SetParametersValue(Formula item)
        {
            base.SetParametersValue(item);
            foreach(SqlParameter par in myupdateparams)
                switch(par.ParameterName)
                {
                    case "@codetrue":
                        par.Value = item.HasPropertyOutdatedValue("Code");
                        break;
                    case "@nametrue":
                        par.Value = item.HasPropertyOutdatedValue("Name");
                        break;
                    case "@typetrue":
                        par.Value = item.HasPropertyOutdatedValue("FormulaType");
                        break;
                    case "@formula1true":
                        par.Value = item.HasPropertyOutdatedValue("Formula1");
                        break;
                    case "@formula2true":
                        par.Value = item.HasPropertyOutdatedValue("Formula2");
                        break;
                    case "@ordinaltrue":
                        par.Value = item.HasPropertyOutdatedValue("Order");
                        break;
                }
            foreach (SqlParameter par in myinsertupdateparams)
                switch (par.ParameterName)
                {
                    case "@code":
                        par.Value = item.Code;
                        break;
                    case "@name":
                        par.Value = item.Name;
                        break;
                    case "@type":
                        par.Value = item.FormulaType;
                        break;
                    case "@formula1":
                        par.Value = item.Formula1;
                        break;
                    case "@formula2":
                        par.Value = item.Formula2;
                        break;
                    case "@ordinal":
                        par.Value = item.Order;
                        break;
                }

            return true;
        }
    }

    public class FormulaVM : lib.ViewModelErrorNotifyItem<Formula>
    {
        public FormulaVM(Formula item) : base(item)
        {
            ValidetingProperties.AddRange(new string[] { "Code", "Formula1", "Formula2" });
            DeleteRefreshProperties.AddRange(new string[] { "Code", "Formula1", "Formula2", "Name" });
            InitProperties();
        }

        private string mycode;
        public string Code
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(mycode, value)))
                {
                    string name = "Code";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, mycode);
                    mycode = value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.Code = value;
                        if (ValidateProperty("Formula1"))
                        {
                            ChangingDomainProperty = "Formula1"; this.DomainObject.Formula1 = myformula1;
                        }
                        if (ValidateProperty("Formula2"))
                        {
                            ChangingDomainProperty = "Formula2"; this.DomainObject.Formula2 = myformula2;
                        }
                    }
                }
            }
            get { return this.IsEnabled ? mycode : null; }
        }
        private string myformula1;
        public string Formula1
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(myformula1, value)))
                {
                    string name = "Formula1";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myformula1);
                    myformula1 = value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.Formula1 = value;
                    }
                    if (ValidateProperty("Code"))
                    {
                        ChangingDomainProperty = "Code"; this.DomainObject.Code = mycode;
                    }
                }
            }
            get { return this.IsEnabled ? myformula1 : null; }
        }
        public Brush Formula1Background
        {
            get
            {
                SolidColorBrush brush = null;
                if (this.FormulaType > 100)
                {
                    switch(this.Code)
                    {
                        case "П21":
                            brush = new System.Windows.Media.SolidColorBrush(lib.Common.MsOfficeHelperColor.StringToColor("#FFD9D9D9"));
                            break;
                        case "П31":
                        case "П32":
                        case "П33":
                        case "П34":
                            brush = new System.Windows.Media.SolidColorBrush(lib.Common.MsOfficeHelperColor.StringToColor("#FFC4D79B"));
                            break;
                        case "П40":
                        case "П46":
                        case "П47":
                        case "П48":
                            brush = new System.Windows.Media.SolidColorBrush(lib.Common.MsOfficeHelperColor.StringToColor("#FF92D050"));
                            break;
                        case "П9":
                        case "П10":
                        case "П11":
                        case "П12":
                        case "П17":
                        case "П18":
                        case "П19":
                        case "П20":
                        case "П30":
                        case "П37":
                        case "П38":
                        case "П39":
                            brush = new System.Windows.Media.SolidColorBrush(lib.Common.MsOfficeHelperColor.StringToColor("#FFEBF1DE"));
                            break;
                    }
                }
                return brush;
            }
        }
        public FontWeight Formula1FontWeight
        {
            get
            {
                FontWeight weight = FontWeights.Normal;
                if (this.FormulaType > 100)
                {
                    if (this.Code == "П9" || this.Code == "П10" || this.Code == "П11" || this.Code == "П12" || this.Code == "П21" || this.Code == "П31" || this.Code == "П32" || this.Code == "П33" || this.Code == "П34" || this.Code == "П40" || this.Code == "П46" || this.Code == "П47" || this.Code == "П47")
                        weight = FontWeights.Bold;
                }
                return weight;
            }
        }
        private string myformula2;
        public string Formula2
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(myformula2, value)))
                {
                    string name = "Formula2";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, myformula2);
                    myformula2 = value;
                    if (ValidateProperty(name))
                    {
                        ChangingDomainProperty = name; this.DomainObject.Formula2 = value;
                        ClearErrorMessageForProperty(name);
                    }
                    if (ValidateProperty("Code"))
                    {
                        ChangingDomainProperty = "Code"; this.DomainObject.Code = mycode;
                    }
                }
            }
            get { return this.IsEnabled ? myformula2 : null; }
        }
        public Brush Formula2Background
        {
            get
            {
                SolidColorBrush brush = null;
                if (this.FormulaType > 100)
                {
                    switch (this.Code)
                    {
                        case "П21":
                            brush = new System.Windows.Media.SolidColorBrush(lib.Common.MsOfficeHelperColor.StringToColor("#FFD9D9D9"));
                            break;
                        case "П31":
                        case "П32":
                        case "П33":
                        case "П34":
                            brush = new System.Windows.Media.SolidColorBrush(lib.Common.MsOfficeHelperColor.StringToColor("#FFC4D79B"));
                            break;
                        case "П40":
                        case "П46":
                        case "П47":
                        case "П48":
                            brush = new System.Windows.Media.SolidColorBrush(lib.Common.MsOfficeHelperColor.StringToColor("#FF92D050"));
                            break;
                    }
                }
                return brush;
            }
        }
        public FontWeight Formula2FontWeight
        {
            get
            {
                FontWeight weight = FontWeights.Normal;
                if (this.FormulaType > 100)
                {
                    if (this.Code == "П9" || this.Code == "П10" || this.Code == "П11" || this.Code == "П12" || this.Code == "П21" || this.Code == "П31" || this.Code == "П32" || this.Code == "П33" || this.Code == "П34" || this.Code == "П40" || this.Code == "П46" || this.Code == "П47" || this.Code == "П48")
                        weight = FontWeights.Bold;
                }
                return weight;
            }
        }
        public string Name
        {
            set
            {
                if (!(this.IsReadOnly || string.Equals(this.DomainObject.Name, value)))
                {
                    string name = "Name";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Name);
                    ChangingDomainProperty = name; this.DomainObject.Name = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Name : null; }
        }
        public Brush NameBackground
        {
            get
            {
                SolidColorBrush brush = null;
                if (this.FormulaType > 100)
                {
                    switch (this.Code)
                    {
                        case "П10":
                            brush = new System.Windows.Media.SolidColorBrush(lib.Common.MsOfficeHelperColor.StringToColor("#FFFFFF00"));
                            break;
                        case "П21":
                            brush = new System.Windows.Media.SolidColorBrush(lib.Common.MsOfficeHelperColor.StringToColor("#FFD9D9D9"));
                            break;
                        case "П31":
                        case "П32":
                        case "П33":
                        case "П34":
                            brush = new System.Windows.Media.SolidColorBrush(lib.Common.MsOfficeHelperColor.StringToColor("#FFC4D79B"));
                            break;
                        case "П40":
                        case "П46":
                        case "П47":
                        case "П48":
                            brush = new System.Windows.Media.SolidColorBrush(lib.Common.MsOfficeHelperColor.StringToColor("#FF92D050"));
                            break;
                    }
                }
                return brush;
            }
        }
        public FontWeight NameFontWeight
        {
            get
            {
                FontWeight weight = FontWeights.Normal;
                if (this.FormulaType > 100)
                {
                    if (this.Code == "П9" || this.Code == "П10" || this.Code == "П11" || this.Code == "П12" || this.Code == "П21" || this.Code == "П31" || this.Code == "П32" || this.Code == "П33" || this.Code == "П34" || this.Code == "П40" || this.Code == "П46" || this.Code == "П47" || this.Code == "П48")
                        weight = FontWeights.Bold;
                }
                return weight;
            }
        }
        public byte? FormulaType
        {
            set
            {
                if (value.HasValue && !(this.IsReadOnly || this.DomainObject.FormulaType == value.Value))
                {
                    string name = "FormulaType";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.FormulaType);
                    ChangingDomainProperty = name; this.DomainObject.FormulaType = value.Value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.FormulaType : (byte?)null; }
        }
        public int Order
        { get { return this.DomainObject.Order; } }//string.IsNullOrWhiteSpace(mycode) ? int.MaxValue : int.Parse(mycode.Substring(1))

        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case "Code":
                    mycode = this.DomainObject.Code;
                    this.PropertyChangedNotification("Formula1Background");
                    this.PropertyChangedNotification("Formula1FontWeight");
                    this.PropertyChangedNotification("Formula2Background");
                    this.PropertyChangedNotification("Formula2FontWeight");
                    this.PropertyChangedNotification("NameBackground");
                    this.PropertyChangedNotification("NameFontWeight");
                    break;
                case "Formula1":
                    myformula1 = this.DomainObject.Formula1;
                    break;
                case "Formula2":
                    myformula2 = this.DomainObject.Formula2;
                    break;
            }
        }
        protected override void InitProperties()
        {
            mycode = this.DomainObject.Code;
            myformula1 = this.DomainObject.Formula1;
            myformula2 = this.DomainObject.Formula2;
        }

        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Code":
                    if (mycode != this.DomainObject.Code)
                        mycode = this.DomainObject.Code;
                    else
                        this.Code = (string)value;
                    break;
                case "Formula1":
                    if (myformula1 != this.DomainObject.Formula1)
                        myformula1 = this.DomainObject.Formula1;
                    else
                        this.Formula1 = (string)value;
                    break;
                case "Formula2":
                    if (myformula2 != this.DomainObject.Formula2)
                        myformula2 = this.DomainObject.Formula2;
                    else
                        this.Formula2 = (string)value;
                    break;
                case "Name":
                    this.DomainObject.Name = (string)value;
                    break;
                    ;
            }
        }
        protected override bool DirtyCheckProperty()
        {
            return mycode!=this.DomainObject.Code || myformula1!=this.DomainObject.Formula1 || myformula2!=this.DomainObject.Formula2;
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            byte errcode = 0;
            switch (propertyname)
            {
                case "Code":
                    isvalid = this.DomainObject.ValidateProperty(propertyname, this.Code, out errmsg, out errcode);
                    break;
                case "Formula1":
                    isvalid = this.DomainObject.ValidateProperty(propertyname, this.Formula1, out errmsg, out errcode);
                    break;
                case "Formula2":
                    isvalid = this.DomainObject.ValidateProperty(propertyname, this.Formula2, out errmsg, out errcode);
                    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            else if (isvalid) ClearErrorMessageForProperty(propertyname);
            return isvalid;
        }
    }

    public class FormulaSynchronizer : lib.ModelViewCollectionsSynchronizer<Formula, FormulaVM>
    {
        protected override Formula UnWrap(FormulaVM wrap)
        {
            return wrap.DomainObject as Formula;
        }
        protected override FormulaVM Wrap(Formula fill)
        {
            return new FormulaVM(fill);
        }
    }
}
