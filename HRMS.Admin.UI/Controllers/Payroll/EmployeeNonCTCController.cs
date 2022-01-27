using HRMS.Admin.UI.AuthenticateService;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.Helpers.ExcelHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Payroll
{
    [CustomAuthenticate]
    public class EmployeeNonCTCController : Controller
    {
        private readonly IGenericRepository<EmployeeNonCTC, int> _IEmployeeNonCTCRepository;
        private readonly IGenericRepository<CtcComponentDetail, int> _ICtcComponentDetailRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;
        public EmployeeNonCTCController(IGenericRepository<EmployeeNonCTC, int> employeeNonCTCRepo,
            IGenericRepository<CtcComponentDetail, int> CtcComponentDetailRepo, IHostingEnvironment hostingEnvironment)
        {
            _IEmployeeNonCTCRepository = employeeNonCTCRepo;
            _ICtcComponentDetailRepository = CtcComponentDetailRepo;
            _IHostingEnviroment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View(ViewHelper.GetViewPathDetails("NonCTC", "NonCTCCreate"));
        }

        public async Task<IActionResult> DownloadExcelFormat()
        {
            string sWebRootFolder = _IHostingEnviroment.WebRootPath;
            string sFileName = @"EmployeeNonCTC.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            var response = await _ICtcComponentDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.ComponentValueType == 3);
            string[] cells = { "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG" };
            ExcelPackage Eps = new ExcelPackage();
            ExcelWorksheet Sheets = Eps.Workbook.Worksheets.Add("non-ctc");
            Sheets.Cells["A1"].Value = "Month";
            Sheets.Cells["B1"].Value = "Year";
            Sheets.Cells["C1"].Value = "EmpCode";
            int cell = 0;
            foreach (var item in response.Entities)
            {
                Sheets.Cells[cells[cell] + "1"].Value = item.ComponentName.Trim();
                cell++;
            }
            var stream = new MemoryStream(Eps.GetAsByteArray());
            return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
        }

        [HttpPost]
        public async Task<IActionResult> UploadNonCTCComponent(UploadExcelVm model)
        {
            try
            {
                var response = new ReadNonCTCComponentExcelHelper().GetEmployeeNonCTCComponent(model.UploadFile);
                var ctcresponse = await _ICtcComponentDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.ComponentValueType == 3);
                response.ToList().ForEach(data =>
                {
                    data.ComponentId = ctcresponse.Entities.Where(x => x.ComponentName == data.ComponentName).FirstOrDefault().Id;
                });
                var dbResponse = await _IEmployeeNonCTCRepository.CreateEntities(response.ToArray());
                return Json("Non CTC Uploaded Sucessfully");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return Json("");
        }


        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        private void DeleteFile(FileInfo file)
        {
            if (file.Exists)//check file exsit or not  
            {
                file.Delete();
            }

        }
    }
}
