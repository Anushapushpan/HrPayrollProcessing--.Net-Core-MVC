using Microsoft.AspNetCore.Mvc;
using HrPayrollProcessingCore.Filters;
namespace HrPayrollProcessingCore.Areas.Master.Controllers
{
    [Authorize]
    [Area("Master")]
    public class UserListingController : Controller
    {
        public IActionResult UserListing()
        {
            return View();
        }
    }
}
