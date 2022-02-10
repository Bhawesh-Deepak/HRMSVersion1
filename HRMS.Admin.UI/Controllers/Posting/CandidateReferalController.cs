using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
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
    public class CandidateReferalController : Controller
    {
        private readonly IGenericRepository<CurrentOpening, int> _ICurrentOpeningRepository;
        private readonly IHostingEnvironment _IhostingEnviroment;
        private readonly IGenericRepository<ReferCandidate, int> _IReferCandidateRepository;
        public CandidateReferalController(IGenericRepository<CurrentOpening, int> iCurrentOpeningRepository, IHostingEnvironment ihostingEnviroment, IGenericRepository<ReferCandidate, int> iReferCandidateRepository)
        {
            _ICurrentOpeningRepository = iCurrentOpeningRepository;
            _IhostingEnviroment = ihostingEnviroment;
            _IReferCandidateRepository = iReferCandidateRepository;
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.HeaderTitle = PageHeader.HeaderSetting["CandidateReferal"];

            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("CandidateReferal", "CandidateReferalIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CandidateReferalController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateReferal(int id)
        {
            try
            {
                var model = new ReferCandidate();
                model.OpeningId = id;
                return PartialView(ViewHelper.GetViewPathDetails("CandidateReferal", "CandidateReferalCreate"), model);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CandidateReferalController)} action name {nameof(CreateReferal)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetCurrentOpeningDetails()
        {
            try
            {
                var openingDetails = await _ICurrentOpeningRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

                return PartialView(ViewHelper.GetViewPathDetails("CandidateReferal", "CandidateReferalDetail"), openingDetails.Entities);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CandidateReferalController)} action name {nameof(GetCurrentOpeningDetails)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpPost]
        public async Task<IActionResult> ReferalCandidate(ReferCandidate model, IFormFile Resume)
        {
            try
            {
                model.ResumePath = Resume == null ? model.ResumePath : await UploadPDFFile(Resume);

                var response = model.Id == 0 ? await CreateReferCandidate(model) : await UpdateReferCandidate(model);

                return Json(response);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CandidateReferalController)} action name {nameof(ReferalCandidate)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCandidateRefer(int openingId)
        {
            try
            {
                var models = await GetCandidateReferDetails(openingId);
                return PartialView(ViewHelper.GetViewPathDetails("CandidateReferal", "RefereCandidateDetail"), models);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CandidateReferalController)} action name {nameof(GetCandidateRefer)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpGet]
        public async Task<IActionResult> DeleteCandidate(int id)
        {
            try
            {
                var model = await _IReferCandidateRepository.GetAllEntityById(x => x.Id == id);

                var deleteModel = CrudHelper.DeleteHelper(model.Entity, Convert.ToInt32(HttpContext.Session.GetString("EmployeeId")));

                var deleteResponse = await _IReferCandidateRepository.DeleteEntity(deleteModel);

                return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CandidateReferalController)} action name {nameof(DeleteCandidate)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        #region PrivateMethods
        private async Task<ResponseStatus> CreateReferCandidate(ReferCandidate model)
        {
            model.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
            var response = await _IReferCandidateRepository.CreateEntity(CrudHelper.CreateHelper<ReferCandidate>(model));
            return response.ResponseStatus;
        }

        private async Task<ResponseStatus> UpdateReferCandidate(ReferCandidate model)
        {
            model.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
            var response = await _IReferCandidateRepository.UpdateEntity(CrudHelper.UpdateHelper<ReferCandidate>(model, 1));
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

        private async Task<List<ReferCandidate>> GetCandidateReferDetails(int openingId)
        {
            int empId = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));

            var response = await _IReferCandidateRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted
             && x.OpeningId == openingId && x.CreatedBy == empId);

            return response.Entities.ToList();
        }

        #endregion
    }
}
