using DatabaseLayer;
using EntityLayer.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Master
{
    public class ErrorCodeMasterManager
    {
        ErrorCodeMasterEntity objErrEntity = new ErrorCodeMasterEntity();
        public int IsInsertErrorMaster(ErrorCodeMasterEntity model)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                string query = "INSERT INTO ERROR_CODE_MASTER(ERR_CODE, ERR_TYPE, ERR_DESC, ERR_CR_BY,ERR_CR_DT) VALUES (:errCode,:errType,:errDesc,:errCrBy,:errCrDt)";
                dict.Add("errCode", model.errCode);
                dict.Add("errType", model.errType);
                dict.Add("errDesc", model.errDesc);
                dict.Add("errCrBy", model.errCrBy);
                dict.Add("errCrDt", model.errCrDt);
                int rows = DBConnection.ExecuteQuery(dict, query);
                return rows;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int IsUnique(ErrorCodeMasterEntity objErrEntity)
        {
            try
            {
                int count;
                string query = $"SELECT * FROM ERROR_CODE_MASTER WHERE ERR_CODE='{objErrEntity.errCode}'";
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

        public DataTable FetchErrorGrid()
        {
            try
            {
                string query = $"SELECT * FROM ERROR_CODE_MASTER";
                DataTable dt = DBConnection.ExecuteDataset(query);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable FetchErrorMaster(ErrorCodeMasterEntity objErrorEntity)
        {
            try
            {
                string query = $"SELECT * FROM ERROR_CODE_MASTER WHERE ERR_CODE='{objErrorEntity.errCode}'";
                DataTable dt = DBConnection.ExecuteDataset(query);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int DeleteErr(ErrorCodeMasterEntity objerrEntity)
        {
            try
            {
                string query = $"DELETE FROM ERROR_CODE_MASTER WHERE ERR_CODE='{objerrEntity.errCode}'";
                int rows = DBConnection.ExecuteQuery(query);
                return rows;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int UpdateErrorMaster(ErrorCodeMasterEntity objErrorEntity)
        {
            try
            {
                string query = $"UPDATE ERROR_CODE_MASTER SET  ERR_TYPE='{objErrorEntity.errType}',ERR_DESC='{objErrorEntity.errDesc}',ERR_CR_BY='{objErrorEntity.errUpBy}',ERR_UP_DT='{System.DateTime.Now.ToString("dd/MMMM/yyyy")}' WHERE ERR_CODE = '{objErrorEntity.errCode}'";
                int gd = DBConnection.ExecuteQuery(query);
                return gd;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable FetchLoginError()
        {
            try
            {
                string query = $"SELECT ERR_DESC FROM ERROR_CODE_MASTER WHERE ERR_CODE='5'";
                DataTable dt = DBConnection.ExecuteDataset(query);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable FetchDeletion()
        {
            try
            {
                string query = $"SELECT ERR_DESC FROM ERROR_CODE_MASTER WHERE ERR_CODE='3'";
                DataTable dt = DBConnection.ExecuteDataset(query);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable SavedSuccessfully()
        {
            try
            {
                string query = $"SELECT ERR_DESC FROM ERROR_CODE_MASTER WHERE ERR_CODE='1'";
                DataTable dt = DBConnection.ExecuteDataset(query);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable UpdatedSuccessfully()
        {
            try
            {
                string query = $"SELECT ERR_DESC FROM ERROR_CODE_MASTER WHERE ERR_CODE='2'";
                DataTable dt = DBConnection.ExecuteDataset(query);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
