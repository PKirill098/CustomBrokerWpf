using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using lib = KirillPolyanskiy.DataModelClassLibrary;
using System.Threading;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Algorithm
{
    public struct AlgorithmValuesRecord
    {
        internal int id;
        internal long stamp;
        internal int algorithm;
        internal int formula;
        internal decimal? value1;
        internal decimal? value2;
    }

    public class AlgorithmValues : lib.DomainBaseStamp
    {
        public AlgorithmValues(int id, long stamp, lib.DomainObjectState state
            , Algorithm algorithm, Formula formula, decimal? value1, decimal? value2
            ,bool init=false
            ) : base(id, stamp, null, null, state)
        {
            myalgorithm = algorithm;
            myformula = formula;
            myvalue1 = value1;
            myvalue2 = value2;
            myinitlater = !init;
            algorithm.Formulas.Add(this);
            if(init) this.FormulaInit(); // initializing
        }
        public AlgorithmValues(Algorithm algorithm, Formula formula) : this(lib.NewObjectId.NewId, 0, lib.DomainObjectState.Added, algorithm, formula, null, null) { }
        public AlgorithmValues() : this(lib.NewObjectId.NewId, 0, lib.DomainObjectState.Added, null, null, null, null, false) { }

        private Algorithm myalgorithm;
        public Algorithm Algorithm
        { set 
            {
                SetProperty<Algorithm>(ref myalgorithm, value, () => { if(!value.Formulas.Contains(this)) value.Formulas.Add(this); });
            }
            get { return myalgorithm; } }
        private bool myinitlater;
        internal bool InitLater
        { set { myinitlater = value; } get { return myinitlater; } }
        private Formula myformula;
        public Formula Formula
        {
            set { SetProperty<Formula>(ref myformula, value, ()=> {if(!myinitlater) FormulaInit(); }); }
            get { return myformula; }
        }

        private bool myisvalid1, myisvalid2;
        internal bool isValid1 { set { if (myisvalid1 != value) { myisvalid1 = value; this.PropertyChangedNotification("isValid1"); } } get { return myisvalid1; } }
        internal bool isValid2 { set { if (myisvalid2 != value) { myisvalid2 = value; this.PropertyChangedNotification("isValid2"); } } get { return myisvalid2; } }
        private bool myvalue1editable;
        public virtual bool Value1IsReadOnly
        { get { return !myvalue1editable; } }
        protected decimal? myvalue1;
        public virtual decimal? Value1
        { set { if (myvalue1editable) SetProperty<decimal?>(ref myvalue1, value); } get { return myvalue1; } }
        protected string myvalue1err;
        public virtual string Value1Err { set { myvalue1err = value; this.PropertyChangedNotification("Value1Err"); } get { return myvalue1err; } }
        private bool myvalue2editable;
        public virtual bool Value2IsReadOnly
        { get { return !myvalue2editable; } }
        protected decimal? myvalue2;
        public virtual decimal? Value2
        { set { if (myvalue2editable) SetProperty<decimal?>(ref myvalue2, value); } get { return myvalue2; } }
        protected string myvalue2err;
        public virtual string Value2Err { set { myvalue2err = value; this.PropertyChangedNotification("Value2Err"); } get { return myvalue2err; } }
        #region FuncValue
        //private string myformula1;
        private Func<string, decimal> myfunc1;
        internal Func<string, decimal> FuncValue1
        {
            set
            {
                //if (value != null & string.IsNullOrEmpty(myformula1))
                //{
                //    myformula1 = myformula.Formula1;
                //    myformula.Formula1 = "{ПРОГРАММА}";
                //}
                //else if (!(value != null | string.IsNullOrEmpty(myformula1)))
                //{
                //    myformula.Formula1 = myformula1;
                //    myformula1 = string.Empty;
                //}
                myfunc1 = value; // для расчета нужно указать в формуле {}
            }
        }
        //private string myformula2;
        private Func<string, decimal> myfunc2;
        internal Func<string, decimal> FuncValue2
        {
            set
            {
                //if (value != null & string.IsNullOrEmpty(myformula2))
                //{
                //    myformula2 = myformula.Formula2;
                //    myformula.Formula2 = "ПРОГРАММА";
                //}
                //else if (!(value != null | string.IsNullOrEmpty(myformula2)))
                //{
                //    myformula.Formula2 = myformula2;
                //    myformula2 = string.Empty;
                //}
                myfunc2 = value;
            }
        }
        #endregion

        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            AlgorithmValues newitem = (AlgorithmValues)sample;
            if (myvalue1editable) this.Value1 = newitem.Value1;
            if (myvalue2editable) this.Value2 = newitem.Value2;
        }
        protected override void RejectProperty(string property, object value)
        {
            throw new NotImplementedException();
        }
        private void Formula_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Formula1":
                    SetValue1();
                    SetValue2(); // если Ф2 ссылается на Ф1
                    break;
                case "Formula2":
                    SetValue2();
                    break;
            }
        }
        private void AlgoritmValue_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "isValid1":
                case "Value1":
                    //case "Value2":
                    if (sender != this) SetValue1();
                    SetValue2();
                    break;
            }
        }
        private void AlgorithmFormulas_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                this.Algorithm.Formulas.CollectionChanged -= AlgorithmFormulas_CollectionChanged;
                SetValue1();
                SetValue2();
            }
        }
        internal virtual bool SetValue1()
        {
            bool iserr = false;
            decimal? oldvalue = myvalue1;
            //if (myfunc1 != null)
            //{
            //    myvalue1editable = false;
            //    iserr = myfunc1();
            //}
            //else
            //{
                if (myformula.Code != "П9" && string.IsNullOrWhiteSpace(myformula.Formula1))
                    myvalue1editable = false;
                if ((myformula.FormulaType < 100 && myformula.Code != "П1" && myformula.Code != "П4") || string.IsNullOrWhiteSpace(myformula.Formula1))
                    myvalue1editable = true;
                else
                {
                    myvalue1editable = false;
                    decimal v;
                    if (decimal.TryParse(myformula.Formula1, out v))
                        myvalue1 = v;
                    else
                    {
                        myvalue1 = Calculate(myformula.Formula1, 1, out iserr);
                    }
                }
            //}
            if (oldvalue.HasValue != myvalue1.HasValue || (myvalue1.HasValue && !decimal.Equals(oldvalue.Value, myvalue1.Value)))
            {
                myisvalid1 = !iserr; // не сообщаем если Value1 изменилось
                this.PropertyChangedNotification("Value1");
            }
            else
                this.isValid1 = !iserr;
            this.PropertyChangedNotification("Value1IsReadOnly");
            return myisvalid1;
        }
        internal virtual bool SetValue2()
        {
            bool iserr = false;
            decimal? oldvalue = myvalue2;
            if (string.IsNullOrWhiteSpace(myformula.Formula2))
                myvalue2editable = true;
            else
            {
                myvalue2editable = false;
                decimal v;
                if (decimal.TryParse(myformula.Formula2, out v))
                    myvalue2 = v;
                else
                {
                    myvalue2 = Calculate(myformula.Formula2, 2, out iserr);
                }
            }
            if (oldvalue.HasValue != myvalue2.HasValue || (myvalue2.HasValue && !decimal.Equals(oldvalue.Value, myvalue2.Value)))
            {
                myisvalid2 = !iserr; // для избежания двойного расчета
                this.PropertyChangedNotification("Value2");
            }
            else
                this.isValid2 = !iserr;
            this.PropertyChangedNotification("Value2IsReadOnly");
            return myisvalid2;
        }
        private decimal? Calculate(string formula, int n, out bool iserr)
        {
            string err = string.Empty;
            decimal value1 = 0M, value2 = 0M, value3 = 0M;
            int operposition1, operposition2, operposition3 = 0;

            value1 = CalculateOperand(formula, n, out operposition1, out iserr);
            if (iserr) return null;
            if (operposition1 < formula.Length)
            {
                value2 = CalculateOperand(formula.Substring(operposition1 + 1), n, out operposition2, out iserr);
                if (iserr) return null;
                operposition2 += operposition1 + 1;
            }
            else
                operposition2 = operposition1;
            do
            {
                if (operposition2 < formula.Length)
                {
                    value3 = CalculateOperand(formula.Substring(operposition2 + 1), n, out operposition3, out iserr);
                    if (iserr) return null;
                    operposition3 += operposition2 + 1;
                }
                if (operposition2 < formula.Length)
                {
                    char oper1 = formula[operposition1], oper2 = formula[operposition2];
                    if ((oper1 == '+' | oper1 == '-') & (oper2 == '*' | oper2 == '/'))
                    {
                        value2 = PerformOperation(value2, value3, formula[operposition2], out err);
                        operposition2 = operposition3;
                    }
                    else
                    {
                        value1 = PerformOperation(value1, value2, formula[operposition1], out err);
                        value2 = value3;
                        operposition1 = operposition2;
                        operposition2 = operposition3;
                    }
                }
                else if (operposition1 < formula.Length)
                {
                    value1 = PerformOperation(value1, value2, formula[operposition1], out err);
                    operposition1 = operposition2;
                }
                if (!string.IsNullOrEmpty(err))
                {
                    iserr = true;
                    if (n == 1)
                        Value1Err = err;
                    else
                        Value2Err = err;
                    //err = string.Empty;
                    return null;
                }
            } while (operposition2 < formula.Length);
            if (operposition1 < formula.Length)
            {
                value1 = PerformOperation(value1, value2, formula[operposition1], out err);
            }
            if (!string.IsNullOrEmpty(err))
            {
                iserr = true;
                if (n == 1)
                    Value1Err = err;
                else
                    Value2Err = err;
            }
            return iserr ? (decimal?)null : value1;
        }
        private decimal CalculateOperand(string formula, int n, out int operposition, out bool iserr)
        {
            operposition = 0;
            decimal value1 = 0M;
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
                    if (n == 1)
                        Value1Err = @"Ошибка в Формула 1, отсутствует "")""!";
                    else
                        Value2Err = @"Ошибка в Формула 2, отсутствует "")""!";
                    iserr = true;
                    return 0M;
                }
                else
                {
                    value1 = Calculate(formula.Substring(1, c - 1), 1, out iserr) ?? 0M;
                    if (iserr) return 0M;
                }
            }
            else if (formula[0] == '{')
            {
                int o = formula.IndexOf('{', 1), c = formula.IndexOf('}');
                while (c > 0 && o > 0 && o < c)
                {
                    o = formula.IndexOf('{', o + 1);
                    c = formula.IndexOf('}', c + 1);
                }
                operposition = c + 1;
                if (operposition < 2)
                {
                    if (n == 1)
                        Value1Err = @"Ошибка в Формула 1, отсутствует ""}""!";
                    else
                        Value2Err = @"Ошибка в Формула 2, отсутствует ""}""!";
                    iserr = true;
                    return 0M;
                }
                else if(n == 1 && myfunc1==null)
                {
                    Value1Err = @"Не задана программа расчета " + formula.Substring(0, c+1);
                    iserr = true;
                    return 0M;
                }
                else if (n == 2 && myfunc2 == null)
                {
                    Value2Err = @"Не задана программа расчета " + formula.Substring(0, c+1);
                    iserr = true;
                    return 0M;
                }
                else
                {
                    string err=string.Empty;
                    if (n == 1)
                    {
                        value1 = myfunc1(err);
                        Value1Err = err;
                    }
                    else
                    {
                        value1 = myfunc2(err);
                        Value2Err = err;
                    }
                    iserr = !string.IsNullOrEmpty(err);
                    if (iserr) return 0M;
                }
            }
            else if (char.IsDigit(formula[0]))
            {
                int i = 1;
                while (i < formula.Length && (char.IsDigit(formula[i]) || formula[i] == '.' || formula[i] == ','))
                    i++;
                if (!decimal.TryParse(formula.Substring(0, i), System.Globalization.NumberStyles.Number, Formula.FormulaCulture, out value1))
                {
                    if (n == 1)
                        Value1Err = @"Ошибка в Формула 1, некорректный формат числа " + formula.Substring(0, i) + "!";
                    else
                        Value2Err = @"Ошибка в Формула 2, некорректный формат числа " + formula.Substring(0, i) + "!";
                    iserr = true;
                    return 0M;
                }
                operposition = i;
            }
            else if (formula.Length > 5 && formula.Substring(0, 5) == "СУММ(")
            {
                operposition = formula.IndexOf(')') + 1;
                if (operposition == 0)
                {
                    if (n == 1)
                        Value1Err = @"Ошибка в Формула 1, отсутствует СУММ( "")""!";
                    else
                        Value2Err = @"Ошибка в Формула 2, отсутствует СУММ( "")""!";
                    iserr = true;
                    return 0M;
                }
                else
                {
                    string sum = formula.Substring(5, operposition - 6);
                    while (sum.IndexOf(';') > 0)
                    {
                        value1 += Sum(sum.Substring(0, sum.IndexOf(';')), n, out iserr);
                        sum = sum.Substring(sum.IndexOf(';') + 1);
                        if (iserr) return 0M;
                    }
                    value1 += Sum(sum, n, out iserr);
                    if (iserr) return 0M;
                }
            }
            else if (char.IsLetter(formula[0]))
            {
                int i = 1;
                while (i < formula.Length && char.IsDigit(formula[i]))
                    i++;
                if (i == 1)
                {
                    if (n == 1)
                        Value1Err = @"Ошибка в Формула 1, некорректная ссылка на № !";
                    else
                        Value2Err = @"Ошибка в Формула 2, некорректная ссылка на № !";
                    iserr = true;
                    return 0M;
                }
                else
                {
                    operposition = i;
                    string pname = formula.Substring(0, i);
                    if (pname == this.Formula.Code)
                    {
                        if (n == 1)
                        {
                            Value1Err = "Формула 1 ссылается сама на себя !";
                            iserr = true;
                            return 0M;
                        }
                        else
                        {
                            this.PropertyChanged -= AlgoritmValue_PropertyChanged;
                            this.PropertyChanged += AlgoritmValue_PropertyChanged;
                            if (this.isValid1)
                                value1 = this.Value1 ?? 0M;
                            else
                            {
                                Value2Err = @"Значение в формуле 1 " + pname + " не корректно !";
                                iserr = true;
                                return 0M;
                            }
                        }
                    }
                    else
                    {
                        bool find = false;
                        foreach (AlgorithmValues item in this.Algorithm.Formulas)
                            if (item.Formula.Code == pname)
                            {
                                find = true;
                                item.PropertyChanged -= AlgoritmValue_PropertyChanged;
                                item.PropertyChanged += AlgoritmValue_PropertyChanged;
                                if (item.isValid1)
                                    value1 = item.Value1 ?? 0M;
                                else
                                {
                                    if (n == 1)
                                        Value1Err = @"Значение в формуле 1 " + pname + " не корректно !";
                                    else
                                        Value2Err = @"Значение в формуле 1 " + pname + " не корректно !";
                                    iserr = true;
                                    return 0M;
                                }
                                break;
                            }
                        if (!find) //слушаем не появилось ли value
                        {
                            this.Algorithm.Formulas.CollectionChanged -= AlgorithmFormulas_CollectionChanged;
                            this.Algorithm.Formulas.CollectionChanged += AlgorithmFormulas_CollectionChanged;
                            if (n == 1)
                                Value1Err = @"Ошибка в Формула 1, формула " + pname + " не найдена !";
                            else
                                Value2Err = @"Ошибка в Формула 2, формула " + pname + " не найдена !";
                            iserr = true;
                            return 0M;
                        }
                    }
                }
            }
            iserr = false;
            if (n == 1)
                Value1Err = string.Empty;
            else
                Value2Err = string.Empty;
            return value1;
        }

        private decimal PerformOperation(decimal value1, decimal value2, char operation, out string err)
        {
            err = string.Empty;
            switch (operation)
            {
                case '*':
                    value1 = value1 * value2;
                    break;
                case '/':
                    if (value2 == 0M)
                        err = "Ошибка, деление на ноль !";
                    else
                        value1 = decimal.Divide(value1, value2);
                    break;
                case '+':
                    value1 = value1 + value2;
                    break;
                case '-':
                    value1 = value1 - value2;
                    break;
                default:
                    err = "Не обрабатываемая  или пропущенная операция - " + operation;
                    break;
            }
            return value1;
        }
        private decimal Sum(string arg, int n, out bool iserr)
        {
            iserr = false;
            int pos = arg.IndexOf(':'), start, stop;
            decimal value = 0M;
            if (pos > 0)
            {
                if (arg[0] == arg[pos + 1] & int.TryParse(arg.Substring(1, pos - 1), out start) & int.TryParse(arg.Substring(pos + 2), out stop))
                    for (int i = start; i <= stop; i++)
                    {
                        value += Calculate(arg[0] + i.ToString(), 1, out iserr) ?? 0M;
                        if (iserr) break;
                    }
                else
                {
                    if (n == 1)
                        Value1Err = @"Ошибка в Формула 1, некорректный аргумент функции СУММ !";
                    else
                        Value2Err = @"Ошибка в Формула 2, некорректный аргумент функции СУММ !";
                    iserr = true;
                }
            }
            else
                value = Calculate(arg, 1, out iserr) ?? 0M;
            return value;
        }
        internal void FormulaInit() // initializing
        {
            if (myformula != null)
            {
                myformula.PropertyChanged += Formula_PropertyChanged;
                SetValue1(); SetValue2();
            }
        }
    }

    public class AlgorithmValuesStorage : lib.DomainStorage<AlgorithmValues>
    {
        protected override void UpdateProperties(AlgorithmValues olditem, AlgorithmValues newitem)
        {
            olditem.UpdateProperties(newitem);
        }
    }

    public class AlgorithmValuesDBM : lib.DBManagerStamp<AlgorithmValuesRecord,AlgorithmValues>
    {
        public AlgorithmValuesDBM(ObservableCollection<Algorithm> algorithms, ObservableCollection<Formula> formulas, AlgorithmValuesStorage storage) : base()
        {
            base.ConnectionString = CustomBrokerWpf.References.ConnectionString;

            SelectProcedure = true;
            InsertProcedure = true;
            UpdateProcedure = true;
            DeleteProcedure = true;
            SelectCommandText = "[dbo].[AlgorithmFormula_sp]";
            InsertCommandText = "[dbo].[AlgorithmFormulaAdd_sp]";
            UpdateCommandText = "[dbo].[AlgorithmFormulaUpd_sp]";
            DeleteCommandText = "[dbo].[AlgorithmFormulaDel_sp]";

            SelectParams = new SqlParameter[]
            {
                new SqlParameter("@id", System.Data.SqlDbType.Int),
                new SqlParameter("@algorithmid", System.Data.SqlDbType.Int),
                new SqlParameter("@formulaid", System.Data.SqlDbType.Int)
            };
            InsertParams = new SqlParameter[]
            {
                myinsertparams[0],myinsertparams[1],
                new SqlParameter("@algorithmid", System.Data.SqlDbType.Int),
                new SqlParameter("@formulaid", System.Data.SqlDbType.Int)
            };
            UpdateParams = new SqlParameter[]
            {
                myupdateparams[0],
                new SqlParameter("@value1true", System.Data.SqlDbType.Bit),
                new SqlParameter("@value2true", System.Data.SqlDbType.Bit),
            };
            InsertUpdateParams = new SqlParameter[]
            {
                new SqlParameter("@value1", System.Data.SqlDbType.Decimal){Precision=18,Scale=8 },
                new SqlParameter("@value2", System.Data.SqlDbType.Decimal){Precision=18,Scale=8 },
            };

            myalgorithms = algorithms;
            myformulas = formulas;
            mystorage = storage;
        }

        private ObservableCollection<Algorithm> myalgorithms;
        private ObservableCollection<Formula> myformulas;
        private AlgorithmValuesStorage mystorage;
        internal int? AlgorithmId
        { set { SelectParams[1].Value = value; } }

        protected override AlgorithmValuesRecord CreateRecord(SqlDataReader reader)
        {
            return new AlgorithmValuesRecord()
            {
                id = reader.GetInt32(0)
                , stamp = reader.GetInt64(1)
                , algorithm=reader.GetInt32(2)
                , formula=reader.GetInt32(3)
                , value1=reader.IsDBNull(4) ? (decimal?)null : reader.GetDecimal(4)
                , value2=reader.IsDBNull(5) ? (decimal?)null : reader.GetDecimal(5)
            };
        }
        protected override AlgorithmValues CreateModel(AlgorithmValuesRecord record, SqlConnection addcon, CancellationToken canceltasktoken = default)
        {
            Algorithm algorithm = null;
            Formula formula = null;
            foreach (Algorithm alg in myalgorithms)
                if (alg.Id == record.algorithm)
                {
                    algorithm = alg;
                    break;
                }
            foreach (Formula frm in myformulas)
                if (frm.Id == record.formula)
                {
                    formula = frm;
                    break;
                }
            AlgorithmValues newitem = new AlgorithmValues(record.id, record.stamp, lib.DomainObjectState.Unchanged
                , algorithm, formula, record.value1, record.value2);
            return newitem;//mystorage.UpdateItem()
        }
        protected override void GetOutputSpecificParametersValue(AlgorithmValues item)
        {
        }
        protected override bool SaveChildObjects(AlgorithmValues item)
        {
            return true;
        }
        protected override bool SaveIncludedObject(AlgorithmValues item)
        {
            return true;
        }
        protected override bool SaveReferenceObjects()
        {
            return true;
        }
        protected override void SetSelectParametersValue(SqlConnection addcon)
        {

        }
        protected override bool SetSpecificParametersValue(AlgorithmValues item)
        {
            bool isSuccess = item.Algorithm.DomainState != lib.DomainObjectState.Added & item.Formula.DomainState != lib.DomainObjectState.Added;
            if (isSuccess) //this.Errors.Add(new lib.DBMError(item, "Форлула/Алгоритм не сохранены!","incerr"));
            {
                myinsertparams[2].Value = item.Algorithm.Id;
                myinsertparams[3].Value = item.Formula.Id;
                myupdateparams[1].Value = item.HasPropertyOutdatedValue("Value1");
                myupdateparams[2].Value = item.HasPropertyOutdatedValue("Value2");
                myinsertupdateparams[0].Value = item.Value1;
                myinsertupdateparams[1].Value = item.Value2;
            }
            return isSuccess;
        }
    }

    public class AlgorithmValuesVM : lib.ViewModelErrorNotifyItem<AlgorithmValues>
    {
        public AlgorithmValuesVM(AlgorithmValues model) : base(model)
        {
            ValidetingProperties.AddRange(new string[] { "Value1", "Value2" });
            DeleteRefreshProperties.AddRange(new string[] { "Value1", "Value2" });
            InitProperties();
        }
        public AlgorithmValuesVM() : this(new AlgorithmValues()) { }

        public decimal? Value1
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.Value1.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.Value1.Value, value.Value))))
                {
                    string name = "Value1";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Value1);
                    ChangingDomainProperty = name; this.DomainObject.Value1 = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Value1 : null; }
        }
        public bool Value1IsReadOnly
        { get { return this.DomainObject.Value1IsReadOnly; } }
        public virtual Brush Value1Background
        {
            get
            {
                Brush brush = null;
                if (this.DomainObject.Formula.FormulaType < 100)
                    switch (this.DomainObject.Formula.Code)
                    {
                        case "П7":
                        case "П8":
                            brush = new System.Windows.Media.SolidColorBrush(lib.Common.MsOfficeHelperColor.StringToColor("#FFE6B8B7"));
                            break;
                    }
                else
                    switch (this.DomainObject.Formula.Code)
                    {
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
                return brush;
            }
        }
        public decimal? Value2
        {
            set
            {
                if (!this.IsReadOnly && (this.DomainObject.Value2.HasValue != value.HasValue || (value.HasValue && !decimal.Equals(this.DomainObject.Value2.Value, value.Value))))
                {
                    string name = "Value2";
                    if (!myUnchangedPropertyCollection.ContainsKey(name))
                        this.myUnchangedPropertyCollection.Add(name, this.DomainObject.Value2);
                    ChangingDomainProperty = name; this.DomainObject.Value2 = value;
                }
            }
            get { return this.IsEnabled ? this.DomainObject.Value2 : null; }
        }
        public bool Value2IsReadOnly
        { get { return this.DomainObject.Value2IsReadOnly; } }
        public virtual Brush Value2Background
        {
            get
            {
                Brush brush = null;
                if (this.DomainObject.Formula.FormulaType > 100)
                    switch (this.DomainObject.Formula.Code)
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
                return brush;
            }
        }

        protected override void DomainObjectPropertyChanged(string property)
        {
            switch (property)
            {
                case "Value1Err":
                    this.ValidateProperty("Value1", true);
                    break;
                case "Value2Err":
                    this.ValidateProperty("Value2", true);
                    break;
            }
        }
        protected override void InitProperties()
        {
            this.Validate(true);
        }
        protected override void RejectProperty(string property, object value)
        {
            switch (property)
            {
                case "Value1":
                    this.DomainObject.Value1 = (decimal?)value;
                    break;
                case "Value2":
                    this.DomainObject.Value2 = (decimal?)value;
                    break;
            }
        }
        protected override bool ValidateProperty(string propertyname, bool inform = true)
        {
            bool isvalid = true;
            string errmsg = null;
            switch (propertyname)
            {
                case "Value1":
                    if (!string.IsNullOrWhiteSpace(this.DomainObject.Value1Err))
                    {
                        errmsg = this.DomainObject.Value1Err;
                        isvalid = false;
                    }
                    break;
                case "Value2":
                    if (!string.IsNullOrWhiteSpace(this.DomainObject.Value2Err))
                    {
                        errmsg = this.DomainObject.Value2Err;
                        isvalid = false;
                    }
                    break;
            }
            if (inform & !isvalid) AddErrorMessageForProperty(propertyname, errmsg);
            else if (isvalid) ClearErrorMessageForProperty(propertyname);
            return isvalid;
        }
        protected override bool DirtyCheckProperty()
        {
            return false;
        }
    }

    public class AlgorithmValuesSynchronizer : lib.ModelViewCollectionsSynchronizer<AlgorithmValues, AlgorithmValuesVM>
    {
        protected override AlgorithmValues UnWrap(AlgorithmValuesVM wrap)
        {
            return wrap.DomainObject as AlgorithmValues;
        }
        protected override AlgorithmValuesVM Wrap(AlgorithmValues fill)
        {
            return new AlgorithmValuesVM(fill);
        }
    }
}
