using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Entities.Organisation;
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
        private readonly IGenericRepository<LegalEntity, int> _ILegalEntityRepository;

        private readonly IHostingEnvironment _IHostingEnviroment;
        public CompanyPolicyController(IGenericRepository<CompanyPolicy, int> CompanyPolicyRepo,
            IGenericRepository<LegalEntity, int> legalentityRepo, IHostingEnvironment hostingEnvironment)
        {
            _IHostingEnviroment = hostingEnvironment;
            _ICompanyPolicyRepository = CompanyPolicyRepo;
            _ILegalEntityRepository = legalentityRepo;

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
                string template = $"Controller name {nameof(CompanyPolicy)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetCompanyPolicyList()
        {
            try
            {
                var ploicyresponse = await _ICompanyPolicyRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var legalentityresponse = await _ILegalEntityRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var responseDetails = (from ploicy in ploicyresponse.Entities
                                       join legalentity in legalentityresponse.Entities
                                       on ploicy.LegalEntityId equals legalentity.Id
                                       into ploicyes
                                       from legalentity in ploicyes.DefaultIfEmpty()
                                       select new CompanyPolicyDetails
                                       {
                                           Id = ploicy.Id,
                                           LegalEntityName = legalentity != null ? legalentity.Name : "Pan India",
                                           CalenderDate = ploicy.CalenderDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                                           Name = ploicy.Name,
                                           DocumentUrl = ploicy.DocumentUrl
                                       }).ToList();

                return PartialView(ViewHelper.GetViewPathDetails("CompanyPolicy", "CompanyPolicyDetails"), responseDetails);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CompanyPolicy)} action name {nameof(GetCompanyPolicyList)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> CreateCompanyPolicy(int id)
        {
            try
            {
                await PopulateViewBag();
                if (id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("CompanyPolicy", "CompanyPolicyCreate"));
                }
                else
                {

                    var response = await _ICompanyPolicyRepository.GetAllEntities(x => x.Id == id);
                    return PartialView(ViewHelper.GetViewPathDetails("CompanyPolicy", "CompanyPolicyCreate"), response.Entities.First());
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CompanyPolicy)} action name {nameof(CreateCompanyPolicy)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertCompanyPolicy(CompanyPolicy model, List<int> LegalEntityId, IFormFile DocumentUrl)
        {
            try
            {
                model.DocumentUrl = await new BlobHelper().UploadImageToFolder(DocumentUrl, _IHostingEnviroment);

                var companypolicy = new List<CompanyPolicy>();
                if (model.Id == 0)
                {
                    if (LegalEntityId.Count() > 0)
                    {
                        foreach (var data in LegalEntityId)
                        {
                            companypolicy.Add(new CompanyPolicy()
                            {
                                FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId")),
                                LegalEntityId = data,
                                Name = model.Name,
                                CalenderDate = model.CalenderDate,
                                DocumentUrl = model.DocumentUrl,
                                CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId")),
                                CreatedDate = DateTime.Now,
                            });
                        }
                    }
                    else
                    {
                        companypolicy.Add(new CompanyPolicy()
                        {
                            FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId")),
                            Name = model.Name,
                            CalenderDate = model.CalenderDate,
                            DocumentUrl = model.DocumentUrl,
                            CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId")),
                            CreatedDate = DateTime.Now,
                        });
                    }
                    var response = await _ICompanyPolicyRepository.CreateEntities(companypolicy.ToArray());
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
                string template = $"Controller name {nameof(CompanyPolicy)} action name {nameof(UpsertCompanyPolicy)} exception is {ex.Message}";
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
                string template = $"Controller name {nameof(CompanyPolicy)} action name {nameof(DeleteCompanyPolicy)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var legalentity = await _ILegalEntityRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            if (legalentity.ResponseStatus == ResponseStatus.Success)
                ViewBag.LegalEntityList = legalentity.Entities;

        }

        #endregion
    }
}
