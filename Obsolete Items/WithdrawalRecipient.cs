using System.Data;
using System.Data.SqlClient;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    public class WithdrawalRecipient
    {
        public string Name { set; get; }
        internal WithdrawalRecipient() {}
        internal WithdrawalRecipient(string name)
        { this.Name = name; }
    }

    internal class WithdrawalRecipientList : ListNotifyChanged<WithdrawalRecipient>, IReference<WithdrawalRecipient>
    {
        internal WithdrawalRecipientList()
            : base()
        {
            this.Fill();
        }

        private void Fill()
        {
            
            using (SqlConnection conn = new SqlConnection(CustomBrokerWpf.Properties.Settings.Default.CustomBrokerConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT recipient FROM ExpenditureRecipientList_vw ORDER BY recipient";
                cmd.Connection = conn;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection & CommandBehavior.SingleResult);
                while (reader.Read())
                {
                    if(!reader.IsDBNull(0)) this.Add(new WithdrawalRecipient(reader.GetString(0)));
                }
                reader.Close();
            }
            if(!(this.Count>0 && this[0].Name.Equals(string.Empty))) this.Insert(0,new WithdrawalRecipient(string.Empty));
        }
        
        public void Refresh()
        {
            using (SqlConnection conn = new SqlConnection(CustomBrokerWpf.Properties.Settings.Default.CustomBrokerConnectionString))
            {
                int compare,startIndex;
                startIndex = 0;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT recipient FROM ExpenditureRecipientList_vw ORDER BY recipient";
                cmd.Connection = conn;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection & CommandBehavior.SingleResult);
                while (reader.Read())
                {
                    if (reader.IsDBNull(0)) continue;
                    compare = -1;
                    WithdrawalRecipient newRecipient = new WithdrawalRecipient(reader.GetString(0));
                    for (int i = startIndex; i < this.Count; i++)
                    {
                        startIndex = i+1;
                        WithdrawalRecipient oldRecipient = this[i];
                        compare=oldRecipient.Name.CompareTo(newRecipient.Name);
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
                            this.Insert(i, newRecipient);
                            break;
                        }
                    }
                    if(compare<0) this.Add(newRecipient);
                }
                reader.Close();
            }
            this.LaunchCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedAction.Reset);
        }
        public WithdrawalRecipient FindFirstItem(string propertyName, object value)
        {
            return this.Find(x => x.Name.Equals(value));
        }

        private bool FindRecipient(WithdrawalRecipient recipient)
        {
            return false;
        }

    }
}
