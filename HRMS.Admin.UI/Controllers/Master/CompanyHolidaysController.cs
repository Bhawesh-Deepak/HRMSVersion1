using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Master
{
    public class CompanyHolidaysController : Controller
    {
        private readonly IGenericRepository<CompanyHolidays, int> _ICompanyHolidaysRepository;

        public CompanyHolidaysController(IGenericRepository<CompanyHolidays, int> CompanyHolidayRepo)
        {
            _ICompanyHolidaysRepository = CompanyHolidayRepo;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["CompanyHolidaysIndex"];
            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("CompanyHolidays", "CompanyHolidaysIndex")));
        }

        public async Task<IActionResult> GetHolidayList()
        {
            try
            {
                var response = new DBResponseHelper<CompanyHolidays, int>().GetDBResponseHelper(await _ICompanyHolidaysRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted));

                return PartialView(ViewHelper.GetViewPathDetails("CompanyHolidays", "CompanyHolidayDetails"), response.Item2.Entities);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Department)} action name {nameof(GetHolidayList)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }

        public async Task<IActionResult> HolidayCreate(int id)
        {
            await PopulateViewBag();
            var response = await _ICompanyHolidaysRepository.GetAllEntities(x => x.Id == id);

            if (id == 0)
            {
                return PartialView(ViewHelper.GetViewPathDetails("CompanyHolidays", "CompanyHolidayCreate"));
            }
            else
            {

                return PartialView(ViewHelper.GetViewPathDetails("CompanyHolidays", "CompanyHolidayCreate"), response.Entities.First());
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertCompanyHoliday(CompanyHolidays model)
        {
            if (model.Id == 0)
            {
                var response = await _ICompanyHolidaysRepository.CreateEntity(model);
                return Json(response.Message);
            }
            else
            {
                var response = await _ICompanyHolidaysRepository.UpdateEntity(model);
                return Json(response.Message);
            }
        }

        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var departmentResponse = await _ICompanyHolidaysRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            if (departmentResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.DepartmentList = departmentResponse.Entities;

        }

        #endregion

    }
}
