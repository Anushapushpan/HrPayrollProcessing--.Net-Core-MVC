
using HrPayrollProcessingCore.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HrPayrollProcessingCore.Areas.Master.Controllers
{
    [Authorize]
    [Area("Master")]
    public class CodesListingController : Controller
    {
        public IActionResult CodesListing()
        {
            return View();
        }
    }
}
