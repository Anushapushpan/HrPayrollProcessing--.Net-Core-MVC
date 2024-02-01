using BusinessLayer.Master;
using EntityLayer.Master;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentListingController : ControllerBase
    {
        DepartmentMasterManager objDeptManager= new DepartmentMasterManager();
        [HttpPost]
        [Route("DepartmentListing")]
        public IActionResult DepartmentListing()
        {
            DepartmentMasterManager objDeptManager = new DepartmentMasterManager();
            DataTable dt = objDeptManager.FetDepartmentDetails();
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
        [Route("SaveDeptToDb")]
        public IActionResult SaveDeptToDb(DepartmentMasterEntity model)
        {
            int dt = objDeptManager.InsertDeptMasterToDb(model);
            return Ok(dt);
        }

        [HttpPost]
        [Route("FetchDeptFromDb")]
        public IActionResult FetchDeptFromDb(DepartmentMasterEntity objDeptEntity)
        {

            DataTable dt = objDeptManager.FetchDepartmentMaster(objDeptEntity);

            objDeptEntity.deptNo = dt.Rows[0]["DEPT_NO"].ToString();
            objDeptEntity.deptName = dt.Rows[0]["DEPT_NAME"].ToString();
            return Ok(objDeptEntity);
        }

        [HttpPost]
        [Route("UpdateDeptInDb")]      
        public IActionResult UpdateDeptInDb(DepartmentMasterEntity model)
        {
            int dt = objDeptManager.UpdateDepartment(model);
            return Ok(dt);
        }

        [HttpPost]
        [Route("DeleteDept")]
        public IActionResult DeleteDept(DepartmentMasterEntity objDeptEntity)
        {
            int dt = objDeptManager.DeleteDept(objDeptEntity);
            return Ok(dt);
        }

        [HttpPost]
        [Route("CheckUniqueness")]
        public IActionResult CheckUniqueness(DepartmentMasterEntity objDeptEntity)
        {
            int dt = objDeptManager.isValidateUnique(objDeptEntity);
            return Ok(dt);
        }
    }
}
