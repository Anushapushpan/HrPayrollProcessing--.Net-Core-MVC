using EntityLayer.Transaction;
using Microsoft.AspNetCore.Mvc;
using HrPayrollProcessingCore.Filters;
namespace HrPayrollProcessingCore.Areas.Transaction.Controllers
{
    [Authorize]
    [Area("Transaction")]
    public class EmployeeListController : Controller
    {
        private readonly IConfiguration _configuration;
        public EmployeeListController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult EmployeeList()
        {
            return View();
        }

        public async Task<IActionResult> DeleteEmployee(string id1)
        {
            HttpResponseMessage response;
            string apiresponse = "";
            if (id1 != null)
            {

                string result = "0";
                int d = 0;
                PrEmployeeEntity objEmpEntity = new PrEmployeeEntity();
                objEmpEntity.empNo = id1;
                using (var client = new HttpClient())
                {
                    string baseAddress = _configuration.GetValue<string>("BaseAddress");
                    client.BaseAddress = new Uri(baseAddress);
                    response = await client.PostAsJsonAsync("/api/EmployeeListApi/DeleteEmployee", objEmpEntity);
                    result = await response.Content.ReadAsStringAsync();
                    d = Convert.ToInt32(result);
                }
                if (d >= 1)
                {
                    //ViewBag.message = "Deleted Successfully";
                    return Redirect("/Transaction/EmployeeList/EmployeeList");
                }
            }
            return View();
        }
    }
}
