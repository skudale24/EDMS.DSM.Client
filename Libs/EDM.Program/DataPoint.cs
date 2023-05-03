using System;
using System.Collections;
using System.Data;
using VTI.Common;

namespace EDM.Program
{
    public class DataPoint
    {
        #region --- Properties ---
        public String Module;
        public String Message;
        public long ProgramId;
        public long ByUserId;
        public String ByUser;
        public int StatusId = 1;
        public String TranDate;

        public long SubMeasureId;
        public long DocTypeId;
        public long DocTypeGroupId;
        public String DocTypeName = String.Empty;
        public String DocTypeDesc = String.Empty;
        public int DocTypeVersion = 1;
        public int DisplayIn = 1;
        public int System = 0;
        public int DocTypeSeq = 1;
        public int DocTypeParam;
        public String DocTypeParamDesc = String.Empty;
        public String Validation = String.Empty;
        public int Required = 1;
        public int Inventory = 0;
        public int InUse;

        public static SortedList ParamTypes()
        {
            SortedList lst = new SortedList();
            lst.Add(1, "TextBox");
            lst.Add(5, "DropDownList");
            lst.Add(6, "CheckBoxList");
            lst.Add(7, "Label");
            lst.Add(8, "RadioButtonList");
            lst.Add(9, "Date");
            lst.Add(10, "Heading");
            return lst;
        }
        #endregion

        #region --- Constructors ---
        public DataPoint() { ProgramId = Setting.Session.ProgramId; ByUserId = EDM.Setting.Session.UserId; }
        public DataPoint(String module) : this() { Module = module; }
        #endregion

        #region --- Public Methods ---
        /// <summary>
        /// Module, SubMeasureId are required.
        /// </summary>
        public DataSet GetBySMId()
        {
            try
            {
                if (SubMeasureId <= 0) { Message = "SubMeasureId is required."; return null; }

                Hashtable prms = new Hashtable();
                prms["SubMeasureID"] = SubMeasureId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_DataPoints", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Program.DataPoint", "GetBySMId", SqlforLog);

                return MsSql.ExecuteQuery(sql);
            }
            catch (Exception ex) { Message = ex.Message; Common.Log.Error(Module, Module + ":EDM.Program.DataPoint", "GetBySMId", ex); return null; }
        }

        /// <summary>
        /// Module, DocTypeId are required.
        /// </summary>
        public Boolean GetById()
        {
            try
            {
                if (DocTypeId <= 0) { Message = "DocTypeId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["DocTypeID"] = DocTypeId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_DataPoint", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Program.DataPoint", "GetById", SqlforLog);

                DataSet ds = MsSql.ExecuteQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error fetching record.";
                    Common.Log.Info(Module + ":EDM.Program.DataPoint", "GetById", SqlforLog + "|Error:" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                DocTypeGroupId = MsSql.CheckLongDBNull(dr["DocTypeGroupID"]);
                DocTypeName = MsSql.CheckStringDBNull(dr["DocTypeName"]);
                DocTypeDesc = MsSql.CheckStringDBNull(dr["DocTypeDesc"]);
                DocTypeVersion = MsSql.CheckIntDBNull(dr["DocTypeVersion"]);
                DisplayIn = MsSql.CheckIntDBNull(dr["DisplayIn"]);
                System = MsSql.CheckIntDBNull(dr["System"]);
                DocTypeSeq = MsSql.CheckIntDBNull(dr["DocTypeSeq"]);
                DocTypeParam = MsSql.CheckIntDBNull(dr["DocTypeParam"]);
                Validation = MsSql.CheckStringDBNull(dr["Validation"]);
                Required = MsSql.CheckIntDBNull(dr["Required"]);
                Inventory = MsSql.CheckIntDBNull(dr["Inventory"]);
                InUse = MsSql.CheckIntDBNull(dr["InUse"]);
                StatusId = MsSql.CheckIntDBNull(dr["StatusID"]);
                TranDate = MsSql.CheckStringDBNull(dr["TranDateFtd"]);
                ByUserId = MsSql.CheckLongDBNull(dr["ByUserID"]);
                ByUser = MsSql.CheckStringDBNull(dr["ByUser"]);

                return true;
            }
            catch (Exception ex) { Message = ex.Message; Common.Log.Error(Module, Module + ":EDM.Program.DataPoint", "GetById", ex); return false; }
        }

        /// <summary>
        /// Module, DocTypeId are required.
        /// </summary>
        public DataSet GetValues()
        {
            try
            {
                if (DocTypeId <= 0) { Message = "DocTypeId is required."; return null; }

                Hashtable prms = new Hashtable();
                prms["DocTypeID"] = DocTypeId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_GET_DataPointValues", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Program.DataPoint", "GetValues", SqlforLog);

                return MsSql.ExecuteQuery(sql);
            }
            catch (Exception ex) { Message = ex.Message; Common.Log.Error(Module, Module + ":EDM.Program.DataPoint", "GetValues", ex); return null; }
        }

        /// <summary>
        /// Module, DocTypeId are required. ProgramID, ByUserId is fetched from Session, if not available, please specify.
        /// </summary>
        public Boolean Delete()
        {
            try
            {
                if (ProgramId <= 0) { Message = "ProgramId is required."; return false; }
                if (DocTypeId <= 0) { Message = "DocTypeId is required."; return false; }
                if (ByUserId <= 0) { Message = "ByUserId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["ProgramID"] = ProgramId;
                prms["DocTypeID"] = DocTypeId;
                prms["ByUserID"] = ByUserId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_D_DataPoint", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Program.DataPoint", "Delete", SqlforLog);

                DataSet ds = MsSql.ExecuteQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error deleting record.";
                    Common.Log.Info(Module + ":EDM.Program.DataPoint", "Delete", SqlforLog + "|Error:" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = MsSql.CheckStringDBNull(dr["Message"]);
                return MsSql.CheckLongDBNull(dr["DocTypeID"]) <= 0 ? false : true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Program.DataPoint", "Delete", ex);
                return false;
            }
        }

        /// <summary>
        /// Module, DocTypeGroupId/SubMeasureId, DocTypeName are required. ByUserId is fetched from Session, if not available, please specify.
        /// </summary>
        public Boolean Add()
        {
            try
            {
                if (DocTypeGroupId <= 0 && SubMeasureId <= 0) { Message = "DocTypeGroupId or SubMeasureId is required."; return false; }
                if (DocTypeName.Length <= 0) { Message = "DocTypeName is required."; return false; }
                if (ByUserId <= 0) { Message = "ByUserId is required."; return false; }

                Hashtable prms = new Hashtable();
                if (DocTypeGroupId > 0)
                    prms["DocTypeGroupID"] = DocTypeGroupId;
                else
                    prms["SubMeasureID"] = SubMeasureId;
                prms["DocTypeName"] = DocTypeName;
                prms["DocTypeDesc"] = DocTypeDesc;
                prms["DocTypeParam"] = DocTypeParam;
                prms["DisplayIn"] = DisplayIn;
                prms["System"] = System;
                prms["DocTypeSeq"] = DocTypeSeq;
                prms["Required"] = Required;
                prms["Inventory"] = Inventory;
                prms["Validation"] = Validation;
                prms["StatusID"] = StatusId;
                prms["ByUserID"] = ByUserId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_AU_DataPoint", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Program.DataPoint", "Add", SqlforLog);

                DataSet ds = MsSql.ExecuteQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error adding record.";
                    Common.Log.Info(Module + ":EDM.Program.DataPoint", "Add", SqlforLog + "|Error:" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = MsSql.CheckStringDBNull(dr["Message"]);
                DocTypeId = MsSql.CheckLongDBNull(dr["DocTypeID"]);
                return DocTypeId <= 0 ? false : true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Program.DataPoint", "Add", ex);
                return false;
            }
        }

        /// <summary>
        /// Module, DocTypeId are required. ByUserId is fetched from Session, if not available, please specify.
        /// </summary>
        public Boolean Update()
        {
            try
            {
                if (DocTypeId <= 0) { Message = "DocTypeId is required."; return false; }
                if (ByUserId <= 0) { Message = "ByUserId is required."; return false; }

                Hashtable prms = new Hashtable();
                prms["DocTypeID"] = DocTypeId;
                if (DocTypeName.Length > 0) prms["DocTypeName"] = DocTypeName;
                prms["DocTypeDesc"] = DocTypeDesc;
                if (DocTypeParam > 0) prms["DocTypeParam"] = DocTypeParam;
                if (DocTypeParamDesc.Length > 0) prms["DocTypeParamDesc"] = DocTypeParamDesc;
                prms["DisplayIn"] = DisplayIn;
                prms["System"] = System;
                if (DocTypeSeq > 0) prms["DocTypeSeq"] = DocTypeSeq;
                prms["Required"] = Required;
                prms["Inventory"] = Inventory;
                prms["Validation"] = Validation;
                if (StatusId > 0) prms["StatusID"] = StatusId;
                prms["ByUserID"] = ByUserId;

                String SqlforLog = string.Empty;
                String sql = MsSql.GetSqlStmt("p_AU_DataPoint", prms, out SqlforLog);
                Common.Log.Info(Module + ":EDM.Program.DataPoint", "Update", SqlforLog);

                DataSet ds = MsSql.ExecuteQuery(sql);
                if (MsSql.IsEmpty(ds))
                {
                    Message = "Error adding record.";
                    Common.Log.Info(Module + ":EDM.Program.DataPoint", "Update", SqlforLog + "|Error:" + Message);
                    return false;
                }

                DataRow dr = ds.Tables[0].Rows[0];
                Message = MsSql.CheckStringDBNull(dr["Message"]);
                return MsSql.CheckLongDBNull(dr["DocTypeID"]) <= 0 ? false : true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                Common.Log.Error(Module, Module + ":EDM.Program.DataPoint", "Update", ex);
                return false;
            }
        }
        #endregion
    }
}
