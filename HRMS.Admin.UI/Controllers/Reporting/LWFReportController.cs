using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Core.ReqRespVm.Response.Reporting;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Admin.UI.AuthenticateService;

namespace HRMS.Admin.UI.Controllers.Reporting
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class LWFReportController : Controller
    {
        private readonly IGenericRepository<StateMaster, int> _IStateMasterRepository;
        private readonly IGenericRepository<AssesmentYear, int> _IAssesmentYearRepository;
        private readonly IDapperRepository<LWFReportParams> _ILWFReportRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;
        public LWFReportController(IGenericRepository<StateMaster, int> statemasterRepo,
            IGenericRepository<AssesmentYear, int> assesmentyearRepo, IHostingEnvironment hostingEnvironment,
             IDapperRepository<LWFReportParams> lwfreportRepository)
        {
            _IStateMasterRepository = statemasterRepo;
            _IAssesmentYearRepository = assesmentyearRepo;
            _ILWFReportRepository = lwfreportRepository;
            _IHostingEnviroment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                await PopulateViewBag();
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("LWFReport", "_LWFReportIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LWFReportController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DownloadLWFReport(EmployeeSalaryRegisterVM model)
        {
            try
            {
                System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                string strMonthName = mfi.GetMonthName(model.DateMonth).ToString();

                var request = new LWFReportParams()
                {
                    DateMonth = model.DateMonth,
                    DateYear = model.DateYear,
                    PTStateName = model.Name
                };
                var response = (await Task.Run(() => _ILWFReportRepository.GetAll<LWFReportVM>(SqlQuery.GetLWFReport, request))).ToList();
                var sWebRootFolder = _IHostingEnviroment.WebRootPath;
                var sFileName = @"LWFReport.xlsx";
                var URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                if (file.Exists)
                {
                    file.Delete();
                    file = new FileInfo(fileName: Path.Combine(sWebRootFolder, sFileName));
                }
                ExcelPackage Eps = new ExcelPackage();
                ExcelWorksheet Sheets = Eps.Workbook.Worksheets.Add("LWF");
                Sheets.Cells["A1:D1"].Merge = true;
                using (ExcelRange Rng = Sheets.Cells["A1:D1"])
                {
                    Rng.Value = "LWF Report for the Month of  " + strMonthName + "  " + model.DateYear;
                }
                Sheets.Cells["A2"].Value = "Employee Code";
                Sheets.Cells["B2"].Value = "Employee Name";
                Sheets.Cells["C2"].Value = "State";
                Sheets.Cells["D2"].Value = "LWF ";
                int row = 3;
                foreach (var data in response)
                {
                    Sheets.Cells[string.Format("A{0}", row)].Value = data.EmpCode;
                    Sheets.Cells[string.Format("B{0}", row)].Value = data.EmployeeName;
                    Sheets.Cells[string.Format("C{0}", row)].Value = data.PTStateName;
                    Sheets.Cells[string.Format("D{0}", row)].Value = data.SalaryAmount;
                    row++;
                }
                var stream = new MemoryStream(Eps.GetAsByteArray());
                return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LWFReportController)} action name {nameof(DownloadLWFReport)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }
        private async Task PopulateViewBag()
        {
            var stateResponse = await _IStateMasterRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var assesmentyearResponse = await _IAssesmentYearRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            if (stateResponse.ResponseStatus == ResponseStatus.Success && assesmentyearResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.StateList = stateResponse.Entities;
            ViewBag.AssesmentYearList = assesmentyearResponse.Entities;

        }
    }
}
