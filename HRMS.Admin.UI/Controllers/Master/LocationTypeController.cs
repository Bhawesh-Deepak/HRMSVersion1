using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
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
    public class LocationTypeController : Controller
    {
        private readonly IGenericRepository<LocationType, int> _ILocationTypeRepository;



        public LocationTypeController(IGenericRepository<LocationType, int> LocationTypeRepo)
        {
            _ILocationTypeRepository = LocationTypeRepo;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["DesignationIndex"];

            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("LocationType", "LocationTypeIndex")));
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
                string template = $"Controller name {nameof(Department)} action name {nameof(GetLocationTypeList)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> CreateLocationType(int id)
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

        [HttpPost]
        public async Task<IActionResult> UpsertLocationType(LocationType model)
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

    }
}
