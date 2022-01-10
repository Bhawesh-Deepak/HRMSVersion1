using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Master;
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
    public class InternalTransferController : Controller
    {
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepository;
        private readonly IGenericRepository<Department, int> _IDepartmentRepository;
        private readonly IGenericRepository<Designation, int> _IDesignationRepository;
        private readonly IGenericRepository<Location, int> _ILocationRepository;
        public InternalTransferController(IGenericRepository<EmployeeDetail, int> EmployeeDetailRepo,
            IGenericRepository<Department, int> DepartmentRepo,
            IGenericRepository<Designation, int> DesignationRepo,
            IGenericRepository<Location, int> LocationRepo)
        {
            _IEmployeeDetailRepository = EmployeeDetailRepo;
            _IDepartmentRepository = DepartmentRepo;
            _IDesignationRepository = DesignationRepo;
            _ILocationRepository = LocationRepo;
        }
        public async Task<IActionResult> Index()
        {
            await PopulateViewBag();
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["InternalTransferIndex"];
            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("InternalTransfer", "InternalTransferIndex")));
        }
        public async Task<IActionResult> GetEmployeeDetail(int Id)
        {
            await PopulateInternalTransferViewBag();
            var response = await _IEmployeeDetailRepository.GetAllEntities(x => x.Id == Id);
            return PartialView(ViewHelper.GetViewPathDetails("InternalTransfer", "EmployeeDetail"), response.Entities.First());
        }
        [HttpPost]
        public async Task<IActionResult> UpdateInternalTransfer(EmployeeDetail model)
        {
            var deleteModel = await _IEmployeeDetailRepository.GetAllEntities(x => x.Id == model.Id);
            deleteModel.Entities.ToList().ForEach(data =>
            {
                data.IsActive = false;
                data.IsDeleted = true;
                data.UpdatedDate = DateTime.Now;
                data.DepartmentName = model.DepartmentName;
                data.DesignationName = model.DesignationName;
                data.Location = model.Location;
            });
            var response = await _IEmployeeDetailRepository.DeleteEntity(deleteModel.Entities.First());
            deleteModel.Entities.ToList().ForEach(data =>
            {
                data.CreatedDate = DateTime.Now;
                data.DepartmentName = model.DepartmentName;
                data.DesignationName = model.DesignationName;
                data.Location = model.Location;
                data.Id = 0;
                data.UpdatedDate = null;
            });

            var createresponse = await _IEmployeeDetailRepository.CreateEntity(deleteModel.Entities.First());
            return Json("Internal  Transfer Change Sucrssfully");
        }
        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var employeeResponse = await _IEmployeeDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            ViewBag.EmployeeList = employeeResponse.Entities;
        }

        #endregion
        #region PrivateFields
        private async Task PopulateInternalTransferViewBag()
        {
            var departmentResponse = await _IDepartmentRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var designationResponse = await _IDesignationRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var locationResponse = await _ILocationRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            ViewBag.departmentList = departmentResponse.Entities;
            ViewBag.designationList = designationResponse.Entities;
            ViewBag.locationList = locationResponse.Entities;
        }

        #endregion
    }
}
