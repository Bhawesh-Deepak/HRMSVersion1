using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Master;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HRMS.Admin.UI.Controllers.Master
{
    public class LocationController : Controller
    {
        private readonly IGenericRepository<Location, int> _ILocationRepository;
        private readonly IGenericRepository<LocationType, int> _ILocationTypeRepository;


        public LocationController(IGenericRepository<Location, int> LocationRepo,
            IGenericRepository<LocationType, int> LocationTypeRepo)
        {
            _ILocationRepository = LocationRepo;
            _ILocationTypeRepository = LocationTypeRepo;

        }
        public async Task<IActionResult> Index()
        {
            try
            {
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["LocationIndex"];
            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Location", "LocationIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Location)} action name {nameof(Index)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetLoctionList()
        {
            try
            {
                var LocationList = await _ILocationRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var LocationTypeLIst = await _ILocationTypeRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

                var responseDetails = (from lct in LocationTypeLIst.Entities
                                       join lc in LocationList.Entities
                                       on lct.Id equals lc.LocationTypeid
                                       select new LocationDetails
                                       {
                                           LocationId = lc.Id,
                                           LocationTypeName = lct.Name,
                                           LocationName = lc.Name,
                                       }).ToList();

                return PartialView(ViewHelper.GetViewPathDetails("Location", "LocationDetails"), responseDetails);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Location)} action name {nameof(GetLoctionList)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> CreateLocation(int id)
        {
            try
            {
                await PopulateViewBag();
                var response = await _ILocationRepository.GetAllEntities(x => x.Id == id);
                if(id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("Location", "LocationCreate"));
                }
                else
                {
                    return PartialView(ViewHelper.GetViewPathDetails("Location", "LocationCreate"), response.Entities.First());
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Location)} action name {nameof(CreateLocation)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertLocation(Location model)
        {
            try
            {
                if (model.Id == 0)
                {
                    var response = await _ILocationRepository.CreateEntity(model);
                    return Json(response.Message);
                }
                else
                {
                    var response = await _ILocationRepository.UpdateEntity(model);
                    return Json(response.Message);
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Location)} action name {nameof(UpsertLocation)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            try
            {
                var deleteModel = await _ILocationRepository.GetAllEntityById(x => x.Id == id);
                var deleteDbModel = CrudHelper.DeleteHelper<Location>(deleteModel.Entity, 1);
                var deleteResponse = await _ILocationRepository.DeleteEntity(deleteDbModel);
                if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
                {
                    return Json(deleteResponse.Message);
                }
                return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Location)} action name {nameof(DeleteLocation)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var departmentResponse = await _ILocationTypeRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            if (departmentResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.LocationList = departmentResponse.Entities;

        }

        #endregion
    }
}
