using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.UI.Controllers.UserManagment
{
    public class ChangePasswordController : Controller
    {
        private readonly string APIURL = string.Empty;
        public ChangePasswordController(IConfiguration configuration)
        {
            APIURL = configuration.GetSection("APIURL").Value;

        }
        public async Task<IActionResult> Index()
        {
            try
            {

                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("ChangePassword", "_ChangePasswordIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(ChangePasswordController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ChangePasswordPost(ChangePasswordVm changePasswordVm)
        {
            try
            {
                changePasswordVm.EmpId=Convert.ToInt32( HttpContext.Session.GetString("EmployeeId"));
                using HttpClient client = new HttpClient { BaseAddress = new Uri(APIURL) };
                var stringContent = new StringContent(JsonConvert.SerializeObject(changePasswordVm), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/HRMS/Authenticate/ChangePasswordPost", stringContent);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Authenticate");
                }
                else
                {
                    return RedirectToAction("Index", "Authenticate");
                }
                
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(ChangePasswordController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
