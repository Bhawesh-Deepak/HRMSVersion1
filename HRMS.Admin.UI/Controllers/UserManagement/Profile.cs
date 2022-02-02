using HRMS.Admin.UI.AuthenticateService;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.UserManagement
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class Profile : Controller
    {
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepository;
        public Profile(IGenericRepository<EmployeeDetail, int> employeeDetailRepository)
        {
            _IEmployeeDetailRepository = employeeDetailRepository;

        }
        public async Task<IActionResult> Index()
        {
            try
            {
                int Sessionresponse = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                var response = await _IEmployeeDetailRepository.GetAllEntities(x => x.Id == Sessionresponse);
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Profile", "_EmployeeProfile"), response.Entities.FirstOrDefault()));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Profile)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
