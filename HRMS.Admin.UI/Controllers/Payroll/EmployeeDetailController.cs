using ClosedXML.Excel;
using Fingers10.ExcelExport.ActionResults;
using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Helpers;
using HRMS.Admin.UI.Models;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Entities.Organisation;
using HRMS.Core.Entities.Payroll;
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
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Payroll
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class EmployeeDetailController : Controller
    {
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepository;
        private readonly IGenericRepository<Subsidiary, int> _ISubsidiaryRepository;
        private readonly IGenericRepository<Department, int> _IDepartmentRepository;
        private readonly IGenericRepository<Designation, int> _IDesignationRepository;
        private readonly IGenericRepository<PAndLMaster, int> _IPAndLMasterRepository;
        private readonly IGenericRepository<Location, int> _ILocationRepository;
        private readonly IDapperRepository<EmployeeDetailParams> _IEmployeeRepository;
        private readonly IDapperRepository<EmployeeSingleDetailParam> _IEmployeeSingleDetailRepository;
        private readonly IDapperRepository<EmployeeInformationParams> _IEmployeeInformationRepository;
        private readonly IGenericRepository<Branch, int> _IBranchRepository;
        private readonly IGenericRepository<EmployeeType, int> _IEmployeeTypeRepository;
        private readonly IGenericRepository<RegionMaster, int> _IRegionMasterRepository;
        private readonly IGenericRepository<Shift, int> _IShiftRepository;
        private readonly IGenericRepository<StateMaster, int> _IStateMasterRepository;
        private readonly IDapperRepository<EmployeeInformationParams> _IExportEmployeeRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;
        public EmployeeDetailController(IGenericRepository<EmployeeDetail, int> EmployeeDetailRepo, IHostingEnvironment hostingEnvironment,
            IGenericRepository<Subsidiary, int> SubsidiaryRepo,
            IGenericRepository<Department, int> DepartmentRepo,
            IGenericRepository<Designation, int> DesignationRepo,
             IGenericRepository<PAndLMaster, int> PAndLMasterRepo,
            IGenericRepository<Location, int> LocationRepo,
            IDapperRepository<EmployeeSingleDetailParam> EmployeeSingleDetailRepository,
            IDapperRepository<EmployeeInformationParams> exportemployeeRepository,
            IDapperRepository<EmployeeDetailParams> employeeRepository,
            IDapperRepository<EmployeeInformationParams> EmployeeInformationRepository,
            IGenericRepository<Branch, int> BranchRepo,
            IGenericRepository<EmployeeType, int> EmployeeTypeRepo,
            IGenericRepository<RegionMaster, int> RegionMasterRepo,
            IGenericRepository<Shift, int> ShiftRepo,
            IGenericRepository<StateMaster, int> StateMasterRepo)
        {
            _IExportEmployeeRepository = exportemployeeRepository;
            _IEmployeeDetailRepository = EmployeeDetailRepo;
            _ISubsidiaryRepository = SubsidiaryRepo;
            _IDepartmentRepository = DepartmentRepo;
            _IDesignationRepository = DesignationRepo;
            _IPAndLMasterRepository = PAndLMasterRepo;
            _ILocationRepository = LocationRepo;
            _IEmployeeRepository = employeeRepository;
            _IEmployeeSingleDetailRepository = EmployeeSingleDetailRepository;
            _IBranchRepository = BranchRepo;
            _IEmployeeTypeRepository = EmployeeTypeRepo;
            _IRegionMasterRepository = RegionMasterRepo;
            _IShiftRepository = ShiftRepo;
            _IEmployeeInformationRepository = EmployeeInformationRepository;
            _IStateMasterRepository = StateMasterRepo;
            _IHostingEnviroment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                EmployeeDetailParams searchModelEntity = new EmployeeDetailParams();
                searchModelEntity.PageNo = 1;
                searchModelEntity.PageSize = 10;
                searchModelEntity.SortColumn = string.Empty;
                searchModelEntity.SortOrder = string.Empty;
                searchModelEntity.IsActive = true;

                PagingSortingHelper.PopulateModelForPagging(searchModelEntity, PageSize.Size10, 10, string.Empty, string.Empty);
                var response = _IEmployeeRepository.GetAll<EmployeeDetailVm>(SqlQuery.GetEmployeeDetails, searchModelEntity);

                PagingSortingHelper.PupulateModelToDisplayPagging(response?.First(), PageSize.Size10, 1, string.Empty, string.Empty);

                response.First().SortBy = "Name";
                await PopulateViewBag();

                ViewBag.HeaderTitle = PageHeader.HeaderSetting["EmployeeDetailIndex"];

                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("EmployeeDetail", "EmployeeDetailIndex"), response));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeDetailController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetFilteredData(EmployeeDetailParams searchModelEntity, string sortBy, int pageIndex, PageSize pageSize, string sortOrder)
        {
            try
            {
                searchModelEntity.PageNo = pageIndex;
                searchModelEntity.PageSize = (int)pageSize;
                searchModelEntity.SortColumn = sortBy;
                searchModelEntity.SortOrder = sortOrder;
                searchModelEntity.IsActive = true;

                PagingSortingHelper.PopulateModelForPagging(searchModelEntity, pageSize, pageIndex, sortBy, sortOrder);
                var response = _IEmployeeRepository.GetAll<EmployeeDetailVm>(SqlQuery.GetEmployeeDetails, searchModelEntity);

                PagingSortingHelper.PupulateModelToDisplayPagging(response?.First(), PageSize.Size10, pageIndex, sortBy, sortOrder);

                response.First().SortBy = sortBy;
                return await Task.Run(() => PartialView(ViewHelper.GetViewPathDetails("EmployeeDetail", "EmployeeFilteredList"), response));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeDetailController)} action name {nameof(GetFilteredData)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }


        public async Task<IActionResult> GetActiveInActiveDetails(int status)
        {
            try
            {
                var response = (await _IEmployeeDetailRepository.GetAllEntities(null)).Entities;

                bool statusValue = status != 0;

                response = response.Where(x => x.IsActive == Convert.ToBoolean(statusValue)).ToList();

                HttpContext.Session.SetObjectAsJson("EmpDetail", response.ToList());

                return await Task.Run(() => PartialView(ViewHelper.GetViewPathDetails("EmployeeDetail", "EmployeeFilteredList"), response.ToList()));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeDetailController)} action name {nameof(GetActiveInActiveDetails)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> ExportToExcel()
        {

            var empParams = new EmployeeInformationParams() { };
            var response = _IExportEmployeeRepository.GetAll<ExportEmployeeVM>(SqlQuery.GetExportEmployee, empParams);



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
            //try
            //{
            //    var models = HttpContext.Session.GetObjectFromJson<List<EmployeeDetail>>("EmpDetail");
            //    var dataTable = ListToDataTable.GetDataTableFromList<EmployeeDetail>(models.ToList());
            //    string fileName = "Employee Directory.xlsx";
            //    using XLWorkbook wb = new XLWorkbook();
            //    wb.Worksheets.Add(dataTable);
            //    using MemoryStream stream = new MemoryStream();
            //    wb.SaveAs(stream);
            //    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            //}
            //catch (Exception ex)
            //{
            //    string template = $"Controller name {nameof(EmployeeDetailController)} action name {nameof(ExportToExcel)} exception is {ex.Message}";
            //    Serilog.Log.Error(ex, template);
            //    return RedirectToAction("Error", "Home");
            //}
        }
        public async Task<IActionResult> Edit(int Id)
        {
            try
            {
                await PopulateViewBag();
                var empParams = new EmployeeSingleDetailParam()
                {
                    Id = Id
                };
                var response = _IEmployeeSingleDetailRepository.GetAll<EmployeeDetail>(SqlQuery.GetEmployeeSingleDetails, empParams);

                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("EmployeeDetail", "EditEmployeeDetail"), response.FirstOrDefault()));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeDetailController)} action name {nameof(Edit)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var subsidiaryResponse = await _ISubsidiaryRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var departmentResponse = await _IDepartmentRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var designationResponse = await _IDesignationRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var pandlResponse = await _IPAndLMasterRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var locationResponse = await _ILocationRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var branchResponse = await _IBranchRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var employeetypeResponse = await _IEmployeeTypeRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var regionResponse = await _IRegionMasterRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var shiftResponse = await _IShiftRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var ptstateResponse = await _IShiftRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var model = new EmployeeInformationParams() { };
            var employeeResponse = await Task.Run(() => _IEmployeeInformationRepository.GetAll<EmployeeInformationVM>(SqlQuery.EmployeeInformation, model));
            if (subsidiaryResponse.ResponseStatus == ResponseStatus.Success &&
                departmentResponse.ResponseStatus == ResponseStatus.Success &&
                 designationResponse.ResponseStatus == ResponseStatus.Success &&
                  pandlResponse.ResponseStatus == ResponseStatus.Success &&
                 locationResponse.ResponseStatus == ResponseStatus.Success &&
                 branchResponse.ResponseStatus == ResponseStatus.Success &&
                 employeetypeResponse.ResponseStatus == ResponseStatus.Success &&
                 regionResponse.ResponseStatus == ResponseStatus.Success &&
                 shiftResponse.ResponseStatus == ResponseStatus.Success &&
                 ptstateResponse.ResponseStatus == ResponseStatus.Success
                )
                ViewBag.subsidiaryList = subsidiaryResponse.Entities;
            ViewBag.departmentList = departmentResponse.Entities;
            ViewBag.designationList = designationResponse.Entities;
            ViewBag.pandlList = pandlResponse.Entities;
            ViewBag.locationList = locationResponse.Entities;
            ViewBag.BranchList = branchResponse.Entities;
            ViewBag.EmployeeTypeList = employeetypeResponse.Entities;
            ViewBag.RegionList = regionResponse.Entities;
            ViewBag.ShiftList = shiftResponse.Entities;
            ViewBag.EmployeeList = employeeResponse.ToList();
            ViewBag.PtStateList = ptstateResponse.Entities;

        }

        #endregion
    }
}
