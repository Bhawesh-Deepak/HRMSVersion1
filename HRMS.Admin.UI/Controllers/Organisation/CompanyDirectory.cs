using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Organisation;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Organisation;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Organisation
{
    public class CompanyDirectory : Controller
    {
        private readonly IGenericRepository<Company, int> _ICompanyRepository;
        private readonly IGenericRepository<Subsidiary, int> _ISubsidiaryRepository;
        private readonly IGenericRepository<Branch, int> _IBranchRepository;
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepository;

        public CompanyDirectory(IGenericRepository<Company, int> companyRepository, IGenericRepository<Subsidiary, int> SubsidiaryRepository,
             IGenericRepository<Branch, int> BranchRepository,
             IGenericRepository<EmployeeDetail, int> EmployeeDetailRepository)
        {
            _ICompanyRepository = companyRepository;
            _ISubsidiaryRepository = SubsidiaryRepository;
            _IBranchRepository = BranchRepository;
            _IEmployeeDetailRepository = EmployeeDetailRepository;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["Company Directory"];
            try
            {
                var Subsidryresponse = new DBResponseHelper<Subsidiary, int>()
                    .GetDBResponseHelper(await _ISubsidiaryRepository
                    .GetAllEntities(x => x.IsActive && !x.IsDeleted));

                var Companyresponse = new DBResponseHelper<Company, int>()
                  .GetDBResponseHelper(await _ICompanyRepository
                  .GetAllEntities(x => x.IsActive && !x.IsDeleted));

                var response = from subsidry in Subsidryresponse.Item2.Entities
                               join company in Companyresponse.Item2.Entities
                               on subsidry.OrganisationId equals company.Id
                               select new SubsidiaryVM
                               {
                                   Id = subsidry.Id,
                                   CompanyName = company.Name,
                                   Name = subsidry.Name,
                                   Code = subsidry.Code,
                                   Email = subsidry.Email,
                                   Url = subsidry.Url,
                                   FavIcon = subsidry.FavIcon,
                                   Logo = subsidry.Logo
                               };

                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("CompanyDirectory", "CompanyDirectoryIndex"), response));

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CompanyDirectory)} action name {nameof(Index)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }
        public async Task<IActionResult> GetBranchList(int subsidiaryId)
        {
            try
            {
                var branchList = await _IBranchRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var subsidiaryList = await _ISubsidiaryRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var response = from subsidry in subsidiaryList.Entities
                               join branch in branchList.Entities
                               on subsidry.Id equals branch.CompanyId
                               where branch.CompanyId == subsidiaryId
                               select new BranchDirectoryVM
                               {
                                   SubsidiaryId = subsidry.Id,
                                   SubsidiaryName = subsidry.Name,
                                   BranchId = branch.Id,
                                   BranchName = branch.Name,
                               };
                response.ToList().ForEach(item =>
                {

                    item.TotalEmployee = _IEmployeeDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.BranchOfficeId.Trim() == item.BranchName.Trim()).Result.Entities.Count();
                });
                return PartialView(ViewHelper.GetViewPathDetails("CompanyDirectory", "GetBranchDetails"), response);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CompanyDirectory)} action name {nameof(GetBranchList)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }

    }
}
