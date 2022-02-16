using HRMS.Admin.UI.AuthenticateService;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.Helpers.ExcelHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Master
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class EmployeeAttendanceController : Controller
    {

        private readonly IGenericRepository<EmployeeAttendance, int> _IEmployeeAttendanceRepository;
        private readonly IDapperRepository<AttendanceParams> _IEmployeeDapperRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;
        public EmployeeAttendanceController(IGenericRepository<EmployeeAttendance, int> employeeAttendanceRepo, IHostingEnvironment hostingEnvironment
           , IDapperRepository<AttendanceParams> employeeDapperRepository)
        {
            _IEmployeeAttendanceRepository = employeeAttendanceRepo;
            _IEmployeeDapperRepository = employeeDapperRepository;
            _IHostingEnviroment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            try
            {
                return View(ViewHelper.GetViewPathDetails("EmployeeAttendance", "EmployeeAttendanceCreate"));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeAttendanceController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> DownloadExcelFormat()
        {
            try
            {
                string sWebRootFolder = _IHostingEnviroment.WebRootPath;
                string sFileName = @"Month_Attendance.xlsx";
                string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                if (file.Exists)
                {
                    file.Delete();
                    file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                }
                ExcelPackage Eps = new ExcelPackage();
                ExcelWorksheet Sheets = Eps.Workbook.Worksheets.Add("Attendance");
                Sheets.Cells["A1"].Value = "DateMonth";
                Sheets.Cells["B1"].Value = "DateYear";
                Sheets.Cells["C1"].Value = "EmpCode";
                Sheets.Cells["D1"].Value = "LopDays";

                var stream = new MemoryStream(Eps.GetAsByteArray());
                return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeAttendanceController)} action name {nameof(DownloadExcelFormat)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> DownloadExcelFormatBackData()
        {
            try
            {
                string sWebRootFolder = _IHostingEnviroment.WebRootPath;
                string sFileName = @"Month_AttendanceBackData.xlsx";
                string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                if (file.Exists)
                {
                    file.Delete();
                    file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                }
                ExcelPackage Eps = new ExcelPackage();
                ExcelWorksheet Sheets = Eps.Workbook.Worksheets.Add("Attendance");
                Sheets.Cells["A1"].Value = "DateMonth";
                Sheets.Cells["B1"].Value = "DateYear";
                Sheets.Cells["C1"].Value = "EmpCode";
                Sheets.Cells["D1"].Value = "LopDays";
                Sheets.Cells["E1"].Value = "PresentDays";
                Sheets.Cells["F1"].Value = "FinancialYear";
                var stream = new MemoryStream(Eps.GetAsByteArray());
                return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeAttendanceController)} action name {nameof(DownloadExcelFormatBackData)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UploadAttendance(UploadExcelVm model)
        {
            try
            {
                var response = new ReadAttendanceExcelHelper().GetAttendanceDetailsBackData(model.UploadFile);
                response.ToList().ForEach(item =>
                {
                    var model = new AttendanceParams()
                    {
                        MonthId = item.DateMonth,
                        YearId = item.DateYear,
                        EmpCode = item.EmployeeCode,
                        LopDays = item.LOPDays,
                        FinancialYear = item.FinancialYear
                    };

                    var uploadResponse = _IEmployeeDapperRepository
                    .Execute<AttendanceParams>(SqlQuery.UploadAttendance, model);
                });

                return Json($"Employee Attendance uploaded successfully !!");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeAttendanceController)} action name {nameof(UploadAttendance)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }
        [HttpPost]
        public async Task<IActionResult> UploadAttendanceBackData(UploadExcelVm model)
        {
            try
            {
                var response = new ReadAttendanceExcelHelper().GetAttendanceDetailsBackData(model.UploadFile1);
                var attendanceresponse = await _IEmployeeAttendanceRepository.CreateEntities(response.ToArray());
                return Json($"Employee Attendance uploaded successfully !!");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeAttendanceController)} action name {nameof(UploadAttendanceBackData)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }
    }
}
