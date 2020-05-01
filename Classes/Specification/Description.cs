using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Specification
{
    internal class GoodsDescription
    {
        internal GoodsDescription():base()
        { Ingredients = new List<Part>(); }

        private string myclientdescription;
        internal string ClientDescription
        {
            set
            {
                myclientdescription = value;
                myerror = string.Empty;
                RecognizeDescription();
                SetMapping();
            }
            get { return myclientdescription; }
        }
        private string myclientcomposition;
        internal string ClientComposition
        {
            set
            {
                myclientcomposition = value.Trim();
                RecognizeComposition();
            }
            get { return myclientcomposition; }
        }
        private string mygoodsname;
        internal string GoodsName { get { return mygoodsname; } }
        private Domain.Gender mygender;
        internal Domain.Gender Gender { get { return mygender; } }
        internal string ClientMaterial{set;get;}
        private Material mymaterial;
        internal Material Material { set { mymaterial = value; } get { return mymaterial; } }
        private Mapping mymapping;
        public Mapping Mapping
        {
            set { mymapping = value; }
            get { return mymapping; }
        }
        private string myerror;
        internal string Error
        { set { myerror = value; } get { return myerror; } }
        private System.Collections.ObjectModel.ObservableCollection<Mapping> mymappings; 
        public System.Collections.ObjectModel.ObservableCollection<Mapping> Mappings
        { set { mymappings = value; } }
        internal List<Part> Ingredients { private set;get;}
        internal Part MaxPart
        { get { return Ingredients.Count>0? Ingredients[0]:null; } }
        private int mycountrycategory;
        internal int CountryCategory
        {
            set { mycountrycategory = value; }
            get { return mycountrycategory; }
        }
        private void RecognizeDescription()
        {
            int g = -1, n = -1, m = 0;
            mygender = null;
            mymaterial = null;
            this.ClientMaterial = string.Empty;
            string clientdscr = myclientdescription.ToLower();
            foreach (Classes.Domain.Gender item in References.Genders)
            {
                g = clientdscr.IndexOf(" " + item.ShortName);
                if (!(g < 0))
                {
                    mygender = item;
                    break;
                }
            }
            foreach (Classes.Specification.Material item in References.Materials)
            {
                int e;

                string[] str = item.ShortName.ToLower().Split(' ');
                e = g;
                foreach (string istr in str)
                {
                    e = clientdscr.IndexOf(" " + istr, e + 1);
                    if (n < 0) n = e;
                    if (e < 0) break;
                }
                if (e < 0)
                    n = -1;
                else if(m < item.ShortName.Length)
                {
                    m = item.ShortName.Length;
                    mymaterial = item;
                    this.ClientMaterial = clientdscr.Substring(n);
                }
            }

            if (!(g < 0))
                mygoodsname = clientdscr.Substring(0, g);
            else if (!(n < 0))
                mygoodsname = clientdscr.Substring(0, n);
            else
                mygoodsname = clientdscr;
        }
        private void RecognizeDescriptionAllPrice()
        {
            int g = -1, n = -1, m = 0;
            string clientdscr = myclientdescription.ToLower();
            foreach (Classes.Domain.Gender item in References.Genders)
            {
                g = clientdscr.IndexOf(" " + item.ShortName);
                if (!(g < 0))
                    g = clientdscr.IndexOf(" [" + item.ShortName);
                if (!(g < 0))
                {
                    mygender = item;
                    break;
                }
            }
            foreach (Classes.Specification.Material item in References.Materials)
            {
                int e;

                string[] str = item.ShortName.ToLower().Split(' ');
                e = g;
                foreach (string istr in str)
                {
                    e = clientdscr.IndexOf(" " + istr, e + 1);
                    if (n < 0) n = e;
                    if (e < 0) break;
                }
                if (e < 0)
                    n = -1;
                else if (m < item.ShortName.Length)
                {
                    m = item.ShortName.Length;
                    mymaterial = item;
                    this.ClientMaterial = clientdscr.Substring(n);
                }
            }

            if (!(g < 0))
                mygoodsname = clientdscr.Substring(0, g);
            else if (!(n < 0))
                mygoodsname = clientdscr.Substring(0, n);
            else
                mygoodsname = clientdscr;
        }
        private void SetMapping()
        {
            System.Windows.Data.ListCollectionView items = new System.Windows.Data.ListCollectionView(mymappings);
            items.Filter = OnFilter;
            if (items.Count == 1)
            {
                items.MoveCurrentToFirst();
                mymapping = items.CurrentItem as Mapping;
            }
            else if (items.Count == 0)
                myerror = "НЕ найдено ни одного соответствия в СООТВЕТСТВИИ";
            else
                myerror = "Найдено более одного соответствия в СООТВЕТСТВИИ";
        }
        private bool OnFilter(object item)
        {

            bool where;
            bool orwhere = false;
            Mapping mapp = item as Mapping;
            where = MappingViewCommand.ViewFilterDefault(mapp);
            if (where)
            {
                where &= object.Equals(this.Material,mapp.Material) || (this.Material?.Upper!=null && object.Equals(this.Material.Upper,mapp.Material) || (this.Material?.Upper?.Upper != null && object.Equals(this.Material.Upper.Upper, mapp.Material)));
            }
            //else
            //    myerror = "Материал " + this.Material.Name + " не найден  в СООТВЕТСТВИИ";
            if (where && this.Gender != null)
            {
                foreach (MappingGender mg in mapp.Genders)
                    orwhere |= this.Gender.Equals(mg.Gender);
                where &= orwhere;
            }
            //else
            //    myerror = "Пол " + this.Gender.Name + " не найден  в СООТВЕТСТВИИ";
            if (where)
            {
                orwhere = mapp.Goods.ToLower().Equals(this.mygoodsname.ToLower());
                if (!orwhere)
                    foreach (GoodsSynonym synm in mapp.Synonyms)
                        if (synm.Name.ToLower().Equals(this.mygoodsname.ToLower()))
                        {
                            orwhere=true;
                            break;
                        }
                //if(!orwhere) myerror = "Товар " + mygoodsname + " не найден  в СООТВЕТСТВИИ";
                where &= orwhere;
            }
            return where;
        }

        private void RecognizeComposition()
        {
            bool isnumeric = false, isletter = false, isperready = false, isnameready=false;
            int st,sp;
            Part pt = null;
            Ingredients.Clear();
            for (int n=0;n< myclientcomposition.Length;n++)
            {
                if (!(isperready | isnameready)) pt = new Part();
                isnumeric = char.IsDigit(myclientcomposition[n]);
                isletter = char.IsLetter(myclientcomposition[n]);
                while (!(isnumeric | isletter))
                {
                    n++;
                    isnumeric = char.IsDigit(myclientcomposition[n]);
                    isletter = char.IsLetter(myclientcomposition[n]);
                }
                st = n;
                do n++;
                while (n < myclientcomposition.Length && isnumeric == char.IsDigit(myclientcomposition[n]) & ((isletter & char.IsWhiteSpace(myclientcomposition[n])) || isletter == char.IsLetter(myclientcomposition[n])));
                sp = n;
                if (isnumeric)
                {
                    isperready = true;
                    pt.PartPer = int.Parse(myclientcomposition.Substring(st, sp - st));
                }
                else if(isletter)
                {
                    isnameready = true;
                    pt.PartName = myclientcomposition.Substring(st, sp - st).Trim();
                }
                if(isperready & isnameready)
                {
                    
                    isnameready = false;
                    for (st = 0; st < Ingredients.Count; st++)
                        if (Ingredients[st].CompareTo(pt) < 1)
                        {
                            Ingredients.Insert(st, pt);
                            isperready = false;
                        }
                    if(isperready)
                    {
                        Ingredients.Add(pt);
                        isperready = false;
                    }
                }
            }
        }

        internal Material GetMaterial(string name)
        {
            int e=0, m =0;
            Material material = null;
            foreach (Classes.Specification.Material item in References.Materials)
            {
                e = 0;
                string[] str = item.ShortName.ToLower().Split(' ');
                foreach (string istr in str)
                {
                    e = name.IndexOf(istr, e);
                    if (e < 0) break;
                    else e = e + istr.Length;
                }
                if ((e > 0) & m < item.ShortName.Length)
                {
                    m = item.ShortName.Length;
                    material = item;
                }
            }
            return material;
        }
        internal bool RecognizeCountry(string find)
        {
            bool issuccess = true;
            CustomBrokerWpf.Domain.References.Country country = null;
            foreach (CustomBrokerWpf.Domain.References.Country citem in CustomBrokerWpf.References.Countries)
            {
                if (string.Equals(find, citem.Name.Trim().ToLower()) | string.Equals(find, citem.FullName.Trim().ToLower()))
                {
                    country = citem;
                }
                else
                {
                    string[] strs = citem.Synonym.ToLower().Split(',');
                    foreach (string stritem in strs)
                        if (string.Equals(find, stritem.Trim().ToLower()))
                        {
                            country = citem;
                            break;
                        }
                }
                if (country != null) break;
            }
            if (country?.PriceCategory != null)
                this.CountryCategory = country.PriceCategory.Value;
            else
                issuccess=false;
            return issuccess;
        }
        internal static bool StrsInStrs(string strs1, string strs2, char separator1, char separator2)
        {
            bool isfinds=true;
            string[] strs;
            string[] finds;
            int n = 0;
            if (string.IsNullOrEmpty(strs1) != string.IsNullOrEmpty(strs2))
                return false;
            if (string.IsNullOrEmpty(strs1) & string.IsNullOrEmpty(strs2))
                return true;
            strs = strs1.ToLower().Split(separator1);
            finds = strs2.ToLower().Split(separator2);
            foreach (string gstr in strs)
            {
                n = 0;
                isfinds = true;
                foreach (string fstr in finds)
                {
                    n = gstr.IndexOf(fstr, n);
                    if (n < 0)
                    {
                        isfinds = false;
                        break;
                    }
                    else
                    {
                        n += fstr.Length;
                    }
                }
                if (isfinds) break;
            }
            return isfinds;
        }
    }

    internal class Part : IEquatable<Part>,IComparable<Part>
    {
        internal string PartName { get; set; }
        internal int PartPer { get; set; }

        public override string ToString()
        {
            return PartPer.ToString() + "% " + PartName;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Part objAsPart = obj as Part;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }
        public bool Equals(Part other)
        {
            if (other == null) return false;
            return (this.PartPer.Equals(other.PartPer) & this.PartName.Equals(other.PartName));
        }
        public override int GetHashCode()
        {
            return (PartPer + PartName).GetHashCode();
        }

        public int CompareTo(Part other)
        {
            return this.PartPer.CompareTo(other.PartPer);
        }

        public static bool operator == (Part part1, Part part2)
        {
            return object.Equals(part1, part2) || part1.Equals(part2);
        }
        public static bool operator != (Part part1, Part part2)
        {
            return !(object.Equals(part1, part2) || part1.Equals(part2));
        }

    }

}
