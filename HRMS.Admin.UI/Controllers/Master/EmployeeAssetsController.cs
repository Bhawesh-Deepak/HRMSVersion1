using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Master;
using HRMS.Core.Entities.Organisation;
using HRMS.Core.Entities.Payroll;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Admin.UI.AuthenticateService;

namespace HRMS.Admin.UI.Controllers.Master
{
    [CustomAuthenticate]
    public class EmployeeAssetsController : Controller
    {
        private readonly IGenericRepository<EmployeeAssets, int> _IEmployeeAssetsRepository;
        private readonly IGenericRepository<Branch, int> _IBranchRepository;
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepository;
        private readonly IGenericRepository<AssetsCategory, int> _IAssetsCategoryRepository;

        public EmployeeAssetsController(IGenericRepository<EmployeeAssets, int> EmployeeAssetsRepo, IGenericRepository<Branch, int> BranchRepo,
           IGenericRepository<EmployeeDetail, int> EmployeeDetailRepo, IGenericRepository<AssetsCategory, int> AssetsCategoryRepo)
        {
            _IEmployeeAssetsRepository = EmployeeAssetsRepo;
            _IBranchRepository = BranchRepo;
            _IEmployeeDetailRepository = EmployeeDetailRepo;
            _IAssetsCategoryRepository = AssetsCategoryRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["EmployeeAssetsIndex"];
            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("EmployeeAssets", "EmployeeAssetsIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeAssets)} action name {nameof(Index)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetEmployeeAssetsList()
        {
            try
            {
                var BranchList = await _IBranchRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var EmployeeAssetsList = await _IEmployeeAssetsRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var EmployeeDetailsList = await _IEmployeeDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var AssCatList = await _IAssetsCategoryRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

                var responseDetails = (from eal in EmployeeAssetsList.Entities
                                       join bl in BranchList.Entities
                                       on eal.BranchId equals bl.Id
                                       join edl in EmployeeDetailsList.Entities
                                       on eal.EmployeeId equals edl.Id
                                       join acl in AssCatList.Entities
                                       on eal.AssetsCategoryId equals acl.Id
                                       select new EmployeeAssetsDetails
                                       {
                                         EmployeeAssetId=eal.Id,
                                         Branch=eal.Name,
                                         CategoryAssetsName=acl.Name,
                                         EmployeeDetailsName=edl.EmployeeName,
                                         AssetsStatus=eal.AssetsStatus,
                                         AssetsTag=eal.AssetsTag,
                                         SerialNumber=eal.SerialNumber,
                                         ModelNumber=eal.ModelNumbar,
                                         MEID=eal.MEID,
                                         Category=eal.Category,
                                         Name=eal.Name,
                                         Make=eal.Make,
                                         DongleNumber=eal.DongleNumber
                                       }).ToList();
                return PartialView(ViewHelper.GetViewPathDetails("EmployeeAssets", "EmployeeAssetsDetails"), responseDetails);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeAssets)} action name {nameof(GetEmployeeAssetsList)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }

        public async Task<IActionResult> EmployeeAssetsCreate(int id)
        {
            try
            {
                await PopulateViewBag();
                var response = await _IEmployeeAssetsRepository.GetAllEntities(x => x.Id == id);

                if (id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("EmployeeAssets", "EmployeeAssetsCreate"));
                }
                else
                {
                    return PartialView(ViewHelper.GetViewPathDetails("EmployeeAssets", "EmployeeAssetsCreate"), response.Entities.First());
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeAssets)} action name {nameof(EmployeeAssetsCreate)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertEmployeeAssets(EmployeeAssets model)
        {
            try
            {
                if (model.Id == 0)
                {
                    var response = await _IEmployeeAssetsRepository.CreateEntity(model);
                    return Json(response.Message);
                }
            else
            {
                var response = await _IEmployeeAssetsRepository.UpdateEntity(model);
                return Json(response.Message);
            }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeAssets)} action name {nameof(UpsertEmployeeAssets)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteEmployeeAssets(int id)
        {
            try
            {
            var deleteModel = await _IEmployeeAssetsRepository.GetAllEntityById(x => x.Id == id);
            var deleteDbModel = CrudHelper.DeleteHelper<EmployeeAssets>(deleteModel.Entity, 1);
            var deleteResponse = await _IEmployeeAssetsRepository.DeleteEntity(deleteDbModel);
            if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
            {
                return Json(deleteResponse.Message);
            }
            return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeAssets)} action name {nameof(DeleteEmployeeAssets)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var BranchResponse = await _IBranchRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var EmployeeResponse = await _IEmployeeDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var AssetCatResponse = await _IAssetsCategoryRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            if (BranchResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.Branch = BranchResponse.Entities;
            if (EmployeeResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.EmployeeName = EmployeeResponse.Entities;
            if (AssetCatResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.AssCatName = AssetCatResponse.Entities;

        }

        #endregion
    }
}
