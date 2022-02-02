using ClosedXML.Excel;
using Fingers10.ExcelExport.ActionResults;
using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Helpers;
using HRMS.Admin.UI.Models;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Entities.Organisation;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Core.ReqRespVm.Response.Employee;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Payroll
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class EmployeeDetailController : Controller
    {
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepository;
        private readonly IGenericRepository<Subsidiary, int> _ISubsidiaryRepository;
        private readonly IGenericRepository<Department, int> _IDepartmentRepository;
        private readonly IGenericRepository<Designation, int> _IDesignationRepository;
        private readonly IGenericRepository<PAndLMaster, int> _IPAndLMasterRepository;
        private readonly IGenericRepository<Location, int> _ILocationRepository;
        private readonly IDapperRepository<EmployeeDetailParams> _IEmployeeRepository;
        private readonly IDapperRepository<EmployeeSingleDetailParam> _IEmployeeSingleDetailRepository;
        private readonly IDapperRepository<EmployeeInformationParams> _IEmployeeInformationRepository;
        private readonly IGenericRepository<Branch, int> _IBranchRepository;
        private readonly IGenericRepository<EmployeeType, int> _IEmployeeTypeRepository;
        private readonly IGenericRepository<RegionMaster, int> _IRegionMasterRepository;
        private readonly IGenericRepository<Shift, int> _IShiftRepository;
        private readonly IGenericRepository<StateMaster, int> _IStateMasterRepository;

        public EmployeeDetailController(IGenericRepository<EmployeeDetail, int> EmployeeDetailRepo,
            IGenericRepository<Subsidiary, int> SubsidiaryRepo,
            IGenericRepository<Department, int> DepartmentRepo,
            IGenericRepository<Designation, int> DesignationRepo,
             IGenericRepository<PAndLMaster, int> PAndLMasterRepo,
            IGenericRepository<Location, int> LocationRepo,
            IDapperRepository<EmployeeSingleDetailParam> EmployeeSingleDetailRepository,
            IDapperRepository<EmployeeDetailParams> employeeRepository ,
            IDapperRepository<EmployeeInformationParams> EmployeeInformationRepository,
            IGenericRepository<Branch, int> BranchRepo,
            IGenericRepository<EmployeeType, int> EmployeeTypeRepo,
            IGenericRepository<RegionMaster, int> RegionMasterRepo,
            IGenericRepository<Shift, int> ShiftRepo,
            IGenericRepository<StateMaster, int> StateMasterRepo)
        {
            _IEmployeeDetailRepository = EmployeeDetailRepo;
            _ISubsidiaryRepository = SubsidiaryRepo;
            _IDepartmentRepository = DepartmentRepo;
            _IDesignationRepository = DesignationRepo;
            _IPAndLMasterRepository = PAndLMasterRepo;
            _ILocationRepository = LocationRepo;
            _IEmployeeRepository = employeeRepository;
            _IEmployeeSingleDetailRepository = EmployeeSingleDetailRepository;
            _IBranchRepository = BranchRepo;
            _IEmployeeTypeRepository = EmployeeTypeRepo;
            _IRegionMasterRepository = RegionMasterRepo;
            _IShiftRepository = ShiftRepo;
            _IEmployeeInformationRepository = EmployeeInformationRepository;
            _IStateMasterRepository = StateMasterRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                EmployeeDetailParams searchModelEntity = new EmployeeDetailParams();
                searchModelEntity.PageNo = 1;
                searchModelEntity.PageSize = 10;
                searchModelEntity.SortColumn = string.Empty;
                searchModelEntity.SortOrder = string.Empty;
                searchModelEntity.IsActive = true;

                PagingSortingHelper.PopulateModelForPagging(searchModelEntity, PageSize.Size10, 10, string.Empty, string.Empty);
                var response = _IEmployeeRepository.GetAll<EmployeeDetailVm>(SqlQuery.GetEmployeeDetails, searchModelEntity);

                PagingSortingHelper.PupulateModelToDisplayPagging(response?.First(), PageSize.Size10, 1, string.Empty, string.Empty);

                response.First().SortBy = "Name";
                await PopulateViewBag();

                ViewBag.HeaderTitle = PageHeader.HeaderSetting["EmployeeDetailIndex"];

                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("EmployeeDetail", "EmployeeDetailIndex"), response));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeDetailController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetFilteredData(EmployeeDetailParams searchModelEntity, string sortBy, int pageIndex, PageSize pageSize, string sortOrder)
        {
            try
            {
                searchModelEntity.PageNo = pageIndex;
                searchModelEntity.PageSize = (int)pageSize;
                searchModelEntity.SortColumn = sortBy;
                searchModelEntity.SortOrder = sortOrder;
                searchModelEntity.IsActive = true;

                PagingSortingHelper.PopulateModelForPagging(searchModelEntity, pageSize, pageIndex, sortBy, sortOrder);
                var response = _IEmployeeRepository.GetAll<EmployeeDetailVm>(SqlQuery.GetEmployeeDetails, searchModelEntity);

                PagingSortingHelper.PupulateModelToDisplayPagging(response?.First(), PageSize.Size10, pageIndex, sortBy, sortOrder);

                response.First().SortBy = sortBy;
                return await Task.Run(() => PartialView(ViewHelper.GetViewPathDetails("EmployeeDetail", "EmployeeFilteredList"), response));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeDetailController)} action name {nameof(GetFilteredData)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }


        public async Task<IActionResult> GetActiveInActiveDetails(int status)
        {
            try
            {
                var response = (await _IEmployeeDetailRepository.GetAllEntities(null)).Entities;

                bool statusValue = status != 0;

                response = response.Where(x => x.IsActive == Convert.ToBoolean(statusValue)).ToList();

                HttpContext.Session.SetObjectAsJson("EmpDetail", response.ToList());

                return await Task.Run(() => PartialView(ViewHelper.GetViewPathDetails("EmployeeDetail", "EmployeeFilteredList"), response.ToList()));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeDetailController)} action name {nameof(GetActiveInActiveDetails)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> ExportToExcel()
        {
            try
            {
                var models = HttpContext.Session.GetObjectFromJson<List<EmployeeDetail>>("EmpDetail");
                var dataTable = ListToDataTable.GetDataTableFromList<EmployeeDetail>(models.ToList());
                string fileName = "Employee Directory.xlsx";
                using XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(dataTable);
                using MemoryStream stream = new MemoryStream();
                wb.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeDetailController)} action name {nameof(ExportToExcel)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> Edit(int Id)
        {
            try
            {
                await PopulateViewBag();
                var empParams = new EmployeeSingleDetailParam()
                {
                    Id = Id
                };
                var response = _IEmployeeSingleDetailRepository.GetAll<EmployeeDetail>(SqlQuery.GetEmployeeSingleDetails, empParams);

                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("EmployeeDetail", "EditEmployeeDetail"),response.FirstOrDefault()));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeDetailController)} action name {nameof(Edit)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var subsidiaryResponse = await _ISubsidiaryRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var departmentResponse = await _IDepartmentRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var designationResponse = await _IDesignationRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var pandlResponse = await _IPAndLMasterRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var locationResponse = await _ILocationRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var branchResponse = await _IBranchRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var employeetypeResponse = await _IEmployeeTypeRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var regionResponse = await _IRegionMasterRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var shiftResponse = await _IShiftRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var ptstateResponse = await _IShiftRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            var model = new EmployeeInformationParams(){ };
            var employeeResponse = await Task.Run(() => _IEmployeeInformationRepository.GetAll<EmployeeInformationVM>(SqlQuery.EmployeeInformation, model));
            if (subsidiaryResponse.ResponseStatus == ResponseStatus.Success &&
                departmentResponse.ResponseStatus == ResponseStatus.Success &&
                 designationResponse.ResponseStatus == ResponseStatus.Success &&
                  pandlResponse.ResponseStatus == ResponseStatus.Success &&
                 locationResponse.ResponseStatus == ResponseStatus.Success&&
                 branchResponse.ResponseStatus==ResponseStatus.Success&&
                 employeetypeResponse.ResponseStatus==ResponseStatus.Success&&
                 regionResponse.ResponseStatus==ResponseStatus.Success&&
                 shiftResponse.ResponseStatus == ResponseStatus.Success&&
                 ptstateResponse.ResponseStatus == ResponseStatus.Success 
                )
                ViewBag.subsidiaryList = subsidiaryResponse.Entities;
            ViewBag.departmentList = departmentResponse.Entities;
            ViewBag.designationList = designationResponse.Entities;
            ViewBag.pandlList = pandlResponse.Entities;
            ViewBag.locationList = locationResponse.Entities;
            ViewBag.BranchList = branchResponse.Entities;
            ViewBag.EmployeeTypeList = employeetypeResponse.Entities;
            ViewBag.RegionList = regionResponse.Entities;
            ViewBag.ShiftList = shiftResponse.Entities;
            ViewBag.EmployeeList = employeeResponse.ToList();
            ViewBag.PtStateList = ptstateResponse.Entities;

        }

        #endregion
    }
}
