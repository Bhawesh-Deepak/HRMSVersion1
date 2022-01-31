using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Master
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class DepartmentController : Controller
    {
        private readonly IGenericRepository<Department, int> _IDepartmentRepository;

        public DepartmentController(IGenericRepository<Department, int> departmentRepo)
        {
            _IDepartmentRepository = departmentRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["DepartmentIndex"];
            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Department", "DepartmentIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Department)} action name {nameof(Index)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetDepartmentList()
        {
            try
            {
                var response = new DBResponseHelper<Department, int>()
                    .GetDBResponseHelper(await _IDepartmentRepository
                    .GetAllEntities(x => x.IsActive && !x.IsDeleted));

                return PartialView(ViewHelper.GetViewPathDetails("Department", "DepartmentList"), response.Item2.Entities);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Department)} action name {nameof(GetDepartmentList)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error","Home");
            }

        }

        public async Task<IActionResult> CreateDepartment(int id)
        {
            try
            {
            var response = new DBResponseHelper<Department, int>().GetDBResponseHelper(await _IDepartmentRepository.GetAllEntities(x => x.Id == id));
            if (id == 0)
            {
                return PartialView(ViewHelper.GetViewPathDetails("Department", "_CreateDepartment"));
            }
            else
            {
                return PartialView(ViewHelper.GetViewPathDetails("Department", "_CreateDepartment"), response.Item2.Entities.First());
            }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Department)} action name {nameof(CreateDepartment)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpSertDepartment(Department model)
        {
 
            try
            {
 
            if (model.Id == 0)
            {
                var response = await _IDepartmentRepository.CreateEntity(model);
                return Json(response.Message);
            }
            else
            {
                var response = await _IDepartmentRepository.UpdateEntity(model);
                return Json(response.Message);
            }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Department)} action name {nameof(UpSertDepartment)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
            var deleteModel = await _IDepartmentRepository.GetAllEntityById(x => x.Id == id);
            var deleteDbModel = CrudHelper.DeleteHelper<Department>(deleteModel.Entity, 1);
            var deleteResponse = await _IDepartmentRepository.DeleteEntity(deleteDbModel);
            if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
            {
                return Json(deleteResponse.Message);
            }
            return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Department)} action name {nameof(DeleteDepartment)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
