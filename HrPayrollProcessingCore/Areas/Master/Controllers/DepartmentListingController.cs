﻿using Microsoft.AspNetCore.Mvc;
using HrPayrollProcessingCore.Filters;
namespace HrPayrollProcessingCore.Areas.Master.Controllers
{
    [Authorize]
    [Area("Master")]
    public class DepartmentListingController : Controller
    {
        public IActionResult DepartmentListing()
        {
            return View();
        }
    }
}
