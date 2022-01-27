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
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Master
{
    [CustomAuthenticate]
    public class BranchController : Controller
    {
        private readonly IGenericRepository<Branch, int> _IBranchRepository;
        private readonly IGenericRepository<Subsidiary, int> _ISubsidiaryRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;

        public BranchController(IGenericRepository<Branch, int> BranchRepo, IHostingEnvironment hostingEnvironment,
            IGenericRepository<Subsidiary, int> SubsidiaryRepo)
        {
            _IBranchRepository = BranchRepo;
            _ISubsidiaryRepository = SubsidiaryRepo;
            _IHostingEnviroment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.HeaderTitle = PageHeader.HeaderSetting["BranchIndex"];
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Branch", "BranchIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Branch)} action name {nameof(Index)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetBranchList()
        {
            try
            {
                var BranchList = await _IBranchRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var CompanyList = await _ISubsidiaryRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

                var responseDetails = (from cl in CompanyList.Entities
                                       join bl in BranchList.Entities
                                       on cl.Id equals bl.CompanyId
                                       select new BranchDetails
                                       {
                                           BranchId = bl.Id,
                                           CompanyName = cl.Name,
                                           Name=bl.Name,
                                           Code = bl.Code,
                                           Logo=bl.Logo,
                                           Address=bl.Address,
                                           ZipCode=bl.ZipCode,
                                           Email=bl.Email,
                                           ContactPerson=bl.ContactPerson,
                                           Phone=bl.Phone
                                       }).ToList();

                return PartialView(ViewHelper.GetViewPathDetails("Branch", "BranchDetails"), responseDetails);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Branch)} action name {nameof(GetBranchList)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> CreateBranch(int id)
        {
            try
            {
                await PopulateViewBag();
                var response = await _IBranchRepository.GetAllEntities(x => x.Id == id);
                if(id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("Branch", "BranchCreate"));
                }
                else
                {
                    return PartialView(ViewHelper.GetViewPathDetails("Branch", "BranchCreate"), response.Entities.First());
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Branch)} action name {nameof(CreateBranch)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertBranch(Branch model, IFormFile Logo)
        {
            try
            {
            model.Logo = await new BlobHelper().UploadImageToFolder(Logo, _IHostingEnviroment);
            if (model.Id == 0)
            {
                var response = await _IBranchRepository.CreateEntity(model);
                return Json(response.Message);
            }
            else
            {
                var response = await _IBranchRepository.UpdateEntity(model);
                return Json(response.Message);
            }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Branch)} action name {nameof(UpsertBranch)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            try
            {
            var deleteModel = await _IBranchRepository.GetAllEntityById(x => x.Id == id);
            var deleteDbModel = CrudHelper.DeleteHelper<Branch>(deleteModel.Entity, 1);
            var deleteResponse = await _IBranchRepository.DeleteEntity(deleteDbModel);
            if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
            {
                return Json(deleteResponse.Message);
            }
            return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Branch)} action name {nameof(DeleteBranch)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var departmentResponse = await _ISubsidiaryRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            if (departmentResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.CompanyList = departmentResponse.Entities;

        }

        #endregion
    }
}
