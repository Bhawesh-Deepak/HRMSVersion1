using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Posting;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Posting;
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
    public class CandidateDetailController : Controller
    {
        private readonly IGenericRepository<CurrentOpening, int> _ICurrentOpeningRepository;
        private readonly IGenericRepository<CandidateDetail, int> _ICandidateDetailRepository;
        private readonly IHostingEnvironment _IhostingEnviroment;
        public CandidateDetailController(IGenericRepository<CurrentOpening, int> iCurrentOpeningRepository,
            IGenericRepository<CandidateDetail, int> candidateDetailRepository, IHostingEnvironment hostingEnvironment)
        {
            _ICurrentOpeningRepository = iCurrentOpeningRepository;
            _ICandidateDetailRepository = candidateDetailRepository;
            _IhostingEnviroment = hostingEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.HeaderTitle = PageHeader.HeaderSetting["CurrentOpeningIndex"];

                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("CandidateDetails", "CandidateDetailIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CandidateDetailController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetCandidateDetails()
        {
            try
            {
                var response = await _ICandidateDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var openingModels = await _ICurrentOpeningRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

                var models = (from CO in response.Entities
                              join OM in openingModels.Entities
                              on CO.JobTitleId equals OM.Id
                              select new CandidateDetailVm()
                              {
                                  Id = CO.Id,
                                  CandidateName = CO.CandidateName,
                                  EmailId = CO.EmailId,
                                  PhoneNumber = CO.PhoneNumber,
                                  ResumePath = CO.ResumePath,
                                  CandidateStatus = CO.CandidateStatus,
                                  ReferedBy = CO.ReferedBy,
                                  JobTitle = OM.Title
                              }).ToList();

                return PartialView(ViewHelper.GetViewPathDetails("CandidateDetails", "CandidateDetailDetails"), models);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CandidateDetailController)} action name {nameof(GetCandidateDetails)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> CreateCandidateDetail(int id)
        {
            try
            {
                await PopulateViewBag();

                var response = await _ICandidateDetailRepository.GetAllEntityById(x => x.Id == id);
                return PartialView(ViewHelper.GetViewPathDetails("CandidateDetails", "CandidateDetailCreate"), response.Entity);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CandidateDetailController)} action name {nameof(CreateCandidateDetail)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpSertCandidateDetails(CandidateDetail model, IFormFile resumePath)
        {
            try
            {
                model.ResumePath = await UploadPDFFile(resumePath);

                model.FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId"));

                var response = model.Id == 0 ? await CreateOpeningDb(model) : await UpdateOpeningDb(model);

                return Json(response);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CandidateDetailController)} action name {nameof(UpSertCandidateDetails)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }


        public async Task<IActionResult> DeleteCandidateDetails(int id)
        {
            try
            {
                var response = await _ICandidateDetailRepository.GetAllEntityById(x => x.Id == id);

                var deleteModel = CrudHelper.DeleteHelper(response.Entity, 1);

                var deleteResponse = await _ICandidateDetailRepository.DeleteEntity(deleteModel);

                if (deleteResponse.ResponseStatus == ResponseStatus.Deleted)
                {
                    return Json("Record Deleted successfully...");
                }

                return Json("Something wents wrong, Please contact admin team.");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CandidateDetailController)} action name {nameof(DeleteCandidateDetails)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        #region PrivateMethod

        public async Task<ResponseStatus> CreateOpeningDb(CandidateDetail model)
        {
            var response = await _ICandidateDetailRepository.CreateEntity(CrudHelper.CreateHelper<CandidateDetail>(model));
            return response.ResponseStatus;
        }

        public async Task<ResponseStatus> UpdateOpeningDb(CandidateDetail model)
        {
            var response = await _ICandidateDetailRepository.UpdateEntity(CrudHelper.UpdateHelper<CandidateDetail>(model, 1));
            return response.ResponseStatus;
        }

        public async Task PopulateViewBag()
        {
            ViewBag.CurrentOpening = (await _ICurrentOpeningRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted)).Entities;
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
        #endregion
    }
}
