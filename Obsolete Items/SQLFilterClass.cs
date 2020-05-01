using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;


namespace KirillPolyanskiy.CustomBrokerWpf
{
    internal interface ISQLFiltredWindow
    {
        bool IsShowFilter { set; get; }
        SQLFilter Filter { set; get; }
        void runFilter();
    }

    public class SQLFilter
    {
        bool isdefault = false;
        private int filterID = 0;
        private string connectionString = Properties.Settings.Default.CustomBrokerConnectionString;
        private string mainGroupOperator;
        private string winame;

        internal SQLFilter(string WindowName, string MainGroupOperator)
        {
            winame = WindowName;
            mainGroupOperator = MainGroupOperator;
            GetDefaultFilter();
        }
        ~SQLFilter() { RemoveFilter(); }

        private void CreateFilter()
        {
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 180;
                com.CommandText = "dbo.FilterCreate_sp";
                SqlParameter oper = new SqlParameter("@operator", mainGroupOperator);
                SqlParameter namegroup = new SqlParameter("@namegroup", "main");
                SqlParameter filterid = new SqlParameter("@filterid", 0);
                filterid.SqlDbType = SqlDbType.Int;
                filterid.DbType = DbType.Int32;
                filterid.Direction = ParameterDirection.Output;
                com.Parameters.Add(oper); com.Parameters.Add(namegroup); com.Parameters.Add(filterid);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
                filterID = (int)com.Parameters["@filterid"].Value;
            }
            finally
            {
                con.Close();
            }
        }
        internal void RemoveFilter()
        {
            if (filterID == 0) return;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand();
            try
            {
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 180;
                com.CommandText = "dbo.FilterDelete_sp";
                SqlParameter filter = new SqlParameter("@filterid", filterID);
                com.Parameters.Add(filter);
                con.Open();
                com.ExecuteNonQuery();
                filterID = 0;
            }
            finally
            {
                con.Close();
            }
        }
        internal void GetDefaultFilter()
        {
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 180;
                com.CommandText = "dbo.FilterGetDefault_sp";
                SqlParameter winameParameter = new SqlParameter("@filterClass", winame);
                SqlParameter filterid = new SqlParameter("@filterid", 0);
                filterid.Direction = ParameterDirection.Output;
                filterid.SqlDbType = SqlDbType.Int;
                filterid.IsNullable = true;
                com.Parameters.Add(winameParameter); com.Parameters.Add(filterid);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
                if (!com.Parameters["@filterid"].Value.Equals(System.DBNull.Value))
                    filterID = (int)com.Parameters["@filterid"].Value;
                else
                    filterID = 0;
            }
            finally
            {
                con.Close();
            }
        }
        internal void SetDefaultFilter()
        {
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 180;
                com.CommandText = "dbo.FilterSaveDefault_sp";
                SqlParameter winameParameter = new SqlParameter("@filterClass", winame);
                SqlParameter filterid = new SqlParameter("@filterid", filterID);
                com.Parameters.Add(winameParameter); com.Parameters.Add(filterid);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
            }
            finally
            {
                con.Close();
            }
        }

        internal int GroupAdd(int ownergroup, string namegroup, string operatorgroup)
        {
            if (filterID == 0)
            {
                CreateFilter();
                ownergroup = filterID;
            }
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand();
            try
            {
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 180;
                com.CommandText = "dbo.FilterGroupCreate_sp";
                SqlParameter ownergroupid = new SqlParameter("@ownergroupid", ownergroup);
                SqlParameter groupname = new SqlParameter("@namegroup", namegroup);
                SqlParameter groupoperator = new SqlParameter("@operator", operatorgroup);
                SqlParameter groupid = new SqlParameter("@groupid", 0);
                groupid.Direction = ParameterDirection.Output;
                groupid.SqlDbType = SqlDbType.Int;
                com.Parameters.Add(ownergroupid); com.Parameters.Add(groupname); com.Parameters.Add(groupoperator); com.Parameters.Add(groupid);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
                return (int)com.Parameters["@groupid"].Value;
            }
            finally
            {
                con.Close();
            }
        }
        internal int GroupDel(int idgroup)
        {
            if (filterID == 0) return 0;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand();
            try
            {
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 180;
                com.CommandText = "dbo.FilterGroupDel_sp";
                SqlParameter groupid = new SqlParameter("@idgroup", idgroup);
                com.Parameters.Add(groupid);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
                return 0;
            }
            finally
            {
                con.Close();
            }
        }

        internal int ConditionAdd(int groupid, string propertyName, string ConditionOperator)
        {
            if (filterID == 0)
            {
                CreateFilter();
                groupid = filterID;
            }
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand();
            try
            {
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 180;
                com.CommandText = "dbo.FilterPropertyAdd_sp";
                SqlParameter group = new SqlParameter("@groupid", groupid);
                SqlParameter property = new SqlParameter("@property", propertyName);
                SqlParameter @operator = new SqlParameter("@operator", ConditionOperator);
                SqlParameter propertyid = new SqlParameter("@propertyid", 0);
                @propertyid.Direction = ParameterDirection.Output;
                @propertyid.SqlDbType = SqlDbType.Int;
                com.Parameters.Add(@group); com.Parameters.Add(@property); com.Parameters.Add(@operator); com.Parameters.Add(@propertyid);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
                return (int)com.Parameters["@propertyid"].Value;
            }
            finally
            {
                con.Close();
            }
        }
        internal void ConditionUpd(int ConditionId, string ConditionOperator)
        {
            if (filterID == 0) return;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand();
            try
            {
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 180;
                com.CommandText = "FilterPropertyUpd_sp";
                SqlParameter propertyid = new SqlParameter("@propertyid", ConditionId);
                SqlParameter oper = new SqlParameter("@operator", ConditionOperator);
                com.Parameters.Add(propertyid); com.Parameters.Add(oper);
                con.Open();
                com.ExecuteNonQuery();
            }
            finally
            {
                con.Close();
            }
        }
        internal void ConditionDel(int ConditionId)
        {
            if (filterID == 0) return;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand();
            try
            {
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 180;
                com.CommandText = "dbo.FilterPropertyDel_sp";
                SqlParameter oper = new SqlParameter("@propertyid", ConditionId);
                com.Parameters.Add(oper);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
            }
            finally
            {
                con.Close();
            }
        }

        internal void ConditionValuesAdd(int conditionId, string[] values)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand();
            try
            {
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 180;
                com.CommandText = "dbo.FilterValueAdd_sp";
                SqlParameter conditionIdParameter = new SqlParameter("@propertyid", conditionId);
                SqlParameter ispropertyParameter = new SqlParameter("@isproperty", false);
                SqlParameter valueParameter = new SqlParameter("@value", string.Empty);
                SqlParameter valueidParameter = new SqlParameter("@valueid", 0);
                valueidParameter.Direction = ParameterDirection.Output;
                valueidParameter.SqlDbType = SqlDbType.Int;
                com.Parameters.Add(conditionIdParameter); com.Parameters.Add(valueParameter); com.Parameters.Add(ispropertyParameter); com.Parameters.Add(valueidParameter);
                con.Open();
                foreach (string value in values)
                {
                    com.Parameters["@value"].Value = value;
                    com.ExecuteNonQuery();
                }
                con.Close();
            }
            finally
            {
                con.Close();
            }
        }
        internal int ConditionValueAdd(int conditionId, string value, byte isproperty)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand();
            try
            {
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 180;
                com.CommandText = "dbo.FilterValueAdd_sp";
                SqlParameter conditionIdParameter = new SqlParameter("@propertyid", conditionId);
                SqlParameter valueParameter = new SqlParameter("@value", value);
                SqlParameter ispropertyParameter = new SqlParameter("@isproperty", isproperty);
                SqlParameter valueidParameter = new SqlParameter("@valueid", 0);
                valueidParameter.Direction = ParameterDirection.Output;
                valueidParameter.SqlDbType = SqlDbType.Int;
                com.Parameters.Add(conditionIdParameter); com.Parameters.Add(valueParameter); com.Parameters.Add(ispropertyParameter); com.Parameters.Add(valueidParameter);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
                return (int)com.Parameters["@valueid"].Value;
            }
            finally
            {
                con.Close();
            }
        }
        internal void ConditionValueUpd(int valueid, string value, byte isproperty)
        {
            if (filterID == 0) return;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand();
            try
            {
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 180;
                com.CommandText = "dbo.FilterValueUpd_sp";
                SqlParameter valueIdParameter = new SqlParameter("@valueid", valueid);
                SqlParameter valueParameter = new SqlParameter("@value", value);
                SqlParameter ispropertyParameter = new SqlParameter("@isProperty", isproperty);
                com.Parameters.Add(valueIdParameter); com.Parameters.Add(valueParameter); com.Parameters.Add(ispropertyParameter);
                con.Open();
                com.ExecuteNonQuery();
            }
            finally
            {
                con.Close();
            }
        }
        internal void ConditionValuesDel(int conditionId)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand();
            try
            {
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 180;
                com.CommandText = "dbo.FilterValuesDel_sp";
                SqlParameter conditionIdParameter = new SqlParameter("@propertyid", conditionId);
                com.Parameters.Add(conditionIdParameter);
                con.Open();
                com.ExecuteNonQuery();
            }
            finally
            {
                con.Close();
            }
        }

        internal List<SQLFilterGroup> GroupGet(int ownergroupid, string namegroup)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand();
            try
            {
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 180;
                com.CommandText = "dbo.FilterGetGroupByName_sp";
                SqlParameter groupowner = new SqlParameter("@ownergroupid", ownergroupid);
                SqlParameter groupname = new SqlParameter("@namegroup", namegroup);
                com.Parameters.Add(groupowner); com.Parameters.Add(groupname);
                con.Open();
                SqlDataReader rdr = com.ExecuteReader();
                List<SQLFilterGroup> groups = new List<SQLFilterGroup>();
                while (rdr.Read())
                {
                    SQLFilterGroup group = new SQLFilterGroup();
                    group.groupid = rdr.GetInt32(0);
                    group.ownergroupid = ownergroupid;
                    group.namegroup = namegroup;
                    group.operatorgroup = rdr.GetString(1);
                    groups.Add(group);
                }
                rdr.Close();
                return groups;
            }
            finally
            {
                con.Close();
            }
        }
        internal List<SQLFilterCondition> ConditionGet(int groupid, string propertyName)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand();
            try
            {
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 180;
                com.CommandText = "dbo.FilterGetProperty_sp";
                SqlParameter group = new SqlParameter("@idgroup", groupid);
                SqlParameter property = new SqlParameter("@property", propertyName);
                com.Parameters.Add(@group); com.Parameters.Add(@property);
                con.Open();
                SqlDataReader rdr = com.ExecuteReader();
                List<SQLFilterCondition> conditions = new List<SQLFilterCondition>();
                while (rdr.Read())
                {
                    SQLFilterCondition cond = new SQLFilterCondition();
                    cond.groupId = groupid;
                    cond.propertyid = rdr.GetInt32(0);
                    cond.propertyName = propertyName;
                    cond.propertyOperator = rdr.GetString(1);
                    conditions.Add(cond);
                }
                rdr.Close();
                return conditions;
            }
            finally
            {
                con.Close();
            }
        }
        internal List<SQLFilterValue> ValueGet(int idproperty)
        {
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand();
            try
            {
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 180;
                com.CommandText = "dbo.FilterGetValue_sp";
                SqlParameter propertyid = new SqlParameter("@propertyid", idproperty);
                com.Parameters.Add(propertyid);
                con.Open();
                SqlDataReader rdr = com.ExecuteReader();
                List<SQLFilterValue> values = new List<SQLFilterValue>();
                while (rdr.Read())
                {
                    SQLFilterValue val = new SQLFilterValue();
                    val.valueId = rdr.GetInt32(0);
                    val.value = rdr.GetString(1);
                    val.isproperty = rdr.GetBoolean(2);
                    values.Add(val);
                }
                rdr.Close();
                return values;
            }
            finally
            {
                con.Close();
            }
        }

        internal void PullString(int ownerGroupId, string nameProperty, out string text)
        {
            List<SQLFilterCondition> listCond;
            List<SQLFilterValue> listValue;
            text = string.Empty;
            listCond = ConditionGet(ownerGroupId, nameProperty);
            if (listCond.Count > 0)
            {
                listValue = ValueGet(listCond[0].propertyid);
                text = listValue[0].value;
                if (text.Length > 2)
                {
                    text = text.Remove(0, 1);
                    text = text.Remove(text.Length - 1, 1);
                    text = text.Replace("[%]", "%");
                    text = text.Replace("[_]", "_");
                    text = text.Replace("[[]", "[");
                    text = text.Replace("[]]", "]");
                }
            }
        }
        internal void PullNumber(int ownerGroupId, string nameProperty, out string text, out byte operatorid)
        {
            List<SQLFilterCondition> listCond;
            List<SQLFilterValue> listValue;
            text = string.Empty;
            operatorid = 0;
            listCond = ConditionGet(ownerGroupId, nameProperty);
            if (listCond.Count > 0)
            {
                switch (listCond[0].propertyOperator)
                {
                    case "=":
                        operatorid = 0;
                        break;
                    case ">":
                        operatorid = 1;
                        break;
                    case "<":
                        operatorid = 2;
                        break;
                }
                listValue = ValueGet(listCond[0].propertyid);
                text = listValue[0].value;
            }

        }
        internal void PullDate(int ownerGroupId, string nameGroup, string nameProperty, out string date1, out string date2)
        {
            List<SQLFilterGroup> listGroup;
            List<SQLFilterCondition> listCond;
            List<SQLFilterValue> listValue;
            date1 = string.Empty; date2 = string.Empty;
            listGroup = GroupGet(ownerGroupId, nameGroup);
            if (listGroup.Count > 0)
            {
                DateTime d;
                listCond = ConditionGet(listGroup[0].groupid, nameProperty);
                if (listCond.Count > 1)
                {
                    listValue = ValueGet(listCond[0].propertyid);
                    if (DateTime.TryParseExact(listValue[0].value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                        date1 = d.ToShortDateString();

                    listValue = ValueGet(listCond[1].propertyid);
                    if (DateTime.TryParseExact(listValue[0].value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                        date2 = d.AddDays(-1D).ToShortDateString();
                }
                else if (listCond.Count > 0)
                {
                    listValue = ValueGet(listCond[0].propertyid);
                    if (DateTime.TryParseExact(listValue[0].value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                    {
                        if (listCond[0].propertyOperator == "<")
                        {
                            date2 = d.AddDays(-1D).ToShortDateString();
                        }
                        else
                        {
                            date1 = d.ToShortDateString();
                        }
                    }
                }
            }
        }
        internal void PullListBox(int ownerGroupId, string nameProperty, string itemProperty, System.Windows.Controls.ListBox listBox, bool select)
        {
            List<SQLFilterCondition> listCond;
            List<SQLFilterValue> listValue;
            if (select) listBox.SelectedItems.Clear(); else listBox.SelectAll();
            listCond = ConditionGet(ownerGroupId, nameProperty);
            if (listCond.Count > 0)
            {
                listValue = ValueGet(listCond[0].propertyid);
                System.Windows.Data.BindingListCollectionView coll = System.Windows.Data.CollectionViewSource.GetDefaultView(listBox.ItemsSource) as System.Windows.Data.BindingListCollectionView;
                System.Data.DataView itemview = coll.SourceCollection as System.Data.DataView;
                string s = itemview.Sort;
                itemview.Sort = itemProperty;
                foreach (SQLFilterValue val in listValue)
                {
                    System.Data.DataRowView[] rowval = itemview.FindRows(val.value);
                    if (rowval.Length > 0)
                        if (select) listBox.SelectedItems.Add(rowval[0]); else listBox.SelectedItems.Remove(rowval[0]);
                }
                itemview.Sort = s;
            }
        }

        internal void SetString(int ownerGroupId, string nameProperty, string text)
        {
            StringBuilder textLike = new StringBuilder(text);
            textLike.Replace("[", "[[]");
            textLike.Replace("]", "[]]");
            textLike.Replace("_", "[_]");
            textLike.Insert(0, '%').Append('%');
            List<SQLFilterCondition> cond = ConditionGet(ownerGroupId, nameProperty);
            if (cond.Count > 0)
            {
                if (text.Length > 0)
                {
                    List<SQLFilterValue> val = ValueGet(cond[0].propertyid);
                    if (val.Count > 0)
                    {
                        ConditionValueUpd(val[0].valueId, textLike.ToString(), 0);
                    }
                    else
                    {
                        ConditionValueAdd(cond[0].propertyid, textLike.ToString(), 0);
                    }
                }
                else
                {
                    ConditionDel(cond[0].propertyid);
                }
            }
            else if (text.Length > 0)
            {
                ConditionValueAdd(ConditionAdd(ownerGroupId, nameProperty, "Like"), textLike.ToString(), 0);
            }
        }
        internal void SetNumber(int ownerGroupId, string nameProperty, int operatorid, string text)
        {
            string oper = "=";
            switch (operatorid)
            {
                case 0:
                    oper = "=";
                    break;
                case 1:
                    oper = ">";
                    break;
                case 2:
                    oper = "<";
                    break;
            }
            List<SQLFilterCondition> cond = ConditionGet(ownerGroupId, nameProperty);
            if (cond.Count > 0)
            {
                if (text.Length > 0)
                {
                    if (cond[0].propertyOperator != oper) ConditionUpd(cond[0].propertyid, oper);
                    List<SQLFilterValue> val = ValueGet(cond[0].propertyid);
                    if (val.Count > 0)
                    {
                        ConditionValueUpd(val[0].valueId, text, 0);
                    }
                    else
                    {
                        ConditionValueAdd(cond[0].propertyid, text, 0);
                    }
                }
                else
                {
                    ConditionDel(cond[0].propertyid);
                }
            }
            else if (text.Length > 0)
            {
                ConditionValueAdd(ConditionAdd(ownerGroupId, nameProperty, oper), text, 0);
            }
        }
        internal void SetDate(int ownerGroupId, string nameGroup, string nameProperty, DateTime? date1, DateTime? date2)
        {
            List<SQLFilterGroup> listGroup = GroupGet(ownerGroupId, nameGroup);
            if (listGroup.Count > 0) this.GroupDel(listGroup[0].groupid);
            if (date1.HasValue | date2.HasValue)
            {
                int groupid = this.GroupAdd(ownerGroupId, nameGroup, "AND");
                if (date1.HasValue)
                {
                    ConditionValueAdd(ConditionAdd(groupid, nameProperty, ">="), date1.Value.ToString("yyyyMMdd"), 0);
                }
                if (date2.HasValue)
                {
                    ConditionValueAdd(ConditionAdd(groupid, nameProperty, "<"), date2.Value.AddDays(1).ToString("yyyyMMdd"), 0);
                }
            }
        }
        internal void SetRange(int ownerGroupId, string nameProperty, string minval, string maxval)
        {
            string oper = minval.Length > 0 & maxval.Length > 0 ? "between" : minval.Length > 0 ? ">=" : maxval.Length > 0 ? "<=" : string.Empty;
            List<SQLFilterCondition> cond1 = this.ConditionGet(ownerGroupId, nameProperty);
            if (cond1.Count > 0)
            {
                if (oper.Length > 0)
                {
                    this.ConditionUpd(cond1[0].propertyid, oper);
                    this.ConditionValuesDel(cond1[0].propertyid);
                    if (minval.Length > 0) this.ConditionValueAdd(cond1[0].propertyid, minval, 0);
                    if (maxval.Length > 0) this.ConditionValueAdd(cond1[0].propertyid, maxval, 0);
                }
                else
                {
                    this.ConditionDel(cond1[0].propertyid);
                }
            }
            else if (oper.Length > 0)
            {
                int idCondition = this.ConditionAdd(ownerGroupId, nameProperty, oper);
                if (minval.Length > 0) this.ConditionValueAdd(idCondition, minval, 0);
                if (maxval.Length > 0) this.ConditionValueAdd(idCondition, maxval, 0);
            }
        }
        internal void SetList(int ownerGroupId, string nameProperty, string[] values)
        {
            List<SQLFilterCondition> cond = ConditionGet(ownerGroupId, nameProperty);
            if (cond.Count > 0)
            {
                if (values.Length > 0)
                {
                    ConditionValuesDel(cond[0].propertyid);
                    ConditionValuesAdd(cond[0].propertyid, values);
                }
                else
                {
                    ConditionDel(cond[0].propertyid);
                }
            }
            else if (values.Length > 0)
            {
                ConditionValuesAdd(ConditionAdd(ownerGroupId, nameProperty, "In"), values);
            }
        }

        internal int FilterSQLID { get { return filterID; } }
        internal bool isDefault { set { isdefault = value; } get { return isdefault; } }
        internal bool isEmpty
        {
            get
            {
                if (filterID != 0)
                {
                    SqlConnection con = new SqlConnection(connectionString);
                    SqlCommand com = new SqlCommand();
                    try
                    {
                        com.Connection = con;
                        com.CommandType = CommandType.StoredProcedure;
                        com.CommandTimeout = 180;
                        com.CommandText = "dbo.FilterIsEmpty_sp";
                        SqlParameter filteridp = new SqlParameter("@filterid", filterID);
                        SqlParameter isemptyp = new SqlParameter("@isEmpty", 0);
                        isemptyp.Direction = ParameterDirection.Output;
                        isemptyp.SqlDbType = SqlDbType.Bit;
                        isemptyp.IsNullable = false;
                        com.Parameters.Add(filteridp); com.Parameters.Add(isemptyp);
                        con.Open();
                        com.ExecuteNonQuery();
                        return (bool)isemptyp.Value;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                else return true;
            }
        }
    }
    internal struct SQLFilterValue
    {
        internal int valueId;
        internal string value;
        internal bool isproperty;
    }
    internal struct SQLFilterCondition
    {
        internal int groupId;
        internal int propertyid;
        internal string propertyName;
        internal string propertyOperator;
    }
    internal struct SQLFilterGroup
    {
        internal int groupid;
        internal int ownergroupid;
        internal string namegroup;
        internal string operatorgroup;
    }
}
