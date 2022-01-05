using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Master;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Master
{
    public class CompanyPolicyController : Controller
    {
        private readonly IGenericRepository<CompanyPolicy, int> _ICompanyPolicyRepository;
        private readonly IGenericRepository<Department, int> _IDepartmentRepository;


        public CompanyPolicyController(IGenericRepository<CompanyPolicy, int> CompanyPolicyRepo,
            IGenericRepository<Department, int> DepartmentRepo)
        {
            _ICompanyPolicyRepository = CompanyPolicyRepo;
            _IDepartmentRepository = DepartmentRepo;

        }
        public async Task<IActionResult> Index()
        {
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["DesignationIndex"];

            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("CompanyPolicy", "CompanyPolicyIndex")));
        }

        public async Task<IActionResult> GetCompanyPolicyList()
        {
            try
            {
                var CompanyPolList = await _ICompanyPolicyRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var DepartList = await _IDepartmentRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

                var responseDetails = (from dpt in DepartList.Entities
                                       join cpl in CompanyPolList.Entities
                                       on dpt.Id equals cpl.DepartmentId
                                       select new CompanyPolicyDetails
                                       {
                                           CompanyPolicyId = cpl.Id,
                                           DepartmentName = dpt.Name,
                                           CalenderDate = cpl.CalenderDate,
                                           Name=cpl.Name,
                                           DocumentUrl=cpl.DocumentUrl

                                       }).ToList();

                return PartialView(ViewHelper.GetViewPathDetails("CompanyPolicy", "CompanyPolicyDetails"), responseDetails);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Department)} action name {nameof(GetCompanyPolicyList)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> CreateCompanyPolicy(int id)
        {
            await PopulateViewBag();
            var response = await _ICompanyPolicyRepository.GetAllEntities(x => x.Id == id);

            if (id == 0)
            {
                return PartialView(ViewHelper.GetViewPathDetails("CompanyPolicy", "CompanyPolicyCreate"));
            }
            else
            {
                return PartialView(ViewHelper.GetViewPathDetails("CompanyPolicy", "CompanyPolicyCreate"), response.Entities.First());
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertCompanyPolicy(CompanyPolicy model)
        {
            if (model.Id == 0)
            {
                var response = await _ICompanyPolicyRepository.CreateEntity(model);
                return Json(response.Message);
            }
            else
            {
                var response = await _ICompanyPolicyRepository.UpdateEntity(model);
                return Json(response.Message);
            }
        }

        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var departmentResponse = await _IDepartmentRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            if (departmentResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.DepartmentList = departmentResponse.Entities;

        }

        #endregion
    }
}
