using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Master;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Http;
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
    public class DesignationController : Controller
    {
        private readonly IGenericRepository<Department, int> _IDepartmentRepository;
        private readonly IGenericRepository<Designation, int> _IDesignationRepository;
        

        public DesignationController(IGenericRepository<Department, int> departmentRepo,
            IGenericRepository<Designation, int> designationRepo)
        {
            _IDepartmentRepository = departmentRepo;
            _IDesignationRepository = designationRepo;
            
        }
        public async Task<IActionResult> Index()
        {
            try
            {
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["DesignationIndex"];
            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Designation", "DesignationIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Designation)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetDesignationList()
        {
            try
            {
            var departmentList = await _IDepartmentRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var designationLIst = await _IDesignationRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            var responseDetails = (from department in departmentList.Entities
                                   join designtion in designationLIst.Entities
                                   on department.Id equals designtion.DepartmentId
                                   select new Designation
                                   {
                                       Id = designtion.Id,
                                       Code = designtion.Code,
                                       DepartmentName = department.Name,
                                       Description = designtion.Description,
                                       Name= designtion.Name
                                   }).ToList();

            return PartialView(ViewHelper.GetViewPathDetails("Designation", "DesignationDetails"), responseDetails);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Designation)} action name {nameof(GetDesignationList)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> CreateDesignation(int id)
        {
            try
            {
            await PopulateViewBag();
            var response = await _IDesignationRepository.GetAllEntities(x => x.Id == id);

            if (id == 0)
            {
                return PartialView(ViewHelper.GetViewPathDetails("Designation", "DesignationCreate"));
            }
            else
            {

                return PartialView(ViewHelper.GetViewPathDetails("Designation", "DesignationCreate"), response.Entities.First());
            }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Designation)} action name {nameof(CreateDesignation)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertDesignation(Designation model)
        {
            try
            {
            if (model.Id == 0)
            {
                    model.FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId"));
                    var response = await _IDesignationRepository.CreateEntity(model);
                    return Json(response.Message);
            }
            else
            {
                var response = await _IDesignationRepository.UpdateEntity(model);
                return Json(response.Message);
            }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Designation)} action name {nameof(UpsertDesignation)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteDesignation(int id)
        {
            try
            {
            var deleteModel = await _IDesignationRepository.GetAllEntityById(x => x.Id == id);
            var deleteDbModel = CrudHelper.DeleteHelper<Designation>(deleteModel.Entity, 1);
            var deleteResponse = await _IDesignationRepository.DeleteEntity(deleteDbModel);
            if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
            {
                return Json(deleteResponse.Message);
            }
            return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Designation)} action name {nameof(DeleteDesignation)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var departmentResponse = await _IDepartmentRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            if (departmentResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.DepartmentList = departmentResponse.Entities;

        }

        #endregion
    }
}
