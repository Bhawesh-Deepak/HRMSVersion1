using HRMS.Admin.UI.AuthenticateService;
using HRMS.Admin.UI.Helpers;
using HRMS.Core.Entities.HR;
using HRMS.Core.Entities.Leave;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.Helpers.ExcelHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Leave
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class LeaveAllocationController : Controller
    {
        private readonly IHostingEnvironment _IHostingEnviroment;
        private readonly IGenericRepository<LeaveAllocation, int> _ILeaveAllocationRepository;
        public LeaveAllocationController(IHostingEnvironment hostingEnvironment,
            IGenericRepository<LeaveAllocation, int> leaveallocationRepository)
        {
            _IHostingEnviroment = hostingEnvironment;
            _ILeaveAllocationRepository = leaveallocationRepository;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(ViewHelper.GetViewPathDetails("LeaveAllocation", "_LeaveAllocationIndex"));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LeaveAllocationController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> CreateLeaveAllocation(int Id)
        {
            try
            {
                if (Id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("LeaveAllocation", "_CreateLeaveAllocation"));
                }
                else
                {
                    var response = await _ILeaveAllocationRepository.GetAllEntities(x => x.Id == Id);
                    return PartialView(ViewHelper.GetViewPathDetails("LeaveAllocation", "_CreateLeaveAllocation"), response.Entities.FirstOrDefault());
                }
               
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LeaveAllocationController)} action name {nameof(CreateLeaveAllocation)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpsertLeaveAllocation(LeaveAllocation allocation)
        {
            try
            {
                if (allocation.Id == 0)
                {
                    allocation.FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId"));
                    allocation.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                    allocation.CreatedDate = DateTime.Now;
                    var response = await _ILeaveAllocationRepository.CreateEntity(allocation);
                    return Json(response.Message);
                }
                else
                {
                    allocation.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                    allocation.UpdatedDate = DateTime.Now;
                    var response = await _ILeaveAllocationRepository.UpdateEntity(allocation);
                    return Json(response.Message);
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LeaveAllocationController)} action name {nameof(CreateLeaveAllocation)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> GetLeaveAllocationList()
        {
            try
            {
                var response = new DBResponseHelper<LeaveAllocation, int>()
                    .GetDBResponseHelper(await _ILeaveAllocationRepository
                    .GetAllEntities(x => x.IsActive && !x.IsDeleted));

                return View(ViewHelper.GetViewPathDetails("LeaveAllocation", "_LeaveAllocationList"), response.Item2.Entities);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LeaveAllocationController)} action name {nameof(GetLeaveAllocationList)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }
        [HttpGet]
        public async Task<IActionResult> DeleteLeaveAllocation(int id)
        {
            try
            {
                var deleteModel = await _ILeaveAllocationRepository.GetAllEntityById(x => x.Id == id);
                var deleteDbModel = CrudHelper.DeleteHelper<LeaveAllocation>(deleteModel.Entity, 1);
                var deleteResponse = await _ILeaveAllocationRepository.DeleteEntity(deleteDbModel);
                if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
                {
                    return Json(deleteResponse.Message);
                }
                return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LeaveAllocationController)} action name {nameof(DeleteLeaveAllocation)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> DownloadLeaveAllocation()
        {
            try
            {
                string sWebRootFolder = _IHostingEnviroment.WebRootPath;
                string sFileName = @"LeaveAllocation.xlsx";
                string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                if (file.Exists)
                {
                    file.Delete();
                    file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                }
                ExcelPackage Eps = new ExcelPackage();
                ExcelWorksheet Sheets = Eps.Workbook.Worksheets.Add("LeaveAllocation");

                Sheets.Cells["A1"].Value = "EmpCode";
                Sheets.Cells["B1"].Value = "Annual Leave"; 
                Sheets.Cells["C1"].Value = "Mandatory Leave";
                Sheets.Cells["D1"].Value = "Optional Leave";
                Sheets.Cells["E1"].Value = "Sick Leaves";
                Sheets.Cells["F1"].Value = "Maternity Leave";
                Sheets.Cells["G1"].Value = "Paternity Leave";
                Sheets.Cells["H1"].Value = "Bereavement Leave";
                var stream = new MemoryStream(Eps.GetAsByteArray());
                return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LeaveAllocationController)} action name {nameof(DownloadLeaveAllocation)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UploadLeaveAllocation(UploadExcelVm model)
        {
            try
            {
                var response = new ReadLeaveAllocationExcelHelper().GetLeaveAllocationComponent(model.UploadFile);
                response.ToList().ForEach(data =>
                {
                    data.FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId"));
                    data.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                    data.CreatedDate = DateTime.Now;
                });
                var allocationresponse = await _ILeaveAllocationRepository.CreateEntities(response.ToArray());
                return Json("Leave Allocation Uploaded Sucessfully");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(LeaveAllocationController)} action name {nameof(UploadLeaveAllocation)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }
    }
}
