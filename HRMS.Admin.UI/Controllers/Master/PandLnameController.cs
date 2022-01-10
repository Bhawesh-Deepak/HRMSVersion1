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
    public class PandLnameController : Controller
    {
        private readonly IGenericRepository<PAndLMaster, int> _IPandLnameRepository;



        public PandLnameController(IGenericRepository<PAndLMaster, int> PandLnameRepo)
        {
            _IPandLnameRepository = PandLnameRepo;
            

        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.HeaderTitle = PageHeader.HeaderSetting["P and L Name"];
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("PandLname", "PandLnameIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(PAndLMaster)} action name {nameof(Index)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetPandLList()
        {
            try
            {
                var response = new DBResponseHelper<PAndLMaster, int>()
                    .GetDBResponseHelper(await _IPandLnameRepository
                    .GetAllEntities(x => x.IsActive && !x.IsDeleted));

                return PartialView(ViewHelper.GetViewPathDetails("PandLname", "PandLnameDetails"), response.Item2.Entities);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(PAndLMaster)} action name {nameof(GetPandLList)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> CreatePandLname(int id)
        {
            try
            {
                var response = await _IPandLnameRepository.GetAllEntities(x => x.Id == id);
                if (id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("PandLname", "PandLNameCreate"));
                }
                else
                {
                    return PartialView(ViewHelper.GetViewPathDetails("PandLname", "PandLNameCreate"), response.Entities.First());
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(PAndLMaster)} action name {nameof(CreatePandLname)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertPandLname(PAndLMaster model)
        {
            try
            {
                if (model.Id == 0)
                {
                    var response = await _IPandLnameRepository.CreateEntity(model);
                    return Json(response.Message);
                }
                else
                {
                    var response = await _IPandLnameRepository.UpdateEntity(model);
                    return Json(response.Message);
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(PAndLMaster)} action name {nameof(UpsertPandLname)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeletePandLname(int id)
        {
            try
            {
                var deleteModel = await _IPandLnameRepository.GetAllEntityById(x => x.Id == id);
                var deleteDbModel = CrudHelper.DeleteHelper<PAndLMaster>(deleteModel.Entity, 1);
                var deleteResponse = await _IPandLnameRepository.DeleteEntity(deleteDbModel);
                if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
                {
                    return Json(deleteResponse.Message);
                }
                return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(PAndLMaster)} action name {nameof(DeletePandLname)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
