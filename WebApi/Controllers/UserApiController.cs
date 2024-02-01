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
    public class UserApiController : ControllerBase
    {
        UserMasterManager objUserManager = new UserMasterManager();

        [HttpPost]
        [Route("ValidateLogin")]
        public ActionResult ValidateLogin(UserMasterEntity userMaster)
        {
            UserMasterManager objUserManager = new UserMasterManager();
            return Ok(objUserManager.isLogin(userMaster));
        }

        [HttpPost]
        [Route("UserListing")]
        public IActionResult UserListing()
        {
            UserMasterManager objUsrMaster = new UserMasterManager();
            DataTable dt = objUsrMaster.FetchUserMaster();
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
        [Route("UserEditing")]
        public IActionResult UserEditing(UserMasterEntity objUserMasterEntity)
        {
            
            DataTable dt = objUserManager.FetchUserMaster(objUserMasterEntity);

            UserMasterEntity objUserMasterEnt = new UserMasterEntity();
            objUserMasterEnt.userId = dt.Rows[0]["USER_ID"].ToString();
            objUserMasterEnt.userName = dt.Rows[0]["USER_NAME"].ToString();
            objUserMasterEnt.userPassword = dt.Rows[0]["USER_PASSWORD"].ToString();
            objUserMasterEnt.userActiveYn = dt.Rows[0]["USER_ACTIVE_YN"].ToString();
            return Ok(objUserMasterEnt);
        }

        [HttpPost]
        [Route("DeleteUser")]
        public IActionResult DeleteUser(UserMasterEntity objUserMasterEntity)
        {
            int dt = objUserManager.DeleteUser(objUserMasterEntity.userId);
            return Ok(dt);
        }

        [HttpPost]
        [Route("SaveUser")]     
        public IActionResult SaveUser(UserMasterEntity model)
        {
            int dt = objUserManager.IsInsertToUserMaster(model);
            return Ok(dt);
        }

        [HttpPost]
        [Route("UpdateUser")]
        public IActionResult UpdateUser(UserMasterEntity model)
        {
            int dt = objUserManager.UpdateUser(model);
            return Ok(dt);
        }

        [HttpPost]
        [Route("CheckUniqueness")]
        public IActionResult CheckUniqueness(UserMasterEntity userMasterEntity)
        {
            int dt = objUserManager.IsUnique(userMasterEntity);
            return Ok(dt);
        }

        [HttpPost]
        [Route("CheckCurrentPwd")]
        public IActionResult CheckCurrentPwd(UserMasterEntity userMasterEntity)
        {
            int dt = objUserManager.isPassword(userMasterEntity);
            return Ok(dt);
        }

        [HttpPost]
        [Route("UpdateUserPassword")]
        public IActionResult UpdateUserPassword(UserMasterEntity userMasterEntity)
        {
            int dt = objUserManager.ChangePwd(userMasterEntity);
            return Ok(dt);
        }

    }
}
