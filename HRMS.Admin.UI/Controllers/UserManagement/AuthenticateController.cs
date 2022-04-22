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
using HRMS.Admin.UI.AuthenticateService;
using HRMS.Core.Entities.Master;
using HRMS.Core.ReqRespVm.RequestVm.NeedSupport;
using System.Net.Mail;
using MimeKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace HRMS.Admin.UI.Controllers.UserManagement
{

    public class AuthenticateController : Controller
    {
        private const string BASEURL = "http://smsinteract.in/";
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepository;
        private readonly IGenericRepository<AdminEmployeeDetail, int> _IAdminEmployeeDetailRepository;
        private readonly IGenericRepository<AuthenticateUser, int> _IAuthenticateRepository;
        private readonly IGenericRepository<Company, int> _ICompanyRepository;
        private readonly IGenericRepository<AssesmentYear, int> _IAssesmentYearRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;
        private readonly string IMAGEURL = string.Empty;
        private readonly IConfiguration _configuration;
        public AuthenticateController(IGenericRepository<EmployeeDetail, int> employeeDetailRepository, IGenericRepository<AdminEmployeeDetail, int> adminemployeeDetailRepository,
            IGenericRepository<AuthenticateUser, int> authenticateRepo, IGenericRepository<Company, int> companyRepository,
            IGenericRepository<AssesmentYear, int> assesmentyearRepository, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _IEmployeeDetailRepository = employeeDetailRepository;
            _IAdminEmployeeDetailRepository = adminemployeeDetailRepository;
            _IAuthenticateRepository = authenticateRepo;
            _ICompanyRepository = companyRepository;
            _IAssesmentYearRepository = assesmentyearRepository;
            _IHostingEnviroment = hostingEnvironment;
            IMAGEURL = configuration.GetSection("IMAGEURL").Value;
        }

        public async Task<IActionResult> LoginIndex(string message)
        {
            try
            {
                ViewBag.Message = message;
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Authenticate", "Authenticate")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AuthenticateController)} action name {nameof(LoginIndex)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AuthenticateModel model, string returnurl)
        {
            try
            {
                if (string.IsNullOrEmpty(model.UserName))
                {
                    model.UserName = Request.Cookies["UserName"];
                }

                model.Password = PasswordEncryptor.Instance.Encrypt(model.Password, "HRMSPAYROLLPASSWORDKEY");
                var response = await _IAuthenticateRepository.GetAllEntities(x => x.UserName.ToLower().Trim() == model.UserName.Trim().ToLower()
                && x.Password.Trim().ToLower() == model.Password.Trim().ToLower());

                if (response.Entities.Any())
                {

                    var companyDetail = await _ICompanyRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                    companyDetail.Entities.ToList().ForEach(data => {
                        data.Logo = IMAGEURL + data.Logo;
                    });
                    var assesmentYear = await _IAssesmentYearRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.isCurrentFinancialYear);
                    await SetCookies(model.UserName, "UserName");
                    if (response.Entities.First().RoleId == 1)
                    {

                        var employeeDetails = await _IAdminEmployeeDetailRepository.GetAllEntities(x => x.Id == response.Entities.First().EmployeeId);
                        HttpContext.Session.SetObjectAsJson("companyDetails", companyDetail.Entities.First());
                        HttpContext.Session.SetObjectAsJson("UserDetail", employeeDetails.Entities.First());
                        HttpContext.Session.SetString("UserName", employeeDetails.Entities.First().EmployeeName);
                        HttpContext.Session.SetString("EmployeeId", employeeDetails.Entities.First().Id.ToString());
                        HttpContext.Session.SetString("RoleId", response.Entities.First().RoleId.ToString());
                        HttpContext.Session.SetString("financialYearId", assesmentYear.Entities.First().Id.ToString());
                        HttpContext.Session.SetString("financialYear", assesmentYear.Entities.First().Name.ToString());
                        HttpContext.Session.SetString("EmpCode", employeeDetails.Entities.First().EmpCode.ToString());
                        if (!string.IsNullOrEmpty(returnurl))
                        {
                            return Redirect(returnurl);
                        }

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
                        HttpContext.Session.SetString("financialYearId", assesmentYear.Entities.First().Id.ToString());
                        HttpContext.Session.SetString("financialYear", assesmentYear.Entities.First().Name.ToString());
                        HttpContext.Session.SetString("EmpCode", employeeDetails.Entities.First().EmpCode.ToString());
                        if (!string.IsNullOrEmpty(returnurl))
                        {
                            return Redirect(returnurl);
                        }

                        return RedirectToAction("Index", "CommingSoon");
                    }

                }
                string message = "Invalid Login credential !!!";
                return RedirectToAction("LoginIndex", "Authenticate", new { message = message });
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AuthenticateController)} action name {nameof(Login)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetLoginPopUp(string returnUrl)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;
                return await Task.Run(() => PartialView("~/Views/Authenticate/_LoginPopUp.cshtml"));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AuthenticateController)} action name {nameof(GetLoginPopUp)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await Task.Run(() => HttpContext.Session.Clear());
                return RedirectToAction("LoginIndex", "Authenticate");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AuthenticateController)} action name {nameof(Logout)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            try
            {
                return await Task.Run(() => View("~/Views/Authenticate/ChangePassword.cshtml"));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AuthenticateController)} action name {nameof(ChangePassword)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
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
                string template = $"Controller name {nameof(AuthenticateController)} action name {nameof(ChangePasswordPost)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }


        }

        public async Task<IActionResult> ForgetPassword(string empCode)
        {
            try
            {
                var empDetails = await _IEmployeeDetailRepository.GetAllEntities(x => x.EmpCode.Trim().ToLower() == empCode.Trim().ToLower());
                var companyDetail = await _ICompanyRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var randomOtp = GetRandomOtp();

                if (empDetails != null && empDetails.Entities.Any())
                {
                    var updateResponse = await UpdateEmpForgetPasswordOtp(randomOtp, empCode);

                    if (updateResponse)
                    {
                        var message = GetOtpMessage(empDetails.Entities.First(), randomOtp, companyDetail.Entities.First());

                        var sentOtpStatus = await SendOtp(empDetails.Entities.First(), message);

                        return Json(sentOtpStatus);
                    }

                    return Json(false);
                }

                return Json(false);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AuthenticateController)} action name {nameof(ForgetPassword)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> ForgetPasswordPost(string otpCode, string empCode, string NewPassword)
        {
            try
            {
                var empDetails = await _IEmployeeDetailRepository.GetAllEntities(x => x.EmpCode.Trim().ToLower() == empCode.Trim().ToLower());
                var response = await _IAuthenticateRepository.GetAllEntities(x => x.EmployeeId == empDetails.Entities.First().Id && x.ForgetPasswordCode.Trim().ToLower() == otpCode.Trim().ToLower());
                if (response.Entities.Any())
                {
                    var newPassword = PasswordEncryptor.Instance.Encrypt(NewPassword, "HRMSPAYROLLPASSWORDKEY");
                    var authModel = await _IAuthenticateRepository.GetAllEntities(x => x.EmployeeId == empDetails.Entities.First().Id);
                    authModel.Entities.First().Password = newPassword;
                    authModel.Entities.First().ForgetPasswordCode = null;
                    authModel.Entities.First().ForgetPasswordTime = null;
                    authModel.Entities.First().UpdatedBy = empDetails.Entities.First().Id;
                    authModel.Entities.First().UpdatedDate = DateTime.Now;
                    var updateResponse = await _IAuthenticateRepository.UpdateMultipleEntity(authModel.Entities.ToArray());
                }

                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        #region PrivateMethod
        public async Task<bool> SendOtp(EmployeeDetail empDetail, string message)
        {
            empDetail.ContactNumber = empDetail.ContactNumber;//"9560435849";
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

        public string GetOtpMessage(EmployeeDetail empDetail, string randomPassword,Company company) =>
             $"Dear {empDetail?.EmployeeName}.  Your OTP is {randomPassword}" +
                $" Do not share with any one for security. Regards {company.Name}.";

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


        private async Task SetCookies(string value, string key)
        {
            try
            {
                await Task.Run(() =>
                {
                    var options = new CookieOptions { Expires = DateTime.Now.AddHours(36) };
                    Response.Cookies.Append(key, value, options);

                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion
        #region  Need Help Method
        public async Task<IActionResult> NeedHelp()
        {
            try
            {
                return PartialView(ViewHelper.GetViewPathDetails("Authenticate", "_NeedHelpIndex"));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AuthenticateController)} action name {nameof(NeedHelp)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> NeedHelpPost(NeedSupportVM model)
        {
            //try
            //{
            //    var isMailSend = false;
            //    try
            //    {

            //        SmtpClient client = new SmtpClient();
            //        MimeMessage message = new MimeMessage();

            //        MailboxAddress from = new MailboxAddress("Sunil","");
            //        message.From.Add(from);

            //        MailboxAddress to = new MailboxAddress("User",
            //        "BhaweshDeepak@yahoo.com");

            //        List<MailboxAddress> toMailAddress = new List<MailboxAddress>();
            //        toEmailIds.ForEach(item =>
            //        {
            //            var userName = item.Split("@")[0];
            //            toMailAddress.Add(new MailboxAddress(userName, item.Trim()));
            //        });


            //        message.To.AddRange(toMailAddress);

            //        message.Subject = batchId;

            //        BodyBuilder bodyBuilder = new BodyBuilder();
            //        bodyBuilder.HtmlBody = emailTemplate;


            //        message.Body = bodyBuilder.ToMessageBody();

            //        try
            //        {

            //            client.Connect(_configuration.GetSection("EmailNotification:SMTPServer").Value,
            //                Convert.ToInt32(_configuration.GetSection("EmailNotification:SMTPPort").Value), SecureSocketOptions.StartTls);

            //            client.Authenticate("stcvat1@gmail.com", "vi@pra91");

            //            client.Send(message);
            //            isMailSend = true;

            //        }
            //        catch (Exception ex)
            //        {
            //            //await WriteEmailLoger(ex);
            //            isMailSend = false;
            //        }
            //        finally
            //        {
            //            client.Disconnect(true);
            //            client.Dispose();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //       // await WriteEmailLoger(ex);
            //        isMailSend = false;
            //    }

            //   // return await Task.Run(() => isMailSend);
            //    return PartialView(ViewHelper.GetViewPathDetails("Authenticate", "_NeedHelpIndex"));
            //}
            //catch (Exception ex)
            //{
            //    string template = $"Controller name {nameof(AuthenticateController)} action name {nameof(NeedHelp)} exception is {ex.Message}";
            //    Serilog.Log.Error(ex, template);
            //    return RedirectToAction("Error", "Home");
            //}
            return RedirectToAction("LoginIndex", "Authenticate");
        }
        #endregion
    }
}
