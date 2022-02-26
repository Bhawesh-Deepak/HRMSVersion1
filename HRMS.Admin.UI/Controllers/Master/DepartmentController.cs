using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Entities.Organisation;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Master;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Master
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class DepartmentController : Controller
    {
        private readonly IGenericRepository<Department, int> _IDepartmentRepository;
        private readonly IGenericRepository<Branch, int> _IBranchRepository;
        private readonly IGenericRepository<Designation, int> _IDesignationRepository;
        private readonly IGenericRepository<LegalEntity, int> _ILegalEntityRepository;
        public DepartmentController(IGenericRepository<Department, int> departmentRepo,
            IGenericRepository<Branch, int> branchRepo, IGenericRepository<Designation, int> designationRepo,
            IGenericRepository<LegalEntity, int> legalentityRepo)
        {
            _IDepartmentRepository = departmentRepo;
            _IBranchRepository = branchRepo;
            _IDesignationRepository = designationRepo;
            _ILegalEntityRepository = legalentityRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.HeaderTitle = PageHeader.HeaderSetting["DepartmentIndex"];
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Department", "DepartmentIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Department)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetDepartmentList()
        {
            try
            {
                var departmentresponse = await _IDepartmentRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var designationresponse = await _IDesignationRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var branchresponse = await _IBranchRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var legalentityresponse = await _ILegalEntityRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var responseDetails = (from department in departmentresponse.Entities
                                       join designtion in designationresponse.Entities on department.Id equals designtion.DepartmentId
                                       join branch in branchresponse.Entities on department.BranchId equals branch.Id
                                       join legal in legalentityresponse.Entities on branch.CompanyId equals legal.Id
                                       select new DepartmentAndDesignationVM
                                       {
                                           Id = department.Id,
                                           Name = department.Name,
                                           Code = department.Code,
                                           BranchId = branch.Id,
                                           BranchName = branch.Name+" ( "+legal.Name+" ) ",
                                           BranchCode = branch.Code,
                                           DesignationId = designtion.Id,
                                           DesignationName = designtion.Name,
                                           DesignationCode = designtion.Code
                                       }).ToList();


                return PartialView(ViewHelper.GetViewPathDetails("Department", "DepartmentList"), responseDetails);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Department)} action name {nameof(GetDepartmentList)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }

        public async Task<IActionResult> CreateDepartment(int id)
        {
            try
            {
                await PopulateViewBag();
                var response = await _IDepartmentRepository.GetAllEntities(x => x.Id == id);
                var designationresponse = await _IDesignationRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.DepartmentId == id);
                designationresponse.Entities.ToList().ForEach(data =>
                {
                    data.DepartmentName = response.Entities.First().Name;
                });
                ViewBag.DesignationList = designationresponse.Entities.ToList();
                if (id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("Department", "_CreateDepartment"));
                }
                else
                {


                    return PartialView(ViewHelper.GetViewPathDetails("Department", "_CreateDepartment"), response.Entities.First());
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Department)} action name {nameof(CreateDepartment)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpSertDepartment(Department model, List<string> hdndesignationName, List<string> hdndesignationCode)
        {

            try
            {

                if (model.Id == 0)
                {
                    model.FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId"));
                    model.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                    model.CreatedDate = DateTime.Now;
                    var response = await _IDepartmentRepository.CreateEntity(model);
                    var department = await _IDepartmentRepository.GetAllEntities(x => x.Code.Trim().ToUpper() == model.Code.Trim().ToUpper());
                    var designationlist = new List<Designation>();
                    for (int i = 0; i < hdndesignationName.Count(); i++)
                    {
                        designationlist.Add(new Designation()
                        {
                            DepartmentId = department.Entities.FirstOrDefault().Id,
                            Name = hdndesignationName[i].ToString().Trim(),
                            Code = hdndesignationCode[i].ToString().Trim(),
                            CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId")),
                            CreatedDate = DateTime.Now,
                            FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId")),
                        });
                    }
                    var designationresponse = await _IDesignationRepository.CreateEntities(designationlist.ToArray());
                    return Json(designationresponse.Message);
                }
                else
                {
                    model.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                    model.UpdatedDate = DateTime.Now;
                    var response = await _IDepartmentRepository.UpdateEntity(model);
                    var department = await _IDepartmentRepository.GetAllEntities(x => x.Id == model.Id);
                    var designationlist = new List<Designation>();
                    for (int i = 0; i < hdndesignationName.Count(); i++)
                    {
                        designationlist.Add(new Designation()
                        {
                            DepartmentId = department.Entities.FirstOrDefault().Id,
                            Name = hdndesignationName[i].ToString().Trim(),
                            Code = hdndesignationCode[i].ToString().Trim(),
                            CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId")),
                            CreatedDate = DateTime.Now,
                            FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId")),
                        });
                    }
                    var designationresponse = await _IDesignationRepository.CreateEntities(designationlist.ToArray());

                    return Json(designationresponse.Message);
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Department)} action name {nameof(UpSertDepartment)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var deleteModel = await _IDepartmentRepository.GetAllEntityById(x => x.Id == id);
                var deleteDbModel = CrudHelper.DeleteHelper<Department>(deleteModel.Entity, 1);
                var deleteResponse = await _IDepartmentRepository.DeleteEntity(deleteDbModel);
                if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
                {
                    return Json(deleteResponse.Message);
                }
                return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Department)} action name {nameof(DeleteDepartment)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteDesignation(int id)
        {
            try
            {
                var deleteModel = await _IDesignationRepository.GetAllEntityById(x => x.Id == id);
                var deleteDbModel = CrudHelper.DeleteHelper<Designation>(deleteModel.Entity, 1);
                var deleteResponse = await _IDesignationRepository.DeleteEntity(deleteDbModel);
                if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
                {
                    return Json(deleteResponse.Message);
                }
                return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Department)} action name {nameof(DeleteDesignation)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> GetDesignation(int id)
        {
            try
            {
                await PopulateDesignationViewBag();
                var response = await _IDesignationRepository.GetAllEntities(x => x.Id == id);
                return PartialView(ViewHelper.GetViewPathDetails("Department", "_UpdateDesignation"), response.Entities.First());
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Department)} action name {nameof(GetDesignation)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateDesignation(Designation model)
        {

            try
            {
                model.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                model.UpdatedDate = DateTime.Now;
                var response = await _IDesignationRepository.UpdateEntity(model);
                return Json(response.Message);

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Department)} action name {nameof(UpdateDesignation)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        private async Task PopulateDesignationViewBag()
        {
            var response = await _IDepartmentRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            if (response.ResponseStatus == ResponseStatus.Success)
                ViewBag.DepartmentList = response.Entities;

        }
        private async Task PopulateViewBag()
        {
            var branchResponse = await _IBranchRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var legalentity = await _ILegalEntityRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var responseDetails = (from branch in branchResponse.Entities
                                   join legal in legalentity.Entities on branch.CompanyId equals legal.Id
                                   
                                   select new Branch
                                   {
                                       Id = branch.Id,
                                       Name = branch.Code+" ( "+legal.Name+" ) ",
                                      
                                   }).ToList();

            if (branchResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.BranchList = responseDetails.ToList();
        }

    }
}
