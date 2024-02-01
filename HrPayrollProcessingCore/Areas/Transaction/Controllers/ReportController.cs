using EntityLayer.Std;
using EntityLayer.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Reflection;
using HrPayrollProcessingCore.Filters;
namespace HrPayrollProcessingCore
{
    [Authorize]
    [Area("Transaction")]
    public class ReportController : Controller
    {
        private readonly IConfiguration _configuration;
        public ReportController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IActionResult> Report(string id1)
        {
            if (id1 != null)
            {
                int year = int.Parse(id1.Substring(0, 4));
                int month = int.Parse(id1.Substring(4, 2));
                TempData["Months"] = GetMonths(month.ToString());
                TempData["Years"] = GetYears(year.ToString());
            }
            else
            {
                TempData["Months"] = GetMonths();
                TempData["Years"] = GetYears();
            }
            return View();
        }

        private List<SelectListItem> GetMonths()
        {
            List<SelectListItem> months = new List<SelectListItem>
            {
                new SelectListItem { Text = "January", Value = "01" },
                new SelectListItem { Text = "February", Value = "02" },
                new SelectListItem { Text = "March", Value = "03" },
                new SelectListItem { Text = "April", Value = "04" },
                new SelectListItem { Text = "May", Value = "05" },
                new SelectListItem { Text = "June", Value = "06" },
                new SelectListItem { Text = "July", Value = "07" },
                new SelectListItem { Text = "August", Value = "08" },
                new SelectListItem { Text = "September", Value = "09" },
                new SelectListItem { Text = "October", Value = "10" },
                new SelectListItem { Text = "November", Value = "11" },
                new SelectListItem { Text = "December", Value = "12" },
            };
            string currentMonth = DateTime.Now.ToString("MM");
            SelectListItem currentMonthItem = months.Find(x => x.Value == currentMonth.ToString());
            if (currentMonthItem != null)
            {
                currentMonthItem.Selected = true;
            }
            return months;
        }

        private List<SelectListItem> GetYears()
        {
            List<SelectListItem> years = new List<SelectListItem>
            {
                new SelectListItem { Text = "2021", Value = "2021" },
                new SelectListItem { Text = "2022", Value = "2022" },
                new SelectListItem { Text = "2023", Value = "2023" },
                new SelectListItem { Text = "2024", Value = "2024" },
                new SelectListItem { Text = "2025", Value = "2025" },
            };
            string currentYear = DateTime.Now.Year.ToString();
            SelectListItem currentYearItem = years.Find(x => x.Value == currentYear.ToString());
            if (currentYearItem != null)
            {
                currentYearItem.Selected = true;
            }
            return years;
        }

        private List<SelectListItem> GetMonths(string month)
        {
            List<SelectListItem> months = new List<SelectListItem>
            {
                new SelectListItem { Text = "January", Value = "01" },
                new SelectListItem { Text = "February", Value = "02" },
                new SelectListItem { Text = "March", Value = "03" },
                new SelectListItem { Text = "April", Value = "04" },
                new SelectListItem { Text = "May", Value = "05" },
                new SelectListItem { Text = "June", Value = "06" },
                new SelectListItem { Text = "July", Value = "07" },
                new SelectListItem { Text = "August", Value = "08" },
                new SelectListItem { Text = "September", Value = "09" },
                new SelectListItem { Text = "October", Value = "10" },
                new SelectListItem { Text = "November", Value = "11" },
                new SelectListItem { Text = "December", Value = "12" },
            };
            string currentMonth = month;
            SelectListItem currentMonthItem = months.Find(x => x.Value == currentMonth.ToString());
            if (currentMonthItem != null)
            {
                currentMonthItem.Selected = true;
            }
            return months;
        }

        private List<SelectListItem> GetYears(string year)
        {
            List<SelectListItem> years = new List<SelectListItem>
            {
                new SelectListItem { Text = "2021", Value = "2021" },
                new SelectListItem { Text = "2022", Value = "2022" },
                new SelectListItem { Text = "2023", Value = "2023" },
                new SelectListItem { Text = "2024", Value = "2024" },
                new SelectListItem { Text = "2025", Value = "2025" },
            };
            string currentYear = year;
            SelectListItem currentYearItem = years.Find(x => x.Value == currentYear.ToString());
            if (currentYearItem != null)
            {
                currentYearItem.Selected = true;
            }
            return years;
        }

        [HttpGet]
        public async Task<IActionResult> getDropdowns(string month, string year)
        {

            AttendenceViewModel objAttViewModel = new AttendenceViewModel();
            string yyyymm = year + month;
            List<DdlEmpNo> data = new List<DdlEmpNo>();
            using (var client = new HttpClient())
            {
                string baseAddress = _configuration.GetValue<string>("BaseAddress");
                client.BaseAddress = new Uri(baseAddress);
                HttpResponseMessage response = await client.GetAsync("/api/Dropdown/EmpNo/" + yyyymm);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<List<DdlEmpNo>>(result);
                }

            }
            return Json(data);
        }

        [HttpPost]
        [Route("/Transaction/Report/DownloadReport")]
        public async Task<IActionResult> DownloadReport(string month, string year, string empno)
        {
            int res=0;
            AttendenceViewModel model = new AttendenceViewModel();
            PrEmployeeAttendenceEntity objAttEntity= new PrEmployeeAttendenceEntity();
            objAttEntity.attYyyyMm = year + month;
            objAttEntity.attEmpNo= empno;
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                string baseAddress = _configuration.GetValue<string>("BaseAddress");
                client.BaseAddress = new Uri(baseAddress);
                response = await client.PostAsJsonAsync("/api/AttendenceApi/IsPayrollProcessed", model.Attendence);
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

