using Microsoft.AspNetCore.Mvc;
using HrPayrollProcessingCore.Filters;
namespace HrPayrollProcessingCore.Areas.Master.Controllers
{
    [Authorize]
    [Area("Master")]
    public class ErrorListingController : Controller
    {
        public IActionResult ErrorListing()
        {
            return View();
        }
    }
}
