using ClosedXML.Excel;
using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Organisation;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.CommonHelper;
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
    public class EmployeeDirectory : Controller
    {
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepository;
        private readonly IGenericRepository<Subsidiary, int> _ISubsidiaryRepository;
        public EmployeeDirectory(IGenericRepository<EmployeeDetail, int> employeeDetailRepository, IGenericRepository<Subsidiary, int> subsidiaryRepository)
        {
            _IEmployeeDetailRepository = employeeDetailRepository;
            _ISubsidiaryRepository = subsidiaryRepository;
        }
        private async Task PopulateViewBag()
        {
            var response = await _ISubsidiaryRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

            if (response.ResponseStatus == ResponseStatus.Success)
                ViewBag.SubsidiaryList = response.Entities;

        }
        public async Task<IActionResult> Index()
        {
            ViewBag.HeaderTitle = PageHeader.HeaderSetting["Employee Directory"];
            try
            {
                var response = await _IEmployeeDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                await PopulateViewBag();
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("EmployeeDirectory", "_EmployeeDirectoryIndex"), response.Entities));

            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeDirectory)} action name {nameof(Index)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }
        public async Task<IActionResult> GetEmployeeDetails(int Id)
        {
            var response = await _IEmployeeDetailRepository.GetAllEntities(x => x.Id == Id);
            return PartialView(ViewHelper.GetViewPathDetails("EmployeeDirectory", "_EmployeeDetails"), response.Entities.FirstOrDefault());
        }
        [HttpGet]
        public async Task<IActionResult> EmployeeDirectorySearch(string empCode, string LegalEntity, string IsActive)
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
        public async Task<IActionResult> ExportEmployee()
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
    }
}