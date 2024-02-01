using DatabaseLayer;
using EntityLayer.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Transaction
{
    public class PrEmployeeHrManager
    {
        DataTable dt = new DataTable();

        public int InsertAllowanceToDbb(PrEmployeeHrEntity objPrEmployeeHr)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                string query = "INSERT INTO PR_EMPOLYEE_HR(EH_EMP_NO ,EH_DESIGNATION,EH_GRADE ,EH_BASIC ,EH_HRA ,EH_CONV,EH_DA ,EH_TDS,EH_ESI,EH_CR_BY,EH_CR_DT) VALUES(:ehEmpNo,:ehDesignation,:ehGrade,:ehBasic,:ehHra,:ehConv,:ehDa,:ehTds,:ehEsi,:ehCrBy,:ehCrDt)";
                dict.Add("ehEmpNo", objPrEmployeeHr.ehEmpNo);
                dict.Add("ehDesignation", objPrEmployeeHr.ehDesignation);
                dict.Add("ehGrade", objPrEmployeeHr.ehGrade);
                dict.Add("ehBasic", objPrEmployeeHr.ehBasic);
                dict.Add("ehHra", objPrEmployeeHr.ehHra);
                dict.Add("ehConv", objPrEmployeeHr.ehConv);
                dict.Add("ehDa", objPrEmployeeHr.ehDa);
                dict.Add("ehTds", objPrEmployeeHr.ehTds);
                dict.Add("ehEsi", objPrEmployeeHr.ehEsi);
                dict.Add("ehCrBy", objPrEmployeeHr.ehCrBy);
                dict.Add("ehCrDt", objPrEmployeeHr.ehCrDt);
                int rows = DBConnection.ExecuteQuery(dict, query);
                return rows;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable FetchAllowanceDetails(PrEmployeeHrEntity objEmpHrEntity)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DBConnection.ExecuteDataset($"SELECT * FROM PR_EMPOLYEE_HR WHERE EH_EMP_NO='{objEmpHrEntity.ehEmpNo}'");
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public bool isValidateUnique(string empno)
        {
            try
            {
                int count;
                string query = $"SELECT * FROM PR_EMPOLYEE_HR WHERE EH_EMP_NO='{empno}'";
                DataTable dt = DBConnection.ExecuteDataset(query);
                count = dt.Rows.Count;
                if (count > 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int UpdateAllowanceDetails(PrEmployeeHrEntity objPrEmployeeHr)
        {
            try
            {
                string sql = $"UPDATE PR_EMPOLYEE_HR SET EH_DESIGNATION='{objPrEmployeeHr.ehDesignation}',EH_GRADE='{objPrEmployeeHr.ehGrade}',EH_BASIC={objPrEmployeeHr.ehBasic},EH_HRA={objPrEmployeeHr.ehHra},EH_CONV={objPrEmployeeHr.ehConv},EH_DA={objPrEmployeeHr.ehDa},EH_TDS={objPrEmployeeHr.ehTds},EH_ESI={objPrEmployeeHr.ehEsi},EH_UP_BY='{objPrEmployeeHr.ehUpBy}',EH_UP_DT='{System.DateTime.Now.ToString("dd/MMMM/yyyy")}' WHERE EH_EMP_NO = '{objPrEmployeeHr.ehEmpNo}'";
                int gd = DBConnection.ExecuteQuery(sql);
                return gd;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable FetchAllowanceHistoryDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DBConnection.ExecuteDataset("SELECT EH_EMP_NO,EH_DESIGNATION,EH_GRADE,EH_BASIC,EH_HRA,EH_CONV,EH_DA,EH_TDS,EH_ESI,EH_ACTION_TYPE, CASE WHEN EH_ACTION_TYPE='I' THEN 'INSERT' WHEN EH_ACTION_TYPE='D' THEN 'DELETE' WHEN EH_ACTION_TYPE='U' THEN 'UPDATE' END AS EH_ACTION_TYPES,EH_ACTION_SRL FROM PR_EMPLOYEE_HR_HIST ORDER BY EH_EMP_NO DESC");
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable FetchAllowanceHistoryDetails(string empNo)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DBConnection.ExecuteDataset($"SELECT EH_EMP_NO,EH_DESIGNATION,EH_GRADE,EH_BASIC,EH_HRA,EH_CONV,EH_DA,EH_TDS,EH_ESI,EH_ACTION_TYPE, CASE WHEN EH_ACTION_TYPE='I' THEN 'INSERT' WHEN EH_ACTION_TYPE='D' THEN 'DELETE' WHEN EH_ACTION_TYPE='U' THEN 'UPDATE' END AS EH_ACTION_TYPES,EH_ACTION_SRL FROM PR_EMPLOYEE_HR_HIST WHERE EH_EMP_NO LIKE '%{empNo}%'");
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool IsEmpExist(string empNo)
        {
            try
            {
                dt = DBConnection.ExecuteDataset($"SELECT * FROM PR_EMPLOYEE_HR_HIST WHERE EH_EMP_NO LIKE '%{empNo}%'");
                int rows = dt.Rows.Count;
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int FetchAllowance(string empNo)
        {
            try
            {
                dt = DBConnection.ExecuteDataset($"SELECT * FROM PR_EMPOLYEE_HR WHERE EH_EMP_NO='{empNo}'");
                int rows = dt.Rows.Count;
                return rows;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
