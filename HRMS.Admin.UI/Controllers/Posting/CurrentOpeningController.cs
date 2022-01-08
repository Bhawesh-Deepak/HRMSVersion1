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
    public class CurrentOpeningController : Controller
    {
        private readonly IGenericRepository<CurrentOpening, int> _ICurrentOpeningRepository;
        private readonly IHostingEnvironment _IhostingEnviroment;

        public CurrentOpeningController(IGenericRepository<CurrentOpening, int> currentOpeningRepository,
            IHostingEnvironment hostingEnviroment)
        {
            _ICurrentOpeningRepository = currentOpeningRepository;
            _IhostingEnviroment = hostingEnviroment;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["CurrentOpeningIndex"];

            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("CurrentOpening", "CurrentOpeningIndex")));
        }

        [HttpGet]
        public async Task<IActionResult> GetOpeningDetails()
        {
            var response = await _ICurrentOpeningRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            return PartialView(ViewHelper.GetViewPathDetails("CurrentOpening", "CurrentOpeningDetails"), response.Entities);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOpening(int id)
        {
            var response = await _ICurrentOpeningRepository.GetAllEntityById(x => x.Id == id);
            return PartialView(ViewHelper.GetViewPathDetails("CurrentOpening", "CurrentOpeningCreate"), response.Entity);
        }

        [HttpPost]
        public async Task<IActionResult> UpSertOpening(CurrentOpening model, IFormFile PdfFile)
        {
            model.DescriptionPath = PdfFile == null ? model.DescriptionPath : await UploadPDFFile(PdfFile);

            var response = model.Id == 0 ? await CreateOpeningDb(model) : await UpdateOpeningDb(model);

            return Json(response);
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
                Serilog.Log.Error(ex, $"COntroller name is {nameof(CurrentOpeningController)} action name {nameof(DeleteOpening)}");
                return Json("Something wents wrong, Please contact admin team.");
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

        #endregion
    }
}
