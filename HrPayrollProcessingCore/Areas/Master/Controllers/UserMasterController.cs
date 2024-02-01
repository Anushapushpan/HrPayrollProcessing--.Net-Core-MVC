using BusinessLayer.Master;
using EntityLayer.Master;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HrPayrollProcessingCore.Filters;
namespace HrPayrollProcessingCore.Areas.Master.Controllers
{
    [Authorize]
    [Area("Master")]
    public class UserMasterController : Controller
    {
        UserMasterViewModel objusermasterview = new UserMasterViewModel();
        UserMasterEntity objUserEntity= new UserMasterEntity();
        private readonly IConfiguration _configuration;

        public UserMasterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
       
        
        public async Task<IActionResult> UserMaster(string id1)
        {
            HttpResponseMessage response;
            string apiresponse = "";
            
            if (id1 != null)
            {
                objusermasterview.CurrentPage = "UP";
                UserMasterEntity objUserMasterEntity = new UserMasterEntity();            
                objUserMasterEntity.userId= id1;
                using (var client = new HttpClient())
                {
                    string baseAddress = _configuration.GetValue<string>("BaseAddress");
                    client.BaseAddress = new Uri(baseAddress);
                    response = await client.PostAsJsonAsync("/api/UserApi/UserEditing", objUserMasterEntity);
                    string result = await response.Content.ReadAsStringAsync();
                    objUserMasterEntity = JsonConvert.DeserializeObject<UserMasterEntity>(result);
                }
                if (response.IsSuccessStatusCode)
                {                    
                    objusermasterview.User = objUserMasterEntity;
                    return View(objusermasterview);
                }              
            }

            objusermasterview.CurrentPage = "IN";
            return View(objusermasterview);
        }


        [Route("SaveUser")]
        public async Task<IActionResult> SaveUser(UserMasterViewModel model)
        {
            if (model != null)
            {
                if (model.CurrentPage == "IN")
                {
                    model.User.userCrDt = DateTime.Now;
                    model.User.userCrBy = HttpContext.Session.GetString("UserName");
                    HttpResponseMessage response;
                    using (var client = new HttpClient())
                    {
                        string baseAddress = _configuration.GetValue<string>("BaseAddress");
                        client.BaseAddress = new Uri(baseAddress);
                        response = await client.PostAsJsonAsync("/api/UserApi/SaveUser", model.User);
                        string result = await response.Content.ReadAsStringAsync();
                    }
                    TempData["message"] = "Saved Succesfully";
                    model.CurrentPage = "UP";

                }
                else if (model.CurrentPage == "UP")
                {
                    model.User.userUpDt = DateTime.Now;
                    model.User.userUpBy = HttpContext.Session.GetString("UserName");
                    HttpResponseMessage response;
                    using (var client = new HttpClient())
                    {
                        string baseAddress = _configuration.GetValue<string>("BaseAddress");
                        client.BaseAddress = new Uri(baseAddress);
                        response = await client.PostAsJsonAsync("/api/UserApi/UpdateUser", model.User);
                        string result = await response.Content.ReadAsStringAsync();
                    }
                    TempData["message"] = "Updated Succesfully";
                }
                return View("UserMaster", model);
            }
            return View();
        }

        [Route("/Master/UserMaster/DeleteUser/{id1}")]
        public async Task<IActionResult> DeleteUser(string id1)
        {
            HttpResponseMessage response;
            string apiresponse = "";
            if (id1 != null)
            {
                string result = "0";
                int d = 0;
                UserMasterEntity objusermasterentity = new UserMasterEntity();
                objusermasterentity.userId = id1;
                using (var client = new HttpClient())
                {
                    string baseAddress = _configuration.GetValue<string>("BaseAddress");
                    client.BaseAddress = new Uri(baseAddress);
                    response = await client.PostAsJsonAsync("/api/UserApi/DeleteUser", objusermasterentity);
                    result = await response.Content.ReadAsStringAsync();
                    d= Convert.ToInt32(result);
                }
                if (d >= 1)
                {
                    TempData["message"] = "Deleted Succesfully";
                    return Redirect("/Master/UserListing/UserListing");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CheckUnique(string userid)
        {
            int res = 0;
            objUserEntity.userId = userid;
            objusermasterview.User = objUserEntity;
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                string baseAddress = _configuration.GetValue<string>("BaseAddress");
                client.BaseAddress = new Uri(baseAddress);
                response = await client.PostAsJsonAsync("/api/UserApi/CheckUniqueness", objusermasterview.User);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    res = Convert.ToInt32(result);
                }
            }
            return Json(new { res });
        }


        [HttpGet]
        public async Task<IActionResult> CheckPassword(string currentPwd)
        {
            int res = 0;
            objUserEntity.userPassword = currentPwd;
            objUserEntity.userName= HttpContext.Session.GetString("UserName");
            objusermasterview.User = objUserEntity;
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                string baseAddress = _configuration.GetValue<string>("BaseAddress");
                client.BaseAddress = new Uri(baseAddress);
                response = await client.PostAsJsonAsync("/api/UserApi/CheckCurrentPwd", objusermasterview.User);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    res = Convert.ToInt32(result);
                }
            }
            return Json(new { res });
        }


        [HttpPost]
        public async Task<IActionResult> UpdateUserPassword(string newPwd, string currentPwd)
        {
            
            int res=0;
            objUserEntity.userPassword = currentPwd;
            objUserEntity.userName = HttpContext.Session.GetString("UserName");
           
            objUserEntity.userUpDt = DateTime.Now;
            objUserEntity.userUpBy = HttpContext.Session.GetString("UserName");
            
            HttpResponseMessage response;

            using (var client = new HttpClient())
            {
                string baseAddress = _configuration.GetValue<string>("BaseAddress");
                client.BaseAddress = new Uri(baseAddress);
                response = await client.PostAsJsonAsync("/api/UserApi/CheckCurrentPwd", objusermasterview.User);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    res = Convert.ToInt32(result);
                }
            }
            if(res==1)
            {
                objUserEntity.userPassword = newPwd;
                objUserEntity.userName = HttpContext.Session.GetString("UserName");

                objUserEntity.userUpDt = DateTime.Now;
                objUserEntity.userUpBy = HttpContext.Session.GetString("UserName");
                using (var client = new HttpClient())
                {
                    string baseAddress = _configuration.GetValue<string>("BaseAddress");
                    client.BaseAddress = new Uri(baseAddress);
                    response = await client.PostAsJsonAsync("/api/UserApi/UpdateUserPassword", objUserEntity);
                    string result = await response.Content.ReadAsStringAsync();
                    res = Convert.ToInt32(result);
                }
            }
            else
            {
                res = 2;
            }
            
            return Json(new { res });
        }
    }
}
