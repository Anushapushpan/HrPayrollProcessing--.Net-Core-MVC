using BusinessLayer.Master;
using BusinessLayer.Transaction;
using DatabaseLayer;
using EntityLayer.Master;
using EntityLayer.Std;
using EntityLayer.Transaction;
using HrPayrollProcessingCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;
using HrPayrollProcessingCore.Filters;
namespace HrPayrollProcessingCore.Areas.Transaction.Controllers
{
    [Authorize]
    [Area("Transaction")]
    public class EmployeeController : Controller
    {
        //public IActionResult Employee()
        //{
        //    return View();
        //}
        private readonly IConfiguration _configuration;
        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        PrEmployeeEntity objEmpEntity = new PrEmployeeEntity();
        PrEmployeeHrManager objHrManager= new PrEmployeeHrManager();
        PrEmployeeHrEntity objHrEntity = new PrEmployeeHrEntity();
        [HttpGet]
       
        public async Task<IActionResult> Employee(string id1)
        {

            HttpResponseMessage response;
            string apiresponse = "";
            EmployeeViewModel objEmpViewModel = new EmployeeViewModel()
            {
                statusList = new List<DropdownSelect>(),
                deptList = new List<DeptDdlSelect>(),
                mgrList = new List<ManagerDdlSelect>(),
            };
            await getDropdowns(objEmpViewModel);           
            if (id1 != null)
            {
                objEmpViewModel.CurrentPage = "UP";
                PrEmployeeEntity objEmpEntity = new PrEmployeeEntity();
                objEmpEntity.empNo = id1;               
                using (var client = new HttpClient())
                {
                    string baseAddress = _configuration.GetValue<string>("BaseAddress");
                    client.BaseAddress = new Uri(baseAddress);
                    response = await client.PostAsJsonAsync("/api/EmployeeListApi/EditEmployee", objEmpEntity);
                    string result = await response.Content.ReadAsStringAsync();
                    objEmpEntity = JsonConvert.DeserializeObject<PrEmployeeEntity>(result);
                }
                if (response.IsSuccessStatusCode)
                {
                    objEmpViewModel.Employee = objEmpEntity;
                    TempData["empNo"] = objEmpViewModel.Employee.empNo;                  
                    return View(objEmpViewModel);
                }
            }
            
            objEmpViewModel.AllowancePage = "IN";
            objEmpViewModel.CurrentPage = "IN";
            return View(objEmpViewModel);                                           
        }
        [HttpPost]
        [Route("/Employee/CreateEmployee")]
        public async Task<IActionResult> CreateEmployee(EmployeeViewModel model)
        {
            PrEmployeeManager objPrEmpManager = new PrEmployeeManager();
            if (model != null)
            {

                model.statusList = new List<DropdownSelect>();
                model.deptList = new List<DeptDdlSelect>();
                model.mgrList = new List<ManagerDdlSelect>();
                
                await getDropdowns(model);
                if (model.CurrentPage == "IN")
                {
                    DateTime jd = model.Employee.empJoinDate;
                    string joinYear = jd.ToString("yy");

                    model.Employee.empNo = objPrEmpManager.GenerateEmpNo(joinYear);
                    model.Employee.empPwd = "";
                    model.Employee.empSalary = 0;
                    model.Employee.empCrDt = DateTime.Now;
                    model.Employee.empCrBy = HttpContext.Session.GetString("UserName");
                    HttpResponseMessage response;
                    using (var client = new HttpClient())
                    {
                        string baseAddress = _configuration.GetValue<string>("BaseAddress");
                        client.BaseAddress = new Uri(baseAddress);
                        response = await client.PostAsJsonAsync("/api/EmployeeListApi/SaveEmployee", model.Employee);
                        string result = await response.Content.ReadAsStringAsync();
                        model.Employee.empNo= result;
                    }
                    //model.Employee.empNo = model.Employee.empNo+objPrEmpManager.GetEmpNo();
                    TempData["message"] = "Saved Succesfully";
                    return RedirectToAction("Employee", new{ id1 = model.Employee.empNo });
                    
                }
                else if(model.CurrentPage=="UP")
                {                  
                    model.Employee.empUpDt = DateTime.Now;
                    model.Employee.empUpBy = HttpContext.Session.GetString("UserName");
                    HttpResponseMessage response;
                    using (var client = new HttpClient())
                    {
                        string baseAddress = _configuration.GetValue<string>("BaseAddress");
                        client.BaseAddress = new Uri(baseAddress);
                        response = await client.PostAsJsonAsync("/api/EmployeeListApi/UpdateEmployee", model.Employee);
                        string result = await response.Content.ReadAsStringAsync();
                    }
                    TempData["message"] = "Updated Succesfully";
                }
                
                //ViewBag.message = "Saved Successfully";
                return View("Employee", model);
            }           
            return View();           
        }

       
        [HttpGet]
        public async Task getDropdowns(EmployeeViewModel objEmpViewModel)
        {
            using (var client = new HttpClient())
            {
                string baseAddress = _configuration.GetValue<string>("BaseAddress");
                client.BaseAddress = new Uri(baseAddress);
                HttpResponseMessage response = await client.GetAsync("/api/Dropdown/EmpStatus/" + "EMP_STATUS");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    objEmpViewModel.statusList = JsonConvert.DeserializeObject<List<DropdownSelect>>(result);
                    objEmpViewModel.statusList.Insert(0, new DropdownSelect { Code = null, Text = "---SELECT ONE---" });
                }

                HttpResponseMessage response1 = await client.GetAsync("/api/Dropdown/Department");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response1.Content.ReadAsStringAsync();
                    objEmpViewModel.deptList = JsonConvert.DeserializeObject<List<DeptDdlSelect>>(result);
                    objEmpViewModel.deptList.Insert(0, new DeptDdlSelect { Number = null, Name = "---SELECT ONE---" });
                }

                HttpResponseMessage response2 = await client.GetAsync("/api/Dropdown/ManagerDropdown");
                if (response2.IsSuccessStatusCode)
                {
                    var result = await response2.Content.ReadAsStringAsync();
                    objEmpViewModel.mgrList = JsonConvert.DeserializeObject<List<ManagerDdlSelect>>(result);
                    objEmpViewModel.mgrList.Insert(0, new ManagerDdlSelect { empNo = null, Name = "---SELECT ONE---" });
                }

            }
        }


        [HttpGet]
        public async Task<IActionResult> Counts()
        {
            int empCount=0;
            int activeEmp = 0;
            int managerCount = 0;
            int activemanager = 0;
            string latestPayroll = "";
            int calenderDays = 0;
            string payrollEmpCount = "";
            double payrollCost = 0;
            string commaSeparated="";
            string news = "";
            HttpResponseMessage response1;
            HttpResponseMessage response2;
            HttpResponseMessage response3;
            HttpResponseMessage response4;
            HttpResponseMessage response5;
            HttpResponseMessage response6;
            HttpResponseMessage response7;
            HttpResponseMessage response8;
            HttpResponseMessage response9;
            using (var client = new HttpClient())
            {
                string baseAddress = _configuration.GetValue<string>("BaseAddress");
                client.BaseAddress = new Uri(baseAddress);
                response1 = await client.GetAsync("/api/EmployeeListApi/GetCountsofEmp");
                if (response1.IsSuccessStatusCode)
                {
                    string result = await response1.Content.ReadAsStringAsync();
                    empCount=Convert.ToInt32(result);
                }
                response2 = await client.GetAsync("/api/EmployeeListApi/GetActiveCounts");
                if (response2.IsSuccessStatusCode)
                {
                    string result = await response2.Content.ReadAsStringAsync();
                    activeEmp = Convert.ToInt32(result);
                }
                response3 = await client.GetAsync("/api/EmployeeListApi/GetManagerCount");
                if (response3.IsSuccessStatusCode)
                {
                    string result = await response3.Content.ReadAsStringAsync();
                    managerCount = Convert.ToInt32(result);
                }
                response4 = await client.GetAsync("/api/EmployeeListApi/GetActiveManagers");
                if (response4.IsSuccessStatusCode)
                {
                    string result = await response4.Content.ReadAsStringAsync();
                    activemanager = Convert.ToInt32(result);
                }
                response5 = await client.GetAsync("/api/EmployeeListApi/GetLatestPayrollMonth");
                if (response5.IsSuccessStatusCode)
                {
                    string result = await response5.Content.ReadAsStringAsync();
                    latestPayroll = result.ToString();
                }
                response6 = await client.GetAsync("/api/EmployeeListApi/GetCalenderDay");
                if (response6.IsSuccessStatusCode)
                {
                    string result = await response6.Content.ReadAsStringAsync();
                    calenderDays = Convert.ToInt32(result);
                }
                response7 = await client.GetAsync("/api/EmployeeListApi/PayrollEmpCount");
                if (response7.IsSuccessStatusCode)
                {
                    string result = await response7.Content.ReadAsStringAsync();
                    payrollEmpCount = result.ToString();
                }
                response8 = await client.GetAsync("/api/EmployeeListApi/PayrollCost");
                if (response8.IsSuccessStatusCode)
                {
                    string result = await response8.Content.ReadAsStringAsync();
                    payrollCost = Convert.ToDouble(result);
                    commaSeparated = payrollCost.ToString("#,000.00");
                }
                response9 = await client.GetAsync("/api/EmployeeListApi/ScrollingNews");
                if (response9.IsSuccessStatusCode)
                {
                    string result = await response9.Content.ReadAsStringAsync();
                    news=result.ToString();
                }
            }
            return Json(new { empCount, activeEmp, managerCount, activemanager,latestPayroll, calenderDays, payrollEmpCount, commaSeparated, news });
        }






    }
}
