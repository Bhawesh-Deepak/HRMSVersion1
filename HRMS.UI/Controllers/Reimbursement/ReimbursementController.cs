using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Entities.Reimbursement;
using HRMS.Core.Helpers.BlobHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Reimbursement;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.UI.Controllers.Reimbursement
{
    public class ReimbursementController : Controller
    {
        private readonly string APIURL = string.Empty;
        private readonly IHostingEnvironment _IHostingEnviroment;
        public ReimbursementController(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            APIURL = configuration.GetSection("APIURL").Value;
            _IHostingEnviroment = hostingEnvironment;

        }

        public async Task<IActionResult> Index()
        {
            try
            {
                await PopulateViewBag();
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Reimbursement", "_CreateReimbursement")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(ReimbursementController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostCreate(EmployeeReimbursement model,IFormFile InvoiceFile)
        {
            try
            {
                model.EmpCode = HttpContext.Session.GetString("EmpCode");
                string host = HttpContext.Request.Host.Value;
                model.FilePath = "http://" + host + await new BlobHelper().UploadImageToFolder(InvoiceFile, _IHostingEnviroment);
                model.FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId"));
                using HttpClient client = new HttpClient { BaseAddress = new Uri(APIURL) };
                var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("/api/HRMS/Reimbursement/CreateReimbursement", stringContent);
                if (response.IsSuccessStatusCode)
                {
                }
                return RedirectToAction("Index", "Reimbursement");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Reimbursement)} action name {nameof(PostCreate)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> GetReimbursement()
        {
            try
            {
                List<EmployeeReimbursementVM> EmployeeReimbursement = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var responseTask = await client.GetAsync("/api/HRMS/Reimbursement/GetReimbursement?EmpCode=" + HttpContext.Session.GetString("EmpCode"));
                    if (responseTask.IsSuccessStatusCode)
                    {
                        var responseDetails = await responseTask.Content.ReadAsStringAsync();
                        EmployeeReimbursement = JsonConvert.DeserializeObject<List<EmployeeReimbursementVM>>(responseDetails);
                        return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Reimbursement", "_GetReimbursement"), EmployeeReimbursement));
                    }
                    else
                    {
                        return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Reimbursement", "_GetReimbursement"), EmployeeReimbursement));
                    }
                }

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(ReimbursementController)} action name {nameof(GetReimbursement)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        private async Task PopulateViewBag()
        {
            List<ReimbursementCategory> category = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(APIURL);
                var responseTask = await client.GetAsync("api/HRMS/ReimbursementCategory/GetReimbursementCategory");
                if (responseTask.IsSuccessStatusCode)
                {
                    var responseDetails = await responseTask.Content.ReadAsStringAsync();
                    category = JsonConvert.DeserializeObject<List<ReimbursementCategory>>(responseDetails);
                    ViewBag.CategoryList = category;
                }
                else
                {
                    ViewBag.CategoryList = category;
                }
            }

        }
    }
}
