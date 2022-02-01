using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.BlobHelper;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Master;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Master
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class CompanyPolicyController : Controller
    {
        private readonly IGenericRepository<CompanyPolicy, int> _ICompanyPolicyRepository;
        private readonly IGenericRepository<Department, int> _IDepartmentRepository;

        private readonly IHostingEnvironment _IHostingEnviroment;
        public CompanyPolicyController(IGenericRepository<CompanyPolicy, int> CompanyPolicyRepo,
            IGenericRepository<Department, int> DepartmentRepo, IHostingEnvironment hostingEnvironment)
        {
            _IHostingEnviroment = hostingEnvironment;
            _ICompanyPolicyRepository = CompanyPolicyRepo;
            _IDepartmentRepository = DepartmentRepo;

        }
        public async Task<IActionResult> Index()
        {
            try
            {
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["CompanyPolicyIndex"];
            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("CompanyPolicy", "CompanyPolicyIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CompanyPolicy)} action name {nameof(Index)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetCompanyPolicyList()
        {
            try
            {
                var CompanyPolList = await _ICompanyPolicyRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var DepartList = await _IDepartmentRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

                var responseDetails = (from dpt in DepartList.Entities
                                       join cpl in CompanyPolList.Entities
                                       on dpt.Id equals cpl.DepartmentId
                                       select new CompanyPolicyDetails
                                       {
                                           CompanyPolicyId = cpl.Id,
                                           DepartmentName = dpt.Name,
                                           CalenderDate = cpl.CalenderDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                                           Name=cpl.Name,
                                           DocumentUrl=cpl.DocumentUrl

                                       }).ToList();

                return PartialView(ViewHelper.GetViewPathDetails("CompanyPolicy", "CompanyPolicyDetails"), responseDetails);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CompanyPolicy)} action name {nameof(GetCompanyPolicyList)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> CreateCompanyPolicy(int id)
        {
            try
            {
            await PopulateViewBag();
            var response = await _ICompanyPolicyRepository.GetAllEntities(x => x.Id == id);
            if (id == 0)
            {
                return PartialView(ViewHelper.GetViewPathDetails("CompanyPolicy", "CompanyPolicyCreate"));
            }
            else
            {
                return PartialView(ViewHelper.GetViewPathDetails("CompanyPolicy", "CompanyPolicyCreate"), response.Entities.First());
            }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CompanyPolicy)} action name {nameof(CreateCompanyPolicy)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertCompanyPolicy(CompanyPolicy model,List<int> DepartmentId, IFormFile DocumentUrl)
        {
            try
            {
            model.DocumentUrl= await new BlobHelper().UploadImageToFolder(DocumentUrl, _IHostingEnviroment);
            if (model.Id == 0)
            {
                var response = await _ICompanyPolicyRepository.CreateEntity(model);
                return Json(response.Message);
            }
            else
            {
                var response = await _ICompanyPolicyRepository.UpdateEntity(model);
                return Json(response.Message);
            }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CompanyPolicy)} action name {nameof(UpsertCompanyPolicy)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteCompanyPolicy(int id)
        {
            try
            {
            var deleteModel = await _ICompanyPolicyRepository.GetAllEntityById(x => x.Id == id);
            var deleteDbModel = CrudHelper.DeleteHelper<CompanyPolicy>(deleteModel.Entity, 1);
            var deleteResponse = await _ICompanyPolicyRepository.DeleteEntity(deleteDbModel);

            if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
            {
                return Json(deleteResponse.Message);
            }
            return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CompanyPolicy)} action name {nameof(DeleteCompanyPolicy)} exceptio is {ex.Message}";
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
