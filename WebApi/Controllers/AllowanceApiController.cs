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
    public class AllowanceApiController : ControllerBase
    {
        PrEmployeeHrManager objHrManager = new PrEmployeeHrManager();
        PrEmployeeHrEntity objHrEntity = new PrEmployeeHrEntity();
        PrEmployeeEntity objEmpEntity = new PrEmployeeEntity();
        PrEmployeeManager objEmployeeManager = new PrEmployeeManager();
        //[HttpPost]
        //[Route("InsertAllowanceToDb")]
        ////[Route("/CodeListing/SaveCodeMaster")]
        //public IActionResult InsertAllowanceToDb(PrEmployeeHrEntity model)
        //{           
        //    int dt = objHrManager.InsertAllowanceToDbb(model);
        //    return Ok(dt);
        //}

        [HttpPost]
        [Route("FetchAllowance")]
        public IActionResult FetchAllowance(PrEmployeeHrEntity objEmpHrEntity)
        {

            DataTable dt = objHrManager.FetchAllowanceDetails(objEmpHrEntity);
            objHrEntity.ehEmpNo = dt.Rows[0]["EH_EMP_NO"].ToString();
            objHrEntity.ehDesignation = dt.Rows[0]["EH_DESIGNATION"].ToString();
            objHrEntity.ehGrade = dt.Rows[0]["EH_GRADE"].ToString();
            objHrEntity.ehBasic = Convert.ToInt32(dt.Rows[0]["EH_BASIC"]);
            objHrEntity.ehHra = Convert.ToInt32(dt.Rows[0]["EH_HRA"]);
            objHrEntity.ehConv = Convert.ToInt32(dt.Rows[0]["EH_CONV"].ToString());
            objHrEntity.ehDa = Convert.ToInt32(dt.Rows[0]["EH_DA"].ToString());
            objHrEntity.ehTds = Convert.ToInt32(dt.Rows[0]["EH_TDS"].ToString());
            objHrEntity.ehEsi = Convert.ToInt32(dt.Rows[0]["EH_ESI"].ToString());
            return Ok(objHrEntity);
        }

        [HttpPost]
        [Route("InsertAllowance")]
        public IActionResult InsertAllowance(PrEmployeeHrEntity model)
        {
            int dt = objHrManager.InsertAllowanceToDbb(model);
            objEmpEntity.empSalary=model.ehBasic+model.ehHra+model.ehConv+model.ehDa-model.ehTds-model.ehEsi;
            objEmployeeManager.InsertSalaryToEmployee(model.ehEmpNo, objEmpEntity.empSalary);
            return Ok(dt);
        }

        [HttpPost]
        [Route("UpdateAllowance")]
        [Route("/AllowanceApi/UpdateAllowance")]
        public IActionResult UpdateAllowance(PrEmployeeHrEntity model)
        {            
            int dt = objHrManager.UpdateAllowanceDetails(model);
            objEmpEntity.empSalary = model.ehBasic + model.ehHra + model.ehConv + model.ehDa - model.ehTds - model.ehEsi;
            objEmpEntity.empUpBy = "user";
            objEmployeeManager.UpdateSalaryToEmployee(model.ehEmpNo, objEmpEntity.empSalary,objEmpEntity.empUpBy);
            return Ok(dt);
        }

        [HttpPost]
        [Route("AllowanceList/{empno}")]
        public IActionResult AllowanceList(string empno)
        {
            objHrEntity.ehEmpNo = empno;
            DataTable dt = objHrManager.FetchAllowanceDetails(objHrEntity);
            var dataList = DataTableToDictionaryList(dt);
            return new JsonResult(new { recordsTotal = dt.Rows.Count, data = dataList });
        }

        //[HttpPost]
        //[Route("IsAllowanceInserted/{empno}")]
        //public IActionResult IsAllowanceInserted(string empno)
        //{
        //    objHrEntity.ehEmpNo= empno;
        //    bool dt = objHrManager.isValidateUnique(objHrEntity);
        //    return Ok(dt);
        //}
        
        [HttpPost]
        [Route("AllowanceHistory")]
        public IActionResult AllowanceHistory()
        {
            DataTable dt = objHrManager.FetchAllowanceHistoryDetails();
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

        

       
    }
}
