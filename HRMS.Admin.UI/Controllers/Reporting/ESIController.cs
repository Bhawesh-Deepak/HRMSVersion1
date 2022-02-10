using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.Helpers.ExcelHelper;
using HRMS.Core.ReqRespVm.RequestVm;
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
using System.Linq;
using System.Threading.Tasks;
using HRMS.Admin.UI.AuthenticateService;

namespace HRMS.Admin.UI.Controllers.Reporting
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ESIController : Controller
    {
        private readonly IHostingEnvironment _IHostingEnviroment;
        private readonly IDapperRepository<ECRParams> _IECRRepository;
        private readonly IGenericRepository<AssesmentYear, int> _IAssesmentYearRepository;
        public ESIController(IHostingEnvironment hostingEnvironment, IGenericRepository<AssesmentYear, int> assesmentyearRepo,
            IDapperRepository<ECRParams> ecrRepository)
        {
            _IHostingEnviroment = hostingEnvironment;
            _IECRRepository = ecrRepository;
            _IAssesmentYearRepository = assesmentyearRepo;

        }
        public async Task<IActionResult> Index()
        {
            try
            {
                await PopulateViewBag();
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("ESIReports", "_ESIReportIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(ESIController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ExportESICReportDetails(EmployeeSalaryRegisterVM model)
        {
            try
            {
                string empresponse = null;

            if (model.UploadFile != null)
                empresponse = new ReadEmployeeCode().GetSalaryRegisterEmpCodeDetails(model.UploadFile);

            var ecrParams = new ECRParams()
            {
                DateMonth = model.DateMonth,
                DateYear = model.DateYear,
                EmployeeCode = empresponse
            };

            var response = await Task.Run(() => _IECRRepository.GetAll<ESICReportVM>(SqlQuery.GetESICReport, ecrParams));

            CreateFileInRoot.CreateFileIfNotExistsOrDelete("ESICReport", _IHostingEnviroment,
                string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, "ESICReport.xlsx"));

            ExcelPackage Eps = new ExcelPackage();
            ExcelWorksheet Sheets = Eps.Workbook.Worksheets.Add("ESICReport");

            Sheets.Cells["A1:G1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            Sheets.Cells["A1:G1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);
            // Eps.Encryption.Password = "sqy" + model.DateMonth + "" + model.DateYear;
            Sheets.Cells["A1"].Value = "IP Number (10 Digits)";
            Sheets.Cells["B1"].Value = "IP Name (Only alphabets and space)";
            Sheets.Cells["C1"].Value = "No of Days for which wages paid / payable during the month";
            Sheets.Cells["D1"].Value = "Total Monthly Wages";
            Sheets.Cells["E1"].Value = "Reason Code for Zero workings days(numeric only; provide 0 for all other reasons - Click on the link for reference)";
            Sheets.Cells["F1"].Value = "Last Working Day (Format DD / MM / YYYY)";
            Sheets.Cells["G1"].Value = "ESI Amount";

            int row = 2;
            foreach (var data in response)
            {
                Sheets.Cells[string.Format("A{0}", row)].Value = data.ESICNew;
                Sheets.Cells[string.Format("B{0}", row)].Value = data.EmployeeName;
                Sheets.Cells[string.Format("C{0}", row)].Value = data.WorkingDays;
                Sheets.Cells[string.Format("D{0}", row)].Value = data.GrossAmount;
                Sheets.Cells[string.Format("E{0}", row)].Value = data.Reason;
                Sheets.Cells[string.Format("F{0}", row)].Value = data.ExitDate;
                Sheets.Cells[string.Format("G{0}", row)].Value = data.ESIAmount;
                row++;
            }
            var stream = new MemoryStream(Eps.GetAsByteArray());
            return File(stream.ToArray(), "application/vnd.ms-excel", "ESICReport.xlsx");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(ESIController)} action name {nameof(ExportESICReportDetails)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        private async Task PopulateViewBag()
        {
            var assesmentyearResponse = await _IAssesmentYearRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            if (assesmentyearResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.AssesmentYearList = assesmentyearResponse.Entities;

        }
    }
}
