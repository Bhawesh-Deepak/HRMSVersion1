using HRMS.Admin.UI.AuthenticateService;
using HRMS.Core.Entities.HR;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.Helpers.ExcelHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
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
        private readonly IGenericRepository<LeaveType, int> _ILeaveTypeRepository;
        private readonly IGenericRepository<LeaveAllocation, int> _ILeaveAllocationRepository;
        public LeaveAllocationController(IHostingEnvironment hostingEnvironment, IGenericRepository<LeaveType, int> leavetypeRepository,
            IGenericRepository<LeaveAllocation, int> leaveallocationRepository)
        {
            _IHostingEnviroment = hostingEnvironment;
            _ILeaveTypeRepository = leavetypeRepository;
            _ILeaveAllocationRepository = leaveallocationRepository;
        }
        public IActionResult Index()
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
                var response = await _ILeaveTypeRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                string[] cells = { "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG" };
                ExcelPackage Eps = new ExcelPackage();
                ExcelWorksheet Sheets = Eps.Workbook.Worksheets.Add("LeaveAllocation");

                Sheets.Cells["A1"].Value = "EmpCode";

                int cell = 0;
                foreach (var item in response.Entities)
                {
                    Sheets.Cells[cells[cell] + "1"].Value = item.Code.Trim();
                    cell++;
                }
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
