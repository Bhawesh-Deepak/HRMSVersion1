using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Master;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;


namespace HRMS.Admin.UI.Controllers.Master
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
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
                 
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("CompanyHolidays", "CompanyHolidaysIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CompanyHolidays)} action name {nameof(Index)} exception is {ex.Message}";
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

                var responseDetails = (from holidaylist in HolidayList.Entities
                                       join statelist in StateList.Entities
                                       on holidaylist.StateId equals statelist.Id into f
                                       from statelist in f.DefaultIfEmpty()
                                       select new HolidaysDetails
                                       {
                                           StateName = statelist != null ? statelist.Name : "Pan India",
                                           HolidayId = holidaylist.Id,
                                           HolidayName = holidaylist.Name,
                                           Holidate = holidaylist.HolidayDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)
                                       }).ToList();
                return PartialView(ViewHelper.GetViewPathDetails("CompanyHolidays", "CompanyHolidayDetails"), responseDetails);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CompanyHolidays)} action name {nameof(GetHolidayList)} exception is {ex.Message}";
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
                string template = $"Controller name {nameof(CompanyHolidays)} action name {nameof(HolidayCreate)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertCompanyHoliday(CompanyHolidays model, List<int> StateId)
        {
            try
            {
                List<CompanyHolidays> companyholidays = new List<CompanyHolidays>();
                if (model.Id == 0)
                {
                    if (StateId.Count > 0)
                    {
                        foreach (var data in StateId)
                        {
                            companyholidays.Add(new CompanyHolidays
                            {
                                FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId")),
                                StateId = data,
                                HolidayDate = model.HolidayDate,
                                Name = model.Name,
                                CreatedDate = DateTime.Now,
                                CreatedBy= Convert.ToInt32(HttpContext.Session.GetString("EmployeeId")),
                            });
                        }

                        var response = await _ICompanyHolidaysRepository.CreateEntities(companyholidays.ToArray());
                        return Json(response.Message);
                    }
                    else
                    {
                        model.FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId"));
                        model.CreatedDate = DateTime.Now;
                        model.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                        var response = await _ICompanyHolidaysRepository.CreateEntity(model);
                        return Json(response.Message);
                    }
                }
                else
                {
                    var response = await _ICompanyHolidaysRepository.UpdateEntity(model);
                    return Json(response.Message);
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CompanyHolidays)} action name {nameof(UpsertCompanyHoliday)} exception is {ex.Message}";
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
                string template = $"Controller name {nameof(CompanyHolidays)} action name {nameof(DeleteCompanyHoliday)} exception is {ex.Message}";
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
