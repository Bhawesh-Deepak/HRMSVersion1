using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Master;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;


namespace HRMS.Admin.UI.Controllers.Master
{
    [CustomAuthenticate]
    public class CompanyHolidaysController : Controller
    {
        private readonly IGenericRepository<CompanyHolidays, int> _ICompanyHolidaysRepository;
        private readonly IGenericRepository<StateMaster, int> _IStateRepository;

        public CompanyHolidaysController(IGenericRepository<CompanyHolidays, int> CompanyHolidayRepo,IGenericRepository<StateMaster, int> StateRepo )
        {
            _ICompanyHolidaysRepository = CompanyHolidayRepo;
            _IStateRepository = StateRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.HeaderTitle = PageHeader.HeaderSetting["CompanyHolidaysIndex"];
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("CompanyHolidays", "CompanyHolidaysIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CompanyHolidays)} action name {nameof(Index)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetHolidayList()
        {
            try
            {
                var HolidayList = await _ICompanyHolidaysRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var StateList = await _IStateRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

                var responseDetails = (from stl in StateList.Entities
                                       join hdl in HolidayList.Entities
                                       on stl.Id equals hdl.StateId
                                       select new HolidaysDetails
                                       {
                                           HolidayId=hdl.Id,
                                           StateName=stl.Name,
                                           HolidayName = hdl.Name,
                                           Holidate = hdl.HolidayDate.ToString("dd-M-yyyy", CultureInfo.InvariantCulture)
                                       }).ToList();
                return PartialView(ViewHelper.GetViewPathDetails("CompanyHolidays", "CompanyHolidayDetails"), responseDetails);
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
            try
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
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CompanyHolidays)} action name {nameof(HolidayCreate)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertCompanyHoliday(CompanyHolidays model)
        {
            try
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
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CompanyHolidays)} action name {nameof(UpsertCompanyHoliday)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteCompanyHoliday(int id)
        {
            try
            {
            var deleteModel = await _ICompanyHolidaysRepository.GetAllEntityById(x => x.Id == id);
            var deleteDbModel = CrudHelper.DeleteHelper<CompanyHolidays>(deleteModel.Entity, 1);
            var deleteResponse = await _ICompanyHolidaysRepository.DeleteEntity(deleteDbModel);
            if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
            {
                return Json(deleteResponse.Message);
            }
            return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CompanyHolidays)} action name {nameof(DeleteCompanyHoliday)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var StateResponse = await _IStateRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            if (StateResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.StateList = StateResponse.Entities;

        }

        #endregion

    }
}
