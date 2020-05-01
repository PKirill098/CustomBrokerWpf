using System.Data;
using System.Data.SqlClient;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes
{
	public class Principal
	{
		private bool? myisrole;
        private PrincipalList myprincipals;

		public string Name { set; get; }
		public ListNotifyChanged<Principal> Principals
		{
			get
			{
				if (myprincipals == null & myisrole.HasValue) myprincipals = new PrincipalList(!myisrole.Value);
				return myprincipals;
			}
		}

        internal Principal()
		:this(string.Empty){ }
		internal Principal(string name)
			:this(name,null)
		{}
		internal Principal(bool isrole)
			: this(string.Empty, isrole) { }
		internal Principal(string name,bool? isrole)
		{
			this.Name = name;
			this.myisrole = isrole;
        }
	}

	internal class PrincipalList : ListNotifyChanged<Principal>, IReference<Principal>
	{
		private bool myisrole;
		private string myowner;

		internal PrincipalList(bool isrole) : this(isrole, string.Empty) { }
        internal PrincipalList(bool isrole,string owner) : base()
		{
			myisrole = isrole;
			myowner = owner;
            this.Fill();
		}

		public Principal FindFirstItem(string propertyName, object value)
		{
			return this.Find(x => string.Equals(x.Name, value));
		}

		private SqlDataReader GetReader(SqlConnection conn)
		{
			SqlCommand cmd = new SqlCommand();
			if (string.IsNullOrEmpty(myowner))
			{
				cmd.CommandType = CommandType.Text;
				if (myisrole)
					cmd.CommandText = "SELECT name FROM dbo.Principals_vw WHERE [type]='R' ORDER BY name";
				else
					cmd.CommandText = "SELECT name FROM dbo.Principals_vw WHERE [type]='U' ORDER BY name";
			}
			else
			{
				cmd.CommandType = CommandType.StoredProcedure;
				if (myisrole)
				{
					cmd.CommandText = "dbo.MemberRoles_sp";
					cmd.Parameters.Add(new SqlParameter("@user", myowner));
				}
				else
				{
					cmd.CommandText = "dbo.RoleMembers_sp";
					cmd.Parameters.Add(new SqlParameter("@role", myowner));
                }
			}
			cmd.Connection = conn;
			conn.Open();
			SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection & CommandBehavior.SingleResult);
			return reader;
        }
		private void Fill()
		{
			using (SqlConnection conn = new SqlConnection(CustomBrokerWpf.Properties.Settings.Default.CustomBrokerConnectionString))
			{
				SqlDataReader reader = GetReader(conn);
				while (reader.Read())
				{
					if (!reader.IsDBNull(0)) this.Add(new Principal(reader.GetString(0), myisrole));
				}
				reader.Close();
			}
			//if (!(this.Count > 0 && this[0].Name.Equals(string.Empty))) this.Insert(0, new Principal(string.Empty));
		}
		public void Refresh()
		{
			using (SqlConnection conn = new SqlConnection(CustomBrokerWpf.Properties.Settings.Default.CustomBrokerConnectionString))
			{
				int compare, startIndex;
				startIndex = 0;
				SqlDataReader reader = GetReader(conn);
				while (reader.Read())
				{
					if (reader.IsDBNull(0)) continue;
					compare = -1;
					Principal newPrincipal = new Principal(reader.GetString(0), myisrole);
					for (int i = startIndex; i < this.Count; i++)
					{
						startIndex = i + 1;
						Principal oldPrincipal = this[i];
						compare = oldPrincipal.Name.CompareTo(newPrincipal.Name);
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
							this.Insert(i, newPrincipal);
							break;
						}
					}
					if (compare < 0) this.Add(newPrincipal);
				}
				reader.Close();
			}
			this.OnResetCollectionChanged();
		}
	}
}
