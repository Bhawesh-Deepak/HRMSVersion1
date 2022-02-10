using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Master;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Core.Entities.Organisation;
using HRMS.Admin.UI.AuthenticateService;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using HRMS.Core.Helpers.BlobHelper;
using Microsoft.AspNetCore.Hosting;

namespace HRMS.Admin.UI.Controllers.Master
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class NewsAndUpdateController : Controller
    {
        private readonly IGenericRepository<NewsAndUpdate, int> _INewsAndUpdateRepository;
        private readonly IGenericRepository<Branch, int> _IBranchRepository;
        private readonly IGenericRepository<LocationType, int> _ILocationTypeRepository;
        private readonly IGenericRepository<RegionMaster, int> _IRegionMasterRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;

        public NewsAndUpdateController(IGenericRepository<NewsAndUpdate, int> NewsAndUpdateRepo, IGenericRepository<LocationType, int> LocationTypeRepo,
             IGenericRepository<RegionMaster, int> RegionMasterRepo, IHostingEnvironment hostingEnvironment,
            IGenericRepository<Branch, int> BranchRepo)
        {
            _INewsAndUpdateRepository = NewsAndUpdateRepo;
            _ILocationTypeRepository = LocationTypeRepo;
            _IRegionMasterRepository = RegionMasterRepo;
            _IHostingEnviroment = hostingEnvironment;
            _IBranchRepository = BranchRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {

                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("NewsAndUpdate", "_NewsAndUpdateIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(NewsAndUpdateController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetNewsAndUpdateList()
        {
            try
            {
                var newsandupdateList = await _INewsAndUpdateRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var branchList = await _IBranchRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var regionList = await _IRegionMasterRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var locationTypeList = await _ILocationTypeRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

                var responseDetails = (from newsandupdate in newsandupdateList.Entities
                                       join branch in branchList.Entities on newsandupdate.BranchId equals branch.Id
                                       join region in regionList.Entities on branch.RegionId equals region.Id
                                       join locationtype in locationTypeList.Entities on branch.LocationTypeId equals locationtype.Id
                                       select new NewsAndUpdateVM
                                       {
                                           Id = newsandupdate.Id,
                                           UploadFile = newsandupdate.UploadFile,
                                           News = newsandupdate.News,
                                           CalenderDate = newsandupdate.CalenderDate.ToString("dd-M-yyyy", CultureInfo.InvariantCulture),
                                           BranchName = branch.Name,
                                           LocationTypeName = locationtype.Name,
                                           RegionName = region.Name

                                       }).ToList();
                return PartialView(ViewHelper.GetViewPathDetails("NewsAndUpdate", "_NewsAndUpdateDetails"), responseDetails);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(NewsAndUpdateController)} action name {nameof(GetNewsAndUpdateList)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }
        public async Task<IActionResult> GetLocationAndRegion(int id)
        {
            try
            {
                await PopulateViewBags();
                var BranchResponse = await _IBranchRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.Id == id);
                return PartialView(ViewHelper.GetViewPathDetails("NewsAndUpdate", "_GetLocationAndRegion"), BranchResponse.Entities.FirstOrDefault());

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(NewsAndUpdateController)} action name {nameof(GetLocationAndRegion)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> NewsAndUpdateCreate(int id)
        {
            try
            {
                await PopulateViewBag();
                var response = await _INewsAndUpdateRepository.GetAllEntities(x => x.Id == id);
                if (id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("NewsAndUpdate", "_NewsAndUpdateCreate"));
                }
                else
                {

                    return PartialView(ViewHelper.GetViewPathDetails("NewsAndUpdate", "_NewsAndUpdateCreate"), response.Entities.First());
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(NewsAndUpdateController)} action name {nameof(NewsAndUpdateCreate)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertNewsAndUpdate(NewsAndUpdate model,IFormFile UploadFile)
        {
            try
            {
                model.UploadFile = await new BlobHelper().UploadImageToFolder(UploadFile, _IHostingEnviroment);

                if (model.Id == 0)
                {
                    model.FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId"));
                    model.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                    model.CreatedDate = DateTime.Now;
                    var response = await _INewsAndUpdateRepository.CreateEntity(model);
                    return Json(response.Message);
                }
                else
                {
                    model.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                    model.UpdatedDate = DateTime.Now;
                    var response = await _INewsAndUpdateRepository.UpdateEntity(model);
                    return Json(response.Message);
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(NewsAndUpdateController)} action name {nameof(UpsertNewsAndUpdate)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteNewsAndUpdate(int id)
        {
            try
            {
                var deleteModel = await _INewsAndUpdateRepository.GetAllEntityById(x => x.Id == id);
                var deleteDbModel = CrudHelper.DeleteHelper<NewsAndUpdate>(deleteModel.Entity, 1);
                var deleteResponse = await _INewsAndUpdateRepository.DeleteEntity(deleteDbModel);
                if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
                {
                    return Json(deleteResponse.Message);
                }
                return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(NewsAndUpdateController)} action name {nameof(DeleteNewsAndUpdate)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var BranchResponse = await _IBranchRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            BranchResponse.Entities.ToList().ForEach(data =>
            {
                data.Name = data.Name + " ( " + data.Code + " ) ";

            });

            if (BranchResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.Branch = BranchResponse.Entities;

        }
        private async Task PopulateViewBags()
        {
            var locationtypeResponse = await _ILocationTypeRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var regionResponse = await _IRegionMasterRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            if (locationtypeResponse.ResponseStatus == ResponseStatus.Success
                && regionResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.LocationTypeList = locationtypeResponse.Entities;
            ViewBag.RegionList = regionResponse.Entities;

        }

        #endregion
    }
}
