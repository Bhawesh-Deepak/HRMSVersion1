using HRMS.Admin.UI.AuthenticateService;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Entities.Talent;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace HRMS.Admin.UI.Controllers.Talent
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class LearningAndDevelopmentController : Controller
    {

        private readonly IGenericRepository<LearningAndDevelopment, int> _ILearningandDevelopmentRepository;
        private readonly IGenericRepository<Designation, int> _IDesignationRepository;
        private readonly IGenericRepository<LAndDHour, int> _ILAndDHourRepository;

        public LearningAndDevelopmentController(IGenericRepository<LearningAndDevelopment, int> LearningandDevelopmentRepo,
          IGenericRepository<Designation, int> DesignationRepo, IGenericRepository<LAndDHour, int> landDHourRepo)
        {
            _ILearningandDevelopmentRepository = LearningandDevelopmentRepo;
            _IDesignationRepository = DesignationRepo;
            _ILAndDHourRepository = landDHourRepo;

        }
        public async Task<IActionResult> Index()
        {
            try
            {
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("LearningAndDevelopment", "_LearningAndDevelopmentIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LearningAndDevelopmentController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetLearningAndDevelopmentList()
        {
            try
            {
                var landdhoursResponse = await _ILAndDHourRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var designationResponse = await _IDesignationRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var learninganddevelopmentResponse = await _ILearningandDevelopmentRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

                var responseDetails = (from learnig in learninganddevelopmentResponse.Entities
                                       join designation in designationResponse.Entities on learnig.DesignationId equals designation.Id
                                       join hrs in landdhoursResponse.Entities on learnig.LAndDHourId equals hrs.Id
                                       select new LearningAndDevelopment
                                       {
                                           Id = learnig.Id,
                                           LAndDHourId = learnig.LAndDHourId,
                                           DesignationName = designation.Name,
                                           LAndDHour = hrs.Name

                                       }).ToList();

                return PartialView(ViewHelper.GetViewPathDetails("LearningAndDevelopment", "_LearningAndDevelopmentList"), responseDetails);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LearningAndDevelopmentController)} action name {nameof(GetLearningAndDevelopmentList)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> CreateLearningAndDevelopment(int id)
        {
            try
            {
                await PopulateViewBag();

                if (id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("LearningAndDevelopment", "_LearningAndDevelopmentCreate"));
                }
                else
                {
                    var response = await _ILearningandDevelopmentRepository.GetAllEntities(x => x.Id == id);
                    return PartialView(ViewHelper.GetViewPathDetails("LearningAndDevelopment", "_LearningAndDevelopmentCreate"), response.Entities.First());
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LearningAndDevelopmentController)} action name {nameof(CreateLearningAndDevelopment)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertLearningAndDevelopment(LearningAndDevelopment model, List<int> DesignationId)
        {
            try
            {
                var LearningDevelopment = new List<LearningAndDevelopment>();
                if (model.Id == 0)
                {
                    foreach (var item in DesignationId)
                    {
                        LearningDevelopment.Add(new LearningAndDevelopment
                        {
                            FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId")),
                            CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId")),
                            CreatedDate = DateTime.Now,
                            DesignationId=item,
                            LAndDHourId=model.LAndDHourId
                        });
                    }
                    var response = await _ILearningandDevelopmentRepository.CreateEntities(LearningDevelopment.ToArray());
                    return Json(response.Message);
                }
                else
                {
                    model.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                    model.UpdatedDate = DateTime.Now;
                    var response = await _ILearningandDevelopmentRepository.UpdateEntity(model);
                    return Json(response.Message);
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LearningAndDevelopmentController)} action name {nameof(UpsertLearningAndDevelopment)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteLearningAndDevelopment(int id)
        {
            try
            {
                var deleteModel = await _ILearningandDevelopmentRepository.GetAllEntityById(x => x.Id == id);
                var deleteDbModel = CrudHelper.DeleteHelper<LearningAndDevelopment>(deleteModel.Entity, 1);
                var deleteResponse = await _ILearningandDevelopmentRepository.DeleteEntity(deleteDbModel);
                if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
                {
                    return Json(deleteResponse.Message);
                }
                return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LearningAndDevelopmentController)} action name {nameof(DeleteLearningAndDevelopment)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        private async Task PopulateViewBag()
        {
            var designationResponse = await _IDesignationRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var LAndDHourResponse = await _ILAndDHourRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            if (designationResponse.ResponseStatus == ResponseStatus.Success && LAndDHourResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.DesignationList = designationResponse.Entities;
            ViewBag.LAndDHourList = LAndDHourResponse.Entities;

        }

    }
}
