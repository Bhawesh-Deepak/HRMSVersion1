using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Entities.Organisation;
using HRMS.Core.Entities.Posting;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Posting
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class CurrentOpeningController : Controller
    {
        private readonly IGenericRepository<CurrentOpening, int> _ICurrentOpeningRepository;
        private readonly IGenericRepository<Branch, int> _IBranchMasterRepository;
        private readonly IGenericRepository<Department, int> _IDepartmentRepository;
        private readonly IGenericRepository<Designation, int> _IDesignationRepository;
        private readonly IGenericRepository<LegalEntity, int> _ILegalEntityRepository;
        private readonly IHostingEnvironment _IhostingEnviroment;

        public CurrentOpeningController(IGenericRepository<CurrentOpening, int> currentOpeningRepository,
            IHostingEnvironment hostingEnviroment,
            IGenericRepository<Branch, int> branchMasterRepository,
            IGenericRepository<Department, int> departmentRepository,
            IGenericRepository<Designation, int> designationRepository, IGenericRepository<LegalEntity, int> legalEntityRepository)
        {
            _ICurrentOpeningRepository = currentOpeningRepository;
            _IhostingEnviroment = hostingEnviroment;
            _IBranchMasterRepository = branchMasterRepository;
            _IDepartmentRepository = departmentRepository;
            _IDesignationRepository = designationRepository;
            _ILegalEntityRepository = legalEntityRepository;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.HeaderTitle = PageHeader.HeaderSetting["CurrentOpeningIndex"];

                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("CurrentOpening", "CurrentOpeningIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CurrentOpeningController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOpeningDetails()
        {
            try
            {

                var currentopening = await _ICurrentOpeningRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var legalentitylist = await _ILegalEntityRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var branchlist = await _IBranchMasterRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var departmentlist = await _IDepartmentRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var designationlist = await _IDesignationRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

                var responseDetails = (from currentopeening in currentopening.Entities
                                       join branch in branchlist.Entities on currentopeening.BranchId equals branch.Id
                                       into Bn
                                       from branch in Bn.DefaultIfEmpty()
                                       join department in departmentlist.Entities on currentopeening.DepartmentId equals department.Id
                                       into dp
                                       from department in dp.DefaultIfEmpty()
                                       join legalentity in legalentitylist.Entities on currentopeening.EntityId equals legalentity.Id
                                       into le
                                       from legalentity in le.DefaultIfEmpty()
                                       join designation in designationlist.Entities on currentopeening.DesignationId equals designation.Id
                                       into dl
                                       from designation in dl.DefaultIfEmpty()
                                       select new CurrentOpening
                                       {
                                           BranchName = branch != null ? branch.Name : "",
                                           DepartmentName = department != null ? department.Name : "",
                                           EntityName = legalentity != null ? legalentity.Name : "",
                                           Designation = designation != null ? designation.Name : "",
                                           Title = currentopeening.Title,
                                           RequiredExprience = currentopeening.RequiredExprience,
                                           KeySkills = currentopeening.KeySkills,
                                           Vacancy = currentopeening.Vacancy,
                                           OpeningDate = currentopeening.OpeningDate,
                                           ClosingDate = currentopeening.ClosingDate,
                                           JobDescription = currentopeening.JobDescription

                                       }).ToList();
                return PartialView(ViewHelper.GetViewPathDetails("CurrentOpening", "CurrentOpeningDetails"), responseDetails);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CurrentOpeningController)} action name {nameof(GetOpeningDetails)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateOpening(int id)
        {
            try
            {
                await PopulateViewBag();

                var response = await _ICurrentOpeningRepository.GetAllEntityById(x => x.Id == id);
                return PartialView(ViewHelper.GetViewPathDetails("CurrentOpening", "CurrentOpeningCreate"), response.Entity);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CurrentOpeningController)} action name {nameof(CreateOpening)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpSertOpening(CurrentOpening model, IFormFile PdfFile)
        {
            try
            {
                model.FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId"));
                model.DescriptionPath = PdfFile == null ? model.DescriptionPath : await UploadPDFFile(PdfFile);
                model.LastApplyDate = model.ClosingDate;
                model.Location = string.Empty;


                var response = model.Id == 0 ? await CreateOpeningDb(model) : await UpdateOpeningDb(model);

                return Json(response);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CurrentOpeningController)} action name {nameof(UpSertOpening)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> DeleteOpening(int id)
        {
            try
            {
                var response = await _ICurrentOpeningRepository.GetAllEntityById(x => x.Id == id);

                var deleteModel = CrudHelper.DeleteHelper(response.Entity, 1);

                var deleteResponse = await _ICurrentOpeningRepository.DeleteEntity(deleteModel);

                if (deleteResponse.ResponseStatus == ResponseStatus.Deleted)
                {
                    return Json("Record Deleted successfully...");
                }

                return Json("Something wents wrong, Please contact admin team.");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CurrentOpeningController)} action name {nameof(DeleteOpening)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }



        #region PrivateMethods

        public async Task<ResponseStatus> CreateOpeningDb(CurrentOpening model)
        {
            var response = await _ICurrentOpeningRepository.CreateEntity(CrudHelper.CreateHelper<CurrentOpening>(model));
            return response.ResponseStatus;
        }

        public async Task<ResponseStatus> UpdateOpeningDb(CurrentOpening model)
        {
            var response = await _ICurrentOpeningRepository.UpdateEntity(CrudHelper.UpdateHelper<CurrentOpening>(model, 1));
            return response.ResponseStatus;
        }

        private async Task<string> UploadPDFFile(IFormFile pdfFile)
        {
            string imagePath = string.Empty;

            if (pdfFile != null && pdfFile.Length > 0)
            {
                var upload = Path.Combine(_IhostingEnviroment.WebRootPath, "PDF//");
                using (FileStream fs = new FileStream(Path.Combine(upload, pdfFile.FileName), FileMode.Create))
                {
                    await pdfFile.CopyToAsync(fs);
                }
                imagePath = "/PDF/" + pdfFile.FileName;
            }

            return imagePath;
        }

        private async Task PopulateViewBag()
        {
            ViewBag.Departments = (await _IDepartmentRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted)).Entities;
            ViewBag.Designation = (await _IDesignationRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted)).Entities;
            var branchs = (await _IBranchMasterRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted)).Entities.ToList();
            branchs.ToList().ForEach(data =>
            {
                data.Name = $"{data.Name} ({data.Code})";
            });

            ViewBag.Branchs = branchs;
            var entities = (await _ILegalEntityRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted)).Entities;
            entities.ToList().ForEach(data =>
            {
                data.Name = $"{data.Name} ({data.Code})";

            });

            ViewBag.LegalEntity = entities;
        }
        #endregion
    }
}
