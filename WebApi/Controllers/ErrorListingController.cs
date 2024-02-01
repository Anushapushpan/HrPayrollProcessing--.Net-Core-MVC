using BusinessLayer.Master;
using EntityLayer.Master;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Runtime.Serialization;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorListingController : ControllerBase
    {
        ErrorCodeMasterManager objErrManager = new ErrorCodeMasterManager();
        ErrorCodeMasterEntity objErrEntity= new ErrorCodeMasterEntity();

        [HttpPost]
        [Route("ErrorListing")]
        public IActionResult ErrorListing()
        {
            DataTable dt = objErrManager.FetchErrorGrid();
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
        [Route("FetchErrorMaster")]
        public IActionResult FetchErrorMaster(ErrorCodeMasterEntity objErrorEntity)
        {

            DataTable dt = objErrManager.FetchErrorMaster(objErrorEntity);
            objErrEntity.errCode = dt.Rows[0]["ERR_CODE"].ToString();
            objErrEntity.errType = dt.Rows[0]["ERR_TYPE"].ToString();
            objErrEntity.errDesc = dt.Rows[0]["ERR_DESC"].ToString();
            return Ok(objErrEntity);
        }

        [HttpPost]
        [Route("/api/ErrorListing/SaveErrorToDb")]
       
        public IActionResult SaveErrorToDb(ErrorCodeMasterEntity model)
        {
            int dt = objErrManager.IsInsertErrorMaster(model);
            return Ok(dt);
        }

        [HttpPost]
        [Route("UpdateErrorInDb")]
        public IActionResult UpdateErrorInDb(ErrorCodeMasterEntity model)
        {
            int dt = objErrManager.UpdateErrorMaster(model);
            return Ok(dt);
        }

        [HttpPost]
        [Route("DeleteError")]
        public IActionResult DeleteError(ErrorCodeMasterEntity objerrEntity)
        {
            int dt = objErrManager.DeleteErr(objerrEntity);
            return Ok(dt);
        }

        [HttpPost]
        [Route("CheckUniqueness")]
        public IActionResult CheckUniqueness(ErrorCodeMasterEntity objErrEntity)
        {
            int dt = objErrManager.IsUnique(objErrEntity);
            return Ok(dt);
        }
    }
}
