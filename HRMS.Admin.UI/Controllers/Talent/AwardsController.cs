using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Admin.UI.AuthenticateService;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Entities.Talent;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Admin.UI.Controllers.Talent
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class AwardsController : Controller
    {

        private readonly IGenericRepository<Award, int> _IAwardRepository;
        private readonly IGenericRepository<AwardType, int> _IAwardTypeRepository;
        private readonly IDapperRepository<EmployeeSingleDetailParam> _IEmployeeSingleDetailRepository;
        public AwardsController(IGenericRepository<AwardType, int> awardTypeRepo,IGenericRepository<Award, int> awardRepo, IDapperRepository<EmployeeSingleDetailParam> EmployeeSingleDetailRepository)
        {
            _IAwardRepository = awardRepo;
            _IEmployeeSingleDetailRepository = EmployeeSingleDetailRepository;
            _IAwardTypeRepository = awardTypeRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Awards", "_AwardIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AwardsController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> GetAwardsList()
        {
            try
            {
                var response = await _IAwardRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var awardType=await _IAwardTypeRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var responseDetails = (from award in response.Entities
                                       join awardtype in awardType.Entities
                                       on award.AwardTypeId equals awardtype.Id
                                       select new Award
                                       {
                                           Id = award.Id,
                                           EmpCode = award.EmpCode,
                                           AwardName = award.AwardName,
                                           Project= award.Project,
                                           Amount = award.Amount,
                                           AwardDate=award.AwardDate,
                                           EmployeeName=award.EmployeeName,
                                           LegalEntity=award.LegalEntity,
                                           DepartmentName=award.DepartmentName,
                                           DesignationName=award.DesignationName,
                                           AwardTypeName=awardtype.Name
                                       }).ToList();



                return PartialView(ViewHelper.GetViewPathDetails("Awards", "_AwardsList"), responseDetails);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AwardsController)} action name {nameof(GetAwardsList)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> CreateAwards(int id)
        {
            try
            {
                await PopulateViewBag();
                if (id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("Awards", "_CreateAwards"));
                }
                else
                {
                    var response = await _IAwardRepository.GetAllEntities(x => x.Id == id);
                    return PartialView(ViewHelper.GetViewPathDetails("Awards", "_CreateAwards"), response.Entities.First());
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AwardsController)} action name {nameof(CreateAwards)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpsertAwards(Award model)
        {
            try
            {
                if (model.Id == 0)
                {
                    model.FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId"));
                    model.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                    model.CreatedDate = DateTime.Now;
                    var response = await _IAwardRepository.CreateEntity(model);
                    return Json(response.Message);
                }
                else
                {
                    model.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                    model.UpdatedDate = DateTime.Now;
                    var response = await _IAwardRepository.UpdateEntity(model);
                    return Json(response.Message);
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AwardsController)} action name {nameof(UpsertAwards)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> GetEmployeeDetails(int Id)
        {
            try
            {
                var empParams = new EmployeeSingleDetailParam()
                {
                    Id = Id
                };
                var response = _IEmployeeSingleDetailRepository.GetAll<EmployeeDetail>(SqlQuery.GetEmployeeSingleDetails, empParams);

                return Json(response.FirstOrDefault());
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AwardsController)} action name {nameof(GetEmployeeDetails)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> DeleteAwards(int id)
        {
            try
            {
                var deleteModel = await _IAwardRepository.GetAllEntityById(x => x.Id == id);
                var deleteDbModel = CrudHelper.DeleteHelper<Award>(deleteModel.Entity, 1);
                var deleteResponse = await _IAwardRepository.DeleteEntity(deleteDbModel);
                if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
                {
                    return Json(deleteResponse.Message);
                }
                return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(AwardsController)} action name {nameof(DeleteAwards)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var awardsResponse = await _IAwardTypeRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            if (awardsResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.AwardsTypeList = awardsResponse.Entities;

        }

        #endregion
    }
}
