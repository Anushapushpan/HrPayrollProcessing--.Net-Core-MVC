using BusinessLayer.Master;
using BusinessLayer.Transaction;
using EntityLayer.Master;
using EntityLayer.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendenceApiController : ControllerBase
    {
        PrEmployeeAttendenceEntity objAttEntity = new PrEmployeeAttendenceEntity();
        PrEmployeeAttendenceManager objAttManager = new PrEmployeeAttendenceManager();
        PrEmployeePayrollManager objPayrollManager= new PrEmployeePayrollManager();
        [HttpGet]
        [Route("AttendenceListing")]
        public IActionResult AttendenceListing(string month, string year)
        {
            objAttEntity.attYyyyMm = year + month;
            int year1 = Convert.ToInt32(year);
            int months = Convert.ToInt32(month);
            int days = DateTime.DaysInMonth(year1, months);
            PrEmployeeAttendenceManager objAttManager = new PrEmployeeAttendenceManager();
            DataTable dt = objAttManager.FetchGridDetails(objAttEntity.attYyyyMm, days);
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
                    dataRow[col.ColumnName] = row[col] == DBNull.Value ? null : row[col];
                }
                dataList.Add(dataRow);
            }
            return dataList;
        }

        [HttpPost]
        [Route("FetchAttendenceDetails")]
        public IActionResult FetchAttendenceDetails(PrEmployeeAttendenceEntity objAttEntity)
        {

            DataTable dt = objAttManager.FetchAttendenceDetails(objAttEntity);
            objAttEntity.attDaysPresent = Convert.ToInt32(dt.Rows[0]["ATT_DAYS_PRESENT"]);
            objAttEntity.attDaysAbsent = Convert.ToInt32(dt.Rows[0]["ATT_DAYS_ABSENT"]);
            return Ok(objAttEntity);
        }

        [HttpPost]
        [Route("InsertAttendence")]
        public IActionResult InsertAttendence(PrEmployeeAttendenceEntity model)
        {
            int dt = objAttManager.InsertAttendenceToDb(model);
            return Ok(dt);
        }

        [HttpPost]
        [Route("UpdateAttendence")]
        public IActionResult UpdateAttendence(PrEmployeeAttendenceEntity model)
        {
            int dt = objAttManager.UpdateAttendence(model);
            return Ok(dt);
        }

        [HttpPost]
        [Route("CheckPayroll")]
        public IActionResult CheckPayroll(PrEmployeeAttendenceEntity model)
        {
            DataTable dt = objPayrollManager.CheckPayroll(model);
            int count = 0;
            foreach (DataRow ds in dt.Rows)
            {
                count = Convert.ToInt32(ds["COUNT"]);
            }
            return Ok(count);          
        }

        [HttpPost]
        [Route("ProcessAttendence")]
        public IActionResult ProcessAttendence(PrEmployeeAttendenceEntity model)
        {
            int dt;
            int year = int.Parse(model.attYyyyMm.Substring(0, 4));
            int month = int.Parse(model.attYyyyMm.Substring(4, 2));
            int days = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));                   
            DateTime date = new DateTime(year, month, 1);
            if (System.DateTime.Now <= date)
            {
                dt = 1;
            }
            else
            {
                dt = objAttManager.ProcessAttendence(model.attYyyyMm, days, model.attCrBy);
            }
            return Ok(dt);
        }

        [HttpPost]
        [Route("IsPayrollProcessed")]
        public IActionResult IsPayrollProcessed(PrEmployeeAttendenceEntity model)
        {
            int dt;
            string yyyymm = model.attYyyyMm;
            string empno = model.attEmpNo;
            if (objPayrollManager.IsPayrollProcessed(yyyymm))
            {
                dt = 1;
            }
            else
            {
                dt = 0;
            }
            return Ok(dt);
        }
        //public IActionResult EditAttendence(PrEmployeeAttendenceEntity objAttEntity)
        //{
        //    DataTable dt = objAttManager.FetchUserMaster(objUserMasterEntity);

        //    UserMasterEntity objUserMasterEnt = new UserMasterEntity();
        //    objUserMasterEnt.userId = dt.Rows[0]["USER_ID"].ToString();
        //    objUserMasterEnt.userName = dt.Rows[0]["USER_NAME"].ToString();
        //    objUserMasterEnt.userPassword = dt.Rows[0]["USER_PASSWORD"].ToString();
        //    objUserMasterEnt.userActiveYn = dt.Rows[0]["USER_ACTIVE_YN"].ToString();
        //    return Ok(objUserMasterEnt);
        //}
    }
}

