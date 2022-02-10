using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Entities.Organisation;
using HRMS.Core.Helpers.BlobHelper;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Master;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Master
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class BranchController : Controller
    {
        private readonly IGenericRepository<Branch, int> _IBranchRepository;
        private readonly IGenericRepository<LegalEntity, int> _ISubsidiaryRepository;
        private readonly IGenericRepository<LocationType, int> _ILocationTypeRepository;
        private readonly IGenericRepository<RegionMaster, int> _IRegionMasterRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;

        public BranchController(IGenericRepository<Branch, int> BranchRepo, IHostingEnvironment hostingEnvironment,
            IGenericRepository<LegalEntity, int> SubsidiaryRepo,
             IGenericRepository<LocationType, int> LocationTypeRepo,
             IGenericRepository<RegionMaster, int> RegionMasterRepo)
        {
            _IBranchRepository = BranchRepo;
            _ISubsidiaryRepository = SubsidiaryRepo;
            _IHostingEnviroment = hostingEnvironment;
            _ILocationTypeRepository = LocationTypeRepo;
            _IRegionMasterRepository = RegionMasterRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.HeaderTitle = PageHeader.HeaderSetting["BranchIndex"];
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Branch", "BranchIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Branch)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetBranchList()
        {
            try
            {
                var BranchList = await _IBranchRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var CompanyList = await _ISubsidiaryRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var regionList = await _IRegionMasterRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var LocationTypeList = await _ILocationTypeRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

                var responseDetails = (from company in CompanyList.Entities
                                       join branch in BranchList.Entities on company.Id equals branch.CompanyId
                                       join locationtype in LocationTypeList.Entities on branch.LocationTypeId equals locationtype.Id
                                       join regions in regionList.Entities on branch.RegionId equals regions.Id
                                       select new BranchVM
                                       {
                                           Id = branch.Id,
                                           CompanyName = company.Name,
                                           Name = branch.Name,
                                           Code = branch.Code,
                                           RegionName = regions.Name,
                                           LocationTypeName = locationtype.Name,

                                       }).ToList();

                return PartialView(ViewHelper.GetViewPathDetails("Branch", "BranchDetails"), responseDetails);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Branch)} action name {nameof(GetBranchList)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> GetLegalEntity(int id)
        {
            try
            {
                var response = await _ISubsidiaryRepository.GetAllEntities(x => x.Id == id);
                return Json(response.Entities.FirstOrDefault());
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Branch)} action name {nameof(CreateBranch)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> CreateBranch(int id)
        {
            try
            {
                await PopulateViewBag();

                if (id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("Branch", "BranchCreate"));
                }
                else
                {
                    var response = await _IBranchRepository.GetAllEntities(x => x.Id == id);
                    return PartialView(ViewHelper.GetViewPathDetails("Branch", "BranchCreate"), response.Entities.First());
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Branch)} action name {nameof(CreateBranch)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertBranch(Branch model)
        {
            try
            {

                if (model.Id == 0)
                {
                    model.FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId"));
                    model.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                    model.CreatedDate = DateTime.Now;

                    var response = await _IBranchRepository.CreateEntity(model);
                    return Json(response.Message);
                }
                else
                {
                    model.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                    model.UpdatedDate = DateTime.Now;
                    var response = await _IBranchRepository.UpdateEntity(model);
                    return Json(response.Message);
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Branch)} action name {nameof(UpsertBranch)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            try
            {
                var deleteModel = await _IBranchRepository.GetAllEntityById(x => x.Id == id);
                var deleteDbModel = CrudHelper.DeleteHelper<Branch>(deleteModel.Entity, 1);
                var deleteResponse = await _IBranchRepository.DeleteEntity(deleteDbModel);
                if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
                {
                    return Json(deleteResponse.Message);
                }
                return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Branch)} action name {nameof(DeleteBranch)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var legalEntityResponse = await _ISubsidiaryRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var locationtypeResponse = await _ILocationTypeRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var regionResponse = await _IRegionMasterRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            if (legalEntityResponse.ResponseStatus == ResponseStatus.Success && locationtypeResponse.ResponseStatus == ResponseStatus.Success
                && regionResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.LegalEntityList = legalEntityResponse.Entities;
            ViewBag.LocationTypeList = locationtypeResponse.Entities;
            ViewBag.RegionList = regionResponse.Entities;

        }

        #endregion
    }
}
