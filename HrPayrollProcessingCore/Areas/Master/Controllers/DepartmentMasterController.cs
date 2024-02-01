using EntityLayer.Master;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HrPayrollProcessingCore.Filters;
namespace HrPayrollProcessingCore.Areas.Master.Controllers
{
    [Authorize]
    [Area("Master")]
    public class DepartmentMasterController : Controller
    {
        private readonly IConfiguration _configuration;
        DepartmentMasterEntity objDeptEntity = new DepartmentMasterEntity();
        DepartmentMasterViewModel objDeptViewModel= new DepartmentMasterViewModel();
        public DepartmentMasterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IActionResult> DepartmentMaster(string id1)
        {
            HttpResponseMessage response;
            string apiresponse = "";
            if (id1 != null)
            {
                objDeptViewModel.CurrentPage = "UP";
                objDeptEntity.deptNo = id1;    
                using (var client = new HttpClient())
                {
                    string baseAddress = _configuration.GetValue<string>("BaseAddress");
                    client.BaseAddress = new Uri(baseAddress);
                    response = await client.PostAsJsonAsync("/api/DepartmentListing/FetchDeptFromDb", objDeptEntity);
                    string result = await response.Content.ReadAsStringAsync();
                    objDeptEntity = JsonConvert.DeserializeObject<DepartmentMasterEntity>(result);
                }
                if (response.IsSuccessStatusCode)
                {
                    objDeptViewModel.DepartmentMasterEntity = objDeptEntity;
                    return View(objDeptViewModel);
                }
            }

            objDeptViewModel.CurrentPage = "IN";
            return View(objDeptViewModel);
        }

        [Route("/DepartmentMaster/SaveDepartment")]
        public async Task<IActionResult> SaveDepartment(DepartmentMasterViewModel model)
        {
            if (model != null)
            {
                if (model.CurrentPage == "IN")
                {
                    model.DepartmentMasterEntity.deptCrDt = DateTime.Now;
                    model.DepartmentMasterEntity.deptCrBy = HttpContext.Session.GetString("UserName");
                    HttpResponseMessage response;
                    using (var client = new HttpClient())
                    {
                        string baseAddress = _configuration.GetValue<string>("BaseAddress");
                        client.BaseAddress = new Uri(baseAddress);
                        response = await client.PostAsJsonAsync("/api/DepartmentListing/SaveDeptToDb", model.DepartmentMasterEntity);
                        string result = await response.Content.ReadAsStringAsync();
                    }
                    TempData["message"] = "Saved Succesfully";
                    model.CurrentPage = "UP";

                }
                else if (model.CurrentPage == "UP")
                {
                    model.DepartmentMasterEntity.deptUpDt = DateTime.Now;
                    model.DepartmentMasterEntity.deptUpBy = HttpContext.Session.GetString("UserName");
                    HttpResponseMessage response;
                    using (var client = new HttpClient())
                    {
                        string baseAddress = _configuration.GetValue<string>("BaseAddress");
                        client.BaseAddress = new Uri(baseAddress);
                        response = await client.PostAsJsonAsync("/api/DepartmentListing/UpdateDeptInDb", model.DepartmentMasterEntity);
                        string result = await response.Content.ReadAsStringAsync();
                    }
                    TempData["message"] = "Updated Succesfully";
                }
                return View("DepartmentMaster", model);
            }
            return View();
        }

        public async Task<IActionResult> DeleteDept(string id1)
        {
            HttpResponseMessage response;
            string apiresponse = "";
            if (id1 != null)
            {
                string result = "0";
                int d = 0;
                objDeptEntity.deptNo = id1;
                using (var client = new HttpClient())
                {
                    string baseAddress = _configuration.GetValue<string>("BaseAddress");
                    client.BaseAddress = new Uri(baseAddress);
                    response = await client.PostAsJsonAsync("/api/DepartmentListing/DeleteDept", objDeptEntity);
                    result = await response.Content.ReadAsStringAsync();
                    d = Convert.ToInt32(result);
                }
                if (d >= 1)
                {
                    TempData["message"] = "Deleted Successfully";
                    return Redirect("/Master/DepartmentListing/DepartmentListing");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CheckUnique(string deptno)
        {
            int res = 0;
            objDeptEntity.deptNo = deptno;
            objDeptViewModel.DepartmentMasterEntity = objDeptEntity;
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                string baseAddress = _configuration.GetValue<string>("BaseAddress");
                client.BaseAddress = new Uri(baseAddress);
                response = await client.PostAsJsonAsync("/api/DepartmentListing/CheckUniqueness", objDeptViewModel.DepartmentMasterEntity);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    res = Convert.ToInt32(result);
                }
            }
            return Json(new { res });
        }

        
    }
}
