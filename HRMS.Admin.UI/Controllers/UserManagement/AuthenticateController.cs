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

namespace HRMS.Admin.UI.Controllers.UserManagement
{
    public class AuthenticateController : Controller
    {
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepository;
        private readonly IGenericRepository<AuthenticateUser, int> _IAuthenticateRepository;
        public AuthenticateController(IGenericRepository<EmployeeDetail, int> iEmployeeDetailRepository, IGenericRepository<AuthenticateUser, int> authenticateRepo)
        {
            _IEmployeeDetailRepository = iEmployeeDetailRepository;
            _IAuthenticateRepository = authenticateRepo;
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
                HttpContext.Session.SetObjectAsJson("UserDetail", employeeDetails.Entities.First());
                HttpContext.Session.SetString("UserName", employeeDetails.Entities.First().EmployeeName);
                HttpContext.Session.SetString("EmployeeId", employeeDetails.Entities.First().Id.ToString());
                return RedirectToAction("Index", "Home");
            }
            string message = "Invalid Login credential !!!";
            return RedirectToAction("LoginIndex","Authenticate",new { message =message});
        }

        [HttpGet]
        public async Task<IActionResult> UserRegistration()
        {
            return PartialView();

        }


        #region PrivateMethods

        private async Task PopulateViewBag()
        {
            ViewBag.EmployeeDetails = (await _IEmployeeDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted)).Entities;
        }

        #endregion
    }
}
