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
    public class EmployeetypeController : Controller
    {
        private readonly IGenericRepository<EmployeeType, int> _IEmployeeTypeRepository;



        public EmployeetypeController(IGenericRepository<EmployeeType, int> EmployeeTypeRepo)
        {
            _IEmployeeTypeRepository = EmployeeTypeRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.HeaderTitle = PageHeader.HeaderSetting["EmployeeTypeIndex"];
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Employeetype", "EmployeeTypeIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeType)} action name {nameof(Index)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetEmployeeTypeList()
        {
            try
            {
                var response = new DBResponseHelper<EmployeeType, int>()
                   .GetDBResponseHelper(await _IEmployeeTypeRepository
                   .GetAllEntities(x => x.IsActive && !x.IsDeleted));


                return PartialView(ViewHelper.GetViewPathDetails("Employeetype", "EmployeeTypeList"), response.Item2.Entities);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeType)} action name {nameof(GetEmployeeTypeList)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> CreateEmployeeType(int id)
        {
            try
            {
                var response = await _IEmployeeTypeRepository.GetAllEntities(x => x.Id == id);
                if (id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("Employeetype", "EmployeeTypeCreate"));
                }
                else
                {
                return PartialView(ViewHelper.GetViewPathDetails("Employeetype", "EmployeeTypeCreate"), response.Entities.First());
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeType)} action name {nameof(CreateEmployeeType)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertEmployeeType(EmployeeType model)
        {
            try
            {
                 if (model.Id == 0)
                 {
                    var response = await _IEmployeeTypeRepository.CreateEntity(model);
                    return Json(response.Message);
                 }
                else
                {
                    var response = await _IEmployeeTypeRepository.UpdateEntity(model);
                    return Json(response.Message);
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeType)} action name {nameof(UpsertEmployeeType)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteEmployeeType(int id)
        {
            try
            {
                var deleteModel = await _IEmployeeTypeRepository.GetAllEntityById(x => x.Id == id);
                var deleteDbModel = CrudHelper.DeleteHelper<EmployeeType>(deleteModel.Entity, 1);
                var deleteResponse = await _IEmployeeTypeRepository.DeleteEntity(deleteDbModel);
                if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
                {
                    return Json(deleteResponse.Message);
                }
                return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeType)} action name {nameof(DeleteEmployeeType)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
