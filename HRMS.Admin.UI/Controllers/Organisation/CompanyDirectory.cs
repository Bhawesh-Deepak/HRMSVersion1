using ClosedXML.Excel;
using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Organisation;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Organisation;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Organisation
{
    public class CompanyDirectory : Controller
    {
        private readonly IGenericRepository<Company, int> _ICompanyRepository;
        private readonly IGenericRepository<Subsidiary, int> _ISubsidiaryRepository;
        private readonly IGenericRepository<Branch, int> _IBranchRepository;
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepository;
        public CompanyDirectory(IGenericRepository<Company, int> companyRepository, IGenericRepository<Subsidiary, int> SubsidiaryRepository,
             IGenericRepository<Branch, int> BranchRepository,
             IGenericRepository<EmployeeDetail, int> EmployeeDetailRepository)
        {
            _ICompanyRepository = companyRepository;
            _ISubsidiaryRepository = SubsidiaryRepository;
            _IBranchRepository = BranchRepository;
            _IEmployeeDetailRepository = EmployeeDetailRepository;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["Company Directory"];
            try
            {
                var Subsidryresponse = new DBResponseHelper<Subsidiary, int>()
                    .GetDBResponseHelper(await _ISubsidiaryRepository
                    .GetAllEntities(x => x.IsActive && !x.IsDeleted));
                var Companyresponse = new DBResponseHelper<Company, int>()
                  .GetDBResponseHelper(await _ICompanyRepository
                  .GetAllEntities(x => x.IsActive && !x.IsDeleted));
                var response = from subsidry in Subsidryresponse.Item2.Entities
                               join company in Companyresponse.Item2.Entities
                               on subsidry.OrganisationId equals company.Id
                               select new SubsidiaryVM
                               {
                                   Id = subsidry.Id,
                                   CompanyName = company.Name,
                                   Name = subsidry.Name,
                                   Code = subsidry.Code,
                                   Email = subsidry.Email,
                                   Url = subsidry.Url,
                                   FavIcon = subsidry.FavIcon,
                                   Logo = subsidry.Logo
                               };

                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("CompanyDirectory", "CompanyDirectoryIndex"), response));

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CompanyDirectory)} action name {nameof(Index)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }
        public async Task<IActionResult> GetBranchList(int subsidiaryId)
        {
            try
            {
                var branchList = await _IBranchRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var subsidiaryList = await _ISubsidiaryRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var response = from subsidry in subsidiaryList.Entities
                               join branch in branchList.Entities
                               on subsidry.Id equals branch.CompanyId
                               where branch.CompanyId == subsidiaryId
                               select new BranchDirectoryVM
                               {
                                   SubsidiaryId = subsidry.Id,
                                   SubsidiaryName = subsidry.Name,
                                   BranchId = branch.Id,
                                   BranchName = branch.Name,
                               };
                response.ToList().ForEach(item =>
                {

                    item.TotalEmployee = _IEmployeeDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.BranchOfficeId.Trim().ToLower() == item.BranchName.Trim().ToLower()).Result.Entities.Count();
                });
                return PartialView(ViewHelper.GetViewPathDetails("CompanyDirectory", "GetBranchDetails"), response);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CompanyDirectory)} action name {nameof(GetBranchList)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }
        public async Task<IActionResult> DownloadEmployee(string LegalEntity)
        {
            var response = await _IEmployeeDetailRepository.GetAllEntities(x => x.LegalEntity.Trim().ToLower() == LegalEntity.Trim().ToLower());
            DataTable dt = new DataTable("Employee");
            dt.Columns.AddRange(new DataColumn[72] {
                    new DataColumn("Salutation"),
                    new DataColumn("EmployeeName"),
                    new DataColumn("EmpCode"),
                    new DataColumn("joining Date"),
                    new DataColumn("Employeement Status"),
                    new DataColumn("office EmailId"),
                    new DataColumn("Department"),
                    new DataColumn("Designation"),
                    new DataColumn("Location"),
                    new DataColumn("LegalEntity"),
                     new DataColumn("PnlHeadName"),
                    new DataColumn("SuperVisorCode"),
                    new DataColumn("Level"),
                    new DataColumn("PANCardNumber"),
                    new DataColumn("PassportNumber"),
                    new DataColumn("AadharNumber"),
                    new DataColumn("NameonBankAccount"),
                    new DataColumn("BankAccountNumber"),
                    new DataColumn("BankName"),
                    new DataColumn("IFSCCode"),
                    new DataColumn("PreviousOrganisation"),
                    new DataColumn("WorkExperience"),
                    new DataColumn("EducationalQualification"),
                    new DataColumn("InstituteName"),
                    new DataColumn("ConfirmationDate"),
                    new DataColumn("RecuritmentSource"),
                    new DataColumn("RequiterName"),
                    new DataColumn("FatherName"),
                    new DataColumn("PersonalEmialID"),
                    new DataColumn("ContactNumber"),
                     new DataColumn("DateofBirth"),
                    new DataColumn("CurrentAddress"),
                    new DataColumn("PermanentAddress"),
                    new DataColumn("BiomericCode"),
                    new DataColumn("BloodGroup"),
                    new DataColumn("Gender"),
                    new DataColumn("MaritalStatus"),
                    new DataColumn("Region"),
                    new DataColumn("PIPStartDate"),
                    new DataColumn("PIPEndDate"),
                     new DataColumn("PIP"),
                    new DataColumn("WhatsAppNo"),
                    new DataColumn("NoticePeriod"),
                    new DataColumn("SpouseName"),
                    new DataColumn("DateofMarrage"),
                    new DataColumn("EmergencyNumber"),
                    new DataColumn("EmergencyRelationWithEmployee"),
                    new DataColumn("UANNo"),
                    new DataColumn("ESICNew"),
                    new DataColumn("LeaveSupervisor"),
                      new DataColumn("IJPLocation"),
                    new DataColumn("ShiftTiming"),
                    new DataColumn("ConfirmationStatus"),
                    new DataColumn("Nationality"),
                    new DataColumn("PFBankAccountNumber"),
                    new DataColumn("ESICPreviousNumber"),
                    new DataColumn("Induction"),
                    new DataColumn("IsActive"),
                    new DataColumn("VisaNo"),
                    new DataColumn("VisaDate"),
                     new DataColumn("TaxFileNumber"),
                    new DataColumn("SupernationAccountNumber"),
                    new DataColumn("SwiftCode"),
                    new DataColumn("RoutingCode"),
                    new DataColumn("alternateMobileNumber"),
                    new DataColumn("branchOfficeId"),
                    new DataColumn("exitDate"),
                    new DataColumn("holidayGroupId"),
                    new DataColumn("isEsicEligible"),
                    new DataColumn("landLineNo"),
                     new DataColumn("leaveApprover1"),
                    new DataColumn("leaveApprover2"),

            });

            foreach (var data in response.Entities)
            {
                dt.Rows.Add(
                    data.Salutation,
                    data.EmployeeName,
                    data.EmpCode,
                    data.JoiningDate.ToString("dd/MM/yyyy"),
                    data.EmployementStatus,
                    data.OfficeEmailId,
                    data.DepartmentName,
                    data.DesignationName,
                    data.Location,
                    data.LegalEntity,
                    data.PAndLHeadName,
                    data.SuperVisorCode,
                    data.Level,
                    data.PanCardNumber,
                    data.PassportNumber,
                    data.AadharCardNumber,
                    data.BankAccountName,
                    data.BankAccountNumber,
                    data.BankName,
                    data.IFSCCode,
                    data.PreviousOrganisation,
                    data.WorkExprience,
                    data.EducationalQualification,
                    data.InstituteName,
                    data.ConfirmationDate.ToString("dd/MM/yyyy"),
                    data.RecruitmentName,
                    data.RecruitmentName,
                    data.FatherName,
                    data.PersonalEmailId,
                    data.ContactNumber,
                      data.DateOfBirth,
                    data.CurrentAddress,
                    data.PermanentAddress,
                    data.BiometricCode,
                    data.BloodGroup,
                     data.Gender,
                    data.MaritalStatus,
                    data.Region,
                    data.PIPStartDate,
                    data.PIPEndDate,
                      data.PIP,
                    data.WhatsAppNumber,
                    data.NoticePeriod,
                    data.SpouceName,
                    data.DateOfMairrage,
                     data.EmergencyNumber,
                    data.EmergencyRelationWithEmployee,
                    data.UANNumber,
                    data.ESICNew,
                    data.LeaveSupervisor,
                      data.IJPLocation,
                    data.ShiftTiming,
                    data.ConfirmationStatus,
                    data.Nationality,
                    data.PAndFBankAccountNumberx,
                     data.ESICPreviousNumber,
                    data.Induction,
                    data.IsActive,
                    data.VISANumber,
                    data.VISADate,
                     data.TaxFileNumber,
                    data.SupernationAccountNumber,
                    data.SwiftCode,
                    data.RoutingCode,
                    data.AlternateMobileNumber,
                      data.BranchOfficeId,
                    data.ExitDate,
                    data.HolidayGroupId,
                    data.IsESICEligible,
                    data.LandLineNumber,
                     data.LeaveApprover1,
                    data.LeaveApprover2






                    );
            }

            using XLWorkbook wb = new XLWorkbook();
            wb.Worksheets.Add(dt);
            using MemoryStream stream = new MemoryStream();
            wb.SaveAs(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "EemployeeDetail.xlsx");
        }
        public async Task<IActionResult> DownloadEmployeeByBranch(string BranchName)
        {
            var response = await _IEmployeeDetailRepository.GetAllEntities(x => x.BranchOfficeId.Trim().ToLower() == BranchName.Trim().ToLower());
            DataTable dt = new DataTable("Employee");
            dt.Columns.AddRange(new DataColumn[72] {
                    new DataColumn("Salutation"),
                    new DataColumn("EmployeeName"),
                    new DataColumn("EmpCode"),
                    new DataColumn("joining Date"),
                    new DataColumn("Employeement Status"),
                    new DataColumn("office EmailId"),
                    new DataColumn("Department"),
                    new DataColumn("Designation"),
                    new DataColumn("Location"),
                    new DataColumn("LegalEntity"),
                     new DataColumn("PnlHeadName"),
                    new DataColumn("SuperVisorCode"),
                    new DataColumn("Level"),
                    new DataColumn("PANCardNumber"),
                    new DataColumn("PassportNumber"),
                    new DataColumn("AadharNumber"),
                    new DataColumn("NameonBankAccount"),
                    new DataColumn("BankAccountNumber"),
                    new DataColumn("BankName"),
                    new DataColumn("IFSCCode"),
                    new DataColumn("PreviousOrganisation"),
                    new DataColumn("WorkExperience"),
                    new DataColumn("EducationalQualification"),
                    new DataColumn("InstituteName"),
                    new DataColumn("ConfirmationDate"),
                    new DataColumn("RecuritmentSource"),
                    new DataColumn("RequiterName"),
                    new DataColumn("FatherName"),
                    new DataColumn("PersonalEmialID"),
                    new DataColumn("ContactNumber"),
                     new DataColumn("DateofBirth"),
                    new DataColumn("CurrentAddress"),
                    new DataColumn("PermanentAddress"),
                    new DataColumn("BiomericCode"),
                    new DataColumn("BloodGroup"),
                    new DataColumn("Gender"),
                    new DataColumn("MaritalStatus"),
                    new DataColumn("Region"),
                    new DataColumn("PIPStartDate"),
                    new DataColumn("PIPEndDate"),
                     new DataColumn("PIP"),
                    new DataColumn("WhatsAppNo"),
                    new DataColumn("NoticePeriod"),
                    new DataColumn("SpouseName"),
                    new DataColumn("DateofMarrage"),
                    new DataColumn("EmergencyNumber"),
                    new DataColumn("EmergencyRelationWithEmployee"),
                    new DataColumn("UANNo"),
                    new DataColumn("ESICNew"),
                    new DataColumn("LeaveSupervisor"),
                      new DataColumn("IJPLocation"),
                    new DataColumn("ShiftTiming"),
                    new DataColumn("ConfirmationStatus"),
                    new DataColumn("Nationality"),
                    new DataColumn("PFBankAccountNumber"),
                    new DataColumn("ESICPreviousNumber"),
                    new DataColumn("Induction"),
                    new DataColumn("IsActive"),
                    new DataColumn("VisaNo"),
                    new DataColumn("VisaDate"),
                     new DataColumn("TaxFileNumber"),
                    new DataColumn("SupernationAccountNumber"),
                    new DataColumn("SwiftCode"),
                    new DataColumn("RoutingCode"),
                    new DataColumn("alternateMobileNumber"),
                    new DataColumn("branchOfficeId"),
                    new DataColumn("exitDate"),
                    new DataColumn("holidayGroupId"),
                    new DataColumn("isEsicEligible"),
                    new DataColumn("landLineNo"),
                     new DataColumn("leaveApprover1"),
                    new DataColumn("leaveApprover2"),

            });

            foreach (var data in response.Entities)
            {
                dt.Rows.Add(
                    data.Salutation,
                    data.EmployeeName,
                    data.EmpCode,
                    data.JoiningDate.ToString("dd/MM/yyyy"),
                    data.EmployementStatus,
                    data.OfficeEmailId,
                    data.DepartmentName,
                    data.DesignationName,
                    data.Location,
                    data.LegalEntity,
                    data.PAndLHeadName,
                    data.SuperVisorCode,
                    data.Level,
                    data.PanCardNumber,
                    data.PassportNumber,
                    data.AadharCardNumber,
                    data.BankAccountName,
                    data.BankAccountNumber,
                    data.BankName,
                    data.IFSCCode,
                    data.PreviousOrganisation,
                    data.WorkExprience,
                    data.EducationalQualification,
                    data.InstituteName,
                    data.ConfirmationDate.ToString("dd/MM/yyyy"),
                    data.RecruitmentName,
                    data.RecruitmentName,
                    data.FatherName,
                    data.PersonalEmailId,
                    data.ContactNumber,
                      data.DateOfBirth,
                    data.CurrentAddress,
                    data.PermanentAddress,
                    data.BiometricCode,
                    data.BloodGroup,
                     data.Gender,
                    data.MaritalStatus,
                    data.Region,
                    data.PIPStartDate,
                    data.PIPEndDate,
                      data.PIP,
                    data.WhatsAppNumber,
                    data.NoticePeriod,
                    data.SpouceName,
                    data.DateOfMairrage,
                     data.EmergencyNumber,
                    data.EmergencyRelationWithEmployee,
                    data.UANNumber,
                    data.ESICNew,
                    data.LeaveSupervisor,
                      data.IJPLocation,
                    data.ShiftTiming,
                    data.ConfirmationStatus,
                    data.Nationality,
                    data.PAndFBankAccountNumberx,
                     data.ESICPreviousNumber,
                    data.Induction,
                    data.IsActive,
                    data.VISANumber,
                    data.VISADate,
                     data.TaxFileNumber,
                    data.SupernationAccountNumber,
                    data.SwiftCode,
                    data.RoutingCode,
                    data.AlternateMobileNumber,
                      data.BranchOfficeId,
                    data.ExitDate,
                    data.HolidayGroupId,
                    data.IsESICEligible,
                    data.LandLineNumber,
                     data.LeaveApprover1,
                    data.LeaveApprover2






                    );
            }

            using XLWorkbook wb = new XLWorkbook();
            wb.Worksheets.Add(dt);
            using MemoryStream stream = new MemoryStream();
            wb.SaveAs(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "EemployeeDetail.xlsx");
        }
    }
}
