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
    public class CourseController : Controller
    {
        private readonly IGenericRepository<Course, int> _ICourseRepository;
        private readonly IGenericRepository<Designation, int> _IDesignationRepository;

        public CourseController(IGenericRepository<Course, int> CourseRepo,
         IGenericRepository<Designation, int> DesignationRepo)
        {
            _ICourseRepository = CourseRepo;
            _IDesignationRepository = DesignationRepo;

        }
        public async Task<IActionResult> Index()
        {
            try
            {
                // ViewBag.HeaderTitle = PageHeader.HeaderSetting["LocationIndex"];
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Talent/Course", "CourseIndex")));
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
            return View();
        }

        public async Task<IActionResult> CreateCourse(int id)
        {
            try
            {
                await PopulateViewBag();
                //var response = await _ILocationRepository.GetAllEntities(x => x.Id == id);
                if (id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("Talent/Course", "CourseCreate"));
                }
                else
                {
                    return PartialView(ViewHelper.GetViewPathDetails("Talent/Course", "CourseCreate"));
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CreateCourse)} action name {nameof(CreateCourse)} exception is {ex.Message}";
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
