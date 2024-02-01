using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer;
using BusinessLayer.Master;
using System.Data;
using EntityLayer.Master;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeListingController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        CodeMasterManager objCodeMasterManager = new CodeMasterManager();
        public CodeListingController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        CodeMasterManager objCodesManager = new CodeMasterManager();
        [HttpPost]
        [Route("CodeListing")]
        public IActionResult CodeListing()
        {            
            CodeMasterManager objCodeMasterManager = new CodeMasterManager();
            DataTable dt = objCodeMasterManager.FetchCodesMaster();
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
        [Route("SaveCodeMaster")]
        [Route("/CodeListing/SaveCodeMaster")]
        public IActionResult SaveCodeMaster(CodeMasterEntity model)
        {
            CodeMasterManager objCodeMasterManager = new CodeMasterManager();
            int dt = objCodeMasterManager.InsertCodesMasterToDb(model);
            return Ok(dt);
        }

        [HttpPost]
        [Route("CodeEditing")]
        public IActionResult CodeEditing(CodeMasterEntity objCodeEntity)
        {

            DataTable dt = objCodesManager.FetchCodesMaster(objCodeEntity);             
            CodeMasterEntity objCodesmasterEntity = new CodeMasterEntity();
            objCodesmasterEntity.cmCode = dt.Rows[0]["CM_CODE"].ToString();
            objCodesmasterEntity.cmType = dt.Rows[0]["CM_TYPE"].ToString();
            objCodesmasterEntity.cmDesc = dt.Rows[0]["CM_DESC"].ToString();
            objCodesmasterEntity.cmValue = Convert.ToInt32(dt.Rows[0]["CM_VALUE"]);
            objCodesmasterEntity.cmActiveYn = dt.Rows[0]["CM_ACTIVE_YN"].ToString();
            return Ok(objCodesmasterEntity);
        }

        [HttpPost]
        [Route("UpdateCodeMaster")]
        [Route("/CodeListing/UpdateCodeMaster")]
        public IActionResult UpdateCodeMaster(CodeMasterEntity model)
        {         
            int dt = objCodeMasterManager.UpdateCodesMaster(model);
            return Ok(dt);
        }

        [HttpPost]
        [Route("CheckUniqueness")]
        public IActionResult CheckUniqueness(CodeMasterEntity objCodesEntity)
        {          
            int dt = objCodeMasterManager.isValidateUnique(objCodesEntity);
            return Ok(dt);
        }


        [HttpPost]
        [Route("DeleteCode")]
        public IActionResult DeleteCode(CodeMasterEntity objcodeMasterEntity)
        {
            int dt = objCodesManager.DeleteCodesMaster(objcodeMasterEntity);
            return Ok(dt);
        }

    }
}
