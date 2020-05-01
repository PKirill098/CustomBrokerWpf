using System;
using System.Data;
using System.Data.SqlClient;
using KirillPolyanskiy.CustomBrokerWpf.Classes.Domain;

namespace KirillPolyanskiy.CustomBrokerWpf
{
    internal class RequestConflictResolution
    {
        RequestRow originalrow;
        RequestRow currentrow;
        Request dbrow;
        internal RequestConflictResolution(RequestDS.tableRequestRow row)
        {
            originalrow = new RequestRow(row, DataRowVersion.Original);
            currentrow = new RequestRow(row, DataRowVersion.Current);
            dbrow = References.RequestStore.GetItem(row.requestId);
        }
        internal int isCheckedRow()
        {
            bool needupdate;
            bool ischeck = true;
            ischeck = ischeck & CheckField(originalrow.CellNumber, currentrow.CellNumber, dbrow.CellNumber, out needupdate);
            if (needupdate) currentrow.CellNumber = dbrow.CellNumber;
            ischeck = ischeck & CheckField(originalrow.Status, currentrow.Status, dbrow.Status.Id, out needupdate);
            if (needupdate) currentrow.Status = dbrow.Status.Id;
            ischeck = ischeck & CheckField(originalrow.CustomerId, currentrow.CustomerId, dbrow.CustomerId, out needupdate);
            if (needupdate) currentrow.CustomerId = dbrow.CustomerId;
            ischeck = ischeck & CheckField(originalrow.AgentId, currentrow.AgentId, dbrow.AgentId, out needupdate);
            if (needupdate) currentrow.AgentId = dbrow.AgentId;
            ischeck = ischeck & CheckField(originalrow.StoreId, currentrow.StoreId, dbrow.StoreId, out needupdate);
            if (needupdate) currentrow.StoreId = dbrow.StoreId;
            ischeck = ischeck & CheckField(originalrow.Freight, currentrow.Freight, dbrow.FreightId, out needupdate);
            if (needupdate) currentrow.Freight = dbrow.FreightId;
            ischeck = ischeck & CheckField(originalrow.OfficialWeight, currentrow.OfficialWeight, dbrow.OfficialWeight, out needupdate);
            if (needupdate) currentrow.OfficialWeight = dbrow.OfficialWeight;
            ischeck = ischeck & CheckField(originalrow.ActualWeight, currentrow.ActualWeight, dbrow.ActualWeight, out needupdate);
            if (needupdate) currentrow.ActualWeight = dbrow.ActualWeight;
            ischeck = ischeck & CheckField(originalrow.Volume, currentrow.Volume, dbrow.Volume, out needupdate);
            if (needupdate) currentrow.Volume = dbrow.Volume;
            ischeck = ischeck & CheckField(originalrow.GoodValue, currentrow.GoodValue, dbrow.GoodValue, out needupdate);
            if (needupdate) currentrow.GoodValue = dbrow.GoodValue;
            ischeck = ischeck & CheckField(originalrow.StoragePoint, currentrow.StoragePoint, dbrow.StorePoint, out needupdate);
            if (needupdate) currentrow.StoragePoint = dbrow.StorePoint;
            ischeck = ischeck & CheckField(originalrow.LoadDescription, currentrow.LoadDescription, dbrow.Cargo, out needupdate);
            if (needupdate) currentrow.LoadDescription = dbrow.Cargo;
            ischeck = ischeck & CheckField(originalrow.CustomerNote, currentrow.CustomerNote, dbrow.CustomerNote, out needupdate);
            if (needupdate) currentrow.CustomerNote = dbrow.CustomerNote;
            ischeck = ischeck & CheckField(originalrow.StorageNote, currentrow.StorageNote, dbrow.StoreNote, out needupdate);
            if (needupdate) currentrow.StorageNote = dbrow.StoreNote;
            ischeck = ischeck & CheckField(originalrow.ManagerNote, currentrow.ManagerNote, dbrow.ManagerNote, out needupdate);
            if (needupdate) currentrow.ManagerNote = dbrow.ManagerNote;
            ischeck = ischeck & CheckField(originalrow.ColorMark, currentrow.ColorMark, dbrow.ColorMark, out needupdate);
            if (needupdate) currentrow.ColorMark = dbrow.ColorMark;
            ischeck = ischeck & CheckField(originalrow.RequestDate, currentrow.RequestDate, dbrow.RequestDate, out needupdate);
            if (needupdate) currentrow.RequestDate = dbrow.RequestDate;
            ischeck = ischeck & CheckField(originalrow.Specification, currentrow.Specification, dbrow.Specification.UpdateWhen, out needupdate);
            if (needupdate) currentrow.Specification = dbrow.Specification.UpdateWhen;
            ischeck = ischeck & CheckField(originalrow.StorageDate, currentrow.StorageDate, dbrow.StoreDate, out needupdate);
            if (needupdate) currentrow.StorageDate = dbrow.StoreDate;
            ischeck = ischeck & CheckField(originalrow.ParcelGroup, currentrow.ParcelGroup, dbrow.ParcelGroup, out needupdate);
            if (needupdate) currentrow.ParcelGroup = dbrow.ParcelGroup;
            return ischeck ? (int)dbrow.Stamp : 0;
        }
        private bool CheckField(byte? originalf, byte? currentf, byte? dbf,out bool needupdate)
        {
            bool ischeck = true;
            needupdate = false;
            if (dbf != currentf)
            {
                if (originalf == currentf)
                {
                    needupdate=true;
                }
                else if (originalf != dbf) ischeck = false;
            }
            return ischeck;
        }
        private bool CheckField(int? originalf, int? currentf, int? dbf, out bool needupdate)
        {
            bool ischeck = true;
            needupdate = false;
            if (dbf != currentf)
            {
                if (originalf == currentf)
                {
                    needupdate = true;
                }
                else if (originalf != dbf) ischeck = false;
            }
            return ischeck;
        }
        private bool CheckField(decimal? originalf, decimal? currentf, decimal? dbf, out bool needupdate)
        {
            bool ischeck = true;
            needupdate = false;
            if (dbf != currentf)
            {
                if (originalf == currentf)
                {
                    needupdate = true;
                }
                else if (originalf != dbf) ischeck = false;
            }
            return ischeck;
        }
        private bool CheckField(string originalf, string currentf, string dbf, out bool needupdate)
        {
            bool ischeck = true;
            needupdate = false;
            if (dbf != currentf)
            {
                if (originalf == currentf)
                {
                    needupdate = true;
                }
                else if (originalf != dbf) ischeck = false;
            }
            return ischeck;
        }
        private bool CheckField(Int16? originalf, Int16? currentf, Int16? dbf, out bool needupdate)
        {
            bool ischeck = true;
            needupdate = false;
            if (dbf != currentf)
            {
                if (originalf == currentf)
                {
                    needupdate = true;
                }
                else if (originalf != dbf) ischeck = false;
            }
            return ischeck;
        }
        private bool CheckField(DateTime? originalf, DateTime? currentf, DateTime? dbf, out bool needupdate)
        {
            bool ischeck = true;
            needupdate = false;
            if (dbf != currentf)
            {
                if (originalf == currentf)
                {
                    needupdate = true;
                }
                else if (originalf != dbf) ischeck = false;
            }
            return ischeck;
        }
    }
    internal class RequestRow
    {
        bool isspecification;
        Int16 cellNumber;
        int status;
        int customerId;
        int agentId;
        int storeId;
        int freight;
        int parcelgroup;
        int stamp;
        decimal officialWeight;
        decimal actualWeight;
        decimal volume;
        decimal goodValue;
        string storagePoint;
        string loadDescription;
        string storageNote;
        string managerNote;
        string customerNote;
        string colorMark;
        DateTime requestDate;
        DateTime specification;
        DateTime storageDate;

        bool cellNumberIsNull;
        bool statusIsNull;
        bool customerIdIsNull;
        bool agentIdIsNull;
        bool storeIdIsNull;
        bool freightIsNull;
        bool parcelgroupIsNull;
        bool stampIsNull;
        bool officialWeightIsNull;
        bool actualWeightIsNull;
        bool volumeIsNull;
        bool goodValueIsNull;
        bool storagePointIsNull;
        bool loadDescriptionIsNull;
        bool storageNoteIsNull;
        bool managerNoteIsNull;
        bool customerNoteIsNull;
        bool colorMarkIsNull;
        bool requestDateIsNull;
        bool specificationIsNull;
        bool storageDateIsNull;

        bool updatablerow = false; // синхронизировать RequestRow с RequestDS.tableRequestRow
        RequestDS.tableRequestRow requestRow; //избавиться от типизации

        internal bool isSpecification
        {
            set
            {
                isspecification = value;
                if (updatablerow)
                {
                    requestRow.isspecification = value;
                    requestRow.EndEdit();
                }
            }
            get
            {
                return isspecification;
            }
        }
        internal Int16? CellNumber
        {
            set
            {
                cellNumberIsNull = !value.HasValue;
                if (value.HasValue) cellNumber = value.Value;
                if (updatablerow)
                {
                    if (value.HasValue) requestRow.cellNumber = value.Value;
                    else requestRow.SetcellNumberNull();
                    requestRow.EndEdit();
                }
            }
            get
            {
                if (cellNumberIsNull) return null;
                else return cellNumber;
            }
        }
        internal int? Status
        {
            set
            {
                statusIsNull = !value.HasValue;
                if (value.HasValue) status = value.Value;
                if (updatablerow)
                {
                    requestRow.status = value.Value;
                    requestRow.EndEdit();
                }
            }
            get
            {
                if (statusIsNull) return null;
                else return status;
            }
        }
        internal int? CustomerId
        {
            set
            {
                customerIdIsNull = !value.HasValue;
                if (value.HasValue) customerId = value.Value;
                if (updatablerow)
                {
                    if (value.HasValue) requestRow.customerId = value.Value;
                    else requestRow.SetcustomerIdNull();
                    requestRow.EndEdit();
                }
            }
            get
            {
                if (customerIdIsNull) return null;
                else return customerId;
            }
        }
        internal int? AgentId
        {
            set
            {
                agentIdIsNull = !value.HasValue;
                if (value.HasValue) agentId = value.Value;
                if (updatablerow)
                {
                    if (value.HasValue) requestRow.agentId = value.Value;
                    else requestRow.SetagentIdNull();
                    requestRow.EndEdit();
                }
            }
            get
            {
                if (agentIdIsNull) return null;
                else return agentId;
            }
        }
        internal int? StoreId
        {
            set
            {
                storeIdIsNull = !value.HasValue;
                if (value.HasValue) storeId = value.Value;
                if (updatablerow)
                {
                    if (value.HasValue) requestRow.storeid = value.Value;
                    else requestRow.SetstoreidNull();
                    requestRow.EndEdit();
                }
            }
            get
            {
                if (storeIdIsNull) return null;
                else return storeId;
            }
        }
        internal int? Freight
        {
            set
            {
                freightIsNull = !value.HasValue;
                if (value.HasValue) freight = value.Value;
                if (updatablerow)
                {
                    if (value.HasValue) requestRow.freight = value.Value;
                    else requestRow.SetfreightNull();
                    requestRow.EndEdit();
                }
            }
            get
            {
                if (freightIsNull) return null;
                else return freight;
            }
        }
        internal int? ParcelGroup
        {
            set
            {
                parcelgroupIsNull = !value.HasValue;
                if (value.HasValue) parcelgroup = value.Value;
                if (updatablerow)
                {
                    if (value.HasValue) requestRow.parcelgroup = value.Value;
                    else requestRow.SetparcelgroupNull();
                    requestRow.EndEdit();
                }
            }
            get
            {
                if (parcelgroupIsNull) return null;
                else return parcelgroup;
            }

        }
        internal int? Stamp
        {
            set
            {
                stampIsNull = !value.HasValue;
                if (value.HasValue) stamp = value.Value;
                if (updatablerow)
                {
                    if (value.HasValue) requestRow.stamp = value.Value;
                    else requestRow.SetstampNull();
                    requestRow.EndEdit();
                }
            }
            get
            {
                if (stampIsNull) return null;
                else return stamp;
            }
        }
        internal decimal? OfficialWeight
        {
            set
            {
                officialWeightIsNull = !value.HasValue;
                if (value.HasValue) officialWeight = value.Value;
                if (updatablerow)
                {
                    if (value.HasValue) requestRow.officialWeight = value.Value;
                    else requestRow.SetofficialWeightNull();
                    requestRow.EndEdit();
                }
            }
            get
            {
                if (officialWeightIsNull) return null;
                else return officialWeight;
            }
        }
        internal decimal? ActualWeight
        {
            set
            {
                actualWeightIsNull = !value.HasValue;
                if (value.HasValue) actualWeight = value.Value;
                if (updatablerow)
                {
                    if (value.HasValue) requestRow.actualWeight = value.Value;
                    else requestRow.SetactualWeightNull();
                    requestRow.EndEdit();
                }
            }
            get
            {
                if (actualWeightIsNull) return null;
                else return actualWeight;
            }
        }
        internal decimal? Volume
        {
            set
            {
                volumeIsNull = !value.HasValue;
                if (value.HasValue) volume = value.Value;
                if (updatablerow)
                {
                    if (value.HasValue) requestRow.volume = value.Value;
                    else requestRow.SetvolumeNull();
                    requestRow.EndEdit();
                }
            }
            get
            {
                if (volumeIsNull) return null;
                else return volume;
            }
        }
        internal decimal? GoodValue
        {
            set
            {
                goodValueIsNull = !value.HasValue;
                if (value.HasValue) goodValue = value.Value;
                if (updatablerow)
                {
                    if (value.HasValue) requestRow.goodValue = value.Value;
                    else requestRow.SetgoodValueNull();
                    requestRow.EndEdit();
                }
            }
            get
            {
                if (goodValueIsNull) return null;
                else return goodValue;
            }
        }
        internal string StoragePoint
        {
            set
            {
                storagePointIsNull = value == null;
                if (!storagePointIsNull) storagePoint = value;
                if (updatablerow)
                {
                    if (storagePointIsNull) requestRow.SetstoragePointNull();
                    else requestRow.storagePoint = value;
                    requestRow.EndEdit();
                }
            }
            get
            {
                if (storagePointIsNull) return null;
                else return storagePoint;
            }
        }
        internal string LoadDescription
        {
            set
            {
                loadDescriptionIsNull = value == null;
                if (!loadDescriptionIsNull) loadDescription = value;
                if (updatablerow)
                {
                    if (loadDescriptionIsNull) requestRow.SetloadDescriptionNull();
                    else requestRow.loadDescription = value;
                    requestRow.EndEdit();
                }
            }
            get
            {
                if (loadDescriptionIsNull) return null;
                else return loadDescription;
            }
        }
        internal string StorageNote
        {
            set
            {
                storageNoteIsNull = value == null;
                if (!storageNoteIsNull) storageNote = value;
                if (updatablerow)
                {
                    if (loadDescriptionIsNull) requestRow.SetstorageNoteNull();
                    else requestRow.storageNote = value;
                    requestRow.EndEdit();
                }
            }
            get
            {
                if (storageNoteIsNull) return null;
                else return storageNote;
            }
        }
        internal string ManagerNote
        {
            set
            {
                managerNoteIsNull = value == null;
                if (!managerNoteIsNull) managerNote = value;
                if (updatablerow)
                {
                    if (loadDescriptionIsNull) requestRow.SetmanagerNoteNull();
                    else requestRow.managerNote = value;
                    requestRow.EndEdit();
                }
            }
            get
            {
                if (managerNoteIsNull) return null;
                else return managerNote;
            }
        }
        internal string CustomerNote
        {
            set
            {
                customerNoteIsNull = value == null;
                if (!customerNoteIsNull) customerNote = value;
                if (updatablerow)
                {
                    if (loadDescriptionIsNull) requestRow.SetcustomerNoteNull();
                    else requestRow.customerNote = value;
                    requestRow.EndEdit();
                }
            }
            get
            {
                if (customerNoteIsNull) return null;
                else return customerNote;
            }
        }

        internal string ColorMark
        {
            set
            {
                colorMarkIsNull = value == null;
                if (!colorMarkIsNull) colorMark = value;
                if (updatablerow)
                {
                    if (loadDescriptionIsNull) requestRow.SetcolmarkNull();
                    else requestRow.colmark = value;
                    requestRow.EndEdit();
                }
            }
            get
            {
                if (colorMarkIsNull) return null;
                else return colorMark;
            }
        }
        internal DateTime? RequestDate
        {
            set
            {
                requestDateIsNull = !value.HasValue;
                if (value.HasValue) requestDate = value.Value;
                if (updatablerow)
                {
                    if (value.HasValue) requestRow.requestDate = value.Value;
                    else requestRow.SetrequestDateNull();
                    requestRow.EndEdit();
                }
            }
            get
            {
                if (requestDateIsNull) return null;
                else return requestDate;
            }
        }
        internal DateTime? Specification
        {
            set
            {
                specificationIsNull = !value.HasValue;
                if (value.HasValue) specification = value.Value;
                if (updatablerow)
                {
                    if (value.HasValue) requestRow.specification = value.Value;
                    else requestRow.SetspecificationNull();
                    requestRow.EndEdit();
                }
            }
            get
            {
                if (specificationIsNull) return null;
                else return specification;
            }
        }
        internal DateTime? StorageDate
        {
            set
            {
                storageDateIsNull = !value.HasValue;
                if (value.HasValue) storageDate = value.Value;
                if (updatablerow)
                {
                    if (value.HasValue) requestRow.storageDate = value.Value;
                    else requestRow.SetstorageDateNull();
                    requestRow.EndEdit();
                }
            }
            get
            {
                if (storageDateIsNull) return null;
                else return storageDate;
            }
        }

        internal RequestRow()
        {
            isspecification = false;
            cellNumberIsNull = true;
            statusIsNull = true;
            customerIdIsNull = true;
            agentIdIsNull = true;
            storeIdIsNull = true;
            freightIsNull = true;
            parcelgroupIsNull = true;
            stampIsNull = true;
            officialWeightIsNull = true;
            actualWeightIsNull = true;
            volumeIsNull = true;
            goodValueIsNull = true;
            storagePointIsNull = true;
            loadDescriptionIsNull = true;
            customerNoteIsNull = true;
            storageNoteIsNull = true;
            managerNoteIsNull = true;
            colorMarkIsNull = true;
            requestDateIsNull = true;
            specificationIsNull = true;
            storageDateIsNull = true;
        }
        internal RequestRow(RequestDS.tableRequestRow row, DataRowVersion version)
        {
            CellNumber = row.Field<Int16?>("cellNumber", version);
            Status = row.Field<int?>("status", version);
            CustomerId = row.Field<int?>("customerId", version);
            AgentId = row.Field<int?>("agentId", version);
            StoreId = row.Field<int?>("storeId", version);
            Freight = row.Field<int?>("freight", version);
            Stamp = row.Field<int?>("stamp", version);
            OfficialWeight = row.Field<decimal?>("officialWeight", version);
            ActualWeight = row.Field<decimal?>("actualWeight", version);
            Volume = row.Field<decimal?>("volume", version);
            GoodValue = row.Field<decimal?>("goodValue", version);
            StoragePoint = row.Field<string>("storagePoint", version);
            LoadDescription = row.Field<string>("loadDescription", version);
            CustomerNote = row.Field<string>("customerNote", version);
            StorageNote = row.Field<string>("storageNote", version);
            ManagerNote = row.Field<string>("managerNote", version);
            ColorMark = row.Field<string>("colmark", version);
            this.RequestDate = row.Field<DateTime?>("requestDate", version);
            this.Specification = row.Field<DateTime?>("specification", version);
            this.StorageDate = row.Field<DateTime?>("storageDate", version);
            ParcelGroup=row.Field<int?>("parcelgroup", version);
            if (version == DataRowVersion.Current)
            {
                requestRow = row;
                updatablerow = true;
            }
        }

        internal void Fill(int requestid)
        {
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection())
            {
                connection.ConnectionString = KirillPolyanskiy.CustomBrokerWpf.References.ConnectionString;
                SqlCommand comd = new SqlCommand();
                comd.Connection = connection;
                comd.CommandType = System.Data.CommandType.StoredProcedure;
                comd.CommandText = "dbo.RequestConflictErr_sp";
                SqlParameter par = new SqlParameter();
                par.DbType = System.Data.DbType.Int32;
                par.Direction = System.Data.ParameterDirection.Input;
                par.IsNullable = false;
                par.ParameterName = "@requestid";
                par.SqlDbType = System.Data.SqlDbType.Int;
                par.Value = requestid;
                comd.Parameters.Add(par);
                try
                {
                    connection.Open();
                    SqlDataReader rdr = comd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        for (int c = 0; c < rdr.FieldCount; c++)
                        {
                            switch (rdr.GetName(c))
                            {
                                case "cellNumber":
                                    if (!rdr.IsDBNull(c))
                                    {
                                        cellNumber = rdr.GetInt16(c);
                                        cellNumberIsNull = false;
                                    }
                                    break;
                                case "status":
                                    if (!rdr.IsDBNull(c))
                                    {
                                        status = rdr.GetInt32(c);
                                        statusIsNull = false;
                                    }
                                    break;
                                case "customerId":
                                    if (!rdr.IsDBNull(c))
                                    {
                                        customerId = rdr.GetInt32(c);
                                        customerIdIsNull = false;
                                    }
                                    break;
                                case "agentId":
                                    if (!rdr.IsDBNull(c))
                                    {
                                        agentId = rdr.GetInt32(c);
                                        agentIdIsNull = false;
                                    }
                                    break;
                                case "storeId":
                                    if (!rdr.IsDBNull(c))
                                    {
                                        storeId = rdr.GetInt32(c);
                                        storeIdIsNull = false;
                                    }
                                    break;
                                case "freight":
                                    if (!rdr.IsDBNull(c))
                                    {
                                        freight = rdr.GetInt32(c);
                                        freightIsNull = false;
                                    }
                                    break;
                                case "stamp":
                                    if (!rdr.IsDBNull(c))
                                    {
                                        stamp = rdr.GetInt32(c);
                                        stampIsNull = false;
                                    }
                                    break;
                                case "storagePoint":
                                    if (!rdr.IsDBNull(c))
                                    {
                                        storagePoint = rdr.GetString(c);
                                        storagePointIsNull = false;
                                    }
                                    break;
                                case "loadDescription":
                                    if (!rdr.IsDBNull(c))
                                    {
                                        loadDescription = rdr.GetString(c);
                                        loadDescriptionIsNull = false;
                                    }
                                    break;
                                case "officialWeight":
                                    if (!rdr.IsDBNull(c))
                                    {
                                        officialWeight = rdr.GetDecimal(c);
                                        officialWeightIsNull = false;
                                    }
                                    break;
                                case "actualWeight":
                                    if (!rdr.IsDBNull(c))
                                    {
                                        actualWeight = rdr.GetDecimal(c);
                                        actualWeightIsNull = false;
                                    }
                                    break;
                                case "volume":
                                    if (!rdr.IsDBNull(c))
                                    {
                                        volume = rdr.GetDecimal(c);
                                        volumeIsNull = false;
                                    }
                                    break;
                                case "goodValue":
                                    if (!rdr.IsDBNull(c))
                                    {
                                        goodValue = rdr.GetDecimal(c);
                                        goodValueIsNull = false;
                                    }
                                    break;
                                case "customerNote":
                                    if (!rdr.IsDBNull(c))
                                    {
                                        customerNote = rdr.GetString(c);
                                        customerNoteIsNull = false;
                                    }
                                    break;
                                case "storageNote":
                                    if (!rdr.IsDBNull(c))
                                    {
                                        storageNote = rdr.GetString(c);
                                        storageNoteIsNull = false;
                                    }
                                    break;
                                case "managerNote":
                                    if (!rdr.IsDBNull(c))
                                    {
                                        managerNote = rdr.GetString(c);
                                        managerNoteIsNull = false;
                                    }
                                    break;
                                case "colorMark":
                                    if (!rdr.IsDBNull(c))
                                    {
                                        colorMark = rdr.GetString(c);
                                        colorMarkIsNull = false;
                                    }
                                    break;
                                case "requestDate":
                                    if (!rdr.IsDBNull(c))
                                    {
                                        requestDate = rdr.GetDateTime(c);
                                        requestDateIsNull = false;
                                    }
                                    break;
                                case "specification":
                                    if (!rdr.IsDBNull(c))
                                    {
                                        specification = rdr.GetDateTime(c);
                                        specificationIsNull = false;
                                    }
                                    break;
                                case "storageDate":
                                    if (!rdr.IsDBNull(c))
                                    {
                                        storageDate = rdr.GetDateTime(c);
                                        storageDateIsNull = false;
                                    }
                                    break;
                            }
                        }
                    }
                }
                finally
                {
                    if (connection.State != System.Data.ConnectionState.Closed) connection.Close();
                }
            }
        }
    }
}
