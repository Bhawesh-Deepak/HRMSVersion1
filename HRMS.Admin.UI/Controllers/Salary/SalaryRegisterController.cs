using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.Helpers.ExcelHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Core.ReqRespVm.Response.Salary;
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
namespace HRMS.Admin.UI.Controllers.Salary
{
    public class SalaryRegisterController : Controller
    {
        private readonly IDapperRepository<SalaryRegisterParams> _ISalaryRegisterParamsRepository;
        private readonly IDapperRepository<SalaryRegisterByEmployeeCodeParams> _ISalaryRegisterByEmployeeCodeParamsRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;
        public SalaryRegisterController(IDapperRepository<SalaryRegisterParams> SalaryRegisterParamsRepository,
            IDapperRepository<SalaryRegisterByEmployeeCodeParams> SalaryRegisterByEmployeeCodeParamsRepository, IHostingEnvironment hostingEnvironment)
        {
            _IHostingEnviroment = hostingEnvironment;
            _ISalaryRegisterParamsRepository = SalaryRegisterParamsRepository;
            _ISalaryRegisterByEmployeeCodeParamsRepository = SalaryRegisterByEmployeeCodeParamsRepository;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("SalaryRegister", "_SalaryRegister")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(SalaryRegisterController)} action name {nameof(Index)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ExportSalaryRegister(EmployeeSalaryRegisterVM model)
        {
           
                List<SalaryRegisterVM> response = null;
                if (model.UploadFile == null)
                {
                    var request = new SalaryRegisterParams()
                    {
                        DateMonth = model.DateMonth,
                        DateYear = model.DateYear,
                    };
                    response = (await Task.Run(() => _ISalaryRegisterParamsRepository.GetAll<SalaryRegisterVM>(SqlQuery.GetEmployeeSalary, request))).ToList();
                }
                else
                {
                    var empresponse = new ReadEmployeeCode().GetSalaryRegisterEmpCodeDetails(model.UploadFile);
                    var request = new SalaryRegisterByEmployeeCodeParams()
                    {
                        DateMonth = model.DateMonth,
                        DateYear = model.DateYear,
                        EmployeeCode = empresponse
                    };
                    response = (await Task.Run(() => _ISalaryRegisterByEmployeeCodeParamsRepository.GetAll<SalaryRegisterVM>(SqlQuery.GetEmployeeSalaryByCode, request))).ToList();
                }
                
                string sWebRootFolder = _IHostingEnviroment.WebRootPath;
                string sFileName = @"EmployeeSalaryRegister.xlsx";
                string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                if (file.Exists)
                {
                    file.Delete();
                    file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                }
                ExcelPackage Eps = new ExcelPackage();
                ExcelWorksheet Sheets = Eps.Workbook.Worksheets.Add("EmployeeSalary");
                Sheets.View.FreezePanes(1, 4);
                Sheets.Cells["A1:DB1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                Sheets.Cells["A1:DB1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                Eps.Encryption.Password = "sqy" + model.DateMonth + "" + model.DateYear;
                Sheets.Cells["A1"].Value = "Month";
                Sheets.Cells["B1"].Value = "Year";
                Sheets.Cells["C1"].Value = "Employee_Code";
                Sheets.Cells["D1"].Value = "Employee_Name";
                Sheets.Cells["E1"].Value = "AccountName";
                Sheets.Cells["F1"].Value = "Status_Description";
                Sheets.Cells["G1"].Value = "Date_of_Birth";
                Sheets.Cells["H1"].Value = "Date_Of_Joining";
                Sheets.Cells["I1"].Value = "Date_of_Confirmation";
                Sheets.Cells["J1"].Value = "Date_Of_Leaving";
                Sheets.Cells["K1"].Value = "Band";
                Sheets.Cells["L1"].Value = "BioMetricCode";
                Sheets.Cells["M1"].Value = "Branch";
                Sheets.Cells["N1"].Value = "Department";
                Sheets.Cells["O1"].Value = "Designation ";
                Sheets.Cells["P1"].Value = "Function";
                Sheets.Cells["Q1"].Value = "LegalEntity";
                Sheets.Cells["R1"].Value = "PandLName";
                Sheets.Cells["S1"].Value = "SubDepartment";
                Sheets.Cells["T1"].Value = "Zone";
                Sheets.Cells["U1"].Value = "PT_StateName";
                Sheets.Cells["V1"].Value = "UAN_No";
                Sheets.Cells["W1"].Value = "EFD_PFAcctNo";
                Sheets.Cells["X1"].Value = "EFD_ESICAcctNo";
                Sheets.Cells["Y1"].Value = "PAN";
                Sheets.Cells["Z1"].Value = "adhaarNo";
                Sheets.Cells["AA1"].Value = "IFSCode";
                Sheets.Cells["AB1"].Value = "Bank_name";
                Sheets.Cells["AC1"].Value = "Bank_Account_Number";
                Sheets.Cells["AD1"].Value = "Branch_name";
                Sheets.Cells["AE1"].Value = "Days_Worked";
                Sheets.Cells["AF1"].Value = "Arrears_Days";
                Sheets.Cells["AG1"].Value = "LOP";
                Sheets.Cells["AH1"].Value = "LFDAYS";
                Sheets.Cells["AI1"].Value = "OTHRS";

                string[] CellArray = { "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ", "DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO" };
                int A = 0;
                foreach (var item in response.Where(x => x.Id == response.First().Id && x.ComponentType == 1).OrderBy(x => x.ComponentId))
                {
                    Sheets.Cells[CellArray[A] + "1"].Value = item.ComponentName;
                    A++;
                }
                Sheets.Cells["BZ1"].Value = "Gross Salary";
                int BBB = 1;

                foreach (var itemMMM in response.Where(x => x.Id == response.First().Id && x.ComponentType == 2).OrderBy(x => x.ComponentId))
                {
                    Sheets.Cells[CellArray[42 + BBB] + "1"].Value = itemMMM.ComponentName;
                    BBB++;
                }
                //Sheets.Cells["CY1"].Value = "PI_Tax";
                Sheets.Cells["DC1"].Value = "Total Deduction";
                Sheets.Cells["DD1"].Value = "Net Salary";
                int row = 2;
                foreach (var item in response.GroupBy(x => x.Id))
                {
                    Sheets.Cells[string.Format("A{0}", row)].Value = item.First().Months;
                    Sheets.Cells[string.Format("B{0}", row)].Value = item.First().Years;
                    Sheets.Cells[string.Format("C{0}", row)].Value = item.First().empCode;
                    Sheets.Cells[string.Format("D{0}", row)].Value = item.First().employeeName;
                    Sheets.Cells[string.Format("E{0}", row)].Value = item.First().employeeName;
                    Sheets.Cells[string.Format("F{0}", row)].Value = item.First().Status_Description;
                    Sheets.Cells[string.Format("G{0}", row)].Value = string.Format("{0:dd/MM/yyyy}", item.First().dateOfBirth);
                    Sheets.Cells[string.Format("H{0}", row)].Value = string.Format("{0:dd/MM/yyyy}", item.First().joiningDate);
                    Sheets.Cells[string.Format("I{0}", row)].Value = string.Format("{0:dd/MM/yyyy}", item.First().confirmationDate);
                    Sheets.Cells[string.Format("J{0}", row)].Value = item.First().exitDate;
                    Sheets.Cells[string.Format("K{0}", row)].Value = item.First().Band;
                    Sheets.Cells[string.Format("L{0}", row)].Value = item.First().BiometricCode;
                    Sheets.Cells[string.Format("M{0}", row)].Value = item.First().BranchOfficeId;
                    Sheets.Cells[string.Format("N{0}", row)].Value = item.First().departmentName;
                    Sheets.Cells[string.Format("O{0}", row)].Value = item.First().DesignationName;
                    Sheets.Cells[string.Format("P{0}", row)].Value = item.First().Functions;
                    Sheets.Cells[string.Format("Q{0}", row)].Value = item.First().LegalEntity;
                    //Sheets.Cells[string.Format("R{0}", row)].Value = item.First().BranchName;
                    // Sheets.Cells[string.Format("S{0}", row)].Value = item.First().PermanentLocation;
                    Sheets.Cells[string.Format("R{0}", row)].Value = item.First().PAndLHeadName;
                    Sheets.Cells[string.Format("S{0}", row)].Value = item.First().SubDepartment;
                    Sheets.Cells[string.Format("T{0}", row)].Value = item.First().Zone;
                    Sheets.Cells[string.Format("U{0}", row)].Value = item.First().PTStateName;
                    Sheets.Cells[string.Format("V{0}", row)].Value = item.First().UANNumber;
                    Sheets.Cells[string.Format("W{0}", row)].Value = item.First().PAndFBankAccountNumberx;
                    Sheets.Cells[string.Format("X{0}", row)].Value = item.First().esicNo;
                    Sheets.Cells[string.Format("Y{0}", row)].Value = item.First().PanCardNumber;
                    Sheets.Cells[string.Format("Z{0}", row)].Value = item.First().AadharCardNumber;
                    Sheets.Cells[string.Format("AA{0}", row)].Value = item.First().ifscCode;
                    Sheets.Cells[string.Format("AB{0}", row)].Value = item.First().bankName;
                    Sheets.Cells[string.Format("AC{0}", row)].Value = item.First().bankAccountNumber;
                    Sheets.Cells[string.Format("AD{0}", row)].Value = "";// item.First().ban;
                    Sheets.Cells[string.Format("AE{0}", row)].Value = item.First().WorkingDays;
                    Sheets.Cells[string.Format("AF{0}", row)].Value = item.First().ArrearDays;
                    Sheets.Cells[string.Format("AG{0}", row)].Value = item.First().LOPDays;
                    Sheets.Cells[string.Format("AH{0}", row)].Value = item.First().LFDays;
                    Sheets.Cells[string.Format("AI{0}", row)].Value = item.First().OthersDays;
                    int B = 0;
                    foreach (var itemMMM in item.Where(x => x.ComponentType == 1).OrderBy(x => x.ComponentId))
                    {

                        Sheets.Cells[string.Format(CellArray[B] + "{0}", row)].Value = Math.Round(itemMMM.SalaryAmount, MidpointRounding.AwayFromZero); //string.Format("{0:0}", itemMMM.SalaryAmount);

                        B++;
                    }
                    decimal TotalEarning = item.Where(x => x.ComponentType == 1).Sum(x => x.SalaryAmount);
                    Sheets.Cells[string.Format("BZ{0}", row)].Value = Math.Round(TotalEarning, MidpointRounding.AwayFromZero); //TotalEarning;
                    int BB = 1;
                    foreach (var itemMMM in item.Where(x => x.ComponentType == 2).OrderBy(x => x.ComponentId))
                    {
                        if (itemMMM.ComponentId == 127)
                        {
                            Sheets.Cells[string.Format(CellArray[42 + BB] + "{0}", row)].Value = itemMMM.SalaryAmount;
                        }
                        else
                        {
                            Sheets.Cells[string.Format(CellArray[42 + BB] + "{0}", row)].Value = Math.Round(itemMMM.SalaryAmount, MidpointRounding.AwayFromZero); //itemMMM.SalaryAmount;
                        }
                        BB++;
                    }
                    decimal TotalDeduction = item.Where(x => x.ComponentType == 2).Sum(x => x.SalaryAmount);                    
                    Sheets.Cells[string.Format("DC{0}", row)].Value = Math.Round(TotalDeduction, MidpointRounding.AwayFromZero);
                    Sheets.Cells[string.Format("DD{0}", row)].Value = Math.Round((TotalEarning - TotalDeduction), MidpointRounding.AwayFromZero);

                    row++;
                }
                var stream = new MemoryStream(Eps.GetAsByteArray());
                return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);

            
        }
    }
}
