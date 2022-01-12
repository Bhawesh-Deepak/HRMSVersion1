using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Entities.UserManagement;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.Helpers.ExcelHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Payroll
{
    public class EmployeeSalaryController : Controller
    {
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepository;
        private readonly IGenericRepository<EmployeeSalary, int> _IEmployeeSalaryDetailRepository;
        private readonly IGenericRepository<AuthenticateUser, int> _IAuthenticateRepository;
        public EmployeeSalaryController(IGenericRepository<EmployeeDetail, int> employeeDetailRepo,
            IGenericRepository<EmployeeSalary, int> employeeSalaryDetailRepo, IGenericRepository<AuthenticateUser, int> authRepository)
        {
            _IEmployeeDetailRepository = employeeDetailRepo;
            _IEmployeeSalaryDetailRepository = employeeSalaryDetailRepo;
            _IAuthenticateRepository = authRepository;
        }
        public IActionResult Index()
        {
            return View(ViewHelper.GetViewPathDetails("Employee", "EmployeeSalaryIndex"));
        }
        [HttpPost]
        public async Task<IActionResult> UploadEmployeeSalary(UploadExcelVm model)
        {
            try
            {

                var response = new ReadEmployeeSalaryExcelHelper().GetEmployeeSalaryDetails(model.UploadFile);

                var employeeDetailResponse = await _IEmployeeDetailRepository.CreateEntities(response.EmployeeDetails.ToArray());

                var employeeSalaryReponse = await _IEmployeeSalaryDetailRepository.CreateEntities(response.EmployeeSalaryDetails.ToArray());

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
