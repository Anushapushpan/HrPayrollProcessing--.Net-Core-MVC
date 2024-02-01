using EntityLayer.Std;
using EntityLayer.Transaction;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http.Json;
using HrPayrollProcessingCore.Filters;
namespace HrPayrollProcessingCore.Areas.Transaction.Controllers
{
    [Authorize]
    [Area("Transaction")]
    public class HistoryController : Controller
    {
        private readonly IConfiguration _configuration;
        public HistoryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult History()
        {
            return View();
        }
    }
}
