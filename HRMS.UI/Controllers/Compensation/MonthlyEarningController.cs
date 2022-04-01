using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Reporting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace HRMS.UI.Controllers.Compensation
{
    public class MonthlyEarningController : Controller
    {
        private readonly string APIURL = string.Empty;
        public MonthlyEarningController(IConfiguration configuration)
        {
            APIURL = configuration.GetSection("APIURL").Value;

        }
        public async Task<IActionResult> Index()
        {
            try
            {

                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("MonthlyEarning", "_CompensationIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(MonthlyEarningController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> EmployeeGrossVsNetSalary()
        {
            try
            {
                List<GrossVsNetSalaryVM> grossVsNet = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var responseTask = await client.GetAsync("api/HRMS/MonthlyEarningAPI/GetNetVsGrossSalary?EmpCode=" + HttpContext.Session.GetString("EmpCode"));
                    if (responseTask.IsSuccessStatusCode)
                    {
                        var responseDetails = await responseTask.Content.ReadAsStringAsync();
                        grossVsNet = JsonConvert.DeserializeObject<List<GrossVsNetSalaryVM>>(responseDetails);
                        return Json(grossVsNet);
                    }
                    else
                    {
                        return Json(grossVsNet);
                    }
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(MonthlyEarningController)} action name {nameof(EmployeeGrossVsNetSalary)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> GetEmployeeAttendaceStatus(int DateMonth, int DateYear)
        {
            try
            {
                if (DateMonth == 0 && DateYear == 0)
                    DateMonth = 6;
                DateYear = 2021;
                AttendanceStatusVM attendanceStatus = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var responseTask = await client.GetAsync("/api/HRMS/MonthlyEarningAPI/GetAttendanceStatus?DateMonth=" + DateMonth + "&DateYear=" + DateYear + "&EmpCode=" + HttpContext.Session.GetString("EmpCode"));
                    if (responseTask.IsSuccessStatusCode)
                    {
                        var responseDetails = await responseTask.Content.ReadAsStringAsync();
                        attendanceStatus = JsonConvert.DeserializeObject<AttendanceStatusVM>(responseDetails);
                        return PartialView(ViewHelper.GetViewPathDetails("MonthlyEarning", "_GetEmployeeAttendanceStatus"), attendanceStatus);
                    }
                    else
                    {
                        return PartialView(ViewHelper.GetViewPathDetails("MonthlyEarning", "_GetEmployeeAttendanceStatus"), attendanceStatus);
                    }
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(MonthlyEarningController)} action name {nameof(GetEmployeeAttendaceStatus)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        
    }
}
