using BusinessLayer.Transaction;
using EntityLayer.Std;
using EntityLayer.Transaction;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HrPayrollProcessingCore.Filters;
namespace HrPayrollProcessingCore.Areas.Transaction.Controllers
{
    [Authorize]
    [Area("Transaction")]
    public class AddAllowanceController : Controller
    {
        private readonly IConfiguration _configuration;
        PrEmployeeHrEntity objHrEntity = new PrEmployeeHrEntity();
        AddAllowanceViewModel objAllowanceViewModel = new AddAllowanceViewModel();
        PrEmployeeHrManager objHrManager = new PrEmployeeHrManager();
        public AddAllowanceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> AddAllowance(string id1,string id2)
        {
            HttpResponseMessage response;
            string apiresponse = "";
            AddAllowanceViewModel objAllowanceView = new AddAllowanceViewModel()
            {
                DesignationList = new List<DropdownSelect>(),
                GradeList = new List<DropdownSelect>(),
            };          
            await getDropdowns(objAllowanceView);
            objHrEntity.ehEmpNo = id1;
            objAllowanceView.PrEmployeeHrEntity = objHrEntity;
            if (id2 != null)
            {
                objAllowanceView.CurrentAllowancePage = "UP";
                objHrEntity.ehEmpNo = id1;
                using (var client = new HttpClient())
                {
                    string baseAddress = _configuration.GetValue<string>("BaseAddress");
                    client.BaseAddress = new Uri(baseAddress);
                    response = await client.PostAsJsonAsync("/api/AllowanceApi/FetchAllowance", objHrEntity);
                    string result = await response.Content.ReadAsStringAsync();
                    objHrEntity = JsonConvert.DeserializeObject<PrEmployeeHrEntity>(result);
                }
                if (response.IsSuccessStatusCode)
                {
                    objAllowanceView.PrEmployeeHrEntity = objHrEntity;
                    return View(objAllowanceView);
                }
            }
            objAllowanceView.CurrentAllowancePage = "IN";
            return View(objAllowanceView);
        }

        

        [HttpPost]
        [Route("/AddAllowance/SaveAllowance")]
        public async Task<IActionResult> SaveAllowance(AddAllowanceViewModel model)
        {
            if (model != null)
            {
                model.DesignationList = new List<DropdownSelect>();
                model.GradeList = new List<DropdownSelect>();              
                await getDropdowns(model);
                if (model.CurrentAllowancePage == "IN")
                {                                      
                    model.PrEmployeeHrEntity.ehCrDt = DateTime.Now;
                    model.PrEmployeeHrEntity.ehCrBy = HttpContext.Session.GetString("UserName");
                    HttpResponseMessage response;
                    using (var client = new HttpClient())
                    {
                        string baseAddress = _configuration.GetValue<string>("BaseAddress");
                        client.BaseAddress = new Uri(baseAddress);
                        response = await client.PostAsJsonAsync("/api/AllowanceApi/InsertAllowance", model.PrEmployeeHrEntity);
                        string result = await response.Content.ReadAsStringAsync();
                        
                    }
                    return RedirectToAction("Employee", "Employee", new { id1 = model.PrEmployeeHrEntity.ehEmpNo });
                }              
                else if (model.CurrentAllowancePage == "UP")
                {
                    model.PrEmployeeHrEntity.ehUpDt = DateTime.Now;
                    model.PrEmployeeHrEntity.ehUpBy = HttpContext.Session.GetString("UserName");
                    HttpResponseMessage response;
                    using (var client = new HttpClient())
                    {
                        string baseAddress = _configuration.GetValue<string>("BaseAddress");
                        client.BaseAddress = new Uri(baseAddress);
                        response = await client.PostAsJsonAsync("/api/AllowanceApi/UpdateAllowance", model.PrEmployeeHrEntity);
                        string result = await response.Content.ReadAsStringAsync();
                    }
                    model.CurrentAllowancePage = "UP";
                    return RedirectToAction("Employee", "Employee", new { id1 = model.PrEmployeeHrEntity.ehEmpNo });
                }
                              
            }
            return View();
        }

        [HttpGet]
        public async Task getDropdowns(AddAllowanceViewModel objAllowanceView)
        {
            using (var client = new HttpClient())
            {
                string baseAddress = _configuration.GetValue<string>("BaseAddress");
                client.BaseAddress = new Uri(baseAddress);
                HttpResponseMessage response = await client.GetAsync("/api/Dropdown/Designation/" + "DESIGNATION");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    objAllowanceView.DesignationList = JsonConvert.DeserializeObject<List<DropdownSelect>>(result);
                    objAllowanceView.DesignationList.Insert(0, new DropdownSelect { Code = null, Text = "---SELECT ONE---" });
                }

                HttpResponseMessage response1 = await client.GetAsync("/api/Dropdown/Grade/" + "GRADE");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response1.Content.ReadAsStringAsync();
                    objAllowanceView.GradeList = JsonConvert.DeserializeObject<List<DropdownSelect>>(result);
                    objAllowanceView.GradeList.Insert(0, new DropdownSelect { Code = null, Text = "---SELECT ONE---" });
                }
            }
        }


        


    }
}
