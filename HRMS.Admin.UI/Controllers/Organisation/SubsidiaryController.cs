using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Organisation;
using HRMS.Core.Helpers.BlobHelper;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Organisation;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Organisation
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class SubsidiaryController : Controller
    {
        private readonly IGenericRepository<Company, int> _ICompanyRepository;
        private readonly IGenericRepository<Subsidiary, int> _ISubsidiaryRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;

        public SubsidiaryController(IGenericRepository<Company, int> companyRepository, IGenericRepository<Subsidiary, int> SubsidiaryRepository,

            IHostingEnvironment hostingEnvironment)
        {
            _ICompanyRepository = companyRepository;
            _IHostingEnviroment = hostingEnvironment;
            _ISubsidiaryRepository = SubsidiaryRepository;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.HeaderTitle = PageHeader.HeaderSetting["Subsidiary"];
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Subsidiary", "SubsidiaryIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(SubsidiaryController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> GetSubsidiaryList()
        {
            try
            {
                var Subsidryresponse = new DBResponseHelper<Subsidiary, int>()
                    .GetDBResponseHelper(await _ISubsidiaryRepository
                    .GetAllEntities(x => x.IsActive && !x.IsDeleted));
                var Companyresponse = new DBResponseHelper<Company, int>()
                  .GetDBResponseHelper(await _ICompanyRepository
                  .GetAllEntities(x => x.IsActive && !x.IsDeleted));
                var response = from subsidry in Subsidryresponse.Item2.Entities
                                      join company in Companyresponse.Item2.Entities
                                      on subsidry.OrganisationId equals company.Id
                                      select new SubsidiaryVM
                                      {
                                          Id = subsidry.Id,
                                          CompanyName = company.Name,
                                          Name = subsidry.Name,
                                          Code=subsidry.Code,
                                          Email=subsidry.Email,
                                          Url=subsidry.Url,
                                          FavIcon=subsidry.FavIcon,
                                          Logo=subsidry.Logo
                                      };


                return PartialView(ViewHelper.GetViewPathDetails("Subsidiary", "SubsidiaryList"), response);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(SubsidiaryController)} action name {nameof(GetSubsidiaryList)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }
        public async Task<IActionResult> CreateSubsidiary(int id)
        {
            try
            {
                var response = new DBResponseHelper<Subsidiary, int>().GetDBResponseHelper(await _ISubsidiaryRepository.GetAllEntities(x => x.Id == id));
                await PopulateViewBag();
                if (id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("Subsidiary", "SubsidiaryCreate"));
                }
                else
                {
                    return PartialView(ViewHelper.GetViewPathDetails("Subsidiary", "SubsidiaryCreate"), response.Item2.Entities.First());
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(SubsidiaryController)} action name {nameof(CreateSubsidiary)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpSertSubsidiary(Subsidiary model, IFormFile FavIcon, IFormFile Logo)
        {
            try
            {
                 
                var uploadLogoPath = await new BlobHelper().UploadImageToFolder(Logo, _IHostingEnviroment);
                var favIconPath = await new BlobHelper().UploadImageToFolder(FavIcon, _IHostingEnviroment);

                if (model.Id == 0)
                {
                    var createModel = CrudHelper.CreateHelper<Subsidiary>(model);
                    createModel.Logo = uploadLogoPath;
                    createModel.FavIcon = favIconPath;
                    var response = await _ISubsidiaryRepository.CreateEntity(createModel);

                    return Json(response.Message);
                }
                else
                {
                    var updateModel = CrudHelper.UpdateHelper(model, 1);
                    updateModel.Logo = uploadLogoPath;
                    updateModel.FavIcon = favIconPath;
                    var response = await _ISubsidiaryRepository.UpdateEntity(updateModel);
                    return Json(response.Message);
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(SubsidiaryController)} action name {nameof(UpSertSubsidiary)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteSubsidiary(int id)
        {
            try
            {
                var deleteModel = await _ISubsidiaryRepository.GetAllEntityById(x => x.Id == id);
                var deleteDbModel = CrudHelper.DeleteHelper<Subsidiary>(deleteModel.Entity, 1);
                var deleteResponse = await _ISubsidiaryRepository.DeleteEntity(deleteDbModel);
                if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
                {
                    return Json(deleteResponse.Message);
                }
                return Json(deleteResponse.Message);
                }
                catch (Exception ex)
                {
                    string template = $"Controller name {nameof(SubsidiaryController)} action name {nameof(DeleteSubsidiary)} exception is {ex.Message}";
                    Serilog.Log.Error(ex, template);
                    return RedirectToAction("Error", "Home");
                }
        }
        private async Task PopulateViewBag()
        {
            ViewBag.Organisation = (await _ICompanyRepository.GetAllEntities(x => x.IsActive == true && x.IsDeleted == false)).Entities;
        }

    }
}
