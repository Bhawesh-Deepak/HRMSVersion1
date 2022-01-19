using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Entities.UserManagement;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.Helpers.ExcelHelper;
using HRMS.Core.ReqRespVm.RequestVm;
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
    public class EmployeeSalaryController : Controller
    {
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepository;
        private readonly IGenericRepository<EmployeeSalary, int> _IEmployeeSalaryDetailRepository;
        private readonly IGenericRepository<AuthenticateUser, int> _IAuthenticateRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;
        private readonly IGenericRepository<CtcComponentDetail, int> _ICtcComponentDetailRepository;
        private readonly IGenericRepository<EmployeeCtcComponent, int> _IEmployeeCtcComponentRepository;
        public EmployeeSalaryController(IGenericRepository<EmployeeDetail, int> employeeDetailRepo, IGenericRepository<CtcComponentDetail, int> CtcComponentDetailRepo,
            IGenericRepository<EmployeeSalary, int> employeeSalaryDetailRepo,
            IGenericRepository<EmployeeCtcComponent, int> EmployeeCtcComponentRepo, IGenericRepository<AuthenticateUser, int> authRepository, IHostingEnvironment hostingEnvironment)
        {
            _IEmployeeDetailRepository = employeeDetailRepo;
            _IEmployeeSalaryDetailRepository = employeeSalaryDetailRepo;
            _IAuthenticateRepository = authRepository;
            _IHostingEnviroment = hostingEnvironment;
            _ICtcComponentDetailRepository = CtcComponentDetailRepo;
            _IEmployeeCtcComponentRepository = EmployeeCtcComponentRepo;
        }
        public IActionResult Index()
        {
            return View(ViewHelper.GetViewPathDetails("Employee", "EmployeeSalaryIndex"));
        }
        public async Task<IActionResult> DownloadExcelFormat()
        {
            string sWebRootFolder = _IHostingEnviroment.WebRootPath;
            string sFileName = @"EmployeeNewHire.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            var response = await _ICtcComponentDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.ComponentValueType == 1);
            string[] cells = { "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CX", "CY", "CZ", "DA" };
            ExcelPackage Eps = new ExcelPackage();
            ExcelWorksheet Sheets = Eps.Workbook.Worksheets.Add("Employee");

            Sheets.Cells["A1"].Value = "Salutation";
            Sheets.Cells["B1"].Value = "EmployeeName";
            Sheets.Cells["C1"].Value = "Empcode";
            Sheets.Cells["D1"].Value = "Joiningdate";
            Sheets.Cells["E1"].Value = "Employeementstatus";
            Sheets.Cells["F1"].Value = "Officeemailid";
            Sheets.Cells["G1"].Value = "Department";
            Sheets.Cells["H1"].Value = "Designation";
            Sheets.Cells["I1"].Value = "Location";
            Sheets.Cells["J1"].Value = "Legalentity";
            Sheets.Cells["K1"].Value = "Pnlheadname";
            Sheets.Cells["L1"].Value = "Supervisorcode";
            Sheets.Cells["M1"].Value = "Level";
            Sheets.Cells["N1"].Value = "Pancardnumber";
            Sheets.Cells["O1"].Value = "Passportnumber";
            Sheets.Cells["P1"].Value = "Aadharnumber";
            Sheets.Cells["Q1"].Value = "Nameonbankaccount";
            Sheets.Cells["R1"].Value = "Bankaccountnumber";
            Sheets.Cells["S1"].Value = "Bankname";
            Sheets.Cells["T1"].Value = "Ifsccode";
            Sheets.Cells["U1"].Value = "Previousorganisation";
            Sheets.Cells["V1"].Value = "Workexperience";
            Sheets.Cells["W1"].Value = "Educationalqualification";
            Sheets.Cells["X1"].Value = "Institutename";
            Sheets.Cells["Y1"].Value = "Confirmationdate";
            Sheets.Cells["Z1"].Value = "Recuritmentsource";
            Sheets.Cells["AA1"].Value = "Requitername";
            Sheets.Cells["AB1"].Value = "Fathername";
            Sheets.Cells["AC1"].Value = "Personalemialid";
            Sheets.Cells["AD1"].Value = "Contactnumber";
            Sheets.Cells["AE1"].Value = "Dateofbirth";
            Sheets.Cells["AF1"].Value = "Currentaddress";
            Sheets.Cells["AG1"].Value = "Permanentaddress";
            Sheets.Cells["AH1"].Value = "Biomericcode";
            Sheets.Cells["AI1"].Value = "Bloodgroup";
            Sheets.Cells["AJ1"].Value = "Gender";
            Sheets.Cells["AK1"].Value = "Maritalstatus";
            Sheets.Cells["AL1"].Value = "Region";
            Sheets.Cells["AM1"].Value = "Pipstartdate";
            Sheets.Cells["AN1"].Value = "Pipenddate";
            Sheets.Cells["AO1"].Value = "Pip";
            Sheets.Cells["AP1"].Value = "Whatsappno";
            Sheets.Cells["AQ1"].Value = "Noticeperiod";
            Sheets.Cells["AR1"].Value = "Spousename";
            Sheets.Cells["AS1"].Value = "Dateofmarrage";
            Sheets.Cells["AT1"].Value = "Emergencynumber";
            Sheets.Cells["AU1"].Value = "Emergencyrelationwithemployee";
            Sheets.Cells["AV1"].Value = "Uanno";
            Sheets.Cells["AW1"].Value = "Esicnew";
            Sheets.Cells["AX1"].Value = "Leavesupervisor";
            Sheets.Cells["AY1"].Value = "Ijplocation";
            Sheets.Cells["AZ1"].Value = "Shifttiming";
            Sheets.Cells["BA1"].Value = "Confirmationstatus";
            Sheets.Cells["BB1"].Value = "Nationality";
            Sheets.Cells["BC1"].Value = "Pfbankaccountnumber";
            Sheets.Cells["BD1"].Value = "Esicpreviousnumber";
            Sheets.Cells["BE1"].Value = "Induction";
            Sheets.Cells["BF1"].Value = "Visano";
            Sheets.Cells["BG1"].Value = "Visadate";
            Sheets.Cells["BH1"].Value = "Taxfilenumber";
            Sheets.Cells["BI1"].Value = "Supernationaccountnumber";
            Sheets.Cells["BJ1"].Value = "Swiftcode";
            Sheets.Cells["BK1"].Value = "Routingcode";
            Sheets.Cells["BL1"].Value = "Alternatemobilenumber";
            Sheets.Cells["BM1"].Value = "Branchofficeid";
            Sheets.Cells["BN1"].Value = "Exitdate";
            Sheets.Cells["BO1"].Value = "Holidaygroupid";
            Sheets.Cells["BP1"].Value = "Isesiceligible";
            Sheets.Cells["BQ1"].Value = "Landlineno";
            Sheets.Cells["BR1"].Value = "Leaveapprover1";
            Sheets.Cells["BS1"].Value = "Leaveapprover2";
            Sheets.Cells["BT1"].Value = "Pt_Statename";
            Sheets.Cells["BU1"].Value = "Ispfeligible";
            Sheets.Cells["BV1"].Value = "Ctc";

            Sheets.Cells["A2"].Value = "Mandatory";
            Sheets.Cells["B2"].Value = "Mandatory";
            Sheets.Cells["C2"].Value = "Mandatory";
            Sheets.Cells["D2"].Value = "Mandatory";
            Sheets.Cells["E2"].Value = "Mandatory";
            Sheets.Cells["F2"].Value = "Mandatory";
            Sheets.Cells["G2"].Value = "Mandatory";
            Sheets.Cells["H2"].Value = "Mandatory";
            Sheets.Cells["I2"].Value = "Mandatory";
            Sheets.Cells["J2"].Value = "Mandatory";
            Sheets.Cells["K2"].Value = "Mandatory";
            Sheets.Cells["L2"].Value = "Mandatory";
            Sheets.Cells["M2"].Value = "Mandatory";
            Sheets.Cells["N2"].Value = "Mandatory";
            Sheets.Cells["O2"].Value = "Optional";
            Sheets.Cells["P2"].Value = "Mandatory";
            Sheets.Cells["Q2"].Value = "Optional";
            Sheets.Cells["R2"].Value = "Mandatory";
            Sheets.Cells["S2"].Value = "Mandatory";
            Sheets.Cells["T2"].Value = "Mandatory";
            Sheets.Cells["U2"].Value = "Mandatory";
            Sheets.Cells["V2"].Value = "Mandatory";
            Sheets.Cells["W2"].Value = "Mandatory";
            Sheets.Cells["X2"].Value = "Mandatory";
            Sheets.Cells["Y2"].Value = "Optional(Date)";
            Sheets.Cells["Z2"].Value = "Optional";
            Sheets.Cells["AA2"].Value = "Optional";
            Sheets.Cells["AB2"].Value = "Mandatory";
            Sheets.Cells["AC2"].Value = "Mandatory";
            Sheets.Cells["AD2"].Value = "Mandatory";
            Sheets.Cells["AE2"].Value = "Mandatory";
            Sheets.Cells["AF2"].Value = "Mandatory";
            Sheets.Cells["AG2"].Value = "Mandatory";
            Sheets.Cells["AH2"].Value = "Mandatory";
            Sheets.Cells["AI2"].Value = "Mandatory";
            Sheets.Cells["AJ2"].Value = "Mandatory";
            Sheets.Cells["AK2"].Value = "Optinal(Date)";
            Sheets.Cells["AL2"].Value = "Mandatory";
            Sheets.Cells["AM2"].Value = "Optinal(Date)";
            Sheets.Cells["AN2"].Value = "Optional(Date)";
            Sheets.Cells["AO2"].Value = "Optional";
            Sheets.Cells["AP2"].Value = "Mandatory";
            Sheets.Cells["AQ2"].Value = "Mandatory";
            Sheets.Cells["AR2"].Value = "Optional";
            Sheets.Cells["AS2"].Value = "Optional";
            Sheets.Cells["AT2"].Value = "Optional";
            Sheets.Cells["AU2"].Value = "Optional";
            Sheets.Cells["AV2"].Value = "Optional";
            Sheets.Cells["AW2"].Value = "Optional";
            Sheets.Cells["AX2"].Value = "Mandatory";
            Sheets.Cells["AY2"].Value = "Optinal";
            Sheets.Cells["AZ2"].Value = "Mandatory";
            Sheets.Cells["BA2"].Value = "Mandatory(1,0)";
            Sheets.Cells["BB2"].Value = "Mandatory";
            Sheets.Cells["BC2"].Value = "Optional";
            Sheets.Cells["BD2"].Value = "Optional";
            Sheets.Cells["BE2"].Value = "Mandatory(1,0)";
            Sheets.Cells["BF2"].Value = "Optional";
            Sheets.Cells["BG2"].Value = "Optional";
            Sheets.Cells["BH2"].Value = "Optional";
            Sheets.Cells["BI2"].Value = "Optional";
            Sheets.Cells["BJ2"].Value = "Optional";
            Sheets.Cells["BK2"].Value = "Optional";
            Sheets.Cells["BL2"].Value = "Optional";
            Sheets.Cells["BM2"].Value = "Mandatory";
            Sheets.Cells["BN2"].Value = "Optinal";
            Sheets.Cells["BO2"].Value = "Mandatory";
            Sheets.Cells["BP2"].Value = "Mandatory";
            Sheets.Cells["BQ2"].Value = "optional";
            Sheets.Cells["BR2"].Value = "Optional";
            Sheets.Cells["BS2"].Value = "Optional";
            Sheets.Cells["BT2"].Value = "Mandatory";
            Sheets.Cells["BU2"].Value = "Mandatory(1,0)";
            Sheets.Cells["BV2"].Value = "Mandatory(1,0)";
            int cell = 0;
            foreach (var item in response.Entities)
            {
                Sheets.Cells[cells[cell] + "1"].Value = item.ComponentName.Trim();
                Sheets.Cells[cells[cell] + "2"].Value = "Mandatory";
                cell++;
            }
            Sheets.Cells["A1:" + cells[cell - 1] + "1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            Sheets.Cells["A1:" + cells[cell - 1] + "1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);
            Sheets.Cells["A1:" + cells[cell - 1] + "2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            Sheets.Cells["A1:" + cells[cell - 1] + "2"].Style.Fill.BackgroundColor.SetColor(Color.Gray);
            var stream = new MemoryStream(Eps.GetAsByteArray());
            return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
        }

        [HttpPost]
        public async Task<IActionResult> UploadEmployeeSalary(UploadExcelVm model)
        {
            try
            {

                var response = new ReadEmployeeSalaryExcelHelper().GetEmployeeSalaryDetails(model.UploadFile);

                var employeeDetailResponse = await _IEmployeeDetailRepository.CreateEntities(response.EmployeeDetails.ToArray());

                var employeeSalaryReponse = await _IEmployeeSalaryDetailRepository.CreateEntities(response.EmployeeSalaryDetails.ToArray());
                 
                var employeeSalaryList = await _IEmployeeSalaryDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                response.EmployeeCtcComponentDetails.ToList().ForEach(data =>
                {
                    data.EmployeeSalaryId = employeeSalaryList.Entities.Where(x => x.EmpCode == data.EmpCode && x.EndDate == null).FirstOrDefault().Id;
                });
                var ctccomponentReponse = await _IEmployeeCtcComponentRepository.CreateEntities(response.EmployeeCtcComponentDetails.ToArray());
                await CreateUserCredential(response.EmployeeDetails.ToList());

                return Json("Employee Basic information and Salary Detail Uploaded successfully !!!");
            }
            catch (Exception ex)
            {
                Serilog.Log.Information(ex.InnerException.ToString(), ex);
                return Json("Unable to upload the Excel File, Something wents wrong please contact admin !");
            }
        }

        public async Task<IActionResult> CreateUserCredential(List<EmployeeDetail> responseData)
        {
            var employeemodels = await _IEmployeeDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.CreatedDate.Value.Date == DateTime.Now.Date);
            var models = new List<AuthenticateUser>();

            responseData.ToList().ForEach(data =>
            {
                employeemodels.Entities.ToList().ForEach(item =>
                {
                    if (data.EmpCode.Trim().ToLower() == item.EmpCode.Trim().ToLower())
                    {
                        var model = new AuthenticateUser()
                        {
                            EmployeeId = item.Id,
                            DisplayUserName = data.EmployeeName,
                            IsDeleted = false,
                            IsActive = true,
                            IsLocked = false,
                            IsPasswordExpired = false,
                            CreatedBy = 1,
                            CreatedDate = DateTime.Now,
                            UserName = data.EmpCode,
                            Password = PasswordEncryptor.Instance.Encrypt("123@qwe", "HRMSPAYROLLPASSWORDKEY"),
                            RoleId = 4// for employee role
                        };
                        models.Add(model);

                    }
                });


            });


            var response = await _IAuthenticateRepository.CreateEntities(models.ToArray());


            return Json(response.Message);
        }
    }
}
