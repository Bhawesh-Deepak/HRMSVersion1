using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Entities.Organisation;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
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
        public DepartmentController(IGenericRepository<Department, int> departmentRepo,
            IGenericRepository<Branch, int> branchRepo, IGenericRepository<Designation, int> designationRepo)
        {
            _IDepartmentRepository = departmentRepo;
            _IBranchRepository = branchRepo;
            _IDesignationRepository = designationRepo;
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
                var response = new DBResponseHelper<Department, int>()
                    .GetDBResponseHelper(await _IDepartmentRepository
                    .GetAllEntities(x => x.IsActive && !x.IsDeleted));

                return PartialView(ViewHelper.GetViewPathDetails("Department", "DepartmentList"), response.Item2.Entities);
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

                if (id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("Department", "_CreateDepartment"));
                }
                else
                {
                    var response = new DBResponseHelper<Department, int>().GetDBResponseHelper(await _IDepartmentRepository.GetAllEntities(x => x.Id == id));
                    return PartialView(ViewHelper.GetViewPathDetails("Department", "_CreateDepartment"), response.Item2.Entities.First());
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
                            DepartmentId = department.Entities.FirstOrDefault().BranchId,
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
                    var response = await _IDepartmentRepository.UpdateEntity(model);
                    return Json(response.Message);
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

        private async Task PopulateViewBag()
        {
            var branchResponse = await _IBranchRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            if (branchResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.BranchList = branchResponse.Entities;

        }

    }
}
