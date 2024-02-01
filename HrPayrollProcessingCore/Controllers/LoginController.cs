using HrPayrollProcessingCore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using NuGet.Protocol;
using System.Security.Policy;

namespace HrPayrollProcessingCore.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _session = httpContextAccessor.HttpContext.Session;
        }

        public IActionResult Login()
        {
            HttpContext.Session.Clear();
            return View();
        }


        [HttpPost]
        public async Task<ViewResult> Login(UserViewModel UserData)
        {
            using (var client = new HttpClient())
            {
                string baseAddress = _configuration.GetValue<string>("BaseAddress");
                client.BaseAddress = new Uri(baseAddress);
                HttpResponseMessage response = await client.PostAsJsonAsync("/api/UserApi/ValidateLogin", UserData.usermaster);
                string apiresponse = await response.Content.ReadAsStringAsync();
                if (apiresponse == "true")
                {
                    
                    HttpContext.Session.SetString("UserName", UserData.usermaster.userName);      
                    _session.SetString("User", UserData.usermaster.userName);
                    return View("../Home/Index");
                }
                else
                    ViewBag.message = "Invalid Credentials";
                return View("../Login/Login");
            }


        }
        public IActionResult CodesMaster()
        {
            return Redirect("~/Master/CodesMaster/CodesMaster");
        }
        public IActionResult DepartmentMaster()
        {
            return Redirect("~/Master/DepartmentMaster/DepartmentMaster");
        }
        public IActionResult UserMaster()
        {
            return Redirect("~/Master/UserMaster/UserMaster");
        }
        public IActionResult ErrorMaster()
        {
            return Redirect("~/Master/ErrorMaster/ErrorMaster");
        }
        public IActionResult Employee()
        {
            return Redirect("~/Transaction/Employee/Employee");
        }
        public IActionResult AddAllowance()
        {
            return Redirect("~/Transaction/AddAllowance/AddAllowance");
        }
        public IActionResult Logout()
        {
            _session.Remove("User");
            _session.Clear();



            return Redirect("../Login/Login");
        }
    }
}