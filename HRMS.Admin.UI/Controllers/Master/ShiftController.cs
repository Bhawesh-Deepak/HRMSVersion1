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
    public class ShiftController : Controller
    {
        private readonly IGenericRepository<Shift, int> _IShiftRepository;



        public ShiftController(IGenericRepository<Shift, int> ShiftRepo)
        {
            _IShiftRepository = ShiftRepo;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["ShiftIndex"];

            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Shift", "ShiftIndex")));
        }

        public async Task<IActionResult> GetShiftList()
        {
            try
            {
                var response = new DBResponseHelper<Shift, int>()
                   .GetDBResponseHelper(await _IShiftRepository
                   .GetAllEntities(x => x.IsActive && !x.IsDeleted));


                return PartialView(ViewHelper.GetViewPathDetails("Shift", "ShiftDetails"), response.Item2.Entities);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Department)} action name {nameof(GetShiftList)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> CreateShift(int id)
        {

            var response = await _IShiftRepository.GetAllEntities(x => x.Id == id);

            if (id == 0)
            {
                return PartialView(ViewHelper.GetViewPathDetails("Shift", "ShiftCreate"));
            }
            else
            {

                return PartialView(ViewHelper.GetViewPathDetails("Shift", "ShiftCreate"), response.Entities.First());
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertShift(Shift model)
        {
            if (model.Id == 0)
            {
                var response = await _IShiftRepository.CreateEntity(model);
                return Json(response.Message);
            }
            else
            {
                var response = await _IShiftRepository.UpdateEntity(model);
                return Json(response.Message);
            }
        }
    }
}
