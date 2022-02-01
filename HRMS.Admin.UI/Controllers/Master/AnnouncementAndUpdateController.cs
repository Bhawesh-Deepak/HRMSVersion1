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
using HRMS.Core.Entities.Organisation;
using HRMS.Admin.UI.AuthenticateService;

namespace HRMS.Admin.UI.Controllers.Master
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class AnnouncementAndUpdateController : Controller
    {
        private readonly IGenericRepository<AnnouncementAndUpdate, int> _IAnnoucementandupdateRepository;
        private readonly IGenericRepository<Department, int> _IDepartmentRepository;
        private readonly IGenericRepository<Branch, int> _IBranchRepository;

        public AnnouncementAndUpdateController(IGenericRepository<AnnouncementAndUpdate, int> AnnouncementandupdateRepo, IGenericRepository<Department, int> DepartmentRepo,
            IGenericRepository<Branch, int> BranchRepo)
        {
            _IAnnoucementandupdateRepository = AnnouncementandupdateRepo;
            _IDepartmentRepository = DepartmentRepo;
            _IBranchRepository = BranchRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.HeaderTitle = PageHeader.HeaderSetting["AnnouncementandudpateIndex"];
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("AnnouncementAndUpdate", "AnnouncementandupdateIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AnnouncementAndUpdate)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetAnnounceandupdateList()
        {
            try
            {
                var AnnouncementandupdateList = await _IAnnoucementandupdateRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var DepartmentList = await _IDepartmentRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var BranchList = await _IBranchRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

                var responseDetails = (from aul in AnnouncementandupdateList.Entities
                                       join dl in DepartmentList.Entities
                                       on aul.DepartmentId equals dl.Id
                                       join bl in BranchList.Entities
                                      on aul.BranchId equals bl.Id
                                       select new AnnouncementandupdateDetails
                                       {
                                           AnnandupdId=aul.Id,
                                           Branch = bl.Name,
                                           Department = dl.Name,
                                           Announcement = aul.Announcement,
                                           AnnouncementDate = aul.AnnouncementDate.ToString("dd-M-yyyy", CultureInfo.InvariantCulture),
                                           ApplicableDate = aul.ApplicableDate.ToString("dd-M-yyyy", CultureInfo.InvariantCulture)
                                       }).ToList();
                return PartialView(ViewHelper.GetViewPathDetails("AnnouncementAndUpdate", "AnnouncementandupdateDetails"), responseDetails);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AnnouncementAndUpdate)} action name {nameof(GetAnnounceandupdateList)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }

        public async Task<IActionResult> AnnounceandupdateCreate(int id)
        {
            try
            {
                await PopulateViewBag();
                var response = await _IAnnoucementandupdateRepository.GetAllEntities(x => x.Id == id);
                if (id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("AnnouncementAndUpdate", "AnnouncementandupdateCreate"));
                }
                else
                {

                    return PartialView(ViewHelper.GetViewPathDetails("AnnouncementAndUpdate", "AnnouncementandupdateCreate"), response.Entities.First());
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AnnouncementAndUpdate)} action name {nameof(AnnounceandupdateCreate)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertAnnouncementandupdate(AnnouncementAndUpdate model)
        {
            try
            {
                if (model.Id == 0)
                {
                    var response = await _IAnnoucementandupdateRepository.CreateEntity(model);
                    return Json(response.Message);
                }
                else
                {
                    var response = await _IAnnoucementandupdateRepository.UpdateEntity(model);
                    return Json(response.Message);
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AnnouncementAndUpdate)} action name {nameof(UpsertAnnouncementandupdate)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteAnnouncementandupdate(int id)
        {
            try
            {
                var deleteModel = await _IAnnoucementandupdateRepository.GetAllEntityById(x => x.Id == id);
                var deleteDbModel = CrudHelper.DeleteHelper<AnnouncementAndUpdate>(deleteModel.Entity, 1);
                var deleteResponse = await _IAnnoucementandupdateRepository.DeleteEntity(deleteDbModel);
                if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
                {
                    return Json(deleteResponse.Message);
                }
                return Json(deleteResponse.Message);
            }
            catch (Exception ex)
           {
                string template = $"Controller name {nameof(AnnouncementAndUpdate)} action name {nameof(DeleteAnnouncementandupdate)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var DepartmentResponse = await _IDepartmentRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var BranchResponse = await _IBranchRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            if (DepartmentResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.Department = DepartmentResponse.Entities;
            if (BranchResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.Branch = BranchResponse.Entities;

        }

        #endregion
    }
}
