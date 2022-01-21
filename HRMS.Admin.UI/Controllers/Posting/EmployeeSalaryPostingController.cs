using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Entities.Posting;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.Helpers.ExcelHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Core.ReqRespVm.Response.Employee;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace HRMS.Admin.UI.Controllers.Posting
{
    public class EmployeeSalaryPostingController : Controller
    {
        private readonly IGenericRepository<CtcComponentDetail, int> _ICtcComponentDetailRepository;
        private readonly IGenericRepository<EmployeeSalaryPosted, int> _IEmployeeSalaryPostedRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;         
        public EmployeeSalaryPostingController(IGenericRepository<CtcComponentDetail, int> CtcComponentDetailRepo,
            IGenericRepository<EmployeeSalaryPosted, int> EmployeeSalaryPostedRepo,         
            IHostingEnvironment hostingEnvironment)
        {
            _ICtcComponentDetailRepository = CtcComponentDetailRepo;
            _IHostingEnviroment = hostingEnvironment;
            _IEmployeeSalaryPostedRepository = EmployeeSalaryPostedRepo;
           
        }
        public IActionResult Index()
        {
            return View(ViewHelper.GetViewPathDetails("EmployeeSalaryPosting", "_EmployeeSalaryPosting"));
        }
        public async Task<IActionResult> DownloadExcelFormat()
        {
            string sWebRootFolder = _IHostingEnviroment.WebRootPath;
            string sFileName = @"EmployeeSalaryPosting.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            var response = await _ICtcComponentDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            string[] cells = { "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG","AH","AI","AJ","AK","AL","AM","AN","AO","AP","AQ","AR","AS","AT","AU","AV","AW","AX","AY","AZ","BA","BB","BC","BD","BE","BF","BG","BH","BI","BJ","BK","BL","BM","BN","BO","BP","BQ","BR","BS","BT","BU","BV","BW","BX","BY","BZ" };
            ExcelPackage Eps = new ExcelPackage();
            ExcelWorksheet Sheets = Eps.Workbook.Worksheets.Add("Salary");
            Sheets.Cells["A2"].Value = "DateMonth";
            Sheets.Cells["B2"].Value = "DateYear";
            Sheets.Cells["C2"].Value = "EmpCode";
            int cell = 0;
            foreach (var item in response.Entities)
            {
                Sheets.Cells[cells[cell] + "1"].Value = item.Id;
                Sheets.Cells[cells[cell] + "2"].Value = item.ComponentName.Trim();
                cell++;
            }
            Sheets.Cells[cells[cell] + "2"].Value = "LegalEntity";
            Sheets.Cells[cells[cell+1] + "2"].Value = "Department";
            Sheets.Cells[cells[cell+2] + "2"].Value = "Designation";
            var stream = new MemoryStream(Eps.GetAsByteArray());
            return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
        }
        [HttpPost]
        public async Task<IActionResult> UploadSalaryPosting(UploadExcelVm model)
        {
            try
            {
                var response = new ReadSalaryPostingExcelHelper().GetEmployeeSalaryPostingComponent(model.UploadFile);                
                var dbResponse = await _IEmployeeSalaryPostedRepository.CreateEntities(response.ToArray());
                return Json("Salary Posting Uploaded Sucessfully");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return Json("");
            }
           
        }

    }
}
