using HRMS.Admin.UI.AuthenticateService;
using HRMS.Core.Entities.Organisation;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.BlobHelper;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
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
    public class CompanyController : Controller
    {
        private readonly IGenericRepository<Company, int> _ICompanyRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;

        public CompanyController(IGenericRepository<Company, int> companyRepository, IHostingEnvironment hostingEnvironment)
        {
            _ICompanyRepository = companyRepository;
            _IHostingEnviroment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var companyModel = (await _ICompanyRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted))?.Entities?.FirstOrDefault();
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Organisation", "CompanyCreate"), companyModel));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Company)} action name {nameof(Index)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(Company model, IFormFile FavIcon, IFormFile Logo)
        {
            try
            {
                //store the Image into the folder
                var uploadLogoPath = await new BlobHelper().UploadImageToFolder(Logo, _IHostingEnviroment);
                var favIconPath = await new BlobHelper().UploadImageToFolder(FavIcon, _IHostingEnviroment);

                if (model.Id == 0)
                {
                    var createModel = CrudHelper.CreateHelper<Company>(model);
                    createModel.Logo = uploadLogoPath;
                    createModel.FavIcon = favIconPath;

                    var response = await _ICompanyRepository.CreateEntity(createModel);
                    return Json(response.Message);
                }
                else
                {
                    var updateModel = CrudHelper.UpdateHelper(model, 1);
                    updateModel.Logo = uploadLogoPath;
                    updateModel.FavIcon = favIconPath;
                    var response = await _ICompanyRepository.UpdateEntity(updateModel);
                    return Json(response.Message);
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Company)} action name {nameof(CreateCompany)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }
    }
}
