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
    public class RegionController : Controller
    {
        private readonly IGenericRepository<RegionMaster, int> _IRegionRepository;



        public RegionController(IGenericRepository<RegionMaster, int> RegionRepo)
        {
            _IRegionRepository = RegionRepo;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["DesignationIndex"];

            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Region", "RegionIndex")));
        }

        public async Task<IActionResult> GetRegionList()
        {
            try
            {
                var response = new DBResponseHelper<RegionMaster, int>()
                    .GetDBResponseHelper(await _IRegionRepository
                    .GetAllEntities(x => x.IsActive && !x.IsDeleted));

                return PartialView(ViewHelper.GetViewPathDetails("Region", "RegionDetails"), response.Item2.Entities);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Department)} action name {nameof(GetRegionList)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> CreateRegion(int id)
        {

            var response = await _IRegionRepository.GetAllEntities(x => x.Id == id);

            if (id == 0)
            {
                return PartialView(ViewHelper.GetViewPathDetails("Region", "RegionCreate"));
            }
            else
            {

                return PartialView(ViewHelper.GetViewPathDetails("Region", "RegionCreate"), response.Entities.First());
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertRegion(RegionMaster model)
        {
            if (model.Id == 0)
            {
                var response = await _IRegionRepository.CreateEntity(model);
                return Json(response.Message);
            }
            else
            {
                var response = await _IRegionRepository.UpdateEntity(model);
                return Json(response.Message);
            }
        }
    }
}
