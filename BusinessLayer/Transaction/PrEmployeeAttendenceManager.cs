using EntityLayer.Transaction;
using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLayer;

namespace BusinessLayer.Transaction
{
    public class PrEmployeeAttendenceManager
    {
        public DataTable FetchGridDetails(string yyyymm, int days)
        {
            try
            {
                int year = int.Parse(yyyymm.Substring(0, 4));
                int month = int.Parse(yyyymm.Substring(4, 2));

                DateTime date = new DateTime(year, month, days);
                string date1 = date.ToString("dd-MMM-yyyy");
                DataTable gd = new DataTable();
                //gd = DBConnection.ExecuteDataset("SELECT EMP_NO,(SELECT ATT_DAYS_PRESENT FROM PR_EMPLOYEE_ATTENDANCE WHERE ATT_EMP_NO = EMP_NO)PRESENT,(SELECT ATT_DAYS_ABSENT FROM PR_EMPLOYEE_ATTENDANCE WHERE ATT_EMP_NO = EMP_NO)ABSENT FROM PR_EMPLOYEE WHERE PR_EMPLOYEE.EMP_ACTIVE_YN='Y'");
                gd = DBConnection.ExecuteDataset($" SELECT EMP_NO,(SELECT ATT_DAYS_PRESENT FROM PR_EMPLOYEE_ATTENDANCE WHERE ATT_EMP_NO = EMP_NO AND ATT_YYYYMM = '{yyyymm}')PRESENT,(SELECT ATT_DAYS_ABSENT FROM PR_EMPLOYEE_ATTENDANCE WHERE ATT_EMP_NO = EMP_NO AND ATT_YYYYMM = '{yyyymm}')ABSENT FROM PR_EMPLOYEE WHERE PR_EMPLOYEE.EMP_ACTIVE_YN='Y'  AND PR_EMPLOYEE.EMP_JOIN_DATE<='{date1}'");
                //gd = DBConnection.ExecuteDataset($"SELECT ATT_EMP_NO,ATT_YYYYMM,ATT_DAYS_PRESENT,ATT_DAYS_ABSENT FROM PR_EMPLOYEE_ATTENDANCE WHERE ATT_EMP_NO='{EmpNo}'AND ATT_YYYYMM='{EmpYYMM}'");
                return gd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable FetchGridDetails(string yyyymm, int days, string empNo)
        {
            try
            {
                int year = int.Parse(yyyymm.Substring(0, 4));
                int month = int.Parse(yyyymm.Substring(4, 2));

                DateTime date = new DateTime(year, month, days);
                string date1 = date.ToString("dd-MMM-yyyy");
                DataTable gd = new DataTable();

                gd = DBConnection.ExecuteDataset($" SELECT EMP_NO,(SELECT ATT_DAYS_PRESENT FROM PR_EMPLOYEE_ATTENDANCE WHERE ATT_EMP_NO = EMP_NO AND ATT_YYYYMM = '{yyyymm}')PRESENT,(SELECT ATT_DAYS_ABSENT FROM PR_EMPLOYEE_ATTENDANCE WHERE ATT_EMP_NO = EMP_NO AND ATT_YYYYMM = '{yyyymm}')ABSENT FROM PR_EMPLOYEE WHERE PR_EMPLOYEE.EMP_ACTIVE_YN='Y' AND EMP_NO LIKE '%{empNo}%'");

                return gd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable FetchAttendencWRTDate(string yyyymm)
        {
            try
            {
                DataTable gd = new DataTable();
                gd = DBConnection.ExecuteDataset($" SELECT EMP_NO,(SELECT ATT_DAYS_PRESENT FROM PR_EMPLOYEE_ATTENDANCE WHERE ATT_EMP_NO = EMP_NO AND ATT_YYYYMM = '{yyyymm}')PRESENT,(SELECT ATT_DAYS_ABSENT FROM PR_EMPLOYEE_ATTENDANCE WHERE ATT_EMP_NO = EMP_NO AND ATT_YYYYMM = '{yyyymm}')ABSENT FROM PR_EMPLOYEE WHERE PR_EMPLOYEE.EMP_ACTIVE_YN='Y'");
                return gd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable FetchAttendenceDetails(PrEmployeeAttendenceEntity objAttendencEntity)
        {
            try
            {
                DataTable gd = new DataTable();
                gd = DBConnection.ExecuteDataset($"SELECT ATT_DAYS_PRESENT,ATT_DAYS_ABSENT FROM PR_EMPLOYEE_ATTENDANCE WHERE ATT_EMP_NO= '{objAttendencEntity.attEmpNo}' AND ATT_YYYYMM='{objAttendencEntity.attYyyyMm}'");
                return gd;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int UpdateAttendence(PrEmployeeAttendenceEntity objAttendencEntity)
        {
            try
            {

                string query = $"UPDATE PR_EMPLOYEE_ATTENDANCE SET ATT_DAYS_PRESENT='{objAttendencEntity.attDaysPresent}',ATT_DAYS_ABSENT='{objAttendencEntity.attDaysAbsent}',ATT_UP_BY='{objAttendencEntity.attCrBy}',ATT_UP_DT='{System.DateTime.Now.ToString("dd/MMMM/yyyy")}' WHERE ATT_EMP_NO='{objAttendencEntity.attEmpNo}'";
                int gd = DBConnection.ExecuteQuery(query);
                return gd;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable FetchPresentDays(string attEmpNo, string yyyymm)
        {
            try
            {
                DataTable gd = new DataTable();
                gd = DBConnection.ExecuteDataset($"SELECT ATT_DAYS_PRESENT FROM PR_EMPLOYEE_ATTENDANCE WHERE ATT_EMP_NO='{attEmpNo}' AND ATT_YYYYMM='{yyyymm}'");
                return gd;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public int InsertAttendenceToDb(PrEmployeeAttendenceEntity objAttendencEntity)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                string query = "INSERT INTO PR_EMPLOYEE_ATTENDANCE(ATT_EMP_NO,ATT_YYYYMM,ATT_DAYS_PRESENT,ATT_DAYS_ABSENT,ATT_CR_BY,ATT_CR_DT) VALUES(:attEmpNo,:attYyyyMm,:attDaysPresent,:attDaysAbsent,:attCrBy,:attCrDt)";
                dict.Add("attEmpNo", objAttendencEntity.attEmpNo);
                dict.Add("attYyyyMm", objAttendencEntity.attYyyyMm);
                dict.Add("attDaysPresent", objAttendencEntity.attDaysPresent);
                dict.Add("attDaysAbsent", objAttendencEntity.attDaysAbsent);
                dict.Add("attCrBy", objAttendencEntity.attCrBy);
                dict.Add("attCrDt", objAttendencEntity.attCrDt);

                int rows = DBConnection.ExecuteQuery(dict, query);
                return rows;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateAttendencePk(PrEmployeeAttendenceEntity objAttendencEntity, string ehEmpNo)
        {
            try
            {

                string query = $"UPDATE PR_EMPLOYEE_ATTENDANCE SET ATT_DAYS_PRESENT='{objAttendencEntity.attDaysPresent}' WHERE ATT_EMP_NO='{ehEmpNo}' AND ATT_YYYYMM='{objAttendencEntity.attYyyyMm}'";
                int gd = DBConnection.ExecuteQuery(query);
                return gd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ProcessAttendence(string att_yyyymm, int days, string created_by)
        {
            OracleConnection connection = null;
            try
            {
                int year = int.Parse(att_yyyymm.Substring(0, 4));
                int month = int.Parse(att_yyyymm.Substring(4, 2));

                DateTime date = new DateTime(year, month, days);
                string date1 = date.ToString("dd-MMM-yyyy");
                connection = DBConnection.OpenConnection();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connection;
                cmd.CommandText = "DPRC_INSERT_EMPLOYEE_HAVING_NO_ABSENT";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("P_ATT_YYYYMM", OracleType.VarChar).Value = att_yyyymm;
                cmd.Parameters.Add("P_JOIN_DATE", OracleType.DateTime).Value = date1;
                cmd.Parameters.Add("P_ATT_DAYS_PRESENT", OracleType.Int32).Value = days;
                cmd.Parameters.Add("P_ATT_CR_BY", OracleType.VarChar).Value = created_by;
                cmd.Parameters.Add("P_STATUS", OracleType.Int32).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                int op = Convert.ToInt32(cmd.Parameters["P_STATUS"].Value.ToString());
                return op;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    DBConnection.CloseConnection(connection);
            }
        }

        public bool isValidUnique(string empNo, string attYyyyMm)
        {
            try
            {
                int count;
                string query = $"SELECT * FROM PR_EMPLOYEE_ATTENDANCE WHERE ATT_EMP_NO='{empNo}' AND ATT_YYYYMM='{attYyyyMm}'";
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

        public DataTable CheckUniqueness(string yyyymm)
        {
            try
            {
                DataTable gd = new DataTable();
                gd = DBConnection.ExecuteDataset($"SELECT COUNT(*) COUNT FROM PR_EMPLOYEE_ATTENDANCE WHERE ATT_YYYYMM='{yyyymm}'");
                return gd;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool IsAttendenceProcessed(string yyyymm, int days)
        {
            try
            {
                int year = int.Parse(yyyymm.Substring(0, 4));
                int month = int.Parse(yyyymm.Substring(4, 2));

                DateTime date = new DateTime(year, month, days);
                string date1 = date.ToString("dd-MMM-yyyy");
                DataTable gd1 = new DataTable();
                DataTable gd2 = new DataTable();
                gd1 = DBConnection.ExecuteDataset($"SELECT EMP_NO FROM PR_EMPLOYEE WHERE EMP_ACTIVE_YN='Y'  AND EMP_JOIN_DATE<='{date1}'");
                gd2 = DBConnection.ExecuteDataset($"SELECT ATT_DAYS_ABSENT FROM PR_EMPLOYEE_ATTENDANCE WHERE ATT_YYYYMM='{yyyymm}' AND ATT_DAYS_ABSENT IS NOT NULL");
                int rows = gd2.Rows.Count;
                int countOfEmp = gd1.Rows.Count;
                if (countOfEmp == rows)
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
    }
}
