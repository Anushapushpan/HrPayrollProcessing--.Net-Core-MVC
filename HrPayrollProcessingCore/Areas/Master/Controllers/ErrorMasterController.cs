using EntityLayer.Master;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HrPayrollProcessingCore.Filters;
namespace HrPayrollProcessingCore.Areas.Master.Controllers
{
    [Authorize]
    [Area("Master")]
    public class ErrorMasterController : Controller
    {

        private readonly IConfiguration _configuration;
        ErrorMasterViewModel objErrorViewModel=new ErrorMasterViewModel();
        ErrorCodeMasterEntity objErrorEntity=new ErrorCodeMasterEntity();
        public ErrorMasterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> ErrorMaster(string id1)
        {
            HttpResponseMessage response;
            string apiresponse = "";
            if (id1 != null)
            {
                objErrorViewModel.CurrentPage = "UP";
                objErrorEntity.errCode = id1;
                using (var client = new HttpClient())
                {
                    string baseAddress = _configuration.GetValue<string>("BaseAddress");
                    client.BaseAddress = new Uri(baseAddress);
                    response = await client.PostAsJsonAsync("/api/ErrorListing/FetchErrorMaster", objErrorEntity);
                    string result = await response.Content.ReadAsStringAsync();
                    objErrorEntity = JsonConvert.DeserializeObject<ErrorCodeMasterEntity>(result);
                }
                if (response.IsSuccessStatusCode)
                {
                    objErrorViewModel.ErrorCodeMasterEntity = objErrorEntity;
                    return View(objErrorViewModel);
                }
            }
            objErrorViewModel.CurrentPage = "IN";
            return View(objErrorViewModel);
        }

        [Route("/ErrorMaster/SaveErrorCode")]
        public async Task<IActionResult> SaveErrorCode(ErrorMasterViewModel model)
        {
            if (model != null)
            {
                if (model.CurrentPage == "IN")
                {
                    model.ErrorCodeMasterEntity.errCrDt = DateTime.Now;
                    model.ErrorCodeMasterEntity.errCrBy = HttpContext.Session.GetString("UserName");
                    HttpResponseMessage response;
                    using (var client = new HttpClient())
                    {
                        string baseAddress = _configuration.GetValue<string>("BaseAddress");
                        client.BaseAddress = new Uri(baseAddress);
                        response = await client.PostAsJsonAsync("/api/ErrorListing/SaveErrorToDb", model.ErrorCodeMasterEntity);
                        string result = await response.Content.ReadAsStringAsync();
                    }
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["message"] = "Saved Succesfully";
                        model.CurrentPage = "UP";
                    }                       

                }
                else if (model.CurrentPage == "UP")
                {
                    model.ErrorCodeMasterEntity.errUpDt = DateTime.Now;
                    model.ErrorCodeMasterEntity.errUpBy = HttpContext.Session.GetString("UserName");
                    HttpResponseMessage response;
                    using (var client = new HttpClient())
                    {
                        string baseAddress = _configuration.GetValue<string>("BaseAddress");
                        client.BaseAddress = new Uri(baseAddress);
                        response = await client.PostAsJsonAsync("/api/ErrorListing/UpdateErrorInDb", model.ErrorCodeMasterEntity);
                        string result = await response.Content.ReadAsStringAsync();
                    }
                    TempData["message"] = "Updated Succesfully";

                }
                return View("ErrorMaster", model);
            }
            return View();
        }

        public async Task<IActionResult> DeleteError(string id1)
        {
            HttpResponseMessage response;
            string apiresponse = "";
            if (id1 != null)
            {
                string result = "0";
                int d = 0;
                objErrorEntity.errCode = id1;
                using (var client = new HttpClient())
                {
                    string baseAddress = _configuration.GetValue<string>("BaseAddress");
                    client.BaseAddress = new Uri(baseAddress);
                    response = await client.PostAsJsonAsync("/api/ErrorListing/DeleteError", objErrorEntity);
                    result = await response.Content.ReadAsStringAsync();
                    d = Convert.ToInt32(result);
                }
                if (d >= 1)
                {
                    TempData["message"] = "Deleted Successfully";
                    return Redirect("/Master/ErrorListing/ErrorListing");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CheckUnique(string errcode)
        {
            int res = 0;
            objErrorEntity.errCode = errcode;
            objErrorViewModel.ErrorCodeMasterEntity = objErrorEntity;
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                string baseAddress = _configuration.GetValue<string>("BaseAddress");
                client.BaseAddress = new Uri(baseAddress);
                response = await client.PostAsJsonAsync("/api/ErrorListing/CheckUniqueness", objErrorViewModel.ErrorCodeMasterEntity);
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
