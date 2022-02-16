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
    public class CourseController : Controller
    {
        private readonly IGenericRepository<Course, int> _ICourseRepository;
        private readonly IGenericRepository<Designation, int> _IDesignationRepository;
        private readonly IGenericRepository<CourseMode, int> _ICourseModeRepository;

        public CourseController(IGenericRepository<Course, int> CourseRepo,
         IGenericRepository<Designation, int> DesignationRepo, IGenericRepository<CourseMode, int> courseModeRepo)
        {
            _ICourseRepository = CourseRepo;
            _IDesignationRepository = DesignationRepo;
            _ICourseModeRepository = courseModeRepo;

        }
        public async Task<IActionResult> Index()
        {
            try
            {
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Course", "_CourseIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CourseController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> GetCourseList()
        {
            try
            {
                var courseResponse = await _ICourseRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var designationResponse = await _IDesignationRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var courseModeResponse = await _ICourseModeRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

                var responseDetails = (from course in courseResponse.Entities
                                       join designation in designationResponse.Entities on course.DesignationId equals designation.Id
                                       join coursemode in courseModeResponse.Entities on course.CourseModeId equals coursemode.Id
                                       select new Course
                                       {
                                           Id = course.Id,
                                           Name = course.Name,
                                           Code = course.Code,
                                           StartDate = course.StartDate,
                                           EndDate = course.EndDate,
                                           TrainingDateTime=course.TrainingDateTime,
                                           DesignationName= designation.Name,
                                           CourseModeName=coursemode.Name

                                       }).ToList();

                return PartialView(ViewHelper.GetViewPathDetails("Course", "_CourseList"), responseDetails);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CourseController)} action name {nameof(GetCourseList)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> CreateCourse(int id)
        {
            try
            {
                await PopulateViewBag();

                if (id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("Course", "_CourseCreate"));
                }
                else
                {
                    var response = await _ICourseRepository.GetAllEntities(x => x.Id == id);
                    return PartialView(ViewHelper.GetViewPathDetails("Course", "_CourseCreate"), response.Entities.First());
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CourseController)} action name {nameof(CreateCourse)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertCourse(Course model)
        {
            try
            {

                if (model.Id == 0)
                {
                    model.FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId"));
                    model.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                    model.CreatedDate = DateTime.Now;
                    var response = await _ICourseRepository.CreateEntity(model);
                    return Json(response.Message);
                }
                else
                {
                    model.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                    model.UpdatedDate = DateTime.Now;
                    var response = await _ICourseRepository.UpdateEntity(model);
                    return Json(response.Message);
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CourseController)} action name {nameof(UpsertCourse)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                var deleteModel = await _ICourseRepository.GetAllEntityById(x => x.Id == id);
                var deleteDbModel = CrudHelper.DeleteHelper<Course>(deleteModel.Entity, 1);
                var deleteResponse = await _ICourseRepository.DeleteEntity(deleteDbModel);
                if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
                {
                    return Json(deleteResponse.Message);
                }
                return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CourseController)} action name {nameof(DeleteCourse)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        private async Task PopulateViewBag()
        {
            var designationResponse = await _IDesignationRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var courseModeResponse = await _ICourseModeRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            if (designationResponse.ResponseStatus == ResponseStatus.Success&& courseModeResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.DesignationList = designationResponse.Entities;
            ViewBag.CourseModeList = courseModeResponse.Entities;

        }

    }
}
