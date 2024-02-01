using BusinessLayer.Master;
using BusinessLayer.Transaction;
using EntityLayer.Master;
using EntityLayer.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]   
    public class EmployeeListApiController : ControllerBase
    {
        PrEmployeeManager objPrEmployeeManager = new PrEmployeeManager();
        PrEmployeeEntity objEmpEntityy = new PrEmployeeEntity();
        UserMasterManager objUserManager= new UserMasterManager();

        [HttpPost]
        [Route("EmployeeList")]
        public IActionResult EmployeeList()
        {           
            DataTable dt = objPrEmployeeManager.FetchEmployeeDetails();
            var dataList = DataTableToDictionaryList(dt);
            return new JsonResult(new { recordsTotal = dt.Rows.Count, data = dataList });
        }

       

        private List<Dictionary<string, object>> DataTableToDictionaryList(DataTable dt)
        {
            var dataList = new List<Dictionary<string, object>>();
            foreach (DataRow row in dt.Rows)
            {
                var dataRow = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    dataRow[col.ColumnName] = row[col];
                }
                dataList.Add(dataRow);               
            }
            return dataList;
        }

        [HttpPost]
        [Route("SaveEmployee")]
        [Route("/EmployeeListApi/SaveEmployee")]
        public IActionResult SaveEmployee(PrEmployeeEntity model)
        {
            PrEmployeeManager objEmpManager = new PrEmployeeManager();
            int dt = objEmpManager.InsertEmployeeToDb(model);     
            string empNo = objEmpManager.FetchEmpNo();
            model.empNo = empNo;
            int dt1 = objPrEmployeeManager.InsertIntoUserMaster(model);
            return Ok(empNo);
        }

        [HttpPost]
        [Route("EditEmployee")]
        public IActionResult EditEmployee(PrEmployeeEntity objEmpEntity)
        {
            DataTable dt = objPrEmployeeManager.FetchEmployeeDetails(objEmpEntity);
            PrEmployeeEntity objEmpEntityy = new PrEmployeeEntity();
            objEmpEntityy.empNo = dt.Rows[0]["EMP_NO"].ToString();
            objEmpEntityy.empName = dt.Rows[0]["EMP_NAME"].ToString();
            objEmpEntityy.empSalary = Convert.ToInt32(dt.Rows[0]["EMP_SALARY"]);
            objEmpEntityy.empDob = Convert.ToDateTime(dt.Rows[0]["EMP_DOB"]);
            objEmpEntityy.empJoinDate = Convert.ToDateTime(dt.Rows[0]["EMP_JOIN_DATE"]);
            objEmpEntityy.empDeptNo = dt.Rows[0]["EMP_DEPTNO"].ToString();
            objEmpEntityy.empMgrNo = dt.Rows[0]["EMP_MGRNO"].ToString();
            objEmpEntityy.empStatus = dt.Rows[0]["EMP_STATUS"].ToString();
            objEmpEntityy.empActiveYn= dt.Rows[0]["EMP_ACTIVE_YN"].ToString();
            return Ok(objEmpEntityy);
        }

        [HttpPost]
        [Route("UpdateEmployee")]
        [Route("/EmployeeListApi/UpdateEmployee")]
        public IActionResult UpdateEmployee(PrEmployeeEntity model)
        {
            PrEmployeeManager objEmpMananger = new PrEmployeeManager();
            int dt = objEmpMananger.UpdateEmployee(model);
            return Ok(dt);
        }


        [HttpPost]
        [Route("DeleteEmployee")]
        public IActionResult DeleteEmployee(PrEmployeeEntity objEmpEntity)
        {
            int dt = objPrEmployeeManager.DeleteEmployee(objEmpEntity);
            return Ok(dt);
        }

        [HttpGet]
        [Route("GetCountsofEmp")]
        public IActionResult GetCountsofEmp()
        {
            int dt = objPrEmployeeManager.FetchEmpCount();
            return Ok(dt);
        }

        [HttpGet]
        [Route("GetActiveCounts")]
        public IActionResult GetActiveCounts()
        {
            int dt = objPrEmployeeManager.FetchActiveEmpCount();
            return Ok(dt);
        }

        [HttpGet]
        [Route("GetManagerCount")]
        public IActionResult GetManagerCount()
        {
            int dt = objPrEmployeeManager.FetchManagerCount();
            return Ok(dt);
        }

        [HttpGet]
        [Route("GetActiveManagers")]
        public IActionResult GetActiveManagers()
        {
            int dt = objPrEmployeeManager.FetchActiveManagerCount();
            return Ok(dt);
        }

        [HttpGet]
        [Route("GetLatestPayrollMonth")]
        public IActionResult GetLatestPayrollMonth()
        {
            string dt = objPrEmployeeManager.FetchMaxMonthOfSalaryoProcessed();
            int year = int.Parse(dt.Substring(0, 4));
            int month = int.Parse(dt.Substring(4, 2));
            DateTime date = new DateTime(year, month, 30);         
            string mmName = date.ToString("MMM");
            string res = mmName + " " + year;
            return Ok(res);
        }

        [HttpGet]
        [Route("GetCalenderDay")]
        public IActionResult GetCalenderDay()
        {
            string dt = objPrEmployeeManager.FetchMaxMonthOfSalaryoProcessed();
            int year = int.Parse(dt.Substring(0, 4));
            int month = int.Parse(dt.Substring(4, 2));
            int countOfDays = DateTime.DaysInMonth(year, month);
            return Ok(countOfDays);
        }

        [HttpGet]
        [Route("PayrollEmpCount")]
        public IActionResult PayrollEmpCount()
        {
            string max = objPrEmployeeManager.FetchMaxMonthOfSalaryoProcessed();
            string dt =objPrEmployeeManager.FetchEmpCountWRTAttendence(max) + "/" + objPrEmployeeManager.FetchActiveEmpCount();
            return Ok(dt);
        }

        [HttpGet]
        [Route("PayrollCost")]
        public IActionResult PayrollCost()
        {
            string max = objPrEmployeeManager.FetchMaxMonthOfSalaryoProcessed();
            double TotSum = objPrEmployeeManager.TotalPayrollSum(max);
            return Ok(TotSum);
        }

        [HttpGet]
        [Route("ScrollingNews")]
        public IActionResult ScrollingNews()
        {
            string res;
            string dt = objPrEmployeeManager.FetchMaxMonthOfSalaryoProcessed();
            int year = int.Parse(dt.Substring(0, 4));
            int month = int.Parse(dt.Substring(4, 2));
            DateTime date = new DateTime(year, month, 30);
            string monthName = date.ToString("MMMM");
            if(dt!=null)
            {
                res = "Salary in " + monthName + " " + year + " Processed!";
            }
            else
            {
                res = "Salary Not Processed!";
            }
            
            return Ok(res);
        }
    }
}
