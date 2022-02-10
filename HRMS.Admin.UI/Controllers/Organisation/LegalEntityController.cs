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
    public class LegalEntityController : Controller
    {

        private readonly IGenericRepository<Company, int> _ICompanyRepository;
        private readonly IGenericRepository<LegalEntity, int> _ISubsidiaryRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;

        public LegalEntityController(IGenericRepository<Company, int> companyRepository, IGenericRepository<LegalEntity, int> SubsidiaryRepository,

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
                ViewBag.HeaderTitle = PageHeader.HeaderSetting["LegalEntity"];
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("LegalEntity", "_LegalEntityIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LegalEntityController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> GetLegalEntityList()
        {
            try
            {
                var Subsidryresponse = await _ISubsidiaryRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var Companyresponse = await _ICompanyRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var response = from subsidry in Subsidryresponse.Entities
                               join company in Companyresponse.Entities
                               on subsidry.OrganisationId equals company.Id
                               select new LegalEntityVM
                               {
                                   Id = subsidry.Id,
                                   CompanyName = company.Name,
                                   Name = subsidry.Name,
                                   Code = subsidry.Code,
                                   Email = subsidry.Email,
                                   Address = subsidry.Address,
                                   FavIcon = subsidry.FavIcon,
                                   Logo = subsidry.Logo,
                                   Phone = subsidry.Phone
                               };


                return PartialView(ViewHelper.GetViewPathDetails("LegalEntity", "_LegalEntityList"), response);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LegalEntityController)} action name {nameof(GetLegalEntityList)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }
        public async Task<IActionResult> CreateLegalEntity(int id)
        {
            try
            {

                await PopulateViewBag();
                if (id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("LegalEntity", "_LegalEntityCreate"));
                }
                else
                {
                    var response = new DBResponseHelper<LegalEntity, int>().GetDBResponseHelper(await _ISubsidiaryRepository.GetAllEntities(x => x.Id == id));
                    return PartialView(ViewHelper.GetViewPathDetails("LegalEntity", "_LegalEntityCreate"), response.Item2.Entities.First());
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LegalEntityController)} action name {nameof(CreateLegalEntity)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpSertLegalEntity(LegalEntity model, IFormFile FavIcon, IFormFile Logo)
        {
            try
            {

                var uploadLogoPath = await new BlobHelper().UploadImageToFolder(Logo, _IHostingEnviroment);
                var favIconPath = await new BlobHelper().UploadImageToFolder(FavIcon, _IHostingEnviroment);

                if (model.Id == 0)
                {
                    model.FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId"));
                    model.Logo = uploadLogoPath;
                    model.FavIcon = favIconPath;
                    model.CreatedDate = DateTime.Now;
                    model.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                    var response = await _ISubsidiaryRepository.CreateEntity(model);

                    return Json(response.Message);
                }
                else
                {
                    if (Logo != null)
                        model.Logo = uploadLogoPath;
                    if (FavIcon != null)
                        model.FavIcon = favIconPath;
                    model.UpdatedDate = DateTime.Now;
                    model.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                    var response = await _ISubsidiaryRepository.UpdateEntity(model);
                    return Json(response.Message);
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LegalEntityController)} action name {nameof(UpSertLegalEntity)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteLegalEntity(int id)
        {
            try
            {
                var deleteModel = await _ISubsidiaryRepository.GetAllEntityById(x => x.Id == id);
                var deleteDbModel = CrudHelper.DeleteHelper<LegalEntity>(deleteModel.Entity, 1);
                var deleteResponse = await _ISubsidiaryRepository.DeleteEntity(deleteDbModel);
                if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
                {
                    return Json(deleteResponse.Message);
                }
                return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LegalEntityController)} action name {nameof(DeleteLegalEntity)} exception is {ex.Message}";
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
