using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Entities.Organisation;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Payroll
{
    public class EmployeeDetailController : Controller
    {
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepository;
        private readonly IGenericRepository<Subsidiary, int> _ISubsidiaryRepository;
        private readonly IGenericRepository<Department, int> _IDepartmentRepository;
        private readonly IGenericRepository<Designation, int> _IDesignationRepository;
        private readonly IGenericRepository<PAndLMaster, int> _IPAndLMasterRepository;
        private readonly IGenericRepository<Location, int> _ILocationRepository;
        public EmployeeDetailController(IGenericRepository<EmployeeDetail, int> EmployeeDetailRepo,
            IGenericRepository<Subsidiary, int> SubsidiaryRepo,
            IGenericRepository<Department, int> DepartmentRepo,
            IGenericRepository<Designation, int> DesignationRepo,
             IGenericRepository<PAndLMaster, int> PAndLMasterRepo,
            IGenericRepository<Location, int> LocationRepo)
        {
            _IEmployeeDetailRepository = EmployeeDetailRepo;
            _ISubsidiaryRepository = SubsidiaryRepo;
            _IDepartmentRepository = DepartmentRepo;
            _IDesignationRepository = DesignationRepo;
            _IPAndLMasterRepository = PAndLMasterRepo;
            _ILocationRepository = LocationRepo;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _IEmployeeDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            await PopulateViewBag();
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["EmployeeDetailIndex"];
            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("EmployeeDetail", "EmployeeDetailIndex"), response.Entities));
        }
        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var subsidiaryResponse = await _ISubsidiaryRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var departmentResponse = await _IDepartmentRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var designationResponse = await _IDesignationRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var pandlResponse = await _IPAndLMasterRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var locationResponse = await _ILocationRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            if (subsidiaryResponse.ResponseStatus == ResponseStatus.Success &&
                departmentResponse.ResponseStatus == ResponseStatus.Success &&
                 designationResponse.ResponseStatus == ResponseStatus.Success &&
                  pandlResponse.ResponseStatus == ResponseStatus.Success &&
                 locationResponse.ResponseStatus == ResponseStatus.Success
                )
                ViewBag.subsidiaryList = subsidiaryResponse.Entities;
            ViewBag.departmentList = departmentResponse.Entities;
            ViewBag.designationList = designationResponse.Entities;
            ViewBag.pandlList = pandlResponse.Entities;
            ViewBag.locationList = locationResponse.Entities;

        }

        #endregion
    }
}
