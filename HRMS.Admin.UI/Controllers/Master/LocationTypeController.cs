using HRMS.Admin.UI.AuthenticateService;
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
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class LocationTypeController : Controller
    {
        private readonly IGenericRepository<LocationType, int> _ILocationTypeRepository;



        public LocationTypeController(IGenericRepository<LocationType, int> LocationTypeRepo)
        {
            _ILocationTypeRepository = LocationTypeRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.HeaderTitle = PageHeader.HeaderSetting["LocationTypeIndex"];
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("LocationType", "LocationTypeIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LocationType)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetLocationTypeList()
        {
            try
            {
                var response = new DBResponseHelper<LocationType, int>()
                    .GetDBResponseHelper(await _ILocationTypeRepository
                    .GetAllEntities(x => x.IsActive && !x.IsDeleted));

                return PartialView(ViewHelper.GetViewPathDetails("LocationType", "LocationTypeDetails"), response.Item2.Entities);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LocationType)} action name {nameof(GetLocationTypeList)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> CreateLocationType(int id)
        {
            try
            {
                var response = await _ILocationTypeRepository.GetAllEntities(x => x.Id == id);
                if (id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("LocationType", "LocationTypeCreate"));
                }
                else
                {
                    return PartialView(ViewHelper.GetViewPathDetails("LocationType", "LocationTypeCreate"), response.Entities.First());
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LocationType)} action name {nameof(CreateLocationType)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertLocationType(LocationType model)
        {
            try
            {
                if (model.Id == 0)
                {
                    var response = await _ILocationTypeRepository.CreateEntity(model);
                    return Json(response.Message);
                }
                else
                {
                    var response = await _ILocationTypeRepository.UpdateEntity(model);
                    return Json(response.Message);
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LocationType)} action name {nameof(UpsertLocationType)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteLocationType(int id)
        {
            try
            {
                var deleteModel = await _ILocationTypeRepository.GetAllEntityById(x => x.Id == id);
                var deleteDbModel = CrudHelper.DeleteHelper<LocationType>(deleteModel.Entity, 1);
                var deleteResponse = await _ILocationTypeRepository.DeleteEntity(deleteDbModel);
                if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
                {
                    return Json(deleteResponse.Message);
                }
                return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LocationType)} action name {nameof(DeleteLocationType)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
