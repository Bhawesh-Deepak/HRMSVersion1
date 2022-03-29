using HRMS.API.Helpers;
using HRMS.API.Models.Response;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Entities.Organisation;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Entities.UserManagement;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Core.ReqRespVm.Response.UserManagement;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.API.Controllers.UserManagement
{
    [Route("api/HRMS/[controller]/[action]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private const string BASEURL = "http://smsinteract.in/";
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepository;
        private readonly IGenericRepository<AdminEmployeeDetail, int> _IAdminEmployeeDetailRepository;
        private readonly IGenericRepository<AuthenticateUser, int> _IAuthenticateRepository;
        private readonly IGenericRepository<Company, int> _ICompanyRepository;
        private readonly IGenericRepository<AssesmentYear, int> _IAssesmentYearRepository;
        private readonly IConfiguration _configuration;

        public AuthenticateController(IGenericRepository<EmployeeDetail, int> employeeDetailRepository, IGenericRepository<AdminEmployeeDetail, int> adminemployeeDetailRepository,
          IGenericRepository<AuthenticateUser, int> authenticateRepo, IGenericRepository<Company, int> companyRepository,
          IGenericRepository<AssesmentYear, int> assesmentyearRepository, IConfiguration configuration)
        {
            _IEmployeeDetailRepository = employeeDetailRepository;
            _IAdminEmployeeDetailRepository = adminemployeeDetailRepository;
            _IAuthenticateRepository = authenticateRepo;
            _ICompanyRepository = companyRepository;
            _IAssesmentYearRepository = assesmentyearRepository;
            _configuration = configuration;
        }



        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> Authenticate(AuthenticateModel model)
        {
            try
            {
                model.Password = PasswordEncryptor.Instance.Encrypt(model.Password, "HRMSPAYROLLPASSWORDKEY");
                var response = await _IAuthenticateRepository.GetAllEntities(x => x.UserName.ToLower().Trim() == model.UserName.Trim().ToLower()
                && x.Password.Trim().ToLower() == model.Password.Trim().ToLower());

                if (response.Entities.Any())
                {

                    var companyDetail = await _ICompanyRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                    var assesmentYear = await _IAssesmentYearRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.isCurrentFinancialYear);
                    var employeeDetails = await _IEmployeeDetailRepository.GetAllEntities(x => x.Id == response.Entities.First().EmployeeId);
                    
                    var responseDeatils = new UserAuthenticateVM()
                    {
                       CompanyDetail=companyDetail.Entities.ToList(),
                       AssesmentYearDetail=assesmentYear.Entities.FirstOrDefault(),
                       EmployeeDetail=employeeDetails.Entities.ToList(),
                       AuthenticateUser=response.Entities.FirstOrDefault(),
                       IsSucess=true,
                       Message="Sucess"
                    };
                    return Ok(responseDeatils);
                  //  return await Task.Run(() => Ok(new Helpers.ResponseEntity<UserAuthenticateVM>
                  //(System.Net.HttpStatusCode.OK, ResponseStatus.Success.ToString(), responseDeatils).GetResponseEntity()));


                }
                else
                {
                    var responseDeatils = new UserAuthenticateVM()
                    {
                        
                        IsSucess = false,
                        Message = "Username and password does not match.."
                    };
                    return await Task.Run(() => Ok(new Helpers.ResponseEntity<UserAuthenticateVM>
                  (System.Net.HttpStatusCode.OK, ResponseStatus.Success.ToString(), responseDeatils).GetResponseEntity()));


                }

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AuthenticateController)} action name {nameof(Authenticate)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return Ok();
            }

        }
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> ForgetPassword(string empCode)
        {
            try
            {
                var empDetails = await _IEmployeeDetailRepository.GetAllEntities(x => x.EmpCode.Trim().ToLower() == empCode.Trim().ToLower());

                var randomOtp = new ForgotPasswordOTP().GetRandomOtp();

                if (empDetails != null && empDetails.Entities.Any())
                {
                    var authDetails = await _IAuthenticateRepository.GetAllEntities(x => x.UserName.Trim().ToLower() == empCode.Trim().ToLower());

                    if (authDetails != null && authDetails.Entities.Any())
                    {
                        var updateModel = authDetails.Entities.First();
                        updateModel.ForgetPasswordCode = randomOtp;
                        updateModel.ForgetPasswordTime = DateTime.Now;
                        updateModel.UpdatedBy = 1;
                        updateModel.UpdatedDate = DateTime.Now;
                        var updateResponse = await _IAuthenticateRepository.UpdateEntity(updateModel);
                        if (updateResponse.Message == "success")
                        {
                            var message = new ForgotPasswordOTP().GetOtpMessage(empDetails.Entities.First(), randomOtp);
                            var sentOtpStatus = new ForgotPasswordOTP().SendOtp(empDetails.Entities.First(), message, BASEURL);
                            return Ok(true);
                        }
                        else
                        {
                            return Ok(false);
                        }
                    }
                    else
                    {
                        return Ok(false);
                    }
                }
                else { return Ok(false); }

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AuthenticateController)} action name {nameof(ForgetPassword)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
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

                return Ok(true);
            }
            catch (Exception ex)
            {
                return Ok(false);
            }
        }
    }
}
