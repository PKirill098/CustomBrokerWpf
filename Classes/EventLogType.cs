using System.Data;
using System.Data.SqlClient;

namespace KirillPolyanskiy.CustomBrokerWpf.Classes
{
    public class EventLogType
    {
        public string Name { set; get; }
        public string Description { set; get; }
        internal EventLogType() { }
        internal EventLogType(string name, string description) : this()
        { this.Name = name; this.Description = description; }
    }

    internal class EventLogTypeList : ListNotifyChanged<EventLogType>, IReference<EventLogType>
    {
        internal EventLogTypeList()
            : base()
        {
            this.Fill();
        }

        private SqlDataReader GetReader(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT name,descr FROM EventLogType_vw";
            cmd.Connection = conn;
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection & CommandBehavior.SingleResult);
            return reader;
        }
        private void Fill()
        {

            using (SqlConnection conn = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
            {
                SqlDataReader reader = GetReader(conn);
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0)) this.Add(new EventLogType(reader.GetString(0), reader.GetString(1)));
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
                    EventLogType newitem = new EventLogType(reader.GetString(0), reader.GetString(1));
                    for (int i = startIndex; i < this.Count; i++)
                    {
                        startIndex = i + 1;
                        EventLogType olditem = this[i];
                        compare = olditem.Name.CompareTo(newitem.Name);
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
            }
            this.OnResetCollectionChanged();
        }

        public EventLogType FindFirstItem(string propertyName, object value)
        {
            if (propertyName == "Name")
                return this.Find(x => string.Equals(x.Name, value));
            else
                return this.Find(x => string.Equals(x.Description, value));
        }
    }
}
