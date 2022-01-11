using ClosedXML.Excel;
using Fingers10.ExcelExport.ActionResults;
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
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
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
            var response = await _IEmployeeDetailRepository.GetAllEntities(null);

            HttpContext.Session.SetObjectAsJson("EmpDetail", response.Entities);

            await PopulateViewBag();

            ViewBag.HeaderTitle = PageHeader.HeaderSetting["EmployeeDetailIndex"];

            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("EmployeeDetail", "EmployeeDetailIndex"), response.Entities));
        }

        public async Task<IActionResult> GetFilteredData(string legalEntity, string department,
            string designation, string plName, string doj, string location, string status)
        {
            var response = await _IEmployeeDetailRepository.GetAllEntities(null);
            var models = response.Entities;

            if (!string.IsNullOrEmpty(legalEntity))
            {
                models = models.Where(x => x.LegalEntity.ToLower().Trim() == legalEntity.ToLower().Trim()).ToList();
            }

            if (!string.IsNullOrEmpty(department))
            {
                models = models.Where(x => x.DepartmentName.ToLower().Trim() == department.ToLower().Trim()).ToList();
            }

            if (!string.IsNullOrEmpty(designation))
            {
                models = models.Where(x => x.DesignationName.ToLower().Trim() == designation.ToLower().Trim()).ToList();
            }
            if (!string.IsNullOrEmpty(plName))
            {
                models = models.Where(x => x.PAndLHeadName.ToLower().Trim() == plName.ToLower().Trim()).ToList();
            }
            if (!string.IsNullOrEmpty(doj))
            {
                models = models.Where(x => x.JoiningDate.Date == Convert.ToDateTime(doj).Date).ToList();
            }
            if (!string.IsNullOrEmpty(location))
            {
                models = models.Where(x => x.Location.ToLower().Trim() == location.ToLower().Trim()).ToList();
            }
            if (!string.IsNullOrEmpty(status))
            {
                bool statusValue = status != "0";
                models = models.Where(x => x.IsActive == Convert.ToBoolean(statusValue)).ToList();
            }
            HttpContext.Session.SetObjectAsJson("EmpDetail", models);
            return await Task.Run(() => PartialView(ViewHelper.GetViewPathDetails("EmployeeDetail", "EmployeeFilteredList"), models));
        }


        public async Task<IActionResult> GetActiveInActiveDetails(int status)
        {
            var response = (await _IEmployeeDetailRepository.GetAllEntities(null)).Entities;

            bool statusValue = status != 0;

            response = response.Where(x => x.IsActive == Convert.ToBoolean(statusValue)).ToList();

            HttpContext.Session.SetObjectAsJson("EmpDetail", response.ToList());

            return await Task.Run(() => PartialView(ViewHelper.GetViewPathDetails("EmployeeDetail", "EmployeeFilteredList"), response.ToList()));
        }

        public async Task<IActionResult> ExportToExcel()
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
