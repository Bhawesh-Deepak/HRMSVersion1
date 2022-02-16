using HRMS.Admin.UI.AuthenticateService;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Core.ReqRespVm.Response.Employee;
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

namespace HRMS.Admin.UI.Controllers.Payroll
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class NewHireAndExitEmployeeController : Controller
    {
        private readonly IDapperRepository<NewHireAndExitEmployeeVM> _INewHireAndExitEmployeeVMRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;
        private readonly IDapperRepository<ExportEmployeeParams> _IExportEmployeeRepository;
        public NewHireAndExitEmployeeController(IDapperRepository<NewHireAndExitEmployeeVM> newhireandexitemployeeVMRepository,
            IDapperRepository<ExportEmployeeParams> exportemployeeRepository, IHostingEnvironment hostingEnvironment)
        {
            _INewHireAndExitEmployeeVMRepository = newhireandexitemployeeVMRepository;
            _IHostingEnviroment = hostingEnvironment;
            _IExportEmployeeRepository = exportemployeeRepository;
        }
        public IActionResult Index()
        {
            try
            {
                return View(ViewHelper.GetViewPathDetails("NewHireAndExitEmployee", "_NewHireAndExitEmployeeIndex"));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(NewHireAndExitEmployeeController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> GetEmployeeDetails(NewHireAndExitEmployeeVM model)
        {
            try
            {
                if (model.ValueType == 1)
                    ViewBag.HeaderTitle = "New Hire Employee";
                else
                    ViewBag.HeaderTitle = "Exit Employee";
                var response = await Task.Run(() => _INewHireAndExitEmployeeVMRepository.GetAll<NewHireAndExitEmployeeDetailVM>(SqlQuery.GetNewHireAndExitEmployee, model));
                return PartialView(ViewHelper.GetViewPathDetails("NewHireAndExitEmployee", "_GetEmployeeDetails"), response);

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(NewHireAndExitEmployeeController)} action name {nameof(GetEmployeeDetails)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ExportToExcel(NewHireAndExitEmployeeVM model)
        {
            var param = new ExportEmployeeParams();
            var response = new List<ExportEmployeeVM>();
            var responseDetails = _IExportEmployeeRepository.GetAll<ExportEmployeeVM>(SqlQuery.GetExportEmployee, param);
            if (model.ValueType == 1)
                response = responseDetails.Where(z => z.JoiningDates.Date >= model.FromDate.Date && z.JoiningDates.Date <= model.ToDate.Date).ToList();
            else
                response = responseDetails.Where(z => z.ExitDates.Date >= model.FromDate.Date && z.ExitDates.Date <= model.ToDate.Date).ToList();
            string sWebRootFolder = _IHostingEnviroment.WebRootPath;
            string sFileName = @"EmployeeMaster.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            ExcelPackage Eps = new ExcelPackage();
            ExcelWorksheet Sheets = Eps.Workbook.Worksheets.Add("EmployeeMaster");
            Sheets.View.FreezePanes(1, 4);
            Sheets.Cells["A1:CU1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            Sheets.Cells["A1:CU1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);
            Eps.Encryption.Password = "sqy123";
            Sheets.Cells["A1"].Value = "salutation";
            Sheets.Cells["B1"].Value = "EmployeeName";
            Sheets.Cells["C1"].Value = "EmpCode";
            Sheets.Cells["D1"].Value = "joiningDate";
            Sheets.Cells["E1"].Value = "EmployeementStatus";
            Sheets.Cells["F1"].Value = "officeEmailId";
            Sheets.Cells["G1"].Value = "Department";
            Sheets.Cells["H1"].Value = "Designation";
            Sheets.Cells["I1"].Value = "Location";
            Sheets.Cells["J1"].Value = "LegalEntity";
            Sheets.Cells["K1"].Value = "PnlHeadName";
            Sheets.Cells["L1"].Value = "SuperVisorCode";
            Sheets.Cells["M1"].Value = "Level";
            Sheets.Cells["N1"].Value = "PANCardNumber";
            Sheets.Cells["O1"].Value = "PassportNumber";
            Sheets.Cells["P1"].Value = "AadharNumber";
            Sheets.Cells["Q1"].Value = "NameonBankAccount";
            Sheets.Cells["R1"].Value = "BankAccountNumber";
            Sheets.Cells["S1"].Value = "BankName";
            Sheets.Cells["T1"].Value = "IFSCCode";
            Sheets.Cells["U1"].Value = "PreviousOrganisation";
            Sheets.Cells["V1"].Value = "WorkExperience";
            Sheets.Cells["W1"].Value = "EducationalQualification";
            Sheets.Cells["X1"].Value = "InstituteName";
            Sheets.Cells["Y1"].Value = "ConfirmationDate";
            Sheets.Cells["Z1"].Value = "RecuritmentSource";
            Sheets.Cells["AA1"].Value = "RequiterName";
            Sheets.Cells["AB1"].Value = "FatherName";
            Sheets.Cells["AC1"].Value = "PersonalEmialID";
            Sheets.Cells["AD1"].Value = "ContactNumber";
            Sheets.Cells["AE1"].Value = "DateofBirth";
            Sheets.Cells["AF1"].Value = "CurrentAddress";
            Sheets.Cells["AG1"].Value = "PermanentAddress";
            Sheets.Cells["AH1"].Value = "BiomericCode";
            Sheets.Cells["AI1"].Value = "BloodGroup";
            Sheets.Cells["AJ1"].Value = "Gender";
            Sheets.Cells["AK1"].Value = "MaritalStatus";
            Sheets.Cells["AL1"].Value = "Region";
            Sheets.Cells["AM1"].Value = "PIPStartDate";
            Sheets.Cells["AN1"].Value = "PIPEndDate";
            Sheets.Cells["AO1"].Value = "PIP";
            Sheets.Cells["AP1"].Value = "WhatsAppNo";
            Sheets.Cells["AQ1"].Value = "NoticePeriod";
            Sheets.Cells["AR1"].Value = "SpouseName";
            Sheets.Cells["AS1"].Value = "DateofMarrage";
            Sheets.Cells["AT1"].Value = "EmergencyNumber";
            Sheets.Cells["AU1"].Value = "EmergencyRelationWithEmployee";
            Sheets.Cells["AV1"].Value = "UANNo";
            Sheets.Cells["AW1"].Value = "ESICNew";
            Sheets.Cells["AX1"].Value = "LeaveSupervisor";
            Sheets.Cells["AY1"].Value = "IJPLocation";
            Sheets.Cells["AZ1"].Value = "ShiftTiming";
            Sheets.Cells["BA1"].Value = "ConfirmationStatus";
            Sheets.Cells["BB1"].Value = "Nationality";
            Sheets.Cells["BC1"].Value = "PFBankAccountNumber";
            Sheets.Cells["BD1"].Value = "ESICPreviousNumber";
            Sheets.Cells["BE1"].Value = "Induction";
            Sheets.Cells["BF1"].Value = "IsActive";

            Sheets.Cells["BG1"].Value = "VisaNo";
            Sheets.Cells["BH1"].Value = "VisaDate";
            Sheets.Cells["BI1"].Value = "TaxFileNumber";
            Sheets.Cells["BJ1"].Value = "SupernationAccountNumber";
            Sheets.Cells["BK1"].Value = "SwiftCode";
            Sheets.Cells["BL1"].Value = "RoutingCode";
            Sheets.Cells["BM1"].Value = "alternateMobileNumber";
            Sheets.Cells["BN1"].Value = "branchOfficeId";
            Sheets.Cells["BO1"].Value = "exitDate";
            Sheets.Cells["BP1"].Value = "holidayGroupId";
            Sheets.Cells["BQ1"].Value = "isEsicEligible";
            Sheets.Cells["BR1"].Value = "landLineNo";
            Sheets.Cells["BS1"].Value = "leaveApprover1";
            Sheets.Cells["BT1"].Value = "leaveApprover2";
            Sheets.Cells["BU1"].Value = "CTC";
            Sheets.Cells["BV1"].Value = "PT_StateName";
            Sheets.Cells["BW1"].Value = "isPFeligible";

            string[] CellArray = { "BX", "BY", "BZ", "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ" };
            int INC = 0;

            foreach (var item in response.Where(x => x.ComponentType == 1 && x.Id == response.FirstOrDefault().Id).OrderBy(x => x.ComponentId))
            {
                Sheets.Cells[CellArray[INC] + "1"].Value = item.ComponentName;
                INC++;
            }

            Sheets.Cells["CL1"].Value = "Gross Salary";
            foreach (var item in response.Where(x => x.ComponentType == 2 && x.Id == response.FirstOrDefault().Id).OrderBy(x => x.ComponentId))
            {
                Sheets.Cells[CellArray[((INC) + 1)] + "1"].Value = item.ComponentName;
                INC++;
            }
            Sheets.Cells["CS1"].Value = "Total Deduction";
            Sheets.Cells["CT1"].Value = "Net Salary";
            int row = 2;

            foreach (var item in response.GroupBy(X => X.Id))
            {
                Sheets.Cells[string.Format("A{0}", row)].Value = item.First().Salutation;
                Sheets.Cells[string.Format("B{0}", row)].Value = item.First().EmployeeName;
                Sheets.Cells[string.Format("C{0}", row)].Value = item.First().EmpCode;
                Sheets.Cells[string.Format("D{0}", row)].Value = item.First().JoiningDate;
                Sheets.Cells[string.Format("E{0}", row)].Value = item.First().EmployeeStatus;
                Sheets.Cells[string.Format("F{0}", row)].Value = item.First().OfficeEmailId;
                Sheets.Cells[string.Format("G{0}", row)].Value = item.First().DepartmentName;
                Sheets.Cells[string.Format("H{0}", row)].Value = item.First().DesignationName;
                Sheets.Cells[string.Format("I{0}", row)].Value = item.First().Location;
                Sheets.Cells[string.Format("J{0}", row)].Value = item.First().LegalEntity;
                Sheets.Cells[string.Format("K{0}", row)].Value = item.First().PAndLHeadName;
                Sheets.Cells[string.Format("L{0}", row)].Value = item.First().SuperVisorCode;
                Sheets.Cells[string.Format("M{0}", row)].Value = item.First().Level;
                Sheets.Cells[string.Format("N{0}", row)].Value = item.First().PanCardNumber;
                Sheets.Cells[string.Format("O{0}", row)].Value = item.First().PassportNumber;
                Sheets.Cells[string.Format("P{0}", row)].Value = item.First().AadharCardNumber;
                Sheets.Cells[string.Format("Q{0}", row)].Value = item.First().BankAccountName;
                Sheets.Cells[string.Format("R{0}", row)].Value = item.First().BankAccountNumber;
                Sheets.Cells[string.Format("S{0}", row)].Value = item.First().BankName;
                Sheets.Cells[string.Format("T{0}", row)].Value = item.First().IFSCCode;
                Sheets.Cells[string.Format("U{0}", row)].Value = item.First().PreviousOrganisation;
                Sheets.Cells[string.Format("V{0}", row)].Value = item.First().WorkExprience;
                Sheets.Cells[string.Format("W{0}", row)].Value = item.First().EducationalQualification;
                Sheets.Cells[string.Format("X{0}", row)].Value = item.First().InstituteName;
                Sheets.Cells[string.Format("Y{0}", row)].Value = item.First().ConfirmationDate;
                Sheets.Cells[string.Format("Z{0}", row)].Value = item.First().RecruitmentSource;
                Sheets.Cells[string.Format("AA{0}", row)].Value = item.First().RecruitmentName;
                Sheets.Cells[string.Format("AB{0}", row)].Value = item.First().FatherName;
                Sheets.Cells[string.Format("AC{0}", row)].Value = item.First().PersonalEmailId;
                Sheets.Cells[string.Format("AD{0}", row)].Value = item.First().ContactNumber;
                Sheets.Cells[string.Format("AE{0}", row)].Value = item.First().DateOfBirth;
                Sheets.Cells[string.Format("AF{0}", row)].Value = item.First().CurrentAddress;
                Sheets.Cells[string.Format("AG{0}", row)].Value = item.First().PermanentAddress;
                Sheets.Cells[string.Format("AH{0}", row)].Value = item.First().BiometricCode;
                Sheets.Cells[string.Format("AI{0}", row)].Value = item.First().BloodGroup;
                Sheets.Cells[string.Format("AJ{0}", row)].Value = item.First().Gender;
                Sheets.Cells[string.Format("AK{0}", row)].Value = item.First().MaritalStatus;
                Sheets.Cells[string.Format("AL{0}", row)].Value = item.First().Region;
                Sheets.Cells[string.Format("AM{0}", row)].Value = item.First().PIPStartDate;
                Sheets.Cells[string.Format("AN{0}", row)].Value = item.First().PIPEndDate;
                Sheets.Cells[string.Format("AO{0}", row)].Value = item.First().PIP;
                Sheets.Cells[string.Format("AP{0}", row)].Value = item.First().WhatsAppNumber;
                Sheets.Cells[string.Format("AQ{0}", row)].Value = item.First().NoticePeriod;
                Sheets.Cells[string.Format("AR{0}", row)].Value = item.First().SpouceName;
                Sheets.Cells[string.Format("AS{0}", row)].Value = item.First().DateOfMairrage;
                Sheets.Cells[string.Format("AT{0}", row)].Value = item.First().EmergencyNumber;
                Sheets.Cells[string.Format("AU{0}", row)].Value = item.First().EmergencyRelationWithEmployee;
                Sheets.Cells[string.Format("AV{0}", row)].Value = item.First().UANNumber;
                Sheets.Cells[string.Format("AW{0}", row)].Value = item.First().ESICNew;
                Sheets.Cells[string.Format("AX{0}", row)].Value = item.First().LeaveApprover1;
                Sheets.Cells[string.Format("AY{0}", row)].Value = item.First().IJPLocation;
                Sheets.Cells[string.Format("AZ{0}", row)].Value = item.First().ShiftTiming;
                Sheets.Cells[string.Format("BA{0}", row)].Value = item.First().ConfirmationStatus;
                Sheets.Cells[string.Format("BB{0}", row)].Value = item.First().Nationality;
                Sheets.Cells[string.Format("BC{0}", row)].Value = item.First().PAndFBankAccountNumberx;
                Sheets.Cells[string.Format("BD{0}", row)].Value = item.First().ESICPreviousNumber;
                Sheets.Cells[string.Format("BE{0}", row)].Value = item.First().Induction;
                Sheets.Cells[string.Format("BF{0}", row)].Value = item.First().EmployeeStatus;
                Sheets.Cells[string.Format("BG{0}", row)].Value = item.First().VISANumber;
                Sheets.Cells[string.Format("BH{0}", row)].Value = item.First().VISADate;
                Sheets.Cells[string.Format("BI{0}", row)].Value = item.First().TaxFileNumber;
                Sheets.Cells[string.Format("BJ{0}", row)].Value = item.First().SupernationAccountNumber;
                Sheets.Cells[string.Format("BK{0}", row)].Value = item.First().SwiftCode;
                Sheets.Cells[string.Format("BL{0}", row)].Value = item.First().RoutingCode;
                Sheets.Cells[string.Format("BM{0}", row)].Value = item.First().AlternateMobileNumber;
                Sheets.Cells[string.Format("BN{0}", row)].Value = item.First().BranchOfficeId;
                Sheets.Cells[string.Format("BO{0}", row)].Value = item.First().ExitDate;
                Sheets.Cells[string.Format("BP{0}", row)].Value = item.First().HolidayGroupId;
                Sheets.Cells[string.Format("BQ{0}", row)].Value = item.First().IsESICEligible;
                Sheets.Cells[string.Format("BR{0}", row)].Value = item.First().LandLineNumber;
                Sheets.Cells[string.Format("BS{0}", row)].Value = item.First().LeaveApprover1;
                Sheets.Cells[string.Format("BT{0}", row)].Value = item.First().LeaveApprover2;
                Sheets.Cells[string.Format("BU{0}", row)].Value = item.First().CTC;
                Sheets.Cells[string.Format("BV{0}", row)].Value = item.First().PTStateName;
                Sheets.Cells[string.Format("BW{0}", row)].Value = item.First().IsPFEligible;

                int cnt1 = 0;
                foreach (var itemdata in item.Where(x => x.ComponentType == 1).OrderBy(X => X.ComponentId))
                {
                    Sheets.Cells[string.Format(CellArray[cnt1] + "{0}", row)].Value = Math.Round((Double)itemdata.ComponentValue, 2); //string.Format("{0:0.00}", itemdata.ComponentValue);
                    cnt1++;
                }
                decimal TEarning = item.Where(x => x.ComponentType == 1).Sum(x => x.ComponentValue);
                decimal TDedcution = item.Where(x => x.ComponentType == 2).Sum(x => x.ComponentValue);

                Sheets.Cells[string.Format("CL{0}", row)].Value = Math.Round((Double)TEarning, 2); //string.Format("{0:0.00}", TEarning);
                foreach (var itemdata in item.Where(x => x.ComponentType == 2).OrderBy(X => X.ComponentId))
                {
                    Sheets.Cells[string.Format(CellArray[((cnt1) + 1)] + "{0}", row)].Value = Math.Round((Double)itemdata.ComponentValue, 2);// string.Format("{0:0.00}", itemdata.ComponentValue);
                    cnt1++;
                }
                Sheets.Cells[string.Format("CS{0}", row)].Value = Math.Round((Double)TDedcution, 2); //string.Format("{0:0.00}", TDedcution);
                decimal actual = TEarning - TDedcution;
                Sheets.Cells[string.Format("CT{0}", row)].Value = Math.Round((Double)actual, 2); //string.Format("{0:0.00}", (TEarning - TDedcution));
                row++;
            }
            var stream = new MemoryStream(Eps.GetAsByteArray());
            return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
             
        }
    }
}
