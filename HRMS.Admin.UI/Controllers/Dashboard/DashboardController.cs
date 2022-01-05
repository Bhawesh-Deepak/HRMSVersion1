using HRMS.Core.Entities.Organisation;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Organisation;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Dashboard
{
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
                                CompanyCode=company.Code,
                                Name = subsidiary.Name,
                                Code = subsidiary.Code,
                                Logo = subsidiary.Logo
                            }).ToList();

            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Dashboard", "Dashboard"), response));
        }
    }
}
