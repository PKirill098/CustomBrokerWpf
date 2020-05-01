using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.ComponentModel;


namespace KirillPolyanskiy.CustomBrokerWpf
{
    internal interface ISQLFiltredWindow
    {
        bool IsShowFilter { set; get; }
        SQLFilter Filter { set; get; }
        void RunFilter();
    }

    public class SQLFilter:IDisposable
    {
        bool isdefault = false;
        private int whereidmy = 0, groupidmy = 0;
        private string connectionString = References.ConnectionString;
        private string mainGroupOperator;
        private string winame;

        internal SQLFilter(string WindowName, string MainGroupOperator)
        {
            winame = WindowName;
            mainGroupOperator = MainGroupOperator;
            GetDefaultFilter(SQLFilterPart.All);
        }
        ~SQLFilter() { RemoveFilter(); }
        public void Dispose()
        {
            RemoveFilter();
        }

        private void CreateWhere()
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
                SqlParameter whereid = new SqlParameter("@filterid", 0);
                whereid.SqlDbType = SqlDbType.Int;
                whereid.DbType = DbType.Int32;
                whereid.Direction = ParameterDirection.Output;
                com.Parameters.Add(oper); com.Parameters.Add(namegroup); com.Parameters.Add(whereid);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
                whereidmy = (int)com.Parameters["@filterid"].Value;
            }
            finally
            {
                con.Close();
            }
        }
        private void CreateGroup()
        {
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 180;
                com.CommandText = "dbo.FilterGroupGroupAdd_sp";
                SqlParameter groupid = new SqlParameter("@id", 0);
                groupid.SqlDbType = SqlDbType.Int;
                groupid.DbType = DbType.Int32;
                groupid.Direction = ParameterDirection.Output;
                com.Parameters.Add(groupid);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
                groupidmy = (int)com.Parameters["@id"].Value;
            }
            finally
            {
                con.Close();
            }
        }
        internal void RemoveFilter()
        {
            RemoveCurrentWhere();
            RemoveCurrentGroup();
        }
        internal void RemoveCurrentWhere()
        {
            if (whereidmy + groupidmy == 0) return;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand();
            try
            {
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 180;
                com.CommandText = "dbo.FilterDelete_sp";
                SqlParameter filter = new SqlParameter("@filterid", whereidmy);
                com.Parameters.Add(filter);
                con.Open();
                com.ExecuteNonQuery();
                whereidmy = 0;
            }
            catch { }
            finally
            {
                con.Close();
            }
        }
        internal void RemoveCurrentGroup()
        {
            if (groupidmy == 0) return;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand();
            try
            {
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 180;
                com.CommandText = "dbo.FilterGroupGroupDel_sp";
                com.Parameters.Add(new SqlParameter("@id", groupidmy));
                con.Open();
                com.ExecuteNonQuery();
                groupidmy = 0;
            }
            finally
            {
                con.Close();
            }
        }
        internal void GetFilter(int whereid,int groupid)
    {
            SqlParameter wherepar;
            SqlParameter grouppar;
        SqlConnection con = new SqlConnection(connectionString);
        try
        {
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandTimeout = 180;
            com.CommandText = "FilterGetFilter_sp";
            if (whereid != 0)
            {
                wherepar = new SqlParameter("@whereid", whereid);
                wherepar.Direction = ParameterDirection.InputOutput;
                wherepar.SqlDbType = SqlDbType.Int;
                wherepar.IsNullable = true;
                com.Parameters.Add(wherepar);
            }
            if (groupid != 0)
            {
                grouppar = new SqlParameter("@groupid", groupid);
                grouppar.Direction = ParameterDirection.InputOutput;
                grouppar.SqlDbType = SqlDbType.Int;
                grouppar.IsNullable = true;
                com.Parameters.Add(grouppar);
            }
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
            if (whereid != 0)
            {
                if (!com.Parameters["@whereid"].Value.Equals(System.DBNull.Value))
                    whereidmy = (int)com.Parameters["@whereid"].Value;
                else
                    whereidmy = 0;
            }
            if (groupid != 0)
            {
                if (!com.Parameters["@groupid"].Value.Equals(System.DBNull.Value))
                    groupidmy = (int)com.Parameters["@groupid"].Value;
                else
                    groupidmy = 0;
            }
        }
        finally
        {
            con.Close();
        }
    }
        internal void GetDefaultFilter(SQLFilterPart part)
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
                SqlParameter partParameter = new SqlParameter("@part", part);
                SqlParameter filterid = new SqlParameter("@whereid", 0);
                filterid.Direction = ParameterDirection.Output;
                filterid.SqlDbType = SqlDbType.Int;
                filterid.IsNullable = true;
                SqlParameter groupid = new SqlParameter("@groupid", 0);
                groupid.Direction = ParameterDirection.Output;
                groupid.SqlDbType = SqlDbType.Int;
                groupid.IsNullable = true;
                com.Parameters.Add(winameParameter); com.Parameters.Add(partParameter); com.Parameters.Add(filterid); com.Parameters.Add(groupid);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
                if (part != SQLFilterPart.Group)
                {
                    if (!com.Parameters["@whereid"].Value.Equals(System.DBNull.Value))
                        whereidmy = (int)com.Parameters["@whereid"].Value;
                    else
                        whereidmy = 0;
                }
                if (part != SQLFilterPart.Where)
                {
                    if (!com.Parameters["@groupid"].Value.Equals(System.DBNull.Value))
                        groupidmy = (int)com.Parameters["@groupid"].Value;
                    else
                        groupidmy = 0;
                }
            }
            finally
            {
                con.Close();
            }
        }
        internal void SetDefaultFilter(SQLFilterPart part)
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
                SqlParameter partParametr = new SqlParameter("@part", part);
                SqlParameter filterid = new SqlParameter("@whereid", whereidmy);
                SqlParameter groupid = new SqlParameter("@groupid", groupidmy);
                com.Parameters.Add(winameParameter); com.Parameters.Add(partParametr); com.Parameters.Add(filterid); com.Parameters.Add(groupid);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
            }
            finally
            {
                con.Close();
            }
        }
        internal void SetDefaultFilterWhere() { SetDefaultFilter(SQLFilterPart.Where); }
        internal void SetDefaultFilterGroup() { SetDefaultFilter(SQLFilterPart.Group); }
        internal void SetDefaultSavedFilter(SQLFilterSaved where,SQLFilterSaved group)
        {
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 180;
                com.CommandText = "dbo.FilterSaveSavedDefault_sp";
                com.Parameters.Add(new SqlParameter("@filterClass", winame));
                com.Parameters.Add(new SqlParameter("@whereid", where != null ? where.Id : 0));
                com.Parameters.Add(new SqlParameter("@groupid", group!=null?group.Id:0));
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
            if (whereidmy == 0)
            {
                CreateWhere();
                ownergroup = whereidmy;
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
            if (whereidmy == 0) return 0;
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
            if (whereidmy == 0)
            {
                CreateWhere();
                groupid = whereidmy;
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
            if (whereidmy == 0) return;
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
            if (whereidmy == 0) return;
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
            if (whereidmy == 0) return;
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
                System.Windows.Data.CollectionView coll = System.Windows.Data.CollectionViewSource.GetDefaultView(listBox.ItemsSource) as System.Windows.Data.CollectionView;
                if (coll is System.Windows.Data.BindingListCollectionView)
                {
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
                else if(coll.SourceCollection is DataModelClassLibrary.IReferenceCollection<DataModelClassLibrary.ReferenceSimpleItem>)
                {
                    DataModelClassLibrary.IReferenceCollection<DataModelClassLibrary.ReferenceSimpleItem> refcoll = coll.SourceCollection as DataModelClassLibrary.IReferenceCollection<DataModelClassLibrary.ReferenceSimpleItem>;
                    foreach (SQLFilterValue val in listValue)
                    {
                        object item = refcoll.FindFirstItem(itemProperty, int.Parse(val.value));
                        if (item != null)
                            if (select) listBox.SelectedItems.Add(item); else listBox.SelectedItems.Remove(item);
                    }
                }
                else
                {
                    foreach (SQLFilterValue val in listValue)
                    {
                        foreach (DataModelClassLibrary.DomainBaseClass item in listBox.ItemsSource)
                            if (item.Id == int.Parse(val.value))
                            { if (select) listBox.SelectedItems.Add(item); else listBox.SelectedItems.Remove(item); break; }
                    }
                }
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
        internal void SetDatePeriod(int ownerGroupId, string nameGroup, string namePropertyStart, string namePropertyStop, DateTime? date1, DateTime? date2)
        {
            List<SQLFilterGroup> listGroup = GroupGet(ownerGroupId, nameGroup);
            if (listGroup.Count > 0) this.GroupDel(listGroup[0].groupid);
            if (date1.HasValue | date2.HasValue)
            {
                int groupid = this.GroupAdd(ownerGroupId, nameGroup, "AND");
                if (date1.HasValue)
                {
                    ConditionValueAdd(ConditionAdd(groupid, namePropertyStop, ">="), date1.Value.ToString("yyyyMMdd"), 0);
                }
                if (date2.HasValue)
                {
                    ConditionValueAdd(ConditionAdd(groupid, namePropertyStart, "<"), date2.Value.AddDays(1).ToString("yyyyMMdd"), 0);
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

        internal void FillGroupProperties(List<SQLFilterGroupProperty> properties)
        {
            using (SqlConnection conn = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
            {
                SqlDataReader reader = null;
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.FilterGroupProperty_sp";
                    cmd.Parameters.Add(new SqlParameter("@groupid", groupidmy));
                    cmd.Connection = conn;
                    conn.Open();
                    reader = cmd.ExecuteReader(CommandBehavior.CloseConnection & CommandBehavior.SingleResult);
                    while (reader.Read())
                    {
                        SQLFilterGroupProperty item = new SQLFilterGroupProperty();
                        for (int c = 0; c < reader.FieldCount; c++)
                        {
                            switch (reader.GetName(c))
                            {
                                case "aggfunc":
                                    if (!reader.IsDBNull(c)) item.AggrigateFunction = reader.GetString(c);
                                    break;
                                case "id":
                                    if (!reader.IsDBNull(c)) item.Id = reader.GetInt32(c);
                                    break;
                                case "basisid":
                                    if (!reader.IsDBNull(c)) item.BasisId = reader.GetInt32(c);
                                    break;
                                case "propertySelect":
                                    if (!reader.IsDBNull(c)) item.SelectName = reader.GetString(c);
                                    break;
                                case "sortName":
                                    if (!reader.IsDBNull(c)) item.SortName = reader.GetString(c);
                                    break;
                                case "prefunc":
                                    if (!reader.IsDBNull(c)) item.PreFunction = reader.GetString(c);
                                    break;
                                case "chart":
                                    if (!reader.IsDBNull(c)) item.ChartValueType = reader.GetString(c);
                                    break;
                                case "displayindex":
                                    if (!reader.IsDBNull(c)) item.DisplayIndex = reader.GetInt32(c);
                                    break;
                                case "isvisible":
                                    if (!reader.IsDBNull(c)) item.IsVisible = reader.GetBoolean(c);
                                    break;
                                case "isfix":
                                    if (!reader.IsDBNull(c)) item.IsFix = reader.GetBoolean(c);
                                    break;
                                case "header":
                                    if (!reader.IsDBNull(c)) item.Header = reader.GetString(c);
                                    break;
                                case "sortorder":
                                    if (!reader.IsDBNull(c)) item.SortOrder = reader.GetInt32(c);
                                    break;
                            }
                        }
                        item.AcceptChanged();
                        properties.Add(item);
                    }
                }
                finally { if (reader != null) reader.Close(); }
            }
        }
        internal void FillAggregateFunction(List<AggregateFunction> function)
        {
            using (SqlConnection conn = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
            {
                SqlDataReader reader = null;
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT id,funcname,functype FROM dbo.FilterGroupGroupFunction_vw ORDER BY sort";
                    cmd.Connection = conn;
                    conn.Open();
                    reader = cmd.ExecuteReader(CommandBehavior.CloseConnection & CommandBehavior.SingleResult);
                    while (reader.Read())
                    {
                        function.Add(new AggregateFunction(reader.GetString(0), reader.GetString(1), reader.GetInt32(2)));
                    }
                }
                finally { if (reader != null) reader.Close(); }
            }
        }
        internal void FillSavedGroups(List<SQLFilterSaved> groups)
        {
            using (SqlConnection conn = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
            {
                SqlDataReader reader = null;
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT id,name,isreadonly FROM dbo.FilterGroupGroupName_vw ORDER BY name";
                    cmd.Connection = conn;
                    conn.Open();
                    reader = cmd.ExecuteReader(CommandBehavior.CloseConnection & CommandBehavior.SingleResult);
                    while (reader.Read())
                    {
                        groups.Add(new SQLFilterSaved(reader.GetInt32(0),reader.GetString(1), reader.GetBoolean(2)));
                    }
                }
                finally { if (reader != null) reader.Close(); }
            }
        }

        internal void SetGroupProperties(List<SQLFilterGroupProperty> properties)
        {
            List<SQLFilterGroupProperty> deleted = new List<SQLFilterGroupProperty>();
            foreach (SQLFilterGroupProperty property in properties)
            {
                if (property.State == SQLFilterGroupPropertyState.Modified) GroupPropertyUpd(property);
                else if (property.State == SQLFilterGroupPropertyState.Added) GroupPropertyAdd(property);
                else if (property.State == SQLFilterGroupPropertyState.Deleted)
                {
                    GroupPropertyDel(property);
                    deleted.Add(property);
                }
            }
            for (int i = 0; i < deleted.Count; i++) { properties.Remove(deleted[i]); }
        }
        private void GroupPropertyAdd(SQLFilterGroupProperty property)
        {
            if (groupidmy == 0) CreateGroup();
            using (SqlConnection conn = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
            {
                SqlDataReader reader = null;
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.FilterGroupPropertyAdd_sp";
                    cmd.Parameters.Add(new SqlParameter("@groupid", groupidmy));
                    cmd.Parameters.Add(new SqlParameter("@basisid", property.BasisId));
                    cmd.Parameters.Add(new SqlParameter("@aggregate", property.AggrigateFunction));
                    cmd.Parameters.Add(new SqlParameter("@prefunction", property.PreFunction));
                    cmd.Parameters.Add(new SqlParameter("@displayindex", property.DisplayIndex));
                    cmd.Parameters.Add(new SqlParameter("@isvisible", property.IsVisible));
                    cmd.Parameters.Add(new SqlParameter("@isfix", property.IsFix));
                    cmd.Parameters.Add(new SqlParameter("@header", SqlDbType.NVarChar, 30));
                    cmd.Parameters[7].Value = property.Header;
                    cmd.Parameters.Add(new SqlParameter("@sortorder", property.SortOrder));
                    cmd.Parameters.Add(new SqlParameter("@chart", property.ChartValueType));
                    SqlParameter idParameter = new SqlParameter("@propertyid", SqlDbType.Int);
                    idParameter.Direction = ParameterDirection.Output;
                    SqlParameter selectNameParameter = new SqlParameter("@selectName", SqlDbType.NVarChar,30);
                    selectNameParameter.Direction = ParameterDirection.Output;
                    SqlParameter sortNameParameter = new SqlParameter("@sortName", SqlDbType.NVarChar, 34);
                    sortNameParameter.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(idParameter); cmd.Parameters.Add(selectNameParameter); cmd.Parameters.Add(sortNameParameter);
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    property.Id = (int)idParameter.Value;
                    property.SelectName = (string)selectNameParameter.Value;
                    property.SortName = (string)sortNameParameter.Value;
                    //property.AcceptChanged();
                }
                finally { if (reader != null) reader.Close(); }
            }
        }
        private void GroupPropertyUpd(SQLFilterGroupProperty property)
        {
            using (SqlConnection conn = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
            {
                SqlDataReader reader = null;
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.FilterGroupPropertyUpd_sp";
                    cmd.Parameters.Add(new SqlParameter("@propertyid", property.Id));
                    cmd.Parameters.Add(new SqlParameter("@basisid", property.BasisId));
                    cmd.Parameters.Add(new SqlParameter("@aggregate", property.AggrigateFunction));
                    cmd.Parameters.Add(new SqlParameter("@prefunction", property.PreFunction));
                    cmd.Parameters.Add(new SqlParameter("@displayindex", property.DisplayIndex));
                    cmd.Parameters.Add(new SqlParameter("@isvisible", property.IsVisible));
                    cmd.Parameters.Add(new SqlParameter("@isfix", property.IsFix));
                    cmd.Parameters.Add(new SqlParameter("@header", SqlDbType.NVarChar, 30));
                    cmd.Parameters[7].Value = property.Header;/*if (string.IsNullOrEmpty(property.Header)) cmd.Parameters[7].Value = DBNull.Value; else */
                    cmd.Parameters.Add(new SqlParameter("@sortorder", property.SortOrder));
                    cmd.Parameters.Add(new SqlParameter("@chart", property.ChartValueType));
                    SqlParameter selectNameParameter = new SqlParameter("@selectName", SqlDbType.NVarChar, 30);
                    selectNameParameter.Direction = ParameterDirection.Output;
                    SqlParameter sortNameParameter = new SqlParameter("@sortName", SqlDbType.NVarChar, 34);
                    sortNameParameter.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(selectNameParameter); cmd.Parameters.Add(sortNameParameter);
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    property.SelectName = (string)selectNameParameter.Value;
                    property.SortName = (string)sortNameParameter.Value;
                    //property.AcceptChanged();
                }
                finally { if (reader != null) reader.Close(); }
            }
        }
        private void GroupPropertyDel(SQLFilterGroupProperty property)
        {
            using (SqlConnection conn = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
            {
                SqlDataReader reader = null;
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.FilterGroupPropertyDel_sp";
                    cmd.Parameters.Add(new SqlParameter("@propertyid", property.Id));
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    property.AcceptChanged();
                }
                finally { if (reader != null) reader.Close(); }
            }
        }

        internal void SetSavedGroup(SQLFilterSaved group)
        {
            using (SqlConnection conn = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
            {
                SqlDataReader reader = null;
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.FilterSaveGroupName_sp";
                    cmd.Parameters.Add(new SqlParameter("@groupid", groupidmy));
                    cmd.Parameters.Add(new SqlParameter("@savedid", group.Id));
                    cmd.Parameters[1].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters.Add(new SqlParameter("@name", group.Name));
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    group.Id = (int)cmd.Parameters[1].Value;
                }
                finally { if (reader != null) reader.Close(); }
            }
        }
        internal void DeleteSavedGroup(SQLFilterSaved group)
        {
            using (SqlConnection conn = new SqlConnection(KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString))
            {
                SqlDataReader reader = null;
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.FilterSavedGroupDel_sp";
                    cmd.Parameters.Add(new SqlParameter("@id", group.Id));
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                finally { if (reader != null) reader.Close(); }
            }
        }

        internal int FilterWhereId { get { return whereidmy; } }
        internal int FilterGroupId { get { return groupidmy; } }
        internal bool isDefault { set { isdefault = value; } get { return isdefault; } }
        internal bool isEmpty
        {
            get
            {
                if (whereidmy != 0)
                {
                    SqlConnection con = new SqlConnection(connectionString);
                    SqlCommand com = new SqlCommand();
                    try
                    {
                        com.Connection = con;
                        com.CommandType = CommandType.StoredProcedure;
                        com.CommandTimeout = 180;
                        com.CommandText = "dbo.FilterIsEmpty_sp";
                        SqlParameter filteridp = new SqlParameter("@filterid", whereidmy);
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

    internal enum SQLFilterPart { All, Where, Group }
    internal enum SQLFilterGroupPropertyState { Unchanged, Added, Modified, Deleted }

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

    internal class SQLFilterGroupProperty
    {
        private bool isdirty, isvisible, isfix;
        private int basisid, displayindex, sortorder;
        string aggfunc, prefunc, selectname,sortname, header, chart;

        internal int Id { set; get; }
        internal int BasisId
        {
            set
            {
                if (basisid != value)
                {
                    basisid = value;
                    isdirty = true;
                }
            }
            get { return basisid; }
        }
        internal int DisplayIndex
        {
            set
            {
                if (displayindex != value)
                {
                    displayindex = value;
                    isdirty = true;
                }
            }
            get { return displayindex; }
        }
        internal string AggrigateFunction
        {
            set
            {
                if (aggfunc!=value)
                {
                    aggfunc = value;
                    isdirty = true;
                }
            }
            get { return aggfunc; }
        }
        internal string PreFunction
        {
            set
            {
                if (prefunc!=value)
                {
                    prefunc = value;
                    isdirty = true;
                }
            }
            get { return prefunc; }
        }
        internal string SelectName
        {
            set
            {
                if (selectname!=value)
                {
                    selectname = value;
                }
            }
            get { return selectname; }
        }
        internal string SortName
        {
            set
            {
                if (sortname != value)
                {
                    sortname = value;
                }
            }
            get { return sortname; }
        }
        internal string ChartValueType 
        {
            set
            {
                if(chart!=value)
                {
                    chart = value;
                    isdirty = true;
                }
            }
            get { return chart; }
        }
        internal string Header
        {
            set
            {
                if (header!=value)
                {
                    header = value;
                    isdirty = true;
                }
            }
            get { return header; }
        }
        internal bool IsVisible
        {
            set
            {
                if (isvisible != value)
                {
                    isvisible = value;
                    isdirty = true;
                }
            }
            get { return isvisible; }
        }
        internal bool IsFix
        {
            set
            {
                if (isfix != value)
                {
                    isfix = value;
                    isdirty = true;
                }
            }
            get { return isfix; }
        }
        internal int SortOrder
        {
            set
            {
                if (sortorder != value)
                {
                    sortorder = value;
                    isdirty = true;
                }
            }
            get { return sortorder; } }
        internal SQLFilterGroupPropertyState State
        {
            get
            {
                return
                    this.basisid < 0 ? SQLFilterGroupPropertyState.Deleted :
                  this.Id == 0 ? SQLFilterGroupPropertyState.Added :
                  this.isdirty ? SQLFilterGroupPropertyState.Modified :
                  SQLFilterGroupPropertyState.Unchanged;
            }
        }

        internal SQLFilterGroupProperty() { aggfunc = "VAL"; prefunc = "NothingFnc"; header = string.Empty; isdirty = false; chart = "nthng"; }
        internal void AcceptChanged() { isdirty = false; }
    }
    public class AggregateFunction
    {
        public string Id { set; get; }
        public string Name { set; get; }
        public int Type { set; get; }

        internal AggregateFunction() { }
        internal AggregateFunction(string id, string name, int type)
        {
            this.Id = id;
            this.Name = name;
            this.Type = type;
        }

        internal bool isMemberGroup(Int16 group)
        {
            bool ismember = false;
            switch (group)
            {
                case 0:
                    ismember = (this.Type == 0 | this.Type == 2);
                    break;
                case 1:
                    ismember = this.Type < 2;
                    break;
                case 2:
                    ismember = this.Type < 3;
                    break;
                case 3:
                    ismember = this.Type < 4;
                    break;
                case 4:
                    ismember = this.Type != 2;
                    break;
                case 5:
                    ismember = this.Type != 1 & this.Type != 2;
                    break;
            }
            return ismember;
        }
    }
    public class PreFunction
    {
        public string Id { set; get; }
        public string Name { set; get; }
        public string Type { set; get; }

        internal PreFunction() { }
        internal PreFunction(string id, string name, string type)
        {
            this.Id = id;
            this.Name = name;
            this.Type = type;
        }
    }
    public class SQLFilterSaved : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void PropertyChangedNotification(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
        
        internal SQLFilterSaved():this(0, string.Empty, false)
        {}
        internal SQLFilterSaved(int id, string name,bool unchanged)
        {
            this.Id = id;
            this.Name = name;
            this.isreadonly = unchanged;
        }
        public int Id { set; get; }
        public string Name { set; get; }
        private bool isreadonly;
        public bool IsReadOnly { get { return isreadonly; } }
    }
}
