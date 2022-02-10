using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.HR;
using HRMS.Admin.UI.Helpers;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.Helpers.CommonCRUDHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using HRMS.Core.Helpers.BlobHelper;
using HRMS.Admin.UI.AuthenticateService;

namespace HRMS.Admin.UI.Controllers.HR
{
 
    [CustomAuthenticate]
 
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class PaidRegisterUploadController : Controller
    {
        private readonly IGenericRepository<PaidRegister, int> _IPaidRegisterUploadRepository;

        private readonly IHostingEnvironment _IHostingEnviroment;

        public PaidRegisterUploadController(IGenericRepository<PaidRegister, int> RegisterUploadRepo, IHostingEnvironment hostingEnvironment)
        {
            _IPaidRegisterUploadRepository = RegisterUploadRepo;
            _IHostingEnviroment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.HeaderTitle = PageHeader.HeaderSetting["LocationTypeIndex"];
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("PaidRegisterUpload", "PaidRegisterUploadIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(PaidRegisterUploadController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetPaidUploadRegister()
        {
            try
            {
                var response = new DBResponseHelper<PaidRegister, int>()
                    .GetDBResponseHelper(await _IPaidRegisterUploadRepository
                    .GetAllEntities(x => x.IsActive && !x.IsDeleted));

                return PartialView(ViewHelper.GetViewPathDetails("PaidRegisterUpload", "PaidRegisterUploadDetails"), response.Item2.Entities);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(PaidRegisterUploadController)} action name {nameof(GetPaidUploadRegister)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> CreatePaidUploadRegister(int id)
        {
            try
            {
                var response = await _IPaidRegisterUploadRepository.GetAllEntities(x => x.Id == id);
                if (id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("PaidRegisterUpload", "PaidRegisterUploadCreate"));
                }
                else
                {
                    return PartialView(ViewHelper.GetViewPathDetails("PaidRegisterUpload", "PaidRegisterUploadCreate"), response.Entities.First());
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(PaidRegisterUploadController)} action name {nameof(CreatePaidUploadRegister)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertPaidRegister(PaidRegister model, IFormFile UploadFilePath)
        {
            try
            {
                model.UploadFilePath = await new BlobHelper().UploadImageToFolder(UploadFilePath, _IHostingEnviroment);
                if (model.Id == 0)
                {
                    model.FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId"));
                    var response = await _IPaidRegisterUploadRepository.CreateEntity(model);
                    return Json(response.Message);
                }
                else
                {
                    var response = await _IPaidRegisterUploadRepository.UpdateEntity(model);
                    return Json(response.Message);
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(PaidRegisterUploadController)} action name {nameof(UpsertPaidRegister)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeletePaidRegister(int id)
        {
            try
            {
                var deleteModel = await _IPaidRegisterUploadRepository.GetAllEntityById(x => x.Id == id);
                var deleteDbModel = CrudHelper.DeleteHelper<PaidRegister>(deleteModel.Entity, 1);
                var deleteResponse = await _IPaidRegisterUploadRepository.DeleteEntity(deleteDbModel);
                if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
                {
                    return Json(deleteResponse.Message);
                }
                return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(PaidRegisterUploadController)} action name {nameof(DeletePaidRegister)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

    }
}
