using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Master;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Payroll
{
    public class EmployeeShifTimingController : Controller
    {
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepository;
        private readonly IGenericRepository<Shift, int> _IShiftRepository;
        public EmployeeShifTimingController(IGenericRepository<EmployeeDetail, int> EmployeeDetailRepo,
            IGenericRepository<Shift, int> ShiftRepo)
        {
            _IEmployeeDetailRepository = EmployeeDetailRepo;
            _IShiftRepository = ShiftRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                await PopulateViewBag();
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["EmployeeShifTimingIndex"];
            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("EmployeeShifTiming", "EmployeeShifTimingIndex")));
             }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeShifTimingController)} action name {nameof(Index)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> GetEmployeeDetail(int Id)
        {
            try
            {
                await PopulateShiftViewBag();
            var response = await _IEmployeeDetailRepository.GetAllEntities(x => x.Id == Id);
            return PartialView(ViewHelper.GetViewPathDetails("EmployeeShifTiming", "EmployeeDetail"), response.Entities.First());
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeShifTimingController)} action name {nameof(GetEmployeeDetail)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateShiftTiming(EmployeeDetail model)
        {
            try
            {
                var deleteModel = await _IEmployeeDetailRepository.GetAllEntities(x => x.Id == model.Id);
            deleteModel.Entities.ToList().ForEach(data =>
            {
                data.IsActive = false;
                data.IsDeleted = true;
                data.UpdatedDate = DateTime.Now;
                data.ShiftTiming = model.ShiftTiming;
            });
            var response = await _IEmployeeDetailRepository.DeleteEntity(deleteModel.Entities.First());
            deleteModel.Entities.ToList().ForEach(data =>
            {
                data.CreatedDate = DateTime.Now;
                data.ShiftTiming = model.ShiftTiming;
                data.Id = 0;
                data.UpdatedDate = null;
            });
             
            var createresponse = await _IEmployeeDetailRepository.CreateEntity(deleteModel.Entities.First());
            return Json("Shift Time Change Sucrssfully");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeShifTimingController)} action name {nameof(UpdateShiftTiming)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var employeeResponse = await _IEmployeeDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            ViewBag.EmployeeList = employeeResponse.Entities;
        }

        #endregion
        #region PrivateFields
        private async Task PopulateShiftViewBag()
        {
            var shiftResponse = await _IShiftRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            ViewBag.ShiftList = shiftResponse.Entities;
        }

        #endregion
    }
}
