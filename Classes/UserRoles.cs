using KirillPolyanskiy.CustomBrokerWpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    class VisibilityfromUserRole
    {
        public bool IsMember(string parm)
        {
            bool ismember = false;
            if (References.CurrentUserRoles != null)
            {
                string role = References.CurrentUserRoles.FindFirstItem(string.Empty, parm);
                if (role == null) role = References.CurrentUserRoles.FindFirstItem(string.Empty, "db_owner");
                ismember = role != null;
            }
            else
            {
                App.Current.Shutdown();
            }
            return ismember;
        }
    }
    class VisibilityRole
    {
        public VisibilityRole(string rolename)
        {
            myismember = false;
            VisibilityfromUserRole CheckVisibilityObj = App.Current.TryFindResource("keyVisibilityfromUserRole") as VisibilityfromUserRole;
            if (CheckVisibilityObj != null) myismember = CheckVisibilityObj.IsMember(rolename);
        }

        protected bool myismember;
        public System.Windows.Visibility Visibility
        { get { return myismember ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed; } }
        public System.Windows.Visibility Collapsed
        { get { return myismember ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible; } }
        public bool IsMember
        { get { return myismember; } }
        public bool IsOutsider
        { get { return !myismember; } }
    }
    class VisibilityAdmins: VisibilityRole
    {
        public VisibilityAdmins():base("Admins")
        {
        }
    }
    class VisibilityManagers: VisibilityRole
    {
        public VisibilityManagers():base("Managers")
        {
        }
    }
    class VisibilityAccounts : VisibilityRole
    {
        public VisibilityAccounts():base("Accounts")
        {
        }
    }
    class VisibilityAccountVisors : VisibilityRole
    {
        public VisibilityAccountVisors():base("AccountVisors")
        {
        }
    }
    class VisibilityTopManagers : VisibilityRole
    {
        public VisibilityTopManagers():base("TopManagers")
        {
        }
    }
    class VisibilityLManagers : VisibilityRole
    {
        public VisibilityLManagers():base("LManagers")
        {
        }
    }
    class VisibilityManagersLManagers : VisibilityRole
    {
        public VisibilityManagersLManagers():base("Managers")
        {
            VisibilityLManagers lmanager = new VisibilityLManagers();
            myismember &= lmanager.IsMember;
        }
    }
    class VisibilityLAccounts : VisibilityRole
    {
        public VisibilityLAccounts() : base("LAccounts")
        {
        }
    }
    class VisibilityAccountsLAccounts : VisibilityRole
    {
        public VisibilityAccountsLAccounts():base("Accounts")
        {
            VisibilityLAccounts lmember = new VisibilityLAccounts();
            myismember &= lmember.IsMember;
        }
    }
    class VisibilityAlgorithmWriters : VisibilityRole
    {
        public VisibilityAlgorithmWriters() : base("AlgorithmWriters")
        {
        }
    }

    internal class CurrentUserRoleList : List<string>, IReference<string>
    {
        internal CurrentUserRoleList()
        {
            this.Fill();
        }

        private SqlDataReader GetReader(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.UserRoles_sp";
            cmd.Connection = conn;
            conn.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection & CommandBehavior.SingleResult);
        }
        private void Fill()
        {
            using (SqlConnection conn = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
            {
                SqlDataReader reader = GetReader(conn);
                while (reader.Read())
                {
                    this.Add(reader.GetString(0));
                }
                reader.Close();
            }
        }
        public void Refresh()
        {
            using (SqlConnection conn = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
            {
                int compare, startIndex;
                startIndex = 0;
                SqlDataReader reader = GetReader(conn);
                while (reader.Read())
                {
                    if (reader.IsDBNull(0)) continue;
                    compare = -1;
                    string newitem = reader.GetString(0);
                    for (int i = startIndex; i < this.Count; i++)
                    {
                        startIndex = i + 1;
                        string olditem = this[i];
                        compare = olditem.CompareTo(newitem);
                        if (compare < 0)
                        {
                            this.RemoveAt(i);
                            i--;
                        }
                        if (compare == 0)
                        {
                            break;
                        }
                        else if (compare > 0)
                        {
                            this.Insert(i, newitem);
                            break;
                        }
                    }
                    if (compare < 0) this.Add(newitem);
                }
                reader.Close();
                if (this.Count > startIndex) for (int i = startIndex; i < this.Count; i++) this.RemoveAt(i);
            }
        }

        public string FindFirstItem(string propertyName, object value)
        {
            return this.Find(x => x.ToUpper().Equals((value as string).ToUpper()));
        }
    }
}
