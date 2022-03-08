using ClosedXML.Excel;
using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Helpers;
using HRMS.Admin.UI.Models;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Organisation;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Employee;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Core.ExcelPackage;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Organisation
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class EmployeeDirectory : Controller
    {
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepository;
        private readonly IGenericRepository<LegalEntity, int> _ISubsidiaryRepository;
        private readonly IDapperRepository<EmployeeDetailParams> _IEmployeeRepository;
        private readonly IDapperRepository<EmployeeSingleDetailParam> _IEmployeeSingleDetailRepository;
        public EmployeeDirectory(IGenericRepository<EmployeeDetail, int> employeeDetailRepository,
            IDapperRepository<EmployeeDetailParams> employeeRepository
            , IDapperRepository<EmployeeSingleDetailParam> EmployeeSingleDetailRepository, IGenericRepository<LegalEntity, int> subsidiaryRepository)
        {
            _IEmployeeDetailRepository = employeeDetailRepository;
            _ISubsidiaryRepository = subsidiaryRepository;
            _IEmployeeRepository = employeeRepository;
            _IEmployeeSingleDetailRepository = EmployeeSingleDetailRepository;
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
                ViewBag.HeaderTitle = PageHeader.HeaderSetting["Employee Directory"];
                response.First().SortBy = "Name";
                await PopulateViewBag();
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("EmployeeDirectory", "_EmployeeDirectoryIndex"), response));

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeDirectory)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }
        public async Task<IActionResult> GetFilteredData(EmployeeDetailParams searchModelEntity, string sortBy, int pageIndex, PageSize pageSize, string sortOrder)
        {
            try
            {
                int pageSizes = 0;
                if (pageIndex == 0)
                    pageIndex = 1;
                if ((int)pageSize == 0)
                    pageSizes = 10;
                if (sortBy == null)
                    sortBy = "Name";
                if (sortOrder == null)
                    sortOrder = "ASC";
                searchModelEntity.PageNo = pageIndex;
                searchModelEntity.PageSize = (int)pageSize;
                searchModelEntity.SortColumn = sortBy;
                searchModelEntity.SortOrder = sortOrder;
                searchModelEntity.IsActive = true;

                PagingSortingHelper.PopulateModelForPagging(searchModelEntity, pageSize, pageIndex, sortBy, sortOrder);
                var response = _IEmployeeRepository.GetAll<EmployeeDetailVm>(SqlQuery.GetEmployeeDetails, searchModelEntity);
                PagingSortingHelper.PupulateModelToDisplayPagging(response?.First(), PageSize.Size10, pageIndex, sortBy, sortOrder);
                response.First().SortBy = sortBy;
                return await Task.Run(() => PartialView(ViewHelper.GetViewPathDetails("EmployeeDirectory", "EmployeeFilteredList"), response));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeDirectory)} action name {nameof(GetFilteredData)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        private async Task PopulateViewBag()
        {
            var response = await _ISubsidiaryRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            if (response.ResponseStatus == ResponseStatus.Success)
                ViewBag.SubsidiaryList = response.Entities;

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

                return PartialView(ViewHelper.GetViewPathDetails("EmployeeDirectory", "_EmployeeDetails"), response.FirstOrDefault());
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeDirectory)} action name {nameof(GetEmployeeDetails)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> EmployeeDirectorySearch(string empCode, string LegalEntity, string IsActive)
        {
            try
            {
                await PopulateViewBag();
                if (empCode != null && LegalEntity == null && IsActive == null)
                {
                    var response = await _IEmployeeDetailRepository.GetAllEntities(x => x.EmpCode.Trim().ToLower() == empCode.Trim().ToLower());
                    return await Task.Run(() => View(ViewHelper.GetViewPathDetails("EmployeeDirectory", "_EmployeeDirectoryIndex"), response.Entities));
                }
                else if (empCode == null && LegalEntity != null && IsActive == null)
                {
                    var response = await _IEmployeeDetailRepository.GetAllEntities(x => x.EmpCode.Trim().ToLower() == empCode.Trim().ToLower());
                    return await Task.Run(() => View(ViewHelper.GetViewPathDetails("EmployeeDirectory", "_EmployeeDirectoryIndex"), response.Entities));
                }
                else if (empCode == null && LegalEntity == null && IsActive != null)
                {
                    bool status = false;
                    if (IsActive == "1")
                    status = true;
                    else status = false;
                    var response = await _IEmployeeDetailRepository.GetAllEntities(x => x.IsActive == status);
                    return await Task.Run(() => View(ViewHelper.GetViewPathDetails("EmployeeDirectory", "_EmployeeDirectoryIndex"), response.Entities));
                }
                else
                {
                    var response = await _IEmployeeDetailRepository.GetAllEntities(x => x.IsActive == true);
                    return await Task.Run(() => View(ViewHelper.GetViewPathDetails("EmployeeDirectory", "_EmployeeDirectoryIndex"), response.Entities));
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeDirectory)} action name {nameof(EmployeeDirectorySearch)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> ExportEmployee()
        {
            try
            {
                var response = await _IEmployeeDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                DataTable dt = new DataTable("Employee");
                dt.Columns.AddRange(new DataColumn[10] {
                    new DataColumn("Employee Name"),
                    new DataColumn("Emp Code"),
                    new DataColumn("Joining Date"),
                    new DataColumn("Office EmailId"),
                    new DataColumn("Department"),
                    new DataColumn("Designation"),
                    new DataColumn("Location"),
                    new DataColumn("Supervisor Code"),
                    new DataColumn("PAN Number"),
                    new DataColumn("Aadhar Number"),
                });

                foreach (var data in response.Entities)
                {
                  dt.Rows.Add(
                    data.EmployeeName,
                    data.EmpCode,
                    data.JoiningDate.ToString("dd/MM/yyyy"),
                    data.OfficeEmailId,
                    data.DepartmentName,
                    data.DesignationName,
                    data.Location,
                    data.SuperVisorCode, data.PanCardNumber, data.AadharCardNumber);
                }

            using XLWorkbook wb = new XLWorkbook();
            wb.Worksheets.Add(dt);
            using MemoryStream stream = new MemoryStream();
            wb.SaveAs(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "EemployeeDetail.xlsx");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeDirectory)} action name {nameof(ExportEmployee)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}