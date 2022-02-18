using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.Helpers.ExcelHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
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

namespace HRMS.Admin.UI.Controllers.Posting
{
    public class UploadCTCStructureController : Controller
    {
        private readonly IGenericRepository<CtcComponentDetail, int> _ICtcComponentDetailRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;
        private readonly IGenericRepository<EmployeeSalary, int> _IEmployeeSalaryDetailRepository;
        private readonly IGenericRepository<EmployeeCtcComponent, int> _IEmployeeCtcComponentRepository;
        private readonly IDapperRepository<UploadCTCSTructureParams> _IUploadCTCSTructureRepository;
        private readonly IDapperRepository<EmployeeSalaryParams> _IEmployeeSalaryRepository;
        public UploadCTCStructureController(IGenericRepository<CtcComponentDetail, int> CtcComponentDetailRepo,
            IGenericRepository<EmployeeCtcComponent, int> EmployeeCtcComponentRepo,IGenericRepository<EmployeeSalary, int> employeeSalaryDetailRepo,
            IHostingEnvironment hostingEnvironment, IDapperRepository<UploadCTCSTructureParams> uploadCTCSTructureRepository,
            IDapperRepository<EmployeeSalaryParams> employeeSalaryRepository)
        {
            _ICtcComponentDetailRepository = CtcComponentDetailRepo;
            _IHostingEnviroment = hostingEnvironment;
            _IEmployeeSalaryDetailRepository = employeeSalaryDetailRepo;
            _IEmployeeCtcComponentRepository = EmployeeCtcComponentRepo;
            _IUploadCTCSTructureRepository = uploadCTCSTructureRepository;
            _IEmployeeSalaryRepository = employeeSalaryRepository;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("UploadCTCStructure", "_UploadCTCStructureIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UploadCTCStructureController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> DownloadExcelFormat()
        {
            try
            {
                string sWebRootFolder = _IHostingEnviroment.WebRootPath;
                string sFileName = @"CTCStructure.xlsx";
                string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                if (file.Exists)
                {
                    file.Delete();
                    file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                }
                var response = await _ICtcComponentDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.ComponentValueType == 1);
                string[] cells = { "C","D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ" };
                ExcelPackage Eps = new ExcelPackage();
                ExcelWorksheet Sheets = Eps.Workbook.Worksheets.Add("CTCStructure");
                Sheets.Cells["A1"].Value = "EmpCode";
                Sheets.Cells["B1"].Value = "CTC";
                int cell = 0;
                foreach (var item in response.Entities)
                {
                    Sheets.Cells[cells[cell] + "1"].Value = item.ComponentName.Trim();
                    cell++;
                }
                
                var stream = new MemoryStream(Eps.GetAsByteArray());
                return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UploadCTCStructureController)} action name {nameof(DownloadExcelFormat)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ImportUploadCTCStructure(UploadExcelVm model)
        {
            try
            {
                var response = new ReadCTCStructureExcelHelper().GetCTCStructureDetails(model.UploadFile);

                response.EmployeeSalaryDetails.ToList().ForEach(data =>
                {
                    var updateModel = new EmployeeSalaryParams(){
                        CTC = data.CTC,
                        EmpCode = data.EmpCode,
                        UpdatedBy= Convert.ToInt32(HttpContext.Session.GetString("EmployeeId")),
                    };

                    var uploadResponse = _IEmployeeSalaryRepository
                    .Execute<EmployeeSalaryParams>(SqlQuery.UploadEmployeeSalary, updateModel);
                });
                response.EmployeeCtcComponentDetails.ToList().ForEach(data =>
                {
                    var model = new UploadCTCSTructureParams()
                    {
                        UpdatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId")),
                        ComponentId = data.ComponentId,
                        EmpCode = data.EmpCode,
                        ComponentValue = data.ComponentValue,
                    };

                    var uploadResponse = _IUploadCTCSTructureRepository
                    .Execute<UploadCTCSTructureParams>(SqlQuery.UploadCTCStructure, model);

                });
                    return Json("Employee Basic information and Salary Detail Uploaded successfully !!!");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UploadCTCStructureController)} action name {nameof(ImportUploadCTCStructure)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

    }
}
