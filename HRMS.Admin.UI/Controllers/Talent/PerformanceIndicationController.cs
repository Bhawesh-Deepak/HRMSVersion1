using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Admin.UI.AuthenticateService;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Entities.Talent;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Admin.UI.Controllers.Talent
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class PerformanceIndicationController : Controller
    {

        private readonly IGenericRepository<Designation, int> _IDesignationRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;
        private readonly IGenericRepository<PerformanceIndication, int> _IPerformanceIndicationRepository;
        public PerformanceIndicationController(IHostingEnvironment hostingEnvironment, IGenericRepository<Designation, int> designationRepo,
            IGenericRepository<PerformanceIndication, int> performanceIndicationRepo)
        {
            _IDesignationRepository = designationRepo;
            _IPerformanceIndicationRepository = performanceIndicationRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("PerformanceIndication", "_PerformanceIndicationIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(PerformanceIndicationController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> CreatePerformanceIndication(int id)
        {
            try
            {
                await PopulateViewBag();
                var response = await _IPerformanceIndicationRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.DesgignationId==id );
                 
                return PartialView(ViewHelper.GetViewPathDetails("PerformanceIndication", "_PerformanceIndicationCreate"), response.Entities);

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(PerformanceIndicationController)} action name {nameof(CreatePerformanceIndication)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var designationResponse = await _IDesignationRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            if (designationResponse.ResponseStatus == ResponseStatus.Success)

                ViewBag.DesignationList = designationResponse.Entities;


        }

        #endregion
    }
}