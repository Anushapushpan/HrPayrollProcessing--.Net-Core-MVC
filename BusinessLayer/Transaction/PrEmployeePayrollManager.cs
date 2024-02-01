using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLayer;
using EntityLayer.Transaction;

namespace BusinessLayer.Transaction
{
    public class PrEmployeePayrollManager
    {
        public int ProcessPayroll(string att_yyyymm, int days, string created_by)
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
                cmd.CommandText = "DPRC_PROCESS_PAYROLL";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("P_ATT_YYYYMM", OracleType.VarChar).Value = att_yyyymm;
                cmd.Parameters.Add("P_JOIN_DATE", OracleType.DateTime).Value = date1;
                cmd.Parameters.Add("P_DAYS", OracleType.Int32).Value = days;
                cmd.Parameters.Add("P_CR_BY", OracleType.VarChar).Value = created_by;
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

        public DataTable FetchPayrollDetails(string yyyymm)
        {
            try
            {
                DataTable gd = new DataTable();
                gd = DBConnection.ExecuteDataset($"SELECT * FROM PR_EMPLOYEE_PAYROLL WHERE PR_YYYMM='{yyyymm}' ORDER BY PR_EMP_NO");
                return gd;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable CheckPayroll(PrEmployeeAttendenceEntity model)
        {
            try
            {
                DataTable gd = new DataTable();
                gd = DBConnection.ExecuteDataset($"SELECT COUNT(*) COUNT FROM PR_EMPLOYEE_PAYROLL WHERE PR_YYYMM='{model.attYyyyMm}'");
                return gd;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        public DataTable FetchEmpNo(string yyyymm)
        {
            try
            {
                string query = $"SELECT PR_EMP_NO FROM PR_EMPLOYEE_PAYROLL WHERE PR_YYYMM='{yyyymm}'";
                DataTable dt = DBConnection.ExecuteDataset(query);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable GetEmpDetails(string empNo)
        {
            try
            {
                string query = $"SELECT A.EMP_NO,A.EMP_NAME,(SELECT B.DEPT_NAME FROM DEPARTMENT_MASTER B WHERE B.DEPT_NO=A.EMP_DEPTNO) EMP_DEPTNO, A.EMP_JOIN_DATE FROM PR_EMPLOYEE A WHERE A.EMP_NO='{empNo}'";
                DataTable dt = DBConnection.ExecuteDataset(query);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public DataTable GetPayrollDetails(string empNo, string yyyymm)
        {
            try
            {
                string query = $"SELECT PR_EMP_NO,PR_YYYMM, PR_BASIC,PR_HRA,PR_CONV, PR_DA, PR_TDS,PR_ESI, PR_TOT_EARNINGS, PR_TOT_DEDU, PR_NET_PAYABLE,EXTRACT(YEAR FROM TO_DATE(PR_YYYMM, 'YYYYMM')) AS PR_YEAR, EXTRACT(MONTH FROM TO_DATE(PR_YYYMM, 'YYYYMM')) AS PR_MONTH_NUMBER, CASE EXTRACT(MONTH FROM TO_DATE(PR_YYYMM, 'YYYYMM'))  WHEN 1 THEN 'January' WHEN 2 THEN 'February' WHEN 3 THEN 'March' WHEN 4 THEN 'April' WHEN 5 THEN 'May' WHEN 6 THEN 'June' WHEN 7 THEN 'July'WHEN 8 THEN 'August' WHEN 9 THEN 'September' WHEN 10 THEN 'October' WHEN 11 THEN 'November' WHEN 12 THEN 'December' END AS PR_MONTH_NAME FROM PR_EMPLOYEE_PAYROLL WHERE PR_EMP_NO = '{empNo}' AND PR_YYYMM = '{yyyymm}'";
                DataTable dt = DBConnection.ExecuteDataset(query);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool IsPayrollProcessed(string yyyymm)
        {
            try
            {
                string query = $"SELECT * FROM PR_EMPLOYEE_PAYROLL WHERE PR_YYYMM='{yyyymm}'";
                DataTable dt = DBConnection.ExecuteDataset(query);
                int rows = dt.Rows.Count;
                if (rows > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool IsPayrollProcessedByYear(string year)
        {
            try
            {
                string query = $"SELECT * FROM PR_EMPLOYEE_PAYROLL WHERE PR_YYYMM LIKE '{year}%'";
                DataTable dt = DBConnection.ExecuteDataset(query);
                int rows = dt.Rows.Count;
                if (rows > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool IsPreviousMonthPayrollProcessed(string yyyymm)
        {
            try
            {
                string query = $"SELECT * FROM PR_EMPLOYEE_PAYROLL WHERE PR_YYYMM=TO_CHAR(ADD_MONTHS(TO_DATE('{yyyymm}','YYYYMM'),-1),'YYYYMM')";
                DataTable dt = DBConnection.ExecuteDataset(query);
                int rows = dt.Rows.Count;
                if (rows > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public DataTable ExcelDwd()
        {
            string sql = "SELECT A.PR_YYYMM,A.PR_EMP_NO,B.EMP_NAME, A.PR_DESIGNATION,A.PR_DAYS_PRESENT,A.PR_DAYS_ABSENT,A.PR_BASIC,A.PR_HRA,A.PR_CONV,A.PR_DA,A.PR_TDS,A.PR_ESI,A.PR_TOT_EARNINGS,A.PR_TOT_DEDU,A.PR_NET_PAYABLE FROM PR_EMPLOYEE_PAYROLL A JOIN PR_EMPLOYEE B ON A.PR_EMP_NO = B.EMP_NO ORDER BY A.PR_YYYMM ASC";
            DataTable dt = DBConnection.ExecuteDataset(sql);
            return dt;
        }

        public DataTable ExcelDwdByMonth(string yyyymm)
        {
            string sql = $"SELECT A.PR_YYYMM,A.PR_EMP_NO,B.EMP_NAME, A.PR_DESIGNATION,A.PR_DAYS_PRESENT,A.PR_DAYS_ABSENT,A.PR_BASIC,A.PR_HRA,A.PR_CONV,A.PR_DA,A.PR_TDS,A.PR_ESI,A.PR_TOT_EARNINGS,A.PR_TOT_DEDU,A.PR_NET_PAYABLE FROM PR_EMPLOYEE_PAYROLL A JOIN PR_EMPLOYEE B ON A.PR_EMP_NO = B.EMP_NO AND A.PR_YYYMM='{yyyymm}'ORDER BY A.PR_YYYMM ASC";
            DataTable dt = DBConnection.ExecuteDataset(sql);
            return dt;
        }

        public DataTable ExcelDwdByYear(string year)
        {
            string sql = $"SELECT A.PR_YYYMM,A.PR_EMP_NO,B.EMP_NAME, A.PR_DESIGNATION,A.PR_DAYS_PRESENT,A.PR_DAYS_ABSENT,A.PR_BASIC,A.PR_HRA,A.PR_CONV,A.PR_DA,A.PR_TDS,A.PR_ESI,A.PR_TOT_EARNINGS,A.PR_TOT_DEDU,A.PR_NET_PAYABLE FROM PR_EMPLOYEE_PAYROLL A JOIN PR_EMPLOYEE B ON A.PR_EMP_NO = B.EMP_NO AND A.PR_YYYMM LIKE'{year}%'ORDER BY A.PR_YYYMM ASC";
            DataTable dt = DBConnection.ExecuteDataset(sql);
            return dt;
        }
    }
}
