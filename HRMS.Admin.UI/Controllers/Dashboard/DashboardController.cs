using HRMS.Admin.UI.AuthenticateService;
using HRMS.Core.Entities.Organisation;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Organisation;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Dashboard
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class DashboardController : Controller
    {
        private readonly IGenericRepository<Subsidiary, int> _ISubsidiaryRepository;
        private readonly IGenericRepository<Company, int> _ICompanyRepository;

        public DashboardController(IGenericRepository<Subsidiary, int> subsidryRepository, IGenericRepository<Company, int> companyRepository)
        {
            _ISubsidiaryRepository = subsidryRepository;
            _ICompanyRepository = companyRepository;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var subsidiarymodel = await _ISubsidiaryRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var companymodel = await _ICompanyRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var response = (from subsidiary in subsidiarymodel.Entities
                                join company in companymodel.Entities
                                on subsidiary.OrganisationId equals company.Id
                                select new SubsidiaryVM
                                {
                                    Id = subsidiary.Id,
                                    CompanyName = company.Name,
                                    CompanyLogo = company.Logo,
                                    CompanyCode = company.Code,
                                    Name = subsidiary.Name,
                                    Code = subsidiary.Code,
                                    Logo = subsidiary.Logo
                                }).ToList();

                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Dashboard", "Dashboard"), response));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(DashboardController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> SetSessionValue(int id)
        {
            try
            {
                await Task.Run(() =>
                {
                    HttpContext.Session.SetString("SubsidryId", id.ToString());
                    var options = new CookieOptions { Expires = DateTime.Now.AddHours(36) };
                    Response.Cookies.Append("Id", id.ToString(), options);
                });

                return Json(true);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(DashboardController)} action name {nameof(SetSessionValue)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
