using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Models;
using HRMS.Core.Entities.Master;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Reporting;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers
{
    [CustomAuthenticate]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenericRepository<AssesmentYear, int> _IAssesmentYearRepository;
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepository;
        private readonly IDapperRepository<AttendanceGraphParams> _IAttendanceGraphRepository;
        public HomeController(ILogger<HomeController> logger, IGenericRepository<AssesmentYear, int> assesmentYearRepository,
            IGenericRepository<EmployeeDetail, int> employeedetailRepository,
            IDapperRepository<AttendanceGraphParams> attendancegraphRepository)
        {
            _logger = logger;
            _IAssesmentYearRepository = assesmentYearRepository;
            _IEmployeeDetailRepository = employeedetailRepository;
            _IAttendanceGraphRepository = attendancegraphRepository;
        }

        public async Task<IActionResult> Index()
        {
            await PopulateViewBag();
            return View();
        }
        public async Task<IActionResult> GetBirthDayAnniversary(int Id)
        {
            try
            {

                if (Id == 1)
                {
                    var response = await _IEmployeeDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted &&
                    x.DateOfBirth.Value.Day == DateTime.Now.Day && x.DateOfBirth.Value.Month == DateTime.Now.Month);
                    return PartialView(ViewHelper.GetViewPathDetails("Home", "_BirthDayAndAnniversary"), response.Entities);
                }
                else
                {
                    var response = await _IEmployeeDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted &&
                     x.JoiningDate.Day == DateTime.Now.Day && x.JoiningDate.Month == DateTime.Now.Month);
                    return PartialView(ViewHelper.GetViewPathDetails("Home", "_BirthDayAndAnniversary"), response.Entities);
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(HomeController)} action name {nameof(GetBirthDayAnniversary)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAttendanceGraph(int FinancialYear)
        {
            if (FinancialYear == 0)
                FinancialYear = _IAssesmentYearRepository.GetAllEntities(x=>x.isCurrentFinancialYear==true).Id;
            else FinancialYear = 3;
            var attendanceParams = new AttendanceGraphParams()
            {
                FinancialYear = FinancialYear
            };

            var response = await Task.Run(() => _IAttendanceGraphRepository.GetAll<AttendanceGraphVM>(SqlQuery.GetAttendanceGraph, attendanceParams));
            return Json(response);
        }

        private async Task PopulateViewBag()
        {
            ViewBag.AssesmentYear = (await _IAssesmentYearRepository.GetAllEntities(x => x.IsActive == true && x.IsDeleted == false)).Entities;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
