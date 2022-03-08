using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Core.ReqRespVm.Response.Attendance;
using HRMS.Core.ReqRespVm.Response.Reporting;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Reporting
{
    public class AttendanceDayWiseReportController : Controller
    {
        private readonly IDapperRepository<AttendanceReportParams> _IEmployeeAttendanceRepository;
        private readonly IGenericRepository<AssesmentYear, int> _IAssesmentYearRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;
        public AttendanceDayWiseReportController(IDapperRepository<AttendanceReportParams> attendanceDapper, IHostingEnvironment hostingEnvironment, IGenericRepository<AssesmentYear, int> assesmentyearRepo)
        {
            _IHostingEnviroment = hostingEnvironment;
            _IEmployeeAttendanceRepository = attendanceDapper;
            _IAssesmentYearRepository = assesmentyearRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                await PopulateViewBag();
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("AttendanceDayWiseReport", "_AttendanceDayWiseIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AttendanceDayWiseReportController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DownloadAttendanceDayWiseReport(EmployeeSalaryRegisterVM model)
        {
            var sqlParams = new AttendanceReportParams()
            {
                DateMonthId = model.DateMonth,
                DateYearId = model.DateYear
            };
            var response = _IEmployeeAttendanceRepository.GetAll<AttendanceReportVm>(SqlQuery.GetEmployeeAttendanceDetails, sqlParams);
            var models = new List<AttendanceVm>();
            response.ForEach(data =>
            {
                var model = new AttendanceVm();
                model.EmployeeName = data.EmployeeName;
                model.EmployeeCode = data.EmpCode;
                model.EmployeeLevel = data.Level;
                model.Month = data.DateMonth;
                model.Year = data.DateYear;
                model.TotalDays = data.TotalDays;
                model.LOPDays = data.LOPDays;
                model.PresentDays = data.PresentDays;
                model.MonthsName = data.MonthsName;
                model.StartDate = data.StartDate;
                model.EndDate = data.EndDate;
                IDictionary<DateTime, string> attendanceDates = new Dictionary<DateTime, string>();

                for (int i = 1; i <= data.TotalDays; i++)
                {
                    if (i <= data.PresentDays)
                    {
                        attendanceDates.Add(data.StartDate.AddDays(i - 1), "P");
                    }
                    else
                    {
                        attendanceDates.Add(data.StartDate.AddDays(i - 1), "A");
                    }
                    model.DatWiseAttendance = attendanceDates;
                }

                models.Add(model);

            });
            var sWebRootFolder = _IHostingEnviroment.WebRootPath;
            var sFileName = @"AttendanceDayWiseReport.xlsx";
            var URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(fileName: Path.Combine(sWebRootFolder, sFileName));
            }
            ExcelPackage Eps = new ExcelPackage();
            ExcelWorksheet Sheets = Eps.Workbook.Worksheets.Add("Attendance");
            Sheets.Cells["A1"].Value = "Employee Code";
            Sheets.Cells["B1"].Value = "Employee Name";
            Sheets.Cells["C1"].Value = "Level";
            Sheets.Cells["D1"].Value = "Month";
            Sheets.Cells["E1"].Value = "Year";
            Sheets.Cells["F1"].Value = "Start Date";
            Sheets.Cells["G1"].Value = "End Date";
            Sheets.Cells["H1"].Value = "Total Days";
            Sheets.Cells["I1"].Value = "Present Days";
            Sheets.Cells["J1"].Value = "Lop Days";

            string[] cells = { "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ" };
            foreach (var data in models)
            {
                int headercell = 0;
                foreach (var item in data.DatWiseAttendance)
                {
                    Sheets.Cells[cells[headercell] + "1"].Style.Numberformat.Format = "DD MMMM yyyy";
                    Sheets.Cells[cells[headercell] + "1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    Sheets.Cells[cells[headercell] + "1"].AutoFitColumns();
                    Sheets.Cells[cells[headercell] + "1"].Value = Convert.ToDateTime(item.Key);
                    headercell++;
                }
                break;
            }
            int row = 2;
            int CELLCOUNT = 0;
            foreach (var data in models)
            {
                Sheets.Cells[string.Format("A{0}", row)].Value = data.EmployeeCode;
                Sheets.Cells[string.Format("B{0}", row)].Value = data.EmployeeName;
                Sheets.Cells[string.Format("C{0}", row)].Value = data.EmployeeLevel;
                Sheets.Cells[string.Format("D{0}", row)].Value = data.MonthsName;
                Sheets.Cells[string.Format("E{0}", row)].Value = data.Year;
                Sheets.Cells[string.Format("F{0}", row)].Value = data.StartDate.ToString("dd/MM/yyyy");
                Sheets.Cells[string.Format("G{0}", row)].Value = data.EndDate.ToString("dd/MM/yyyy");
                Sheets.Cells[string.Format("H{0}", row)].Value = data.TotalDays;
                Sheets.Cells[string.Format("I{0}", row)].Value = data.PresentDays;
                Sheets.Cells[string.Format("J{0}", row)].Value = data.LOPDays;
                  CELLCOUNT = 0;
                foreach (var item in data.DatWiseAttendance)
                {
                    Sheets.Cells[string.Format(cells[CELLCOUNT] + "{0}", row)].Value = item.Value;
                    CELLCOUNT++;
                }
                row++;
            }
            Sheets.Cells["A1:" + cells[CELLCOUNT - 1] + "1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            Sheets.Cells["A1:" + cells[CELLCOUNT - 1] + "1"].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

            var stream = new MemoryStream(Eps.GetAsByteArray());
            return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
            // return Json(models);

        }

        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var assesmentyearResponse = await _IAssesmentYearRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            if (assesmentyearResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.AssesmentYearList = assesmentyearResponse.Entities;

        }

        #endregion

    }
}
