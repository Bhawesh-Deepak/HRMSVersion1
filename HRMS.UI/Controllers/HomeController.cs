using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Core.ReqRespVm.Response.Reporting;
using HRMS.UI.AuthenticateService;
using HRMS.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HRMS.UI.Controllers
{
    [CustomAuthenticate]
    public class HomeController : Controller
    {
        private readonly string APIURL = string.Empty;
        private readonly ILogger<HomeController> _logger;
        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            APIURL = configuration.GetSection("APIURL").Value;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> EmployeePayslip()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var responseTask = await client.GetAsync("api/HRMS/MonthlyEarningAPI/EmployeePaySlip?Id=" + Convert.ToInt32(HttpContext.Session.GetString("EmployeeId")));
                    if (responseTask.IsSuccessStatusCode)
                    {
                        var responseDetails = await responseTask.Content.ReadAsStringAsync();
                        var paySlips = JsonConvert.DeserializeObject<List<PaySlipVM>>(responseDetails);
                        return PartialView(ViewHelper.GetViewPathDetails("Home", "_EmployeePayslip"), paySlips);
                    }
                    else
                    {
                        return PartialView(ViewHelper.GetViewPathDetails("Home", "_EmployeePayslip"));
                    }
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(HomeController)} action name {nameof(EmployeePayslip)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> Form16()
        {
            await PopulateViewBag();
            return PartialView(ViewHelper.GetViewPathDetails("Home", "_Form16"));
        }
        public async Task<IActionResult> getEmployeeGrossAndPI(int FinancialYear)
        {
            try
            {
                List<GrossAndPIReportVM> grossAndPIReport = null;
                if (FinancialYear == 0)
                    FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId"));
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var responseTask = await client.GetAsync("/api/HRMS/DashBoardAPI/GetEmployeeGrossAndPerformanceInsentive?EmpCode=" + HttpContext.Session.GetString("EmpCode") + "&FinancialYear=" + FinancialYear);
                    if (responseTask.IsSuccessStatusCode)
                    {
                        var responseDetails = await responseTask.Content.ReadAsStringAsync();
                        grossAndPIReport = JsonConvert.DeserializeObject<List<GrossAndPIReportVM>>(responseDetails);
                        return Json(grossAndPIReport);
                    }
                    else
                    {
                        return Json(grossAndPIReport);
                    }
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(HomeController)} action name {nameof(getEmployeeGrossAndPI)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> DownloadEmployeePayslip(int DateMonth, int DateYear)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var responseTask = await client.GetAsync("api/HRMS/MonthlyEarningAPI/GetEmployeePaySlip?DateMonth=" + DateMonth + "&DateYear=" + DateYear + "&EmpCode=" + HttpContext.Session.GetString("EmpCode"));
                    if (responseTask.IsSuccessStatusCode)
                    {
                        var responseDetails = await responseTask.Content.ReadAsStringAsync();
                        var paySlips = JsonConvert.DeserializeObject<List<EmployeePaySlipVM>>(responseDetails);
                        if (paySlips.Count() > 0)
                        {
                            System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                            string strMonthName = mfi.GetMonthName(DateMonth).ToString();
                            var responsepdf = new Rotativa.AspNetCore.ViewAsPdf(ViewHelper.GetViewPathDetails("Home", "_Payslip"), paySlips, null)
                            {
                                FileName = strMonthName + "_" + DateYear + "_PaySlip.pdf",
                            };
                            return responsepdf;
                            //return PartialView(ViewHelper.GetViewPathDetails("Home", "_Payslip"), paySlips);
                            //return new ViewAsPdf(ViewHelper.GetViewPathDetails("Home", "_Payslip"), paySlips.OrderBy(x => x.ComponentId));
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                      

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(HomeController)} action name {nameof(DownloadEmployeePayslip)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        #region PrivateFields
        private async Task PopulateViewBag()
        {
            List<AssesmentYear> assesmentYear = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(APIURL);
                var responseTask = await client.GetAsync("api/HRMS/FinancialYearAPI/GetAllFinancialYear");
                if (responseTask.IsSuccessStatusCode)
                {
                    var responseDetails = await responseTask.Content.ReadAsStringAsync();
                    assesmentYear = JsonConvert.DeserializeObject<List<AssesmentYear>>(responseDetails);

                }
            }
            ViewBag.assesmentYearList = assesmentYear;
        }

        #endregion
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
