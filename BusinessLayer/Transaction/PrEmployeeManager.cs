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
    public class PrEmployeeManager
    {
        DataTable dt = new DataTable();
        public string GenerateEmpNo(string joinYear)
        {

            string empNo = "E-" + joinYear + "-";

            return empNo;
        }

        //public string GetEmpNo()
        //{
        //    DataTable gd = new DataTable();
        //    string q = "SELECT SEQNC_EMPNO.CURRVAL FROM DUAL";
        //    int empno = DBConnection.ExecuteScalar(q);

        //    return empno.ToString();
        //}
        public int InsertEmployeeToDb(PrEmployeeEntity employeeEntity)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                string sql = "INSERT INTO PR_EMPLOYEE(EMP_NO,EMP_PWD,EMP_NAME,EMP_SALARY,EMP_DOB,EMP_JOIN_DATE,EMP_DEPTNO,EMP_MGRNO,EMP_STATUS,EMP_ACTIVE_YN,EMP_CR_BY,EMP_CR_DT) VALUES(:empNo||LPAD(SEQNC_EMPNO.NEXTVAL,5,0),:empNo||LPAD(SEQNC_EMPNO.NEXTVAL,5,0),:empName,:empSalary,TO_DATE(:empDob,'DD/MM/RRRR'),TO_DATE(:empJoinDate,'DD/MM/RRRR'),:empDeptNo,:empMgrNo,:empStatus,:empActiveYn,:empCrBy,TO_DATE(:empCrDt,'DD/MM/RRRR'))";
                dict.Add("empNo", employeeEntity.empNo);
                dict.Add("empPwd", employeeEntity.empPwd);
                dict.Add("empName", employeeEntity.empName);
                dict.Add("empSalary", employeeEntity.empSalary);
                dict.Add("empDob", employeeEntity.empDob.ToString("dd/MM/yyyy"));
                dict.Add("empJoinDate", employeeEntity.empJoinDate.ToString("dd/MM/yyyy"));
                dict.Add("empDeptNo", employeeEntity.empDeptNo);
                dict.Add("empMgrNo", employeeEntity.empMgrNo);
                dict.Add("empStatus", employeeEntity.empStatus);
                dict.Add("empActiveYn", employeeEntity.empActiveYn);
                dict.Add("empCrBy", employeeEntity.empCrBy);
                dict.Add("empCrDt", employeeEntity.empCrDt.ToString("dd/MM/yyyy"));
                int rows = DBConnection.ExecuteQuery(dict, sql);
                return rows;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int InsertIntoUserMaster(PrEmployeeEntity model)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                string sql = "INSERT INTO USER_MASTER(USER_ID,USER_NAME,USER_PASSWORD,USER_CR_BY,USER_CR_DT,USER_ACTIVE_YN) VALUES(:userId,:userName,:userPassword,:userCrBy,TO_DATE(:userCrDt,'DD/MM/RRRR'),:empActiveYn)";
                dict.Add("userId", model.empNo);
                dict.Add("userName", model.empNo);
                dict.Add("userPassword", model.empNo);
                dict.Add("userCrBy", model.empCrBy);
                dict.Add("userCrDt", model.empCrDt.ToString("dd/MM/yyyy"));
                dict.Add("empActiveYn", model.empActiveYn);
                int rows = DBConnection.ExecuteQuery(dict, sql);
                return rows;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int UpdateEmployee(PrEmployeeEntity model)
        {
            try
            {
                string query = $"UPDATE PR_EMPLOYEE SET EMP_NAME='{model.empName}',EMP_DOB='{model.empDob.ToString("dd/MMMM/yyyy")}',EMP_JOIN_DATE='{model.empJoinDate.ToString("dd/MMMM/yyyy")}',EMP_DEPTNO='{model.empDeptNo}',EMP_MGRNO='{model.empMgrNo}',EMP_STATUS='{model.empStatus}',EMP_ACTIVE_YN='{model.empActiveYn}',EMP_UP_BY='{model.empUpBy}',EMP_UP_DT='{model.empUpDt.ToString("dd/MMMM/yyyy")}' WHERE EMP_NO='{model.empNo}'";
                int rows = DBConnection.ExecuteQuery(query);
                return rows;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void InsertSalaryToEmployee(string empNo, double salary)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                string query = $"UPDATE PR_EMPLOYEE SET EMP_SALARY={salary} WHERE EMP_NO='{empNo}'";
                DBConnection.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateSalaryToEmployee(string empNo, double salary,string user)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                string query = $"UPDATE PR_EMPLOYEE SET EMP_SALARY={salary},EMP_UP_BY='{user}',EMP_UP_DT='{System.DateTime.Now.ToString("dd/MMMM/yyyy")}' WHERE EMP_NO='{empNo}'";
                DBConnection.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string FetchEmpNo()
        {
            try
            {
                DataTable gd = new DataTable();
                gd = DBConnection.ExecuteDataset("SELECT EMP_NO FROM PR_EMPLOYEE WHERE EMP_NO=(SELECT MAX(EMP_NO) MAX FROM PR_EMPLOYEE)");
                string max = "";
                foreach (DataRow ds in gd.Rows)
                {
                    max = ds["EMP_NO"].ToString();
                }
                return max;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable FetchManagerNo()
        {
            try
            {
                DataTable gd = new DataTable();
                gd = DBConnection.ExecuteDataset("SELECT EH_EMP_NO, EH_EMP_NO||'-'||B.EMP_NAME EMP_NAME  FROM (SELECT A.EH_EMP_NO FROM PR_EMPOLYEE_HR A WHERE EH_DESIGNATION='MGR') EMPLOYEE_HR JOIN PR_EMPLOYEE B ON EH_EMP_NO=B.EMP_NO AND B.EMP_ACTIVE_YN='Y'");
                return gd;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int FetchEmpCount()
        {
            try
            {
                DataTable gd = new DataTable();
                gd = DBConnection.ExecuteDataset("SELECT COUNT(*) COUNT FROM PR_EMPLOYEE");
                int count = 0;
                foreach (DataRow ds in gd.Rows)
                {
                    count = Convert.ToInt32(ds["COUNT"]);

                }
                return count;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int FetchActiveEmpCount()
        {
            try
            {
                DataTable gd = new DataTable();
                gd = DBConnection.ExecuteDataset("SELECT COUNT(*) COUNT FROM PR_EMPLOYEE WHERE EMP_ACTIVE_YN='Y'");
                int count = 0;
                foreach (DataRow ds in gd.Rows)
                {
                    count = Convert.ToInt32(ds["COUNT"]);

                }
                return count;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int FetchEmpCountWRTAttendence(string yyyymm)
        {
            try
            {
                DataTable gd = new DataTable();
                gd = DBConnection.ExecuteDataset($"SELECT COUNT(ATT_EMP_NO) COUNT FROM PR_EMPLOYEE_ATTENDANCE WHERE ATT_YYYYMM='{yyyymm}'");
                int count = 0;
                foreach (DataRow ds in gd.Rows)
                {
                    count = Convert.ToInt32(ds["COUNT"]);

                }
                return count;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int TotalPayrollSum(string yyyymm)
        {
            try
            {
                DataTable gd = new DataTable();
                gd = DBConnection.ExecuteDataset($"SELECT SUM(PR_NET_PAYABLE) SUM FROM PR_EMPLOYEE_PAYROLL WHERE PR_YYYMM='{yyyymm}'");
                int sum = 0;
                foreach (DataRow ds in gd.Rows)
                {
                    sum = Convert.ToInt32(ds["SUM"]);

                }
                return sum;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int FetchManagerCount()
        {
            try
            {
                DataTable gd = new DataTable();
                gd = DBConnection.ExecuteDataset("SELECT COUNT(*) COUNT FROM PR_EMPOLYEE_HR WHERE EH_DESIGNATION='MGR'");
                int count = 0;
                foreach (DataRow ds in gd.Rows)
                {
                    count = Convert.ToInt32(ds["COUNT"]);

                }
                return count;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int FetchActiveManagerCount()
        {
            try
            {
                DataTable gd = new DataTable();
                gd = DBConnection.ExecuteDataset("SELECT COUNT(*) COUNT FROM (SELECT A.EMP_NO,A.EMP_ACTIVE_YN,B.EH_DESIGNATION FROM PR_EMPLOYEE A JOIN PR_EMPOLYEE_HR B ON B.EH_EMP_NO=A.EMP_NO) WHERE EMP_ACTIVE_YN='Y' AND EH_DESIGNATION='MGR'");
                int count = 0;
                foreach (DataRow ds in gd.Rows)
                {
                    count = Convert.ToInt32(ds["COUNT"]);

                }
                return count;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string FetchMaxMonthOfSalaryoProcessed()
        {
            try
            {
                DataTable gd = new DataTable();
                gd = DBConnection.ExecuteDataset("SELECT MAX(PR_YYYMM) MAX_MONTH FROM PR_EMPLOYEE_PAYROLL");
                string maxMonth = "";
                foreach (DataRow ds in gd.Rows)
                {
                    maxMonth = ds["MAX_MONTH"].ToString();

                }
                return maxMonth;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable FetchLastMonthPayrollDetails(string yyyymm)
        {
            try
            {
                DataTable gd = new DataTable();
                gd = DBConnection.ExecuteDataset($"SELECT * FROM PR_EMPLOYEE_PAYROLL WHERE PR_YYYMM='{yyyymm}'");
                return gd;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //public DataTable FetchSalary()
        //{
        //    DataTable gd = new DataTable();
        //    gd = DBConnection.ExecuteDataset("SELECT EMP_SALARY FROM PR_EMPLOYEE WHERE EMP_NO=(SELECT MAX(EMP_NO) FROM PR_EMPLOYEE)");
        //    return gd;
        //}

        public DataTable FetchEmployeeDetails(PrEmployeeEntity objEmpEntity)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DBConnection.ExecuteDataset($"SELECT EMP_NO,EMP_PWD,EMP_NAME,TO_CHAR(EMP_DOB,'DD/MM/YYYY') EMP_DOB,TO_CHAR(EMP_JOIN_DATE,'DD/MM/YYYY') EMP_JOIN_DATE,EMP_SALARY,EMP_DEPTNO,EMP_MGRNO,EMP_STATUS,EMP_ACTIVE_YN FROM PR_EMPLOYEE WHERE EMP_NO='{objEmpEntity.empNo}'");
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable FetchEmployeeDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DBConnection.ExecuteDataset("SELECT A.EMP_NO,A.EMP_NAME,A.EMP_DOB,A.EMP_JOIN_DATE,A.EMP_SALARY,(SELECT B.DEPT_NAME FROM DEPARTMENT_MASTER B WHERE B.DEPT_NO=A.EMP_DEPTNO) DEPT_NAME,EMP_MGRNO, (SELECT C.CM_DESC FROM CODES_MASTER C WHERE C.CM_CODE=A.EMP_STATUS) STATUS,EMP_ACTIVE_YN FROM PR_EMPLOYEE A ORDER BY A.EMP_NAME ASC");
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable FetchEmployee(string EmpName)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DBConnection.ExecuteDataset($"SELECT A.EMP_NO,A.EMP_NAME,A.EMP_DOB,A.EMP_JOIN_DATE,A.EMP_SALARY,(SELECT B.DEPT_NAME FROM DEPARTMENT_MASTER B WHERE B.DEPT_NO=A.EMP_DEPTNO) DEPT_NAME,EMP_MGRNO, (SELECT C.CM_DESC FROM CODES_MASTER C WHERE C.CM_CODE=A.EMP_STATUS) STATUS,EMP_ACTIVE_YN FROM PR_EMPLOYEE A WHERE LOWER(A.EMP_NAME) LIKE LOWER('%{EmpName}%') OR A.EMP_NO LIKE '%{EmpName}%'");
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable FetchManagerDetails(string choice)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DBConnection.ExecuteDataset($"SELECT A.EMP_NO,A.EMP_NAME,A.EMP_DOB,A.EMP_JOIN_DATE,A.EMP_SALARY,A.EMP_DEPTNO DEPT_NAME,A.EMP_MGRNO,A.EMP_STATUS STATUS,A.EMP_ACTIVE_YN FROM PR_EMPLOYEE A inner join PR_EMPOLYEE_HR B on B.EH_DESIGNATION='{choice}' AND A.EMP_NO=B.EH_EMP_NO ORDER BY A.EMP_NAME ASC");
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int DeleteEmployee(PrEmployeeEntity objEmpEntity)
        {
            try
            {
                DataTable gd = new DataTable();
                string sql1 = $"DELETE FROM PR_EMPLOYEE WHERE EMP_NO='{objEmpEntity.empNo}'";
                string sql2 = $"DELETE FROM PR_EMPOLYEE_HR WHERE EH_EMP_NO='{objEmpEntity.empNo}'";
                string sql3 = $"DELETE FROM USER_MASTER WHERE USER_ID='{objEmpEntity.empNo}'";
                int rows1 = DBConnection.ExecuteQuery(sql1);
                int rows2 = DBConnection.ExecuteQuery(sql2);
                int rows3 = DBConnection.ExecuteQuery(sql3);
                int rows = rows1 + rows2;
                return rows;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable FetchAllowance()
        {
            try
            {
                DataTable gd = new DataTable();
                string query = $"select CM_CODE, CM_CODE||'-'||CM_DESC CM_DESC from codes_master where cm_type='EMP_STATUS'";
                return gd;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable FetchAllowanceDetails(string empNo)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DBConnection.ExecuteDataset($"SELECT * FROM PR_EMPOLYEE_HR WHERE EH_EMP_NO='{empNo}'");
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        
        public int InsertAllowanceToDb(PrEmployeeHrEntity objPrEmployeeHr)
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

        public bool IsEmployeeUpdated(PrEmployeeEntity objEmployeeEntity)
        {
            try
            {
                string query = $"UPDATE PR_EMPLOYEE SET EMP_NAME='{objEmployeeEntity.empName}',EMP_DOB='{objEmployeeEntity.empDob.ToString("dd/MMMM/yyyy")}',EMP_JOIN_DATE='{objEmployeeEntity.empJoinDate.ToString("dd/MMMM/yyyy")}',EMP_DEPTNO='{objEmployeeEntity.empDeptNo}',EMP_MGRNO='{objEmployeeEntity.empMgrNo}',EMP_STATUS='{objEmployeeEntity.empStatus}',EMP_ACTIVE_YN='{objEmployeeEntity.empActiveYn}',EMP_UP_BY='{objEmployeeEntity.empUpBy}',EMP_UP_DT='{System.DateTime.Now.ToString("dd/MMMM/yyyy")}' WHERE EMP_NO='{objEmployeeEntity.empNo}'";
                int rows = DBConnection.ExecuteQuery(query);
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

        public bool IsEmpExist(string empName)
        {
            try
            {

                dt = DBConnection.ExecuteDataset($"SELECT A.EMP_NO,A.EMP_NAME,A.EMP_DOB,A.EMP_JOIN_DATE,A.EMP_SALARY,(SELECT B.DEPT_NAME FROM DEPARTMENT_MASTER B WHERE B.DEPT_NO=A.EMP_DEPTNO) DEPT_NAME,EMP_MGRNO, (SELECT C.CM_DESC FROM CODES_MASTER C WHERE C.CM_CODE=A.EMP_STATUS) STATUS,EMP_ACTIVE_YN FROM PR_EMPLOYEE A WHERE LOWER(A.EMP_NAME) LIKE LOWER('%{empName}%') OR A.EMP_NO LIKE'%{empName}%'");
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
    }
}
