using DatabaseLayer;
using EntityLayer.Master;
using EntityLayer.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Master
{
    public class CodeMasterManager
    {
        public int InsertCodesMasterToDb(CodeMasterEntity model)
        {

            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                string sql = "INSERT INTO CODES_MASTER (CM_CODE, CM_TYPE, CM_DESC, CM_VALUE, CM_CR_BY, CM_CR_DT, CM_ACTIVE_YN)  VALUES (:cmCode,:cmType,:cmDesc,:cmValue,:cmCrBy,:cmCrDt,:cmActiveYn)";
                dict.Add("cmCode", model.cmCode);
                dict.Add("cmType", model.cmType);
                dict.Add("cmDesc", model.cmDesc);
                dict.Add("cmValue", model.cmValue);
                dict.Add("cmCrBy", model.cmCrBy);
                dict.Add("cmCrDt", model.cmCrDt);
                dict.Add("cmActiveYn", model.cmActiveYn);
                int rows = DBConnection.ExecuteQuery(dict, sql);
                return rows;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int isValidateUnique(CodeMasterEntity objCodesEntity)
        {
            try
            {
                int count;
                string query = $"SELECT * FROM CODES_MASTER WHERE CM_CODE='{objCodesEntity.cmCode}' AND CM_TYPE='{objCodesEntity.cmType}'";
                DataTable dt = DBConnection.ExecuteDataset(query);
                count = dt.Rows.Count;
                if (count > 0)
                {
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int UpdateCodeMaster(CodeMasterEntity model)
        {
            try
            {
                string sql = $"UPDATE CODES_MASTER SET CM_DESC='{model.cmDesc}',CM_VALUE={model.cmValue},CM_UP_BY='{model.cmUpBy}',CM_UP_DT='{model.cmUpDt}',CM_ACTIVE_YN='{model.cmActiveYn}' WHERE CM_CODE = '{model.cmCode}' AND CM_TYPE='{model.cmType}'";
                int gd = DBConnection.ExecuteQuery(sql);
                return gd;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int UpdateCodesMaster(CodeMasterEntity objCodesMasterEntity)
        {
            try
            {
                string sql = $"UPDATE CODES_MASTER SET CM_DESC='{objCodesMasterEntity.cmDesc}',CM_VALUE={objCodesMasterEntity.cmValue},CM_UP_BY='{objCodesMasterEntity.cmUpBy}',CM_UP_DT='{System.DateTime.Now.ToString("dd/MMMM/yyyy")}',CM_ACTIVE_YN='{objCodesMasterEntity.cmActiveYn}' WHERE CM_CODE = '{objCodesMasterEntity.cmCode}' AND CM_TYPE='{objCodesMasterEntity.cmType}'";
                int gd = DBConnection.ExecuteQuery(sql);
                return gd;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable FetchEmpStatus(string cmtype)
        {
            try
            {
                string query = $"select CM_CODE, CM_TYPE, CM_CODE||'-'||CM_DESC CM_DESC, CM_VALUE from codes_master where cm_type='{cmtype}' AND CM_ACTIVE_YN='Y'";
                DataTable dt = DBConnection.ExecuteDataset(query);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable FetchMonth(PrEmployeeAttendenceEntity objattnance)
        {
            try
            {
                string query = $"select CM_CODE, CM_TYPE, CM_DESC, CM_VALUE from codes_master where cm_type='{objattnance.month}' AND CM_ACTIVE_YN='Y'";
                DataTable dt = DBConnection.ExecuteDataset(query);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable FetchDesignationFromCodesMaster(string cmtype)
        {
            try
            {
                string query = $"select CM_CODE,CM_TYPE, CM_CODE||'-'||CM_DESC CM_DESC,CM_VALUE from codes_master where cm_type='{cmtype}' AND CM_ACTIVE_YN='Y'";
                DataTable dt = DBConnection.ExecuteDataset(query);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable FetchDesignation()
        {
            try
            {
                string query = $"select CM_CODE,CM_TYPE, CM_DESC from codes_master where cm_type='DESIGNATION' AND CM_ACTIVE_YN='Y'";
                DataTable dt = DBConnection.ExecuteDataset(query);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable FetchYear()
        {
            try
            {
                string query = $"select CM_CODE, CM_VALUE from codes_master where cm_type='YEAR' AND CM_ACTIVE_YN='Y'";
                DataTable dt = DBConnection.ExecuteDataset(query);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable FetchGradesFromCodesMaster(string cmtype)
        {
            try
            {
                string query = $"select CM_CODE,CM_TYPE, CM_DESC,CM_VALUE from codes_master where cm_type='{cmtype}' AND CM_ACTIVE_YN='Y'";
                DataTable dt = DBConnection.ExecuteDataset(query);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable FetchCodesMaster()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DBConnection.ExecuteDataset("SELECT * FROM CODES_MASTER");
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable FetchCodesMaster(CodeMasterEntity objCodeEntity)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DBConnection.ExecuteDataset($"SELECT * FROM CODES_MASTER WHERE CM_CODE='{objCodeEntity.cmCode}' AND CM_TYPE='{objCodeEntity.cmType}'");
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int DeleteCodesMaster(CodeMasterEntity objcodeMasterEntity)
        {
            try
            {
                DataTable gd = new DataTable();
                string sql1 = $"DELETE FROM CODES_MASTER WHERE CM_CODE='{objcodeMasterEntity.cmCode}' AND CM_TYPE='{objcodeMasterEntity.cmType}'";
                int rows = DBConnection.ExecuteQuery(sql1);
                return rows;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int UpdateCodesMaster(string cmCode, string cmType, string cmDesc, int cmValue, string active, string cmUpBy)
        {
            try
            {
                string sql = $"UPDATE CODES_MASTER SET CM_DESC='{cmDesc}',CM_VALUE={cmValue},CM_UP_BY='{cmUpBy}',CM_UP_DT='{System.DateTime.Now.ToString("dd/MMMM/yyyy")}',CM_ACTIVE_YN='{active}' WHERE CM_CODE = '{cmCode}' AND CM_TYPE='{cmType}'";
                int gd = DBConnection.ExecuteQuery(sql);
                return gd;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
