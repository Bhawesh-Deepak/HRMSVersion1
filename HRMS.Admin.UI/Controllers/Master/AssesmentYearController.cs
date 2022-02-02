using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Master
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class AssesmentYearController : Controller
    {
        private readonly IGenericRepository<AssesmentYear, int> _IAssesmentYearRepository;

        public AssesmentYearController(IGenericRepository<AssesmentYear, int> AssesmentYearRepo)
        {
            _IAssesmentYearRepository = AssesmentYearRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.HeaderTitle = PageHeader.HeaderSetting["AssesmentYearIndex"];
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("AssesmentYear", "AssesmentYearIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AssesmentYearController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetAssesmentYearList()
        {
            try
            {
                var response = new DBResponseHelper<AssesmentYear, int>()
                    .GetDBResponseHelper(await _IAssesmentYearRepository
                    .GetAllEntities(x => x.IsActive && !x.IsDeleted));

                return PartialView(ViewHelper.GetViewPathDetails("AssesmentYear", "AssesmentYearList"), response.Item2.Entities);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AnnouncementAndUpdateController)} action name {nameof(GetAssesmentYearList)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }

        public async Task<IActionResult> CreateAssesmentYear(int id)
        {
            try
            {
                var response = new DBResponseHelper<AssesmentYear, int>().GetDBResponseHelper(await _IAssesmentYearRepository.GetAllEntities(x => x.Id == id));

                if (id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("AssesmentYear", "_CreateAssesmentYear"));
                }
                else
                {

                    return PartialView(ViewHelper.GetViewPathDetails("AssesmentYear", "_CreateAssesmentYear"), response.Item2.Entities.First());
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AssesmentYearController)} action name {nameof(CreateAssesmentYear)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpSertAssesmentYear(AssesmentYear model)
        {
            try
            {
                model.Name = model.StartYear + "-" + model.EndYear;
                model.CreatedDate = DateTime.Now;
                if (model.Id == 0)
                {
                    var response = await _IAssesmentYearRepository.CreateEntity(model);
                    return Json(response.Message);
                }
                else
                {
                    var response = await _IAssesmentYearRepository.UpdateEntity(model);
                    return Json(response.Message);
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AssesmentYearController)} action name {nameof(UpSertAssesmentYear)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpGet]
        public async Task<IActionResult> DeleteAssesmentYear(int id)
        {
            try
            {
                var deleteModel = await _IAssesmentYearRepository.GetAllEntityById(x => x.Id == id);

                var deleteDbModel = CrudHelper.DeleteHelper<AssesmentYear>(deleteModel.Entity, 1);

                var deleteResponse = await _IAssesmentYearRepository.DeleteEntity(deleteDbModel);

                if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
                {
                    return Json(deleteResponse.Message);
                }
                return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AssesmentYearController)} action name {nameof(DeleteAssesmentYear)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
