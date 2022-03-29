using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HRMS.Core.Entities.Master;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Employee;
using HRMS.UI.AuthenticateService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace HRMS.UI.Controllers.Common
{
    [CustomAuthenticate]
    public class CommonController : Controller
    {
        private readonly string APIURL = string.Empty;
        public CommonController(IConfiguration configuration)
        {
            APIURL = configuration.GetSection("APIURL").Value;
        }
        [HttpPost]
        public async Task<IActionResult> getEmployeeAutoComplete(string term)
        {
            try
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var responseTask = await client.GetAsync("api/HRMS/Common/getEmployeeAutoComplete?term=" + term);
                    if (responseTask.IsSuccessStatusCode)
                    {
                        var responseDetails = await responseTask.Content.ReadAsStringAsync();
                        var businessunits = JsonConvert.DeserializeObject<List<EmployeeAutoCompleteVM>>(responseDetails);
                        // var responseData = await responseTask.Content.ReadFromJsonAsync<EmployeeAutoCompleteVM>();
                        return Json(businessunits);
                    }
                    else
                    {
                        return Json(null);
                    }
                }

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CommonController)} action name {nameof(getEmployeeAutoComplete)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> getEmployeeInformantion(int Id)
        {
            try
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var responseTask = await client.GetAsync("api/HRMS/EmployeeMaster/EmployeeProfile?Id=" + Id);
                    if (responseTask.IsSuccessStatusCode)
                    {
                        var responseDetails = await responseTask.Content.ReadAsStringAsync();
                        var businessunits = JsonConvert.DeserializeObject<List<EmployeeDetail>>(responseDetails);
                        //var responseData = await responseTask.Content.ReadFromJsonAsync<EmployeeDetail>();
                        return PartialView(ViewHelper.GetViewPathDetails("Common", "_EmployeeInformation"), businessunits.FirstOrDefault());
                    }
                    else
                    {
                        return PartialView(ViewHelper.GetViewPathDetails("Common", "_EmployeeInformation"));
                    }
                }

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CommonController)} action name {nameof(getEmployeeAutoComplete)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCompanyHoliday()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var responseTask = await client.GetAsync("api/HRMS/HolidayAPI/GetCompanyHolidays");
                    if (responseTask.IsSuccessStatusCode)
                    {
                        var responseDetails = await responseTask.Content.ReadAsStringAsync();
                        var response = JsonConvert.DeserializeObject<List<CompanyHolidays>>(responseDetails);
                        return PartialView(ViewHelper.GetViewPathDetails("Common", "_CompanyHoliday"), response.ToList());
                    }
                    else
                    {
                        return PartialView(ViewHelper.GetViewPathDetails("Common", "_CompanyHoliday"));
                    }
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CommonController)} action name {nameof(GetCompanyHoliday)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
