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
        private readonly IGenericRepository<LegalEntity, int> _ILegalEntityRepository;
        private readonly IGenericRepository<Company, int> _ICompanyRepository;

        public DashboardController(IGenericRepository<LegalEntity, int> LegalEntityRepository, IGenericRepository<Company, int> companyRepository)
        {
            _ILegalEntityRepository = LegalEntityRepository;
            _ICompanyRepository = companyRepository;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(HttpContext.Session.GetString("LegalEntityName"))))
                {
                    HttpContext.Session.Remove("LegalEntityName");
                    HttpContext.Session.Remove("LegalEntityId");
                }
                var legalentitymodel = await _ILegalEntityRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var companymodel = await _ICompanyRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var response = (from legalentity in legalentitymodel.Entities
                                join company in companymodel.Entities
                                on legalentity.OrganisationId equals company.Id
                                select new LegalEntityVM
                                {
                                    Id = legalentity.Id,
                                    CompanyName = company.Name,
                                    CompanyLogo = company.Logo,
                                    CompanyCode = company.Code,
                                    Name = legalentity.Name,
                                    Code = legalentity.Code,
                                    Logo = legalentity.Logo
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

        public async Task<IActionResult> SetSessionValue(int id, string Name)
        {
            try
            {
                await Task.Run(() =>
                {
                    HttpContext.Session.SetString("LegalEntityId", id.ToString());
                    HttpContext.Session.SetString("LegalEntityName", Name.ToString());
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
