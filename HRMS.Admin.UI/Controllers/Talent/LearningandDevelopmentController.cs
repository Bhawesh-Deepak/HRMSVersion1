using HRMS.Admin.UI.AuthenticateService;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Entities.Talent;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Talent
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class LearningandDevelopmentController : Controller
    {

        private readonly IGenericRepository<LearningandDevelopment, int> _ILearningandDevelopmentRepository;
        private readonly IGenericRepository<Designation, int> _IDesignationRepository;

        public LearningandDevelopmentController(IGenericRepository<LearningandDevelopment, int> LearningandDevelopmentRepo,
          IGenericRepository<Designation, int> DesignationRepo)
        {
            _ILearningandDevelopmentRepository = LearningandDevelopmentRepo;
            _IDesignationRepository = DesignationRepo;

        }
        public async Task<IActionResult> Index()
        {
            try
            {

                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Talent/LearningandDevelopment", "LearningandDevelopmentIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LearningandDevelopmentController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetLearningandDepartmentList()
        {
            return View();
        }

        public async Task<IActionResult> CreateLearningandDevelopment(int id)
        {
            try
            {
                await PopulateViewBag();
                //var response = await _ILocationRepository.GetAllEntities(x => x.Id == id);
                if (id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("Talent/LearningandDevelopment", "LearningandDevelopmentCreate"));
                }
                else
                {
                    return PartialView(ViewHelper.GetViewPathDetails("Talent/LearningandDevelopment", "LearningandDevelopmentCreate"));
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LearningandDevelopment)} action name {nameof(CreateLearningandDevelopment)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        private async Task PopulateViewBag()
        {
            var designationResponse = await _IDesignationRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            if (designationResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.DesignationList = designationResponse.Entities;

        }

    }
}
