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
using System.Net.Http;

namespace HRMS.Admin.UI.Controllers.UserManagement
{
    public class AuthenticateController : Controller
    {
        private const string BASEURL = "http://smsinteract.in/";
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepository;
        private readonly IGenericRepository<AdminEmployeeDetail, int> _IAdminEmployeeDetailRepository;
        private readonly IGenericRepository<AuthenticateUser, int> _IAuthenticateRepository;
        private readonly IGenericRepository<Company, int> _ICompanyRepository;

        public AuthenticateController(IGenericRepository<EmployeeDetail, int> employeeDetailRepository, IGenericRepository<AdminEmployeeDetail, int> adminemployeeDetailRepository,
            IGenericRepository<AuthenticateUser, int> authenticateRepo, IGenericRepository<Company, int> companyRepository)
        {
            _IEmployeeDetailRepository = employeeDetailRepository;
            _IAdminEmployeeDetailRepository = adminemployeeDetailRepository;
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
               
                var companyDetail = await _ICompanyRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

               
                //Dashboard page will open for admin and super admin role only
                // roleId=1 For SuperAdmin :: roleId=2 for admin

                if (response.Entities.First().RoleId == 1)
                {
                    var employeeDetails = await _IAdminEmployeeDetailRepository.GetAllEntities(x => x.Id == response.Entities.First().EmployeeId);
                    HttpContext.Session.SetObjectAsJson("companyDetails", companyDetail.Entities.First());
                    HttpContext.Session.SetObjectAsJson("UserDetail", employeeDetails.Entities.First());
                    HttpContext.Session.SetString("UserName", employeeDetails.Entities.First().EmployeeName);
                    HttpContext.Session.SetString("EmployeeId", employeeDetails.Entities.First().Id.ToString());
                    HttpContext.Session.SetString("RoleId", response.Entities.First().RoleId.ToString());

                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    var employeeDetails = await _IEmployeeDetailRepository.GetAllEntities(x => x.Id == response.Entities.First().EmployeeId);
                    HttpContext.Session.SetObjectAsJson("companyDetails", companyDetail.Entities.First());
                    HttpContext.Session.SetObjectAsJson("UserDetail", employeeDetails.Entities.First());
                    HttpContext.Session.SetString("UserName", employeeDetails.Entities.First().EmployeeName);
                    HttpContext.Session.SetString("EmployeeId", employeeDetails.Entities.First().Id.ToString());
                    HttpContext.Session.SetString("RoleId", response.Entities.First().RoleId.ToString());
                    return RedirectToAction("Index", "Home");
                }
               
            }
            string message = "Invalid Login credential !!!";
            return RedirectToAction("LoginIndex", "Authenticate", new { message = message });
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await Task.Run(() => HttpContext.Session.Clear());
            return RedirectToAction("LoginIndex", "Authenticate");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {

            return await Task.Run(() => PartialView("~/Views/Authenticate/ChangePassword.cshtml"));
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordPost(ChangePasswordVm model)
        {
            try
            {
                var empId = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                var oldPassword = PasswordEncryptor.Instance.Encrypt(model.OldPassword, "HRMSPAYROLLPASSWORDKEY");
                var newPassword = PasswordEncryptor.Instance.Encrypt(model.NewPassword, "HRMSPAYROLLPASSWORDKEY");

                var response = await _IAuthenticateRepository.GetAllEntities(x => x.EmployeeId == empId && x.Password.Trim().ToLower() == oldPassword.Trim().ToLower());
                if (response.Entities.Any())
                {
                    var authModel = await _IAuthenticateRepository.GetAllEntities(x => x.EmployeeId == empId);
                    authModel.Entities.First().Password = newPassword;

                    var updateResponse = await _IAuthenticateRepository.UpdateMultipleEntity(authModel.Entities.ToArray());

                    return Json(updateResponse.Message);
                }
                return Json("Old Password is not macthed, Please enter valid password !");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AuthenticateController)} action name {nameof(ChangePassword)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }


        }

        public async Task<IActionResult> ForgetPassword(string empCode)
        {
            try
            {
                var empDetails = await _IEmployeeDetailRepository.GetAllEntities(x => x.EmpCode.Trim().ToLower() == empCode.Trim().ToLower());

                var randomOtp = GetRandomOtp();

                if (empDetails != null && empDetails.Entities.Any())
                {
                    var updateResponse = await UpdateEmpForgetPasswordOtp(randomOtp, empCode);

                    if (updateResponse)
                    {
                        var message = GetOtpMessage(empDetails.Entities.First(), randomOtp);

                        var sentOtpStatus = await SendOtp(empDetails.Entities.First(), message);

                        return Json(sentOtpStatus);
                    }

                    return Json(false);
                }

                return Json(false);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return Json(false);
            }
        }

        #region PrivateMethod
        public async Task<bool> SendOtp(EmployeeDetail empDetail, string message)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(BASEURL);
            var response = await client.GetAsync("SMSApi/send?userid=klbsotp&password=Klb@2020&sendMethod=quick&mobile=" + empDetail?.ContactNumber + "&msg=" + message + "&senderid=MODOTP&msgType=text&duplicatecheck=true&format=text");
            return response.IsSuccessStatusCode;
        }

        private string GetRandomOtp()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var stringChars = new char[5];

            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }

        public string GetOtpMessage(EmployeeDetail empDetail, string randomPassword) =>
             $"Dear {empDetail?.EmployeeName}.  Your OTP is {randomPassword}" +
                $" Do not share with any one for security. Regards Square HR.";

        private async Task<bool> UpdateEmpForgetPasswordOtp(string otpCode, string empCode)
        {
            if (!string.IsNullOrEmpty(empCode))
            {
                var authDetails = await _IAuthenticateRepository.GetAllEntities(x => x.UserName.Trim().ToLower() == empCode.Trim().ToLower());

                if (authDetails != null && authDetails.Entities.Any())
                {
                    var updateModel = authDetails.Entities.First();
                    updateModel.ForgetPasswordCode = otpCode;
                    updateModel.ForgetPasswordTime = DateTime.Now;
                    updateModel.UpdatedBy = 1;
                    updateModel.UpdatedDate = DateTime.Now;

                    var updateResponse = await _IAuthenticateRepository.UpdateEntity(updateModel);

                    return updateResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Updated;
                }

                return false;
            }
            return false;

        }

        #endregion
    }
}
