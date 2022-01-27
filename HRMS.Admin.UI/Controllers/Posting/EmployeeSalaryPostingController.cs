using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Admin.UI.AuthenticateService;
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
    [CustomAuthenticate]
    public class EmployeeSalaryPostingController : Controller
    {
        private readonly IGenericRepository<CtcComponentDetail, int> _ICtcComponentDetailRepository;
        private readonly IGenericRepository<EmployeeSalaryPosted, int> _IEmployeeSalaryPostedRepository;
        private readonly IGenericRepository<EmployeeTDSSummery, int> _IEmployeeTDSSummeryRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;
        public EmployeeSalaryPostingController(IGenericRepository<CtcComponentDetail, int> CtcComponentDetailRepo,
            IGenericRepository<EmployeeSalaryPosted, int> EmployeeSalaryPostedRepo, IGenericRepository<EmployeeTDSSummery, int> employeeTDSSummeryRepo,
            IHostingEnvironment hostingEnvironment)
        {
            _ICtcComponentDetailRepository = CtcComponentDetailRepo;
            _IHostingEnviroment = hostingEnvironment;
            _IEmployeeSalaryPostedRepository = EmployeeSalaryPostedRepo;
            _IEmployeeTDSSummeryRepository = employeeTDSSummeryRepo;
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
            string[] cells = { "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ" };
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
            Sheets.Cells[cells[cell + 1] + "2"].Value = "Department";
            Sheets.Cells[cells[cell + 2] + "2"].Value = "Designation";
            var stream = new MemoryStream(Eps.GetAsByteArray());
            return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
        }
        public async Task<IActionResult> UploadSalaryPostingBackData()
        {
            string sWebRootFolder = _IHostingEnviroment.WebRootPath;
            string sFileName = @"EmployeeSalaryPostingBackData.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            var response = await _ICtcComponentDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            string[] cells = { "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ" };
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
            Sheets.Cells[cells[cell + 1] + "2"].Value = "Department";
            Sheets.Cells[cells[cell + 2] + "2"].Value = "Designation";
            Sheets.Cells[cells[cell + 3] + "2"].Value = "Financial Year";
            var stream = new MemoryStream(Eps.GetAsByteArray());
            return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
        }
        [HttpPost]
        public async Task<IActionResult> UploadSalaryPosting(UploadExcelVm model)
        {
            try
            {
                var response = new ReadSalaryPostingExcelHelper().GetEmployeeSalaryPostingComponent(model.UploadFile);
                var tdssummeryresponse = response.Where(x => x.ComponentId == 70).ToList();
                var models = new List<EmployeeTDSSummery>();
                tdssummeryresponse.ForEach(data =>
                {
                    models.Add(new EmployeeTDSSummery()
                    {
                        DateMonth = data.DateMonth,
                        DateYear = data.DateYear,
                        EmpCode = data.EmpCode,
                        CurrentCTC = 0,
                        CurrentNONCTC = 0,
                        TDSTaxableValue = 0,
                        DeductPercentage = 0,
                        DeductAGE = 0,
                        Surcharge = 0,
                        HECAmount = 0,
                        TDSAmountYearly = 0,
                        TDSAmountMonthly = data.SalaryAmount,
                        FinancialYear = data.FinancialYear,
                        CreatedDate=DateTime.Now

                    });
                });
                var dbResponse = await _IEmployeeSalaryPostedRepository.CreateEntities(response.ToArray());
                var tdsresponse = await _IEmployeeTDSSummeryRepository.CreateEntities(models.ToArray());
                return Json("Salary Posting Uploaded Sucessfully");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return Json("");
            }

        }
        [HttpPost]
        public async Task<IActionResult> UploadSalaryPostingBackData(UploadExcelVm model)
        {
            try
            {
                var response = new ReadSalaryPostingExcelHelper().GetEmployeeSalaryPostingComponentBackData(model.UploadFile1);
                var tdssummeryresponse = response.Where(x => x.ComponentId == 70).ToList();
                var models = new List<EmployeeTDSSummery>();
                tdssummeryresponse.ForEach(data =>
                {
                    models.Add(new EmployeeTDSSummery()
                    {
                        DateMonth = data.DateMonth,
                        DateYear = data.DateYear,
                        EmpCode = data.EmpCode,
                        CurrentCTC = 0,
                        CurrentNONCTC = 0,
                        TDSTaxableValue = 0,
                        DeductPercentage = 0,
                        DeductAGE = 0,
                        Surcharge = 0,
                        HECAmount = 0,
                        TDSAmountYearly = 0,
                        TDSAmountMonthly = data.SalaryAmount,
                        FinancialYear = data.FinancialYear,
                        CreatedDate=DateTime.Now
                       
                    });
                });
                var dbResponse = await _IEmployeeSalaryPostedRepository.CreateEntities(response.ToArray());
                var tdsresponse = await _IEmployeeTDSSummeryRepository.CreateEntities(models.ToArray());
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
