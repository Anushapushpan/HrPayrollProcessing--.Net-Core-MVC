using BusinessLayer.Transaction;
using EntityLayer.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Runtime.Serialization;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollApiController : ControllerBase
    {
        PrEmployeePayrollEntity objPayrollEntity = new PrEmployeePayrollEntity();
        PrEmployeePayrollManager objPayrollManager = new PrEmployeePayrollManager();
        PrEmployeeAttendenceManager objAttManager= new PrEmployeeAttendenceManager();
        [HttpGet]
        [Route("PayrollListing")]
        public IActionResult PayrollListing(string month, string year)
        {
            objPayrollEntity.prYyyMm = year + month;           
            PrEmployeePayrollManager objPayrollManager = new PrEmployeePayrollManager();
            DataTable dt = objPayrollManager.FetchPayrollDetails(objPayrollEntity.prYyyMm);
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
        [Route("PayrollProcess")]
        public IActionResult PayrollProcess(PrEmployeePayrollEntity model)
        {
            int year = int.Parse(model.prYyyMm.Substring(0, 4));
            int month = int.Parse(model.prYyyMm.Substring(4, 2));
            int days = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));
            int res1=0;
            if (objPayrollManager.IsPayrollProcessed(model.prYyyMm))
            {
                res1 = 2;
            }
            else
            {
                if (objAttManager.IsAttendenceProcessed(model.prYyyMm, days))
                {
                    if (objPayrollManager.IsPreviousMonthPayrollProcessed(model.prYyyMm))
                    {
                        res1 = objPayrollManager.ProcessPayroll(model.prYyyMm, days, model.ehCrBy);                     
                    }
                    else
                    {
                        res1 = 1;
                    }
                }
                else
                {
                    res1 = 3;
                }
            }
            return Ok(res1);
        }
    }
}
