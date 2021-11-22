using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib = KirillPolyanskiy.DataModelClassLibrary;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes.Domain
{
    public class GTDRegistry : lib.DomainBaseStamp
    {
        public GTDRegistry(int id,long stamp, lib.DomainObjectState dstate
            , string gtd
            ):base(id,stamp,null,null,dstate)
        {
            mygtd = gtd;
        }

        private string mygtd;
        public string GTD
        {
            set { SetProperty<string>(ref mygtd,value); }
            get { return mygtd; }
        }

        protected override void RejectProperty(string property, object value)
        {
            throw new NotImplementedException();
        }
        protected override void PropertiesUpdate(lib.DomainBaseReject sample)
        {
            GTDRegistry temp = (GTDRegistry)sample;
            this.UpdateIsOver = true;
            this.GTD = temp.GTD;
            this.UpdateIsOver = false;
        }
    }
    class GTDRegistryDBM : lib.DBManagerStamp<GTDRegistry>
    {
        protected override GTDRegistry CreateItem(SqlDataReader reader,SqlConnection addcon)
        {
            throw new NotImplementedException();
        }

        protected override void GetOutputSpecificParametersValue(GTDRegistry item)
        {
            throw new NotImplementedException();
        }

        protected override bool SaveChildObjects(GTDRegistry item)
        {
            throw new NotImplementedException();
        }

        protected override bool SaveIncludedObject(GTDRegistry item)
        {
            throw new NotImplementedException();
        }

        protected override bool SaveReferenceObjects()
        {
            throw new NotImplementedException();
        }

        protected override void SetSelectParametersValue()
        {
            throw new NotImplementedException();
        }

        protected override bool SetSpecificParametersValue(GTDRegistry item)
        {
            throw new NotImplementedException();
        }
        protected override void LoadObjects(GTDRegistry item)
        {
        }
        protected override bool LoadObjects()
        { return true; }
    }
}
