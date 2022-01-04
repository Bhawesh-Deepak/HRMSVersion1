﻿using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Master;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace HRMS.Admin.UI.Controllers.Master
{
    public class CompanyHolidaysController : Controller
    {
        private readonly IGenericRepository<CompanyHolidays, int> _ICompanyHolidaysRepository;
        private readonly IGenericRepository<StateMaster, int> _IStateRepository;

        public CompanyHolidaysController(IGenericRepository<CompanyHolidays, int> CompanyHolidayRepo,IGenericRepository<StateMaster, int> StateRepo )
        {
            _ICompanyHolidaysRepository = CompanyHolidayRepo;
            _IStateRepository = StateRepo;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["CompanyHolidaysIndex"];
            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("CompanyHolidays", "CompanyHolidaysIndex")));
        }

        public async Task<IActionResult> GetHolidayList()
        {
            try
            {
                var HolidayList = await _ICompanyHolidaysRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var StateList = await _IStateRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

                var responseDetails = (from stl in StateList.Entities
                                       join hdl in HolidayList.Entities
                                       on stl.Id equals hdl.StateId
                                       select new HolidaysDetails
                                       {
                                           HolidayId=hdl.Id,
                                           StateName=stl.Name,
                                           HolidayName = hdl.Name,
                                           Holidate = hdl.HolidayDate,
                                       }).ToList();
                return PartialView(ViewHelper.GetViewPathDetails("CompanyHolidays", "CompanyHolidayDetails"), responseDetails);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Department)} action name {nameof(GetHolidayList)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }

        public async Task<IActionResult> HolidayCreate(int id)
        {
            await PopulateViewBag();
            var response = await _ICompanyHolidaysRepository.GetAllEntities(x => x.Id == id);

            if (id == 0)
            {
                return PartialView(ViewHelper.GetViewPathDetails("CompanyHolidays", "CompanyHolidayCreate"));
            }
            else
            {

                return PartialView(ViewHelper.GetViewPathDetails("CompanyHolidays", "CompanyHolidayCreate"), response.Entities.First());
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertCompanyHoliday(CompanyHolidays model)
        {
            if (model.Id == 0)
            {
                var response = await _ICompanyHolidaysRepository.CreateEntity(model);
                return Json(response.Message);
            }
            else
            {
                var response = await _ICompanyHolidaysRepository.UpdateEntity(model);
                return Json(response.Message);
            }
        }

        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var StateResponse = await _IStateRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            if (StateResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.StateList = StateResponse.Entities;

        }

        #endregion

    }
}
