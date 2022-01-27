using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Admin.UI.AuthenticateService;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.Helpers.ExcelHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace HRMS.Admin.UI.Controllers.Payroll
{
    [CustomAuthenticate]
    public class EmployeeIncrementController : Controller
    {

        private readonly IGenericRepository<CtcComponentDetail, int> _ICtcComponentDetailRepository;
        private readonly IGenericRepository<EmployeeSalary, int> _IEmployeeSalaryRepository;
        private readonly IGenericRepository<EmployeeCtcComponent, int> _IEmployeeCtcComponentRepository;
        private readonly IDapperRepository<EmployeeIncrementParms> _IEmployeeIncrementDapperRepository;

        private readonly IHostingEnvironment _IHostingEnviroment;
        public EmployeeIncrementController(IGenericRepository<CtcComponentDetail, int> CtcComponentDetailRepo,
            IDapperRepository<EmployeeIncrementParms> EmployeeIncrementDapperRepository,
            IGenericRepository<EmployeeSalary, int> EmployeeSalaryRepo, IGenericRepository<EmployeeCtcComponent, int> EmployeeCtcComponentRepo,
            IHostingEnvironment hostingEnvironment)
        {
            _IEmployeeIncrementDapperRepository = EmployeeIncrementDapperRepository;
            _ICtcComponentDetailRepository = CtcComponentDetailRepo;
            _IHostingEnviroment = hostingEnvironment;
            _IEmployeeSalaryRepository = EmployeeSalaryRepo;
            _IEmployeeCtcComponentRepository = EmployeeCtcComponentRepo;
        }
        public IActionResult Index()
        {
            return View(ViewHelper.GetViewPathDetails("EmployeeIncrement", "_IncrementImport"));
        }
        public async Task<IActionResult> DownloadExcelFormat()
        {
            string sWebRootFolder = _IHostingEnviroment.WebRootPath;
            string sFileName = @"EmployeeIncrement.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            var response = await _ICtcComponentDetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.ComponentValueType == 1);
            string[] cells = { "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG" };
            ExcelPackage Eps = new ExcelPackage();
            ExcelWorksheet Sheets = Eps.Workbook.Worksheets.Add("Increment");

            Sheets.Cells["A1"].Value = "EmpCode";
            Sheets.Cells["B1"].Value = "Effective Date";
            Sheets.Cells["C1"].Value = "CTC";
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
        public async Task<IActionResult> UploadIncrement(UploadExcelVm model)
        {
            try
            {
                var response = new ReadIncrementExcelHelper().GetEmployeeIncrementComponent(model.UploadFile);
                response.EmployeeSalaryDetails.ToList().ForEach(data =>
                {
                    var model = new EmployeeIncrementParms()
                    {
                        EndDate = data.StartDate,
                        EmpCode = data.EmpCode,
                        CTC = data.CTC,
                    };
                    var uploadResponse = _IEmployeeIncrementDapperRepository
                    .Execute<EmployeeIncrementParms>(SqlQuery.EmployeeIncrement, model);
                });

                var employeeSalaryList = await _IEmployeeSalaryRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.EndDate == null);
                response.EmployeeCtcComponentDetails.ToList().ForEach(data =>
                {
                    data.EmployeeSalaryId = employeeSalaryList.Entities.Where(x => x.EmpCode.Trim() == data.EmpCode.Trim() && x.EndDate == null).FirstOrDefault().Id;
                });
                var ctccomponentReponse = await _IEmployeeCtcComponentRepository.CreateEntities(response.EmployeeCtcComponentDetails.ToArray());
                return Json("Employee Increment Uploaded Sucessfully");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeIncrementController)} action name {nameof(UploadIncrement)} exceptio is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }
    }
}
