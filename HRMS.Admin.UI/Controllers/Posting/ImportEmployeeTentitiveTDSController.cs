using HRMS.Core.Entities.Investment;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.Helpers.ExcelHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Posting
{
    public class ImportEmployeeTentitiveTDSController : Controller
    {
        private readonly IGenericRepository<EmployeeTentitiveTDS, int> _IEmployeeTentitiveTDSRepository;
        private readonly IHostingEnvironment _IhostingEnviroment;
        public ImportEmployeeTentitiveTDSController(IGenericRepository<EmployeeTentitiveTDS, int> employeetentitiveTDSRepository, IHostingEnvironment hostingEnvironment)
        {
            _IEmployeeTentitiveTDSRepository = employeetentitiveTDSRepository;
            _IhostingEnviroment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("ImportEmployeeTentitiveTDS", "_TentitiveTDSIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(ImportEmployeeTentitiveTDSController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> DownloadExcelFormat()
        {
            try
            {
                string sWebRootFolder = _IhostingEnviroment.WebRootPath;
                string sFileName = @"UploadTentitiveTDS.xlsx";
                string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                if (file.Exists)
                {
                    file.Delete();
                    file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                }

                ExcelPackage Eps = new ExcelPackage();
                ExcelWorksheet Sheets = Eps.Workbook.Worksheets.Add("TentitiveTDS");
                Sheets.Cells["A1:AF1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                Sheets.Cells["A1:AF1"].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

                Sheets.Cells["A1"].Value = "EmpCode";
                Sheets.Cells["B1"].Value = "DateMonth";
                Sheets.Cells["C1"].Value = "DateYear";
                Sheets.Cells["D1"].Value = "GrossSalary";
                Sheets.Cells["E1"].Value = "HRADeclared";
                Sheets.Cells["F1"].Value = "HRAExamption";
                Sheets.Cells["G1"].Value = "Sec80CExamption";
                Sheets.Cells["H1"].Value = "Sec80CCD1B";
                Sheets.Cells["I1"].Value = "Sec80CCD2";
                Sheets.Cells["J1"].Value = "Sec80D";
                Sheets.Cells["K1"].Value = "Sec80DD";
                Sheets.Cells["L1"].Value = "Sec80E";
                Sheets.Cells["M1"].Value = "Sec80EE";
                Sheets.Cells["N1"].Value = "Sec80EEB";
                Sheets.Cells["O1"].Value = "Sec80G";
                Sheets.Cells["P1"].Value = "Sec80GG";
                Sheets.Cells["Q1"].Value = "Sec80U";
                Sheets.Cells["R1"].Value = "Sec24";
                Sheets.Cells["S1"].Value = "Sec10";
                Sheets.Cells["T1"].Value = "Sec16";
                Sheets.Cells["U1"].Value = "PreviousEmployerSalary";
                Sheets.Cells["V1"].Value = "Age";
                Sheets.Cells["W1"].Value = "TotalExamptAmount";
                Sheets.Cells["X1"].Value = "TaxableAmount";
                Sheets.Cells["Y1"].Value = "StanderedDeduction";
                Sheets.Cells["Z1"].Value = "HECAmount";
                Sheets.Cells["AA1"].Value = "Surcharge";
                Sheets.Cells["AB1"].Value = "FinalTDSAmountYearly";
                Sheets.Cells["AC1"].Value = "FinalTDSAmountMonthly";
                Sheets.Cells["AD1"].Value = "PaidTax";
                Sheets.Cells["AE1"].Value = "RemainingTax";
                Sheets.Cells["AF1"].Value = "FinancialYear";



                var stream = new MemoryStream(Eps.GetAsByteArray());
                return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UploadCTCStructureController)} action name {nameof(DownloadExcelFormat)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ImportTentitiveTDS(UploadExcelVm model)
        {
            try
            {
                var response = new ReadTentitiveTDSExcelHelper().GetEmployeeTentitiveTDs(model.UploadFile);
                response.ForEach(x =>
                {
                    x.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                    x.CreatedDate = DateTime.Now;
                });
                var responsedetail = await _IEmployeeTentitiveTDSRepository.CreateEntities(response.ToArray());
                return Json(responsedetail.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(ImportEmployeeTentitiveTDSController)} action name {nameof(ImportTentitiveTDS)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

    }
}
