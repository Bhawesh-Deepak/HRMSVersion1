using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Entities.Organisation;
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
    public class BranchController : Controller
    {
        private readonly IGenericRepository<Branch, int> _IBranchRepository;
        private readonly IGenericRepository<Subsidiary, int> _ISubsidiaryRepository;


        public BranchController(IGenericRepository<Branch, int> BranchRepo,
            IGenericRepository<Subsidiary, int> SubsidiaryRepo)
        {
            _IBranchRepository = BranchRepo;
            _ISubsidiaryRepository = SubsidiaryRepo;

        }
        public async Task<IActionResult> Index()
        {
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["BranchIndex"];

            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Branch", "BranchIndex")));
        }

        public async Task<IActionResult> GetBranchList()
        {
            try
            {
                var BranchList = await _IBranchRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var CompanyList = await _ISubsidiaryRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

                var responseDetails = (from cl in CompanyList.Entities
                                       join bl in BranchList.Entities
                                       on cl.Id equals bl.CompanyId
                                       select new BranchDetails
                                       {
                                           BranchId = bl.Id,
                                           CompanyName = cl.Name,
                                           Name=bl.Name,
                                           Code = bl.Code,
                                           Logo=bl.Logo,
                                           Address=bl.Address,
                                           ZipCode=bl.ZipCode,
                                           Email=bl.Email,
                                           ContactPerson=bl.ContactPerson,
                                           Phone=bl.Phone
                                       }).ToList();

                return PartialView(ViewHelper.GetViewPathDetails("Branch", "BranchDetails"), responseDetails);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(Department)} action name {nameof(GetBranchList)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> CreateBranch(int id)
        {
            await PopulateViewBag();
            var response = await _IBranchRepository.GetAllEntities(x => x.Id == id);

            if (id == 0)
            {
                return PartialView(ViewHelper.GetViewPathDetails("Branch", "BranchCreate"));
            }
            else
            {
                return PartialView(ViewHelper.GetViewPathDetails("Branch", "BranchCreate"), response.Entities.First());
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertBranch(Branch model)
        {
            if (model.Id == 0)
            {
                var response = await _IBranchRepository.CreateEntity(model);
                return Json(response.Message);
            }
            else
            {
                var response = await _IBranchRepository.UpdateEntity(model);
                return Json(response.Message);
            }
        }

        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var departmentResponse = await _ISubsidiaryRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            if (departmentResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.CompanyList = departmentResponse.Entities;

        }

        #endregion
    }
}
