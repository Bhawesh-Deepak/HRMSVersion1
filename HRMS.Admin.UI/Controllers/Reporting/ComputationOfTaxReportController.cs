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

namespace HRMS.Admin.UI.Controllers.Reporting
{
    public class ComputationOfTaxReportController : Controller
    {
        private readonly IGenericRepository<AssesmentYear, int> _IAssesmentYearRepository;
        private readonly IDapperRepository<ComputationOfTaxReportParams> _IComputationOfTaxReportRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;

        public ComputationOfTaxReportController(IGenericRepository<AssesmentYear, int> assesmentYearRepo,
            IDapperRepository<ComputationOfTaxReportParams> computationOfTaxreportRepository, IHostingEnvironment hostingEnvironment)
        {
            _IComputationOfTaxReportRepository = computationOfTaxreportRepository;
            _IAssesmentYearRepository = assesmentYearRepo;
            _IHostingEnviroment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                await PopulateViewBag();
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("ComputationOfTaxReport", "_ComputationOfTaxReport")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(ComputationOfTaxReportController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ExportComputationOfReport(ComputationOfTaxModel model, IFormFile UploadFile)
        {
            try
            {
                string empresponse = null;
                if (UploadFile != null)
                    empresponse = new ReadEmployeeCode().GetSalaryRegisterEmpCodeDetails(model.UploadFile);

                var request = new ComputationOfTaxReportParams()
                {
                    EmpCode = empresponse,
                    FinancialYear = model.FinancialYear
                };
                var response = (await Task.Run(() => _IComputationOfTaxReportRepository.GetAll<ComputationOfTaxReportVM>(SqlQuery.GetComputationofTaxReport, request))).ToList();


                var sWebRootFolder = _IHostingEnviroment.WebRootPath;
                var sFileName = @"ComputationOfTaxReport.xlsx";
                var URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                if (file.Exists)
                {
                    file.Delete();
                    file = new FileInfo(fileName: Path.Combine(sWebRootFolder, sFileName));
                }
                string[] cells = { "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S" };
                ExcelPackage Eps = new ExcelPackage();
                ExcelWorksheet Sheets = Eps.Workbook.Worksheets.Add("EmployeeInvestment");
                Sheets.Cells["A1:AV1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                Sheets.Cells["A1:AV1"].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                Sheets.Cells["A1"].Value = "EmployeeCode";
                Sheets.Cells["B1"].Value = "EmployeeName";
                Sheets.Cells["C1"].Value = "Pan No";
                Sheets.Cells["D1"].Value = "AssesmentYear";
                Sheets.Cells["E1"].Value = "CTC";
                int i = 0;
                foreach (var item in response.Where(X => X.EmployeeId == response.First().EmployeeId).OrderBy(x => x.ComponentId))
                {
                    Sheets.Cells[cells[i] + "1"].Value = item.ComponentName;
                    i++;
                }

                Sheets.Cells["T1"].Value = "Gross Salary (Prorated Base )";
                Sheets.Cells["U1"].Value = "Section 10 (13A) - HRA Exemption";
                Sheets.Cells["V1"].Value = "Section 10 Allowances (other than HRA)";
                Sheets.Cells["W1"].Value = "Total amount of exemption claimed under section 10";
                Sheets.Cells["X1"].Value = "Total amount of salary received	";
                Sheets.Cells["Y1"].Value = "(a) Standard Deduction - Section 16(ia)";
                Sheets.Cells["Z1"].Value = "(b) Entertainment allowance under section 16(ii)";
                Sheets.Cells["AA1"].Value = "(c) Professional Tax Section 16(iii)";
                Sheets.Cells["AB1"].Value = "Total deductions under Section 16";
                Sheets.Cells["AC1"].Value = "Income chargeable under the head 'Salaries' ";
                Sheets.Cells["AD1"].Value = "Section 24 - Interest on Housing Loan";
                Sheets.Cells["AE1"].Value = "Income under the head Other Sources offered for TDS";
                Sheets.Cells["AF1"].Value = "Gross Total Income";
                Sheets.Cells["AG1"].Value = "Section 80-C";
                Sheets.Cells["AH1"].Value = "Section 80-CCD (1B)";
                Sheets.Cells["AI1"].Value = "Section 80-CCD (2)";
                Sheets.Cells["AJ1"].Value = "Section 80-D";
                Sheets.Cells["AK1"].Value = "Section 80-DD";
                Sheets.Cells["AL1"].Value = "Section 80-E";
                Sheets.Cells["AM1"].Value = "Section 80-EE";
                Sheets.Cells["AN1"].Value = "Section 80 EEB";
                Sheets.Cells["AO1"].Value = "Section 80-G";
                Sheets.Cells["AP1"].Value = "Section 80-GG";
                Sheets.Cells["AQ1"].Value = "Section 80-U";
                Sheets.Cells["AR1"].Value = "Aggregate of deductible amount under Chapter VI-A";
                Sheets.Cells["AS1"].Value = "Total taxable income";
                Sheets.Cells["AT1"].Value = "Tax Payable";
                Sheets.Cells["AU1"].Value = "Tax Deducted from Salary";
                Sheets.Cells["AV1"].Value = "Net Tax Payable";
                int row = 2;
                foreach (var TaxGroup in response.GroupBy(x => x.EmployeeId))
                {
                    Sheets.Cells[string.Format("A{0}", row)].Value = TaxGroup.First().EmpCode;
                    Sheets.Cells[string.Format("B{0}", row)].Value = TaxGroup.First().EmployeeName;
                    Sheets.Cells[string.Format("C{0}", row)].Value = TaxGroup.First().PanCardNumber;
                    Sheets.Cells[string.Format("D{0}", row)].Value = TaxGroup.First().FinancialYear;
                    Sheets.Cells[string.Format("E{0}", row)].Value = TaxGroup.First().CTC;
                    int j = 0;
                    foreach (var item in TaxGroup.OrderBy(x => x.ComponentId))
                    {
                        Sheets.Cells[string.Format(cells[j] + "{0}", row)].Value = item.ComponentValue;
                        j++;
                    }
                    Sheets.Cells[string.Format("T{0}", row)].Value = TaxGroup.First().GrossSalary;
                    Sheets.Cells[string.Format("U{0}", row)].Value = TaxGroup.First().HRAExamption;
                    Sheets.Cells[string.Format("V{0}", row)].Value = TaxGroup.First().Sec10;
                    decimal sec10 = Convert.ToDecimal(TaxGroup.First().Sec10) + Convert.ToDecimal(TaxGroup.First().HRAExamption);
                    Sheets.Cells[string.Format("W{0}", row)].Value = sec10;
                    decimal salaryreceived = TaxGroup.First().GrossSalary - sec10;
                    Sheets.Cells[string.Format("X{0}", row)].Value = salaryreceived;
                    Sheets.Cells[string.Format("Y{0}", row)].Value = TaxGroup.First().StanderedDeduction;
                    Sheets.Cells[string.Format("Z{0}", row)].Value = 0;
                    Sheets.Cells[string.Format("AA{0}", row)].Value = TaxGroup.First().Sec16;
                    decimal deductionsunderSection16 = TaxGroup.First().StanderedDeduction + TaxGroup.First().Sec16;
                    Sheets.Cells[string.Format("AB{0}", row)].Value = deductionsunderSection16;
                    decimal Incomechargeable = salaryreceived - deductionsunderSection16;
                    Sheets.Cells[string.Format("AC{0}", row)].Value = Incomechargeable;
                    Sheets.Cells[string.Format("AD{0}", row)].Value = TaxGroup.First().Sec24;
                    Sheets.Cells[string.Format("AE{0}", row)].Value = 0;
                    decimal GrossTotalIncome = Incomechargeable - TaxGroup.First().Sec24;
                    Sheets.Cells[string.Format("AF{0}", row)].Value = GrossTotalIncome;
                    Sheets.Cells[string.Format("AG{0}", row)].Value = TaxGroup.First().Sec80CExamption;
                    Sheets.Cells[string.Format("AH{0}", row)].Value = TaxGroup.First().Sec80CCD1B;
                    Sheets.Cells[string.Format("AI{0}", row)].Value = TaxGroup.First().Sec80CCD2;
                    Sheets.Cells[string.Format("AJ{0}", row)].Value = TaxGroup.First().Sec80D;
                    Sheets.Cells[string.Format("AK{0}", row)].Value = TaxGroup.First().Sec80DD;
                    Sheets.Cells[string.Format("AL{0}", row)].Value = TaxGroup.First().Sec80E;
                    Sheets.Cells[string.Format("AM{0}", row)].Value = TaxGroup.First().Sec80EE;
                    Sheets.Cells[string.Format("AN{0}", row)].Value = TaxGroup.First().Sec80EEB;
                    Sheets.Cells[string.Format("AO{0}", row)].Value = TaxGroup.First().Sec80G;
                    Sheets.Cells[string.Format("AP{0}", row)].Value = TaxGroup.First().Sec80GG;
                    Sheets.Cells[string.Format("AQ{0}", row)].Value = TaxGroup.First().Sec80U;
                    decimal Aggregatedeductibleamount = TaxGroup.First().Sec80U + TaxGroup.First().Sec80GG + TaxGroup.First().Sec80G + TaxGroup.First().Sec80EEB + TaxGroup.First().Sec80EE + TaxGroup.First().Sec80E + TaxGroup.First().Sec80DD + TaxGroup.First().Sec80D + TaxGroup.First().Sec80CCD2 + TaxGroup.First().Sec80CCD1B + TaxGroup.First().Sec80CExamption;
                    Sheets.Cells[string.Format("AR{0}", row)].Value = Aggregatedeductibleamount;
                    decimal Totaltaxableincome = GrossTotalIncome - Aggregatedeductibleamount;
                    Sheets.Cells[string.Format("AS{0}", row)].Value = Totaltaxableincome;
                    Sheets.Cells[string.Format("AT{0}", row)].Value = TaxGroup.First().FinalTDSAmountYearly;
                    Sheets.Cells[string.Format("AU{0}", row)].Value = TaxGroup.First().PaidTax;
                    Sheets.Cells[string.Format("AV{0}", row)].Value = TaxGroup.First().RemainingTax;

                    row++;
                }
                var stream = new MemoryStream(Eps.GetAsByteArray());
                return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(ComputationOfTaxReportController)} action name {nameof(ExportComputationOfReport)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }
        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var assesmentYear = await _IAssesmentYearRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            if (assesmentYear.ResponseStatus == ResponseStatus.Success)

                ViewBag.AssesmentYearList = assesmentYear.Entities;

        }

        #endregion
    }
}
