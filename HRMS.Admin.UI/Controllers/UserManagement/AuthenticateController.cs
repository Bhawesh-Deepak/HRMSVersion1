using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Entities.UserManagement;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Admin.UI.Helpers;
using Microsoft.AspNetCore.Http;
using HRMS.Core.Entities.Organisation;

namespace HRMS.Admin.UI.Controllers.UserManagement
{
    public class AuthenticateController : Controller
    {
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepository;
        private readonly IGenericRepository<AuthenticateUser, int> _IAuthenticateRepository;
        private readonly IGenericRepository<Company, int> _ICompanyRepository;
        public AuthenticateController(IGenericRepository<EmployeeDetail, int> employeeDetailRepository,
            IGenericRepository<AuthenticateUser, int> authenticateRepo, IGenericRepository<Company, int> companyRepository)
        {
            _IEmployeeDetailRepository = employeeDetailRepository;
            _IAuthenticateRepository = authenticateRepo;
            _ICompanyRepository = companyRepository;
        }

        public async Task<IActionResult> LoginIndex(string message)
        {
            ViewBag.Message = message;
            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Authenticate", "Authenticate")));
        }
        [HttpPost]
        public async Task<IActionResult> Login(AuthenticateModel model)
        {
            model.Password = PasswordEncryptor.Instance.Encrypt(model.Password, "HRMSPAYROLLPASSWORDKEY");
            var response = await _IAuthenticateRepository.GetAllEntities(x => x.UserName.ToLower().Trim() == model.UserName.Trim().ToLower()
             && x.Password.Trim().ToLower() == model.Password.Trim().ToLower());

            if (response.Entities.Any())
            {
                var employeeDetails = await _IEmployeeDetailRepository.GetAllEntities(x => x.Id == response.Entities.First().EmployeeId);
                var companyDetail = await _ICompanyRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

                HttpContext.Session.SetObjectAsJson("companyDetails", companyDetail.Entities.First());
                HttpContext.Session.SetObjectAsJson("UserDetail", employeeDetails.Entities.First());
                HttpContext.Session.SetString("UserName", employeeDetails.Entities.First().EmployeeName);
                HttpContext.Session.SetString("EmployeeId", employeeDetails.Entities.First().Id.ToString());
                HttpContext.Session.SetString("RoleId", response.Entities.First().RoleId.ToString());

                //Dashboard page will open for admin and super admin role only
                // roleId=1 For SuperAdmin :: roleId=2 for admin
                if (response.Entities.First().RoleId == 1 || response.Entities.First().RoleId == 2)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                return RedirectToAction("Index", "Home");
            }
            string message = "Invalid Login credential !!!";
            return RedirectToAction("LoginIndex", "Authenticate", new { message = message });
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await Task.Run(()=> HttpContext.Session.Clear());
            return RedirectToAction("LoginIndex", "Authenticate");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            return RedirectToAction("LoginIndex", "Authenticate");

        }
    }
}
