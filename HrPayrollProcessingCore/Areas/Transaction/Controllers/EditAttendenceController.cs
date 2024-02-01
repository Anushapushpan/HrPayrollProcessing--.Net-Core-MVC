using EntityLayer.Std;
using EntityLayer.Transaction;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using HrPayrollProcessingCore.Filters;
namespace HrPayrollProcessingCore
{
    [Authorize]
    [Area("Transaction")]
    public class EditAttendenceController : Controller
    {
        AttendenceViewModel attendencViewModel = new AttendenceViewModel();
        PrEmployeeAttendenceEntity objAttEntity= new PrEmployeeAttendenceEntity();
        private readonly IConfiguration _configuration;
        public EditAttendenceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IActionResult> EditAttendence(string id1,string id2,string id3,string id4)
        {
            if(id4 != "null")
            {
                attendencViewModel.EditAttPage = "UP";
                objAttEntity.attEmpNo = id1;
                objAttEntity.attYyyyMm = id3 + id2;
                objAttEntity.attUpDt = DateTime.Now;
                objAttEntity.attUpBy = HttpContext.Session.GetString("UserName");
                objAttEntity.attDaysPresent = DateTime.DaysInMonth(Convert.ToInt32(id3), Convert.ToInt32(id2));
                //HttpResponseMessage response;
                //using (var client = new HttpClient())
                //{
                //    string baseAddress = _configuration.GetValue<string>("BaseAddress");
                //    client.BaseAddress = new Uri(baseAddress);
                //    response = await client.PostAsJsonAsync("/api/AttendenceApi/FetchAttendenceDetails", objAttEntity);
                //    string result = await response.Content.ReadAsStringAsync();
                //    objAttEntity = JsonConvert.DeserializeObject<PrEmployeeAttendenceEntity>(result);
                //}
                attendencViewModel.Attendence = objAttEntity;
                return View(attendencViewModel);               
            }
            else
            {
                attendencViewModel.EditAttPage = "IN";
                objAttEntity.attEmpNo = id1;
                objAttEntity.attYyyyMm = id3 + id2;
                objAttEntity.attDaysPresent = DateTime.DaysInMonth(Convert.ToInt32(id3), Convert.ToInt32(id2));
                attendencViewModel.Attendence = objAttEntity;
            }
                  
            return View(attendencViewModel);
        }

        [HttpPost]
        [Route("SaveAttendence")]
        public async Task<IActionResult> SaveAttendence(AttendenceViewModel model)
        {
            if (model != null)
            {              
                if (model.EditAttPage == "IN")
                {
                    model.Attendence.attCrDt = DateTime.Now;
                    model.Attendence.attCrBy = HttpContext.Session.GetString("UserName");
                    HttpResponseMessage response;
                    using (var client = new HttpClient())
                    {
                        string baseAddress = _configuration.GetValue<string>("BaseAddress");
                        client.BaseAddress = new Uri(baseAddress);
                        response = await client.PostAsJsonAsync("/api/AttendenceApi/InsertAttendence", model.Attendence);
                        string result = await response.Content.ReadAsStringAsync();
                    }
                }
                else if (model.EditAttPage == "UP")
                {
                    model.Attendence.attUpDt = DateTime.Now;
                    model.Attendence.attUpBy = HttpContext.Session.GetString("UserName");
                    HttpResponseMessage response;
                    using (var client = new HttpClient())
                    {
                        string baseAddress = _configuration.GetValue<string>("BaseAddress");
                        client.BaseAddress = new Uri(baseAddress);
                        response = await client.PostAsJsonAsync("/api/AttendenceApi/UpdateAttendence", model.Attendence);
                        string result = await response.Content.ReadAsStringAsync();
                    }

                }
                model.EditAttPage = "UP";
                return RedirectToAction("Attendence", "Attendence", new { id1 = model.Attendence.attYyyyMm});

            }
            return View();
        }
    }
}
