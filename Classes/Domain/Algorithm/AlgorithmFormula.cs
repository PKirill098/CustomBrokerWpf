using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain.Algorithm
{
    public class AlgorithmFormula : lib.DomainBaseClass
    {
        public AlgorithmFormula(FormulaVM formula, lib.DomainObjectState state) : base(0, state)
        {
            myformula = formula;
            myalgorithms = new ObservableCollection<AlgorithmValuesVM>();
        }
        public AlgorithmFormula() : this(null, lib.DomainObjectState.Unchanged) { }

        private FormulaVM myformula;
        public FormulaVM Formula
        { set { myformula = value; } get { return myformula; } }
        private ObservableCollection<AlgorithmValuesVM> myalgorithms;
        public ObservableCollection<AlgorithmValuesVM> Algorithms
        {
            set { myalgorithms = value; }
            get { return myalgorithms; }
        }

        public override bool IsDirty
        {
            get
            {
                bool isdirty = myformula.DomainObject.IsDirty;
                foreach (AlgorithmValuesVM value in Algorithms)
                    isdirty |= value.DomainObject.IsDirty;
                return isdirty;
            }
        }
    }

    public class AlgorithmFormulaCommand : lib.ViewModelBaseCommand
    {
        internal AlgorithmFormulaCommand()
        {
            System.Text.StringBuilder err = new System.Text.StringBuilder();
            err.AppendLine("Данные не загружены");
            myadbm = new AlgorithmDBM();
            myadbm.Fill();
            if (myadbm.Errors.Count > 0) err.AppendLine(myadbm.ErrorMessage);
            myalgorithms = myadbm.Collection;
            myfdbm = new FormulaDBM();
            myfdbm.Fill();
            if (myfdbm.Errors.Count > 0) err.AppendLine(myfdbm.ErrorMessage);
            myformulasynchronizer = new FormulaSynchronizer();
            myformulasynchronizer.DomainCollection = myfdbm.Collection;
            myvaluesstorage = new AlgorithmValuesStorage();
            myvdbm = new AlgorithmValuesDBM(this.Algorithms, myformulasynchronizer.DomainCollection, this.ValuesStorage);
            myvdbm.Fill();
            if (myvdbm.Errors.Count > 0) err.AppendLine(myvdbm.ErrorMessage);
            myalgorithmformulas = new ObservableCollection<AlgorithmFormula>();
            AlgorithmValues values = null;
            foreach (FormulaVM frm in myformulasynchronizer.ViewModelCollection)
            {
                AlgorithmFormula algfrm = new AlgorithmFormula(frm, lib.DomainObjectState.Unchanged);
                myalgorithmformulas.Add(algfrm);
                foreach (Algorithm alg in myalgorithms)
                {
                    values = null;
                    foreach (AlgorithmValues vals in myvdbm.Collection)
                    {
                        if (vals.Formula == frm.DomainObject && vals.Algorithm == alg)
                        {
                            values = vals;
                            break;
                        }
                    }
                    if (values == null) values = AlgorithmValuesCreate(alg, frm.DomainObject);
                    algfrm.Algorithms.Add(new AlgorithmValuesVM(values));
                }
            }
            foreach (Algorithm alg in myalgorithms)
                alg.FormulasInit();
            myview1 = new ListCollectionView(myalgorithmformulas);
            myview1.SortDescriptions.Add(new System.ComponentModel.SortDescription("Formula.Order", System.ComponentModel.ListSortDirection.Ascending));
            myview1.Filter = (object item) => { FormulaVM formula = (item as AlgorithmFormula).Formula; return lib.ViewModelViewCommand.ViewFilterDefault(item) && formula.DomainObject.FormulaType < 100; };
            myview1.MoveCurrentToPosition(-1);
            myview2 = new ListCollectionView(myalgorithmformulas);
            myview2.SortDescriptions.Add(new System.ComponentModel.SortDescription("Formula.Order", System.ComponentModel.ListSortDirection.Ascending));
            myview2.Filter = (object item) => { FormulaVM formula = (item as AlgorithmFormula).Formula; return lib.ViewModelViewCommand.ViewFilterDefault(item) && formula.DomainObject.FormulaType > 100; };
            myview2.MoveCurrentToPosition(-1);
            myaddalgorithm = new RelayCommand(AddAlgorithmExec, AddAlgorithmCanExec);
            mydelalgorithm = new RelayCommand(DelAlgorithmExec, DelAlgorithmCanExec);

            if (err.Length > 22)
                this.OpenPopup(err.ToString(), true);
        }
        internal AlgorithmFormulaCommand(bool noload) : base() { }

        protected AlgorithmDBM myadbm;
        protected ObservableCollection<Algorithm> myalgorithms;
        public ObservableCollection<Algorithm> Algorithms
        {
            get { return myalgorithms; }
        }
        protected FormulaDBM myfdbm;
        protected FormulaSynchronizer myformulasynchronizer;
        internal ObservableCollection<FormulaVM> Formulas
        {
            get { return myformulasynchronizer.ViewModelCollection; }
        }
        protected AlgorithmValuesStorage myvaluesstorage;
        internal AlgorithmValuesStorage ValuesStorage
        {
            get { return myvaluesstorage; }
        }
        private AlgorithmValuesDBM myvdbm;
        protected ObservableCollection<AlgorithmFormula> myalgorithmformulas;
        internal ObservableCollection<AlgorithmFormula> AlgorithmFormulas
        {
            get { return myalgorithmformulas; }
        }
        protected ListCollectionView myview1;
        public ListCollectionView View1
        { get { return myview1; } }
        protected ListCollectionView myview2;
        public ListCollectionView View2
        { get { return myview2; } }

        public virtual bool IsReadOnly
        { set { PropertyChangedNotification("IsReadOnly"); } get { return !(App.Current.FindResource("keyVisibilityAlgorithmWriters") as VisibilityAlgorithmWriters).IsMember; } }
        public virtual bool FormulaIsReadOnly
        {
            get { return this.IsReadOnly | false; }
        }
        public virtual bool AlgorithmIsReadOnly
        {
            get { return this.IsReadOnly | false; }
        }
        public virtual Visibility WriterMenuVisible
        {
            get { return this.IsReadOnly ? Visibility.Collapsed : Visibility.Visible; }
        }
        public virtual Visibility SaveMenuVisible
        {
            get { return this.IsReadOnly ? Visibility.Collapsed : Visibility.Visible; }
        }

        private RelayCommand myaddalgorithm;
        public ICommand AddAlgorithm
        {
            get { return myaddalgorithm; }
        }
        private void AddAlgorithmExec(object parametr)
        {
            Algorithm alg = new Algorithm();
            alg.Index = (byte)myalgorithms.Count;
            myalgorithms.Add(alg);
            this.PropertyChangedNotification("Algorithms");
            this.PropertyChangedNotification("Algorithm" + this.Algorithms.Count.ToString() + "ColumnVisibility");
            foreach (AlgorithmFormula algfrm in myalgorithmformulas)
            {
                algfrm.Algorithms.Add(new AlgorithmValuesVM(AlgorithmValuesCreate(alg, algfrm.Formula.DomainObject)));
            }
        }
        private bool AddAlgorithmCanExec(object parametr)
        { return true; }

        private RelayCommand mydelalgorithm;
        public ICommand DelAlgorithm
        {
            get { return mydelalgorithm; }
        }
        private void DelAlgorithmExec(object parametr)
        {
            if (parametr is Algorithm)
            {
                Algorithm alg = parametr as Algorithm;
                //alg.Index = (byte)myalgorithms.Count;
                //myalgorithms.Add(alg);
                //this.PropertyChangedNotification("Algorithms");
                //this.PropertyChangedNotification("Algorithm" + this.Algorithms.Count.ToString() + "ColumnVisibility");
                //foreach (AlgorithmFormula algfrm in myalgorithmformulas)
                //{
                //    algfrm.Algorithms.Add(new AlgorithmValuesVM(AlgorithmValuesCreate(alg, algfrm.Formula.DomainObject)));
                //}
            }
        }
        private bool DelAlgorithmCanExec(object parametr)
        { return true; }

        public Visibility Algorithm1ColumnVisibility
        { get { return this.Algorithms.Count > 0 && this.Algorithms[0].DomainState!=lib.DomainObjectState.Deleted ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility Algorithm2ColumnVisibility
        { get { return this.Algorithms.Count > 1 && this.Algorithms[1].DomainState != lib.DomainObjectState.Deleted ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility Algorithm3ColumnVisibility
        { get { return this.Algorithms.Count > 2 && this.Algorithms[2].DomainState != lib.DomainObjectState.Deleted ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility Algorithm4ColumnVisibility
        { get { return this.Algorithms.Count > 3 && this.Algorithms[3].DomainState != lib.DomainObjectState.Deleted ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility Algorithm5ColumnVisibility
        { get { return this.Algorithms.Count > 4 && this.Algorithms[4].DomainState != lib.DomainObjectState.Deleted ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility Algorithm6ColumnVisibility
        { get { return this.Algorithms.Count > 5 && this.Algorithms[5].DomainState != lib.DomainObjectState.Deleted ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility Algorithm7ColumnVisibility
        { get { return this.Algorithms.Count > 6 && this.Algorithms[6].DomainState != lib.DomainObjectState.Deleted ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility Algorithm8ColumnVisibility
        { get { return this.Algorithms.Count > 7 && this.Algorithms[7].DomainState != lib.DomainObjectState.Deleted ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility Algorithm9ColumnVisibility
        { get { return this.Algorithms.Count > 8 && this.Algorithms[8].DomainState != lib.DomainObjectState.Deleted ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility Algorithm10ColumnVisibility
        { get { return this.Algorithms.Count > 9 && this.Algorithms[9].DomainState != lib.DomainObjectState.Deleted ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility Algorithm11ColumnVisibility
        { get { return this.Algorithms.Count > 10 && this.Algorithms[10].DomainState != lib.DomainObjectState.Deleted ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility Algorithm12ColumnVisibility
        { get { return this.Algorithms.Count > 11 && this.Algorithms[11].DomainState != lib.DomainObjectState.Deleted ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility Algorithm13ColumnVisibility
        { get { return this.Algorithms.Count > 12 && this.Algorithms[12].DomainState != lib.DomainObjectState.Deleted ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility Algorithm14ColumnVisibility
        { get { return this.Algorithms.Count > 13 && this.Algorithms[13].DomainState != lib.DomainObjectState.Deleted ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility Algorithm15ColumnVisibility
        { get { return this.Algorithms.Count > 14 && this.Algorithms[14].DomainState != lib.DomainObjectState.Deleted ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility Algorithm16ColumnVisibility
        { get { return this.Algorithms.Count > 15 && this.Algorithms[15].DomainState != lib.DomainObjectState.Deleted ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility Algorithm17ColumnVisibility
        { get { return this.Algorithms.Count > 16 && this.Algorithms[16].DomainState != lib.DomainObjectState.Deleted ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility Algorithm18ColumnVisibility
        { get { return this.Algorithms.Count > 17 && this.Algorithms[17].DomainState != lib.DomainObjectState.Deleted ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility Algorithm19ColumnVisibility
        { get { return this.Algorithms.Count > 18 && this.Algorithms[18].DomainState != lib.DomainObjectState.Deleted ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility Algorithm20ColumnVisibility
        { get { return this.Algorithms.Count > 19 && this.Algorithms[19].DomainState != lib.DomainObjectState.Deleted ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility Algorithm21ColumnVisibility
        { get { return this.Algorithms.Count > 20 && this.Algorithms[20].DomainState != lib.DomainObjectState.Deleted ? Visibility.Visible : Visibility.Collapsed; } }

        public override bool SaveDataChanges()
        {
            bool isSuccess = true;
            System.Text.StringBuilder err = new System.Text.StringBuilder();
            err.AppendLine("Изменения не сохранены");
            myfdbm.Errors.Clear();
            foreach (AlgorithmFormula item in myalgorithmformulas)
            {
                if (item.Formula.IsDirty && !item.Formula.Validate(true))
                { isSuccess = false; err.AppendLine(item.Formula.Errors); }
                //foreach (AlgorithmValuesVM values in item.Algorithms) // все равно сохраняем
                //    if (values.HasErrors)
                //    { isSuccess = false; err.AppendLine(values.Errors); }
            }
            if (!myfdbm.SaveCollectionChanches())
            {
                isSuccess = false;
                err.AppendLine(myfdbm.ErrorMessage);
            }
            myadbm.Errors.Clear();
            if (!myadbm.SaveCollectionChanches())
            {
                isSuccess = false;
                err.AppendLine(myadbm.ErrorMessage);
            }
            myvdbm.Errors.Clear();
            if (!myvdbm.SaveCollectionChanches())
            {
                isSuccess = false;
                err.AppendLine(myvdbm.ErrorMessage);
            }

            if (!isSuccess)
                this.PopupText = err.ToString();
            return isSuccess;
        }
        protected override bool CanSaveDataChanges()
        {
            return true;
        }
        protected override void RefreshData(object parametr)
        {
            System.Text.StringBuilder err = new System.Text.StringBuilder();
            myadbm.Errors.Clear();
            myadbm.Fill();
            if (myadbm.Errors.Count > 0) err.AppendLine(myadbm.ErrorMessage);
            this.PropertyChangedNotification("Algorithms");
            for (int i = 1; i < 22; i++)
                this.PropertyChangedNotification("Algorithm"+i.ToString()+"ColumnVisibility");
            myfdbm.Errors.Clear();
            myfdbm.Fill();
            if (myfdbm.Errors.Count > 0) err.AppendLine(myfdbm.ErrorMessage);
            myvdbm.Errors.Clear();
            myvdbm.Fill();
            if (myvdbm.Errors.Count > 0) err.AppendLine(myadbm.ErrorMessage);
            myalgorithmformulas.Clear();
            AlgorithmValues values = null;
            foreach (FormulaVM frm in myformulasynchronizer.ViewModelCollection)
            {
                AlgorithmFormula algfrm = new AlgorithmFormula(frm, lib.DomainObjectState.Unchanged);
                myalgorithmformulas.Add(algfrm);
                foreach (Algorithm alg in myalgorithms)
                {
                    values = null;
                    foreach (AlgorithmValues vals in myvdbm.Collection)
                    {
                        if (vals.Formula == frm.DomainObject && vals.Algorithm == alg)
                        {
                            values = vals;
                            break;
                        }
                    }
                    if (values == null) values = AlgorithmValuesCreate(alg, frm.DomainObject);
                    algfrm.Algorithms.Add(new AlgorithmValuesVM(values));
                }
            }
            foreach (Algorithm alg in myalgorithms)
                alg.FormulasInit();
            myview1.MoveCurrentToPosition(-1);
            myview2.MoveCurrentToPosition(-1);
            if (err.Length > 0)
                this.PopupText = err.ToString();
        }
        protected override bool CanRefreshData()
        {
            return true;
        }
        protected override void RejectChanges(object parametr)
        {
            System.Collections.Generic.List<AlgorithmFormula> algfrmdeleted = new System.Collections.Generic.List<AlgorithmFormula>();
            foreach (AlgorithmFormula algfrm in myalgorithmformulas)
            {
                if (algfrm.DomainState == lib.DomainObjectState.Added)
                    algfrmdeleted.Add(algfrm);    
                else 
                {
                    algfrm.Formula.Reject.Execute(null);
                    foreach (AlgorithmValuesVM val in algfrm.Algorithms)
                        val.RejectChanges();
                    if (algfrm.DomainState == lib.DomainObjectState.Deleted)
                    {
                        myview1.EditItem(algfrm);
                        myview2.EditItem(algfrm);
                        algfrm.DomainState = lib.DomainObjectState.Unchanged;
                        myview1.CommitEdit();
                        myview2.CommitEdit();
                    }
                }
            }
            foreach (AlgorithmFormula delitem in algfrmdeleted)
            {
                myalgorithmformulas.Remove(delitem);
                delitem.DomainState = lib.DomainObjectState.Destroyed;
                myformulasynchronizer.ViewModelCollection.Remove(delitem.Formula);
                foreach (AlgorithmValuesVM val in delitem.Algorithms)
                {
                    val.DomainObject.Algorithm.Formulas.Remove(val.DomainObject);
                    myvdbm.Collection.Remove(val.DomainObject);
                }
            }
            for (int i = 0; i < myalgorithms.Count; i++)
            {
                myalgorithms[i].RejectChanges();
                this.PropertyChangedNotification("Algorithm" + (i + 1).ToString() + "ColumnVisibility");
            }
        }
        protected override bool CanRejectChanges()
        {
            return true;
        }
        protected override void AddData(object parametr)
        {
            if (parametr != null)
            {
                this.EndEdit();
                FormulaVM newformula = new FormulaVM(new Formula());
                myformulasynchronizer.ViewModelCollection.Add(newformula);
                AlgorithmFormula newitem = new AlgorithmFormula(newformula, lib.DomainObjectState.Added);
                if (int.Parse((string)parametr) == 1)
                {
                    newformula.FormulaType = 1;
                    myview1.AddNewItem(newitem);
                    myview1.CommitNew();
                }
                else
                {
                    newformula.FormulaType = 101;
                    myview2.AddNewItem(newitem);
                    myview2.CommitNew();
                }
                foreach (Algorithm alg in myalgorithms)
                {
                    newitem.Algorithms.Add(new AlgorithmValuesVM(AlgorithmValuesCreate(alg, newformula.DomainObject)));
                }
            }
        }
        protected override bool CanAddData(object parametr)
        {
            return true;
        }
        protected override void DeleteData(object parametr)
        {
            if (parametr is AlgorithmFormula)
            {
                if (MessageBox.Show("Удалить формулу?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    AlgorithmFormula item = parametr as AlgorithmFormula;
                    if (item.DomainState == lib.DomainObjectState.Added)
                    {
                        myalgorithmformulas.Remove(item);
                        item.DomainState = lib.DomainObjectState.Destroyed;
                        myformulasynchronizer.ViewModelCollection.Remove(item.Formula);
                        foreach (AlgorithmValuesVM val in item.Algorithms)
                        {
                            myvdbm.Collection.Remove(val.DomainObject);
                            val.DomainObject.Algorithm.Formulas.Remove(val.DomainObject);
                        }
                    }
                    else
                    {
                        myview1.EditItem(item);
                        myview2.EditItem(item);
                        item.DomainState = lib.DomainObjectState.Deleted;
                        myview1.CommitEdit();
                        myview2.CommitEdit();
                        item.Formula.DomainState = lib.DomainObjectState.Deleted;
                        foreach (AlgorithmValuesVM val in item.Algorithms)
                            val.DomainState = lib.DomainObjectState.Deleted;
                    }
                }
            }
        }
        protected override bool CanDeleteData(object parametr)
        {
            return true;
        }

        protected AlgorithmValues AlgorithmValuesCreate(Algorithm algorithm, Formula formula)
        {
            AlgorithmValues values = new AlgorithmValues(algorithm, formula);
            myvdbm.Collection.Add(values);
            myvaluesstorage.UpdateItem(values);
            return values;
        }
        internal void DeleteAlgorithm(int n)
        {
            Algorithm item = myalgorithms[n];
            item.DomainState = lib.DomainObjectState.Deleted;
            foreach (AlgorithmValues val in item.Formulas)
                val.DomainState = lib.DomainObjectState.Deleted;
            this.PropertyChangedNotification("Algorithm"+(n+1).ToString()+"ColumnVisibility");
        }
    }
}
