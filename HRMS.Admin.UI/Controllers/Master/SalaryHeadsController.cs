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
    public class SalaryHeadsController : Controller
    {
        private readonly IGenericRepository<SalaryHeads, int> _ISalaryHeadRepository;

        public SalaryHeadsController(IGenericRepository<SalaryHeads, int> salaryHeadRepository)
        {
            _ISalaryHeadRepository = salaryHeadRepository;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.HeaderTitle = PageHeader.HeaderSetting["SalaryHeadIndex"];
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("SalaryHeads", "SalaryHeadsIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(SalaryHeads)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetSalaryHeadList()
        {
            try
            {
                var response = await _ISalaryHeadRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                return PartialView(ViewHelper.GetViewPathDetails("SalaryHeads", "_SalaryHeadList"), response.Entities);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(SalaryHeads)} action name {nameof(GetSalaryHeadList)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> CreateSalaryHead(int id)
        {
            try
            {
                if (id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("SalaryHeads", "_SalaryHeadCreate"));
                }
                else
                {
                    var response = await _ISalaryHeadRepository.GetAllEntities(x => x.Id == id);
                    return PartialView(ViewHelper.GetViewPathDetails("SalaryHeads", "_SalaryHeadCreate"), response.Entities.First());
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(SalaryHeads)} action name {nameof(CreateSalaryHead)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertSalaryHeads(SalaryHeads model)
        {
            try
            {
                if (model.Id == 0)
                {
                    model.FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId"));
                    var response = await _ISalaryHeadRepository.CreateEntity(model);
                    return Json(response.Message);
                }
                else
                {
                    var response = await _ISalaryHeadRepository.UpdateEntity(model);
                    return Json(response.Message);
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(SalaryHeads)} action name {nameof(UpsertSalaryHeads)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteSalaryHead(int id)
        {
            try
            {
                var deleteModel = await _ISalaryHeadRepository.GetAllEntityById(x => x.Id == id);
                var deleteDbModel = CrudHelper.DeleteHelper<SalaryHeads>(deleteModel.Entity, 1);
                var deleteResponse = await _ISalaryHeadRepository.DeleteEntity(deleteDbModel);
                if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
                {
                    return Json(deleteResponse.Message);
                }
                return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(SalaryHeads)} action name {nameof(DeleteSalaryHead)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
