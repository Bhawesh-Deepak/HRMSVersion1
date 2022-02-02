using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Master
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class LeaveTypeController : Controller
    {
        private readonly IGenericRepository<LeaveType, int> _ILeaveTypeRepository;
        public LeaveTypeController(IGenericRepository<LeaveType, int> leavetypeRepo)
        {
            _ILeaveTypeRepository = leavetypeRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("LeaveType", "LeaveTypeIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LeaveTypeController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetLeaveTypeList()
        {
            try
            {
                var response = new DBResponseHelper<LeaveType, int>()
                    .GetDBResponseHelper(await _ILeaveTypeRepository
                    .GetAllEntities(x => x.IsActive && !x.IsDeleted));

                return PartialView(ViewHelper.GetViewPathDetails("LeaveType", "_LeaveTypeList"), response.Item2.Entities);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LeaveTypeController)} action name {nameof(GetLeaveTypeList)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }

        public async Task<IActionResult> CreateLeaveType(int id)
        {
            try
            {
                var response = new DBResponseHelper<LeaveType, int>().GetDBResponseHelper(await _ILeaveTypeRepository.GetAllEntities(x => x.Id == id));
                if (id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("LeaveType", "_CreateLeaveType"));
                }
                else
                {
                    return PartialView(ViewHelper.GetViewPathDetails("LeaveType", "_CreateLeaveType"), response.Item2.Entities.First());
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LeaveTypeController)} action name {nameof(CreateLeaveType)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpSertLeaveType(LeaveType model)
        {

            try
            {

                if (model.Id == 0)
                {
                    model.FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId"));
                    var response = await _ILeaveTypeRepository.CreateEntity(model);
                    return Json(response.Message);
                }
                else
                {
                    var response = await _ILeaveTypeRepository.UpdateEntity(model);
                    return Json(response.Message);
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LeaveTypeController)} action name {nameof(UpSertLeaveType)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteLeaveType(int id)
        {
            try
            {
                var deleteModel = await _ILeaveTypeRepository.GetAllEntityById(x => x.Id == id);
                var deleteDbModel = CrudHelper.DeleteHelper<LeaveType>(deleteModel.Entity, 1);
                var deleteResponse = await _ILeaveTypeRepository.DeleteEntity(deleteDbModel);
                if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
                {
                    return Json(deleteResponse.Message);
                }
                return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LeaveTypeController)} action name {nameof(DeleteLeaveType)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
