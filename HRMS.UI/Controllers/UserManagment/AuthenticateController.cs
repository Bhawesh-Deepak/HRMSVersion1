using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using HRMS.Core.ReqRespVm.Response.UserManagement;
using System.Text;
using System.Net.Http.Headers;
using HRMS.UI.Helpers;
using Microsoft.AspNetCore.Http;

namespace HRMS.UI.Controllers.UserManagment
{
    public class AuthenticateController : Controller
    {
        private readonly string APIURL = string.Empty;
        private readonly string AdminURL = string.Empty;
        private readonly string CompanyImageURL = string.Empty;
        public AuthenticateController(IConfiguration configuration)
        {
            APIURL = configuration.GetSection("APIURL").Value;
            AdminURL = configuration.GetSection("AdminPortalURL").Value;
            CompanyImageURL = configuration.GetSection("CompanyImageURL").Value;
            
        }
        public async Task<IActionResult> Index()
        {
            try
            {

                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Authenticate", "_Authenticate")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AuthenticateController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> LoginPost(AuthenticateModel model, string returnurl)
        {
            try
            {
                if (string.IsNullOrEmpty(model.UserName))
                {
                    model.UserName = Request.Cookies["UserName"];
                }
                using HttpClient client = new HttpClient { BaseAddress = new Uri(APIURL) };
                var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/HRMS/Authenticate/Authenticate", stringContent);
                if (response.IsSuccessStatusCode)
                {
                    var responseDetails = await response.Content.ReadAsStringAsync();
                    var responseData  = JsonConvert.DeserializeObject<UserAuthenticateVM>(responseDetails);
                    /// var responseData = await response.Content.ReadFromJsonAsync<UserAuthenticateVM>();
                    if (responseData.IsSucess)
                    {
                         responseData.EmployeeDetail.ToList().ForEach(item =>
                        {
                            if (string.IsNullOrEmpty(item.ProfilePic))
                            {
                                item.ProfilePic = "/A2Z-contents/PropertyImageNotFound.png";
                            }
                            else
                            {
                                item.ProfilePic = AdminURL + item.ProfilePic;
                            }

                        });
                         responseData.CompanyDetail.ToList().ForEach(item =>
                        {
                            if (string.IsNullOrEmpty(item.Logo))
                            {
                                item.Logo = "/A2Z-contents/PropertyImageNotFound.png";
                            }
                            else
                            {
                                item.Logo = AdminURL + item.Logo;
                                item.FavIcon = AdminURL + item.FavIcon;
                            }

                        });
                        HttpContext.Session.SetObjectAsJson("companyDetails", responseData.CompanyDetail.FirstOrDefault());
                        HttpContext.Session.SetObjectAsJson("UserDetail", responseData.EmployeeDetail.FirstOrDefault());
                        HttpContext.Session.SetString("UserName", responseData.EmployeeDetail.FirstOrDefault().EmployeeName);
                        HttpContext.Session.SetString("EmployeeId", responseData.EmployeeDetail.FirstOrDefault().Id.ToString());
                        HttpContext.Session.SetString("RoleId", responseData.AuthenticateUser.RoleId.ToString());
                        HttpContext.Session.SetString("financialYearId", responseData.AssesmentYearDetail.Id.ToString());
                        HttpContext.Session.SetString("financialYear", responseData.AssesmentYearDetail.Name.ToString());
                        HttpContext.Session.SetString("EmpCode", responseData.EmployeeDetail.FirstOrDefault().EmpCode.ToString());
                       

                        if (!string.IsNullOrEmpty(returnurl))
                        {
                            return Redirect(returnurl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                       
                    }
                    else
                    {
                        return RedirectToAction("Index", "Authenticate", new { message = responseData.Message });
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Authenticate", new { message = "there are some technical error contact admin.."});
                }
                
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AuthenticateController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
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
        public async Task<IActionResult> ForgetPassword(string empCode)
        {
            try
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);

                    var responseTask = await client.GetAsync("api/HRMS/Authenticate/ForgetPassword?empCode=" + empCode);
                    return Json(responseTask.IsSuccessStatusCode);
                }
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

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(APIURL);
                    var responseTask = await client.GetAsync("/api/HRMS/Authenticate/ForgetPasswordPost?otpCode=" + otpCode + "&empCode=" + empCode + "&NewPassword=" + NewPassword);
                    return Json(responseTask.IsSuccessStatusCode);
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AuthenticateController)} action name {nameof(ForgetPassword)} exception is {ex.Message}";
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
                return RedirectToAction("Index", "Authenticate");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AuthenticateController)} action name {nameof(Logout)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

    }
}
