using EntityLayer.Master;
using EntityLayer.Transaction;
using HrPayrollProcessingCore.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HrPayrollProcessingCore.Filters;
namespace HrPayrollProcessingCore.Areas.Master.Controllers
{
    [Authorize]
    [Area("Master")]
    public class CodesMasterController : Controller
    {
        CodeMasterViewModel objCodesViewModel= new CodeMasterViewModel();
        CodeMasterEntity objCodesEntity= new CodeMasterEntity();
        private readonly IConfiguration _configuration;
        public CodesMasterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> CodesMaster(string id1,string id2)
        {
            HttpResponseMessage response;
            string apiresponse = "";
            CodeMasterViewModel objCodesMasterViewModel = new CodeMasterViewModel();
            if (id1 != null)
            {
                objCodesMasterViewModel.CurrentPage = "UP";
                CodeMasterEntity objCodesmasterEntity = new CodeMasterEntity();
                objCodesmasterEntity.cmCode = id1;
                objCodesmasterEntity.cmType = id2;
                using (var client = new HttpClient())
                {
                    string baseAddress = _configuration.GetValue<string>("BaseAddress");
                    client.BaseAddress = new Uri(baseAddress);
                    response = await client.PostAsJsonAsync("/api/CodeListing/CodeEditing", objCodesmasterEntity);
                    string result = await response.Content.ReadAsStringAsync();
                    objCodesmasterEntity = JsonConvert.DeserializeObject<CodeMasterEntity>(result);
                }
                if (response.IsSuccessStatusCode)
                {
                    objCodesMasterViewModel.CodeMasterEntity = objCodesmasterEntity;
                    return View(objCodesMasterViewModel);
                }
            }

            objCodesMasterViewModel.CurrentPage = "IN";
            return View(objCodesMasterViewModel);
        }

        [Route("/CodesMaster/SaveCode")]
        public async Task<IActionResult> SaveCode(CodeMasterViewModel model)
        {
            if (model != null)
            {           
                if(model.CurrentPage=="IN")
                {
                    model.CodeMasterEntity.cmCrDt = DateTime.Now;
                    model.CodeMasterEntity.cmCrBy = HttpContext.Session.GetString("UserName");
                    HttpResponseMessage response;
                    using (var client = new HttpClient())
                    {
                        string baseAddress = _configuration.GetValue<string>("BaseAddress");
                        client.BaseAddress = new Uri(baseAddress);
                        response = await client.PostAsJsonAsync("/api/CodeListing/SaveCodeMaster", model.CodeMasterEntity);
                        string result = await response.Content.ReadAsStringAsync();
                    }
                    TempData["message"] = "Saved Succesfully";
                    model.CurrentPage = "UP";

                }
                else if(model.CurrentPage=="UP")
                {
                    model.CodeMasterEntity.cmUpDt = DateTime.Now;
                    model.CodeMasterEntity.cmUpBy = HttpContext.Session.GetString("UserName");
                    HttpResponseMessage response;
                    using (var client = new HttpClient())
                    {
                        string baseAddress = _configuration.GetValue<string>("BaseAddress");
                        client.BaseAddress = new Uri(baseAddress);
                        response = await client.PostAsJsonAsync("/api/CodeListing/UpdateCodeMaster", model.CodeMasterEntity);
                        string result = await response.Content.ReadAsStringAsync();
                    }
                    TempData["message"] = "Updated Succesfully";               
                }          
                return View("CodesMaster",model);
            }
            return View();
        }

        
        public async Task<IActionResult> DeleteCode(string id1,string id2)
        {
            HttpResponseMessage response;
            string apiresponse = "";
            if (id1 != null)
            {
                string result = "0";
                int d = 0;
                CodeMasterEntity objCodeMasterEntity = new CodeMasterEntity();
                objCodeMasterEntity.cmCode = id1;
                objCodeMasterEntity.cmType = id2;
                using (var client = new HttpClient())
                {
                    string baseAddress = _configuration.GetValue<string>("BaseAddress");
                    client.BaseAddress = new Uri(baseAddress);
                    response = await client.PostAsJsonAsync("/api/CodeListing/DeleteCode", objCodeMasterEntity);
                    result = await response.Content.ReadAsStringAsync();
                    d = Convert.ToInt32(result);
                }
                if (d >= 1)
                {
                    TempData["message"] = "Deleted Successfully";
                    return Redirect("/Master/CodesListing/CodesListing");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CheckUnique(string cmCode, string cmType)
        {
            int res = 0;
            objCodesEntity.cmCode = cmCode;
            objCodesEntity.cmType = cmType;
            objCodesViewModel.CodeMasterEntity = objCodesEntity;
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                string baseAddress = _configuration.GetValue<string>("BaseAddress");
                client.BaseAddress = new Uri(baseAddress);
                response = await client.PostAsJsonAsync("/api/CodeListing/CheckUniqueness", objCodesViewModel.CodeMasterEntity);
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