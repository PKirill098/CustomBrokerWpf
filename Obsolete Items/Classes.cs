using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    class VisibilityfromUserRole
    {
        ReferenceDS refDS;
        public VisibilityfromUserRole()
        {
            try
            {
                refDS = App.Current.TryFindResource("keyReferenceDS") as ReferenceDS;
                ReferenceDSTableAdapters.UserRolesAdapter adapter = new ReferenceDSTableAdapters.UserRolesAdapter();
                if (refDS != null) adapter.Fill(refDS.tableUserRoles);
            }
            catch
            { App.Current.Shutdown(); }
        }
        public System.Windows.Visibility GetVisibility(string parm)
        {
            System.Windows.Visibility visiblt = System.Windows.Visibility.Visible;
            ReferenceDS.tableUserRolesRow row = null; //Desingmode епти
            if (refDS != null)
            {
                row = refDS.tableUserRoles.FindByRoleName(parm);
                if (row == null) row = refDS.tableUserRoles.FindByRoleName("db_owner");
                if (row == null)
                {
                    visiblt = System.Windows.Visibility.Collapsed;
                }
            }
            return visiblt;
        }
        public System.Windows.Visibility GetVisibility()
        {
            return System.Windows.Visibility.Visible;
        }
    }
    class VisibilityManagers
    {
        System.Windows.Visibility visiblt;
        public VisibilityManagers()
        {
            visiblt = System.Windows.Visibility.Visible;
            VisibilityfromUserRole CheckVisibilityObj = App.Current.TryFindResource("keyVisibilityfromUserRole") as VisibilityfromUserRole;
            if (CheckVisibilityObj != null) visiblt = CheckVisibilityObj.GetVisibility("Managers");
        }
        public System.Windows.Visibility Visibility
        { get { return visiblt; } }
        public bool IsMember
        { get { return visiblt == System.Windows.Visibility.Visible; } }
    }
    class VisibilityAccounts
    {
        System.Windows.Visibility visiblt;
        public VisibilityAccounts()
        {
            visiblt = System.Windows.Visibility.Visible;

            VisibilityfromUserRole CheckVisibilityObj = App.Current.TryFindResource("keyVisibilityfromUserRole") as VisibilityfromUserRole;
            if (CheckVisibilityObj != null) visiblt = CheckVisibilityObj.GetVisibility("Accounts");
        }
        public System.Windows.Visibility Visibility
        { get { return visiblt; } }
        public bool IsMember
        { get { return visiblt == System.Windows.Visibility.Visible; } }
    }
    class VisibilityAccountVisors
    {
        System.Windows.Visibility visiblt;
        public VisibilityAccountVisors()
        {
            visiblt = System.Windows.Visibility.Visible;

            VisibilityfromUserRole CheckVisibilityObj = App.Current.TryFindResource("keyVisibilityfromUserRole") as VisibilityfromUserRole;
            if (CheckVisibilityObj != null) visiblt = CheckVisibilityObj.GetVisibility("AccountVisors");
        }
        public System.Windows.Visibility Visibility
        { get { return visiblt; } }
        public bool IsMember
        { get { return visiblt == System.Windows.Visibility.Visible; } }
    }
    class VisibilityTopManagers
    {
        System.Windows.Visibility visiblt;
        public VisibilityTopManagers()
        {
            visiblt = System.Windows.Visibility.Visible;

            VisibilityfromUserRole CheckVisibilityObj = App.Current.TryFindResource("keyVisibilityfromUserRole") as VisibilityfromUserRole;
            if (CheckVisibilityObj != null) visiblt = CheckVisibilityObj.GetVisibility("TopManagers");
        }
        public System.Windows.Visibility Visibility
        { get { return visiblt; } }
        public bool IsMember
        { get { return visiblt == System.Windows.Visibility.Visible; } }
    }

}
