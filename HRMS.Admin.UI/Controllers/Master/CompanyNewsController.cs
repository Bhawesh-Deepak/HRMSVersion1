using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Master;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Master
{
    public class CompanyNewsController : Controller
    {
        private readonly IGenericRepository<CompanyNews, int> _ICompanyNewsRepository;
        private readonly IGenericRepository<Department, int> _IDepartmentRepository;

        public CompanyNewsController(IGenericRepository<CompanyNews, int> CompanynewsRepo, IGenericRepository<Department, int> DepartmentRepo)
        {
            _ICompanyNewsRepository = CompanynewsRepo;
            _IDepartmentRepository = DepartmentRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["CompanyNewsIndex"];
            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("CompanyNews", "CompanyNewsIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CompanyNews)} action name {nameof(Index)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetCompanyNewsList()
        {
            try
            {
                var CompanyNews = await _ICompanyNewsRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var DepartmentList = await _IDepartmentRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

                var responseDetails = (from dpt in DepartmentList.Entities
                                       join cpn in CompanyNews.Entities
                                       on dpt.Id equals cpn.DepartmentId
                                       select new CompanyNewsDetails
                                       {
                                           CompanyNewsId = cpn.Id,
                                           DepartmentName = dpt.Name,
                                           CompanyNewsName = cpn.Name,
                                           NewsDate = cpn.NewsDate.ToString("dd-M-yyyy", CultureInfo.InvariantCulture),
                                       }).ToList();
                return PartialView(ViewHelper.GetViewPathDetails("CompanyNews", "CompanyNewsDetails"), responseDetails);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CompanyNews)} action name {nameof(GetCompanyNewsList)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }

        public async Task<IActionResult> CompanyNewsCreate(int id)
        {
            try
            {
                await PopulateViewBag();
            var response = await _ICompanyNewsRepository.GetAllEntities(x => x.Id == id);

            if (id == 0)
            {
                return PartialView(ViewHelper.GetViewPathDetails("CompanyNews", "CompanyNewsCreate"));
            }
            else
            {
                return PartialView(ViewHelper.GetViewPathDetails("CompanyNews", "CompanyNewsCreate"), response.Entities.First());
            }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CompanyNews)} action name {nameof(CompanyNewsCreate)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertCompanyNews(CompanyNews model)
        {
            try
            {
            if (model.Id == 0)
            {
                var response = await _ICompanyNewsRepository.CreateEntity(model);
                return Json(response.Message);
            }
            else
            {
                var response = await _ICompanyNewsRepository.UpdateEntity(model);
                return Json(response.Message);
            }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CompanyNews)} action name {nameof(UpsertCompanyNews)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteCompanyNews(int id)
        {
            try
            {
                var deleteModel = await _ICompanyNewsRepository.GetAllEntityById(x => x.Id == id);
                var deleteDbModel = CrudHelper.DeleteHelper<CompanyNews>(deleteModel.Entity, 1);
                var deleteResponse = await _ICompanyNewsRepository.DeleteEntity(deleteDbModel);
                if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
                {
                    return Json(deleteResponse.Message);
                }
                return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CompanyNews)} action name {nameof(DeleteCompanyNews)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var StateResponse = await _IDepartmentRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            if (StateResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.Department = StateResponse.Entities;

        }

        #endregion
    }
}
