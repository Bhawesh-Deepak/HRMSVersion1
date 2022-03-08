using HRMS.Admin.UI.AuthenticateService;
using HRMS.Core.Helpers.CommonHelper;
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

namespace HRMS.Admin.UI.Controllers.HR
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class GratuityCalculationController : Controller
    {
        private readonly IDapperRepository<GratuityCalculationParams> _IGratuityCalculationParamsRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;
        public GratuityCalculationController(IDapperRepository<GratuityCalculationParams> gratuityCalculationParamsRepository, IHostingEnvironment hostingEnvironment)
        {
            _IGratuityCalculationParamsRepository = gratuityCalculationParamsRepository;
            _IHostingEnviroment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("GratuityCalculation", "_GratuityCalculationIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(GratuityCalculationController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> CalculateGratuity(string EmpCode,DateTime DateOfLeaving)
        {
            try
            {
                var GratuityParams = new GratuityCalculationParams() {
                    EmpCode = EmpCode,
                    DateOfLeaving = DateOfLeaving
                };
                var gratuityResponse = await Task.Run(() => _IGratuityCalculationParamsRepository.GetAll<GratuityCalculationVM>(SqlQuery.GetGratuityCalculation, GratuityParams));
                return PartialView(ViewHelper.GetViewPathDetails("GratuityCalculation", "GetGratuity"), gratuityResponse.First());

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(GratuityCalculationController)} action name {nameof(CalculateGratuity)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        
        public async Task<IActionResult> ExportGratuityCalculation(string EmpCode, DateTime DateOfLeaving)
        {

            var GratuityParams = new GratuityCalculationParams()
            {
                EmpCode = EmpCode,
                DateOfLeaving = DateOfLeaving
            };
            var gratuityResponse = await Task.Run(() => _IGratuityCalculationParamsRepository.GetAll<GratuityCalculationVM>(SqlQuery.GetGratuityCalculation, GratuityParams));
            var sWebRootFolder = _IHostingEnviroment.WebRootPath;
            var sFileName = @"GratuityCalculation.xlsx";
            var URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(fileName: Path.Combine(sWebRootFolder, sFileName));
            }
            ExcelPackage Eps = new ExcelPackage();
            ExcelWorksheet Sheets = Eps.Workbook.Worksheets.Add("Calculation");
            Sheets.Cells["A1"].Value = "Employee Code";
            Sheets.Cells["B1"].Value = "Employee Name";
            Sheets.Cells["C1"].Value = "Date of Joining";
            Sheets.Cells["D1"].Value = "Date Of Leaving";
            Sheets.Cells["E1"].Value = "Monthly Income (Basic + DA)";
            Sheets.Cells["F1"].Value = "Tenure";
            Sheets.Cells["G1"].Value = "Gratuity Amount";

            int row = 2;
            foreach (var data in gratuityResponse.ToList())
            {
                Sheets.Cells[string.Format("A{0}", row)].Value = data.EmpCode;
                Sheets.Cells[string.Format("B{0}", row)].Value = data.EmployeeName;
                Sheets.Cells[string.Format("C{0}", row)].Value = data.JoiningDate.ToString("dd/MM/yyyy");
                Sheets.Cells[string.Format("D{0}", row)].Value = data.DateOfLeaving.ToString("dd/MM/yyyy");
                Sheets.Cells[string.Format("E{0}", row)].Value = data.BasicAmount;
                Sheets.Cells[string.Format("F{0}", row)].Value = data.NoOfYear;
                Sheets.Cells[string.Format("G{0}", row)].Value = data.GratuityAmount;

                row++;
            }
            Sheets.Cells["A1:" + "G1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            Sheets.Cells["A1:" + "G1"].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

            var stream = new MemoryStream(Eps.GetAsByteArray());
            return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
        }

    }
}
