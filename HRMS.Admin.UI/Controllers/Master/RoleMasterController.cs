using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Master
{
    [CustomAuthenticate]
    public class RoleMasterController : Controller
    {
        private readonly IGenericRepository<RoleMaster, int> _IRoleMasterRepository;

        public RoleMasterController(IGenericRepository<RoleMaster, int> roleMasterRepo)
        {
            _IRoleMasterRepository = roleMasterRepo;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.HeaderTitle = PageHeader.HeaderSetting["RoleMasterIndex"];
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("RoleMaster", "RoleIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(RoleMaster)} action name {nameof(GetRoleDetail)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRoleDetail()
        {
            try
            {
                var response = await _IRoleMasterRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                return PartialView(ViewHelper.GetViewPathDetails("RoleMaster", "RoleDetails"), response.Entities);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(RoleMaster)} action name {nameof(GetRoleDetail)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpGet]
        public async Task<IActionResult> CreateRole(int id)
        {
            try
            {
                if (id == 0)
                {
                    return await Task.Run(() => PartialView(ViewHelper.GetViewPathDetails("RoleMaster", "RoleCreate")));
                }
                var response = await _IRoleMasterRepository.GetAllEntities(x => x.Id == id);
                if (response.ResponseStatus == Core.Entities.Common.ResponseStatus.Success)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("RoleMaster", "RoleCreate"), response.Entities.First());
                }
                return RedirectToAction("Error", "Home");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(RoleMaster)} action name {nameof(CreateRole)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertRole(RoleMaster model)
        {
            try
            {
                model.CreatedDate = DateTime.Now;
                if (model.Id == 0)
                {
                    var response = await _IRoleMasterRepository.CreateEntity(model);
                    return Json(new DBResponseHelper<RoleMaster, int>().GetDBResponseHelper(response).message);
                }
                var updateResponse = await _IRoleMasterRepository.UpdateEntity(model);
                return Json(new DBResponseHelper<RoleMaster, int>().GetDBResponseHelper(updateResponse).message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(RoleMaster)} action name {nameof(UpsertRole)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var deleteModelResponse = await _IRoleMasterRepository.GetAllEntityById(x => x.Id == id);
                if (deleteModelResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Success)
                {
                    deleteModelResponse.Entity.IsActive = false;
                    deleteModelResponse.Entity.IsDeleted = true;
                    var deleteResponse = await _IRoleMasterRepository.DeleteEntity(deleteModelResponse.Entity);
                    return Json(new DBResponseHelper<RoleMaster, int>().GetDBResponseHelper(deleteResponse));
                }

            return Json($"The Role Id {id} you have passed is not valid !!!");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(RoleMaster)} action name {nameof(DeleteRole)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
