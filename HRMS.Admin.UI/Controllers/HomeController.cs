using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Models;
using HRMS.Core.Entities.Master;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Reporting;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenericRepository<AssesmentYear, int> _IAssesmentYearRepository;
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepository;
        private readonly IDapperRepository<AttendanceGraphParams> _IAttendanceGraphRepository;
        private readonly IDapperRepository<BirthdayAnniversaryParams> _IBirthdayAnniversaryRepository;
        private readonly IDapperRepository<GrossSalaryAndPIGraphParams> _IGrossSalaryAndPIGraphRepository;
        private readonly IDapperRepository<NoOfEmployeeWhomSalaryPaidParams> _INoOfEmployeeWhomSalaryPaidGraphRepository;
        private readonly IDapperRepository<NoOfEmployeeWhomIncentivePaidParams> _INoOfEmployeeWhomIncentivePaidGraphRepository;

        public HomeController(ILogger<HomeController> logger, IGenericRepository<AssesmentYear, int> assesmentYearRepository,
            IGenericRepository<EmployeeDetail, int> employeedetailRepository,
            IDapperRepository<AttendanceGraphParams> attendancegraphRepository,
             IDapperRepository<BirthdayAnniversaryParams> birthdayanniversaryRepository,
             IDapperRepository<GrossSalaryAndPIGraphParams> grossSalaryAndPIGraphRepository,
              IDapperRepository<NoOfEmployeeWhomSalaryPaidParams> noOfEmployeeWhomSalaryPaidRepository,
             IDapperRepository<NoOfEmployeeWhomIncentivePaidParams> noOfEmployeeWhomIncentivePaidRepository

            )
        {
            _logger = logger;
            _IAssesmentYearRepository = assesmentYearRepository;
            _IEmployeeDetailRepository = employeedetailRepository;
            _IAttendanceGraphRepository = attendancegraphRepository;
            _IBirthdayAnniversaryRepository = birthdayanniversaryRepository;
            _IGrossSalaryAndPIGraphRepository = grossSalaryAndPIGraphRepository;
            _INoOfEmployeeWhomSalaryPaidGraphRepository = noOfEmployeeWhomSalaryPaidRepository;
            _INoOfEmployeeWhomIncentivePaidGraphRepository = noOfEmployeeWhomIncentivePaidRepository;

        }

        public async Task<IActionResult> Index()
        {
            try
            {

                await PopulateViewBag();
                return View();
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(HomeController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> GetBirthDayAnniversary(int Id)
        {
            try
            {
                var birthdayparam = new BirthdayAnniversaryParams()
                {
                    Id = Id,
                    LegalEntity = Convert.ToString(HttpContext.Session.GetString("LegalEntityName"))
                };
                var response = await Task.Run(() => _IBirthdayAnniversaryRepository.GetAll<BirthdayAnniversaryVM>(SqlQuery.GetBirtdayAnniversary, birthdayparam));
                TempData["BirtdayParameter"] = Id;
                return PartialView(ViewHelper.GetViewPathDetails("Home", "_BirthDayAndAnniversary"), response);

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(HomeController)} action name {nameof(GetBirthDayAnniversary)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> GetAttendanceDayWiseReport()
        {
            try
            {
                await PopulateViewBag();
                return PartialView(ViewHelper.GetViewPathDetails("Home", "GetAttendanceDayWiseReport"));

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(HomeController)} action name {nameof(GetAttendanceDayWiseReport)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> GetPaidRegister()
        {
            try
            {
                await PopulateViewBag();
                return PartialView(ViewHelper.GetViewPathDetails("Home", "GetPaidRegister"));

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(HomeController)} action name {nameof(GetPaidRegister)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAttendanceGraph(int FinancialYear)
        {
            try
            {
                if (FinancialYear == 0)
                    FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId"));
                var attendanceParams = new AttendanceGraphParams()
                {
                    FinancialYear = FinancialYear,
                    LegalEntity = Convert.ToString(HttpContext.Session.GetString("LegalEntityName"))
                };
                var response = await Task.Run(() => _IAttendanceGraphRepository.GetAll<AttendanceGraphVM>(SqlQuery.GetAttendanceGraph, attendanceParams));
                return Json(response);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(HomeController)} action name {nameof(GetAttendanceGraph)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetGrossSalaryAndPIGraph(int FinancialYear)
        {
            try
            {
                if (FinancialYear == 0)
                    FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId"));
                var attendanceParams = new GrossSalaryAndPIGraphParams()
                {
                    FinancialYear = FinancialYear,
                    LegalEntity= Convert.ToString(HttpContext.Session.GetString("LegalEntityName"))
                };
                var response = await Task.Run(() => _IGrossSalaryAndPIGraphRepository.GetAll<GrossSalaryAndPIGraphVM>(SqlQuery.GetGrossSalaryandIncentiveReport, attendanceParams));
                return Json(response);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(HomeController)} action name {nameof(GetGrossSalaryAndPIGraph)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetNoOfEmployeeWhomSalaryPaidGraph(int FinancialYear)
        {
            try
            {
                if (FinancialYear == 0)
                    FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId"));
                var salaryPaidParams = new NoOfEmployeeWhomSalaryPaidParams()
                {
                    FinancialYear = FinancialYear,
                    LegalEntity = Convert.ToString(HttpContext.Session.GetString("LegalEntityName"))
                };
                var response = await Task.Run(() => _INoOfEmployeeWhomSalaryPaidGraphRepository.GetAll<NoOfEmployeeWhomSalaryPaidVM>(SqlQuery.GetNoOfEmployeeWhomSalaryPaid, salaryPaidParams));
                return Json(response);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(HomeController)} action name {nameof(GetNoOfEmployeeWhomSalaryPaidGraph)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetNoOfEmployeeWhomIncentivePaidGraph(int FinancialYear)
        {
            try
            {
                if (FinancialYear == 0)
                    FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId"));
                var incentiveGraphParams = new NoOfEmployeeWhomIncentivePaidParams()
                {
                    FinancialYear = FinancialYear,
                    LegalEntity = Convert.ToString(HttpContext.Session.GetString("LegalEntityName"))
                };
                var response = await Task.Run(() => _INoOfEmployeeWhomIncentivePaidGraphRepository.GetAll<NoOfEmployeeWhomIncentivePaidVM>(SqlQuery.GetNoOfEmployeeWhomIncentivePaid, incentiveGraphParams));
                return Json(response);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(HomeController)} action name {nameof(GetNoOfEmployeeWhomIncentivePaidGraph)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> SendBirtdayAnniversaryWises(int id)
        {
            try
            {

                return PartialView(ViewHelper.GetViewPathDetails("Home", "_SendBirtdayAnniversaryWises"));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(HomeController)} action name {nameof(SendBirtdayAnniversaryWises)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
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
        public IActionResult Privacy()
        {
            return new  ViewAsPdf("Privacy");
        }
    }
}
