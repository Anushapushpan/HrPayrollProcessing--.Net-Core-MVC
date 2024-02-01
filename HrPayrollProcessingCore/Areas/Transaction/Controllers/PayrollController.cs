using EntityLayer.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HrPayrollProcessingCore.Filters;
namespace HrPayrollProcessingCore.Areas.Transaction.Controllers
{
    [Authorize]
    [Area("Transaction")]
    public class PayrollController : Controller
    {
        private readonly IConfiguration _configuration;
        public PayrollController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Payroll(string id1)
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



        [HttpGet]
        public async Task<IActionResult> ProcessPayroll(string month, string year)
        {
            int res = 0;
            PayrollViewModel model = new PayrollViewModel();
            PrEmployeePayrollEntity objPayrollEntity = new PrEmployeePayrollEntity();
            objPayrollEntity.prYyyMm = year + month;
            objPayrollEntity.ehCrDt = DateTime.Now;
            objPayrollEntity.ehCrBy = HttpContext.Session.GetString("UserName");
            model.EmployeePayrollEntity = objPayrollEntity;
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                string baseAddress = _configuration.GetValue<string>("BaseAddress");
                client.BaseAddress = new Uri(baseAddress);
                response = await client.PostAsJsonAsync("/api/PayrollApi/PayrollProcess", model.EmployeePayrollEntity);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    res = Convert.ToInt32(result);
                }
            }
            return Json(new { res });
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

    }
}
