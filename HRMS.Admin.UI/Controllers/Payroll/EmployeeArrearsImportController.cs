using System;
using System.Threading.Tasks;
using HRMS.Admin.UI.AuthenticateService;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.Helpers.ExcelHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Admin.UI.Controllers.Payroll
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class EmployeeArrearsImportController : Controller
    {
        private readonly IGenericRepository<EmployeeArrears, int> _IEmployeeArrearsRepository;
        public EmployeeArrearsImportController(IGenericRepository<EmployeeArrears, int> EmployeeArrearsRepository)
        {
            _IEmployeeArrearsRepository = EmployeeArrearsRepository;
            
        }
        public IActionResult Index()
        {
            return View(ViewHelper.GetViewPathDetails("EmployeeArrearsImport", "EmployeeArrearsImportIndex"));
        }

        [HttpPost]
        public async Task<IActionResult> UploadArrearsSalary(UploadExcelVm model)
        {
            try
            {
                var response = new ReadEmployeeArrear().GetEmployeeArrearsDetails(model.UploadFile);
                var employeeDetailResponse = await _IEmployeeArrearsRepository.CreateEntities(response.ToArray());
                return Json("Employee Arrears information and Salary Detail Uploaded successfully !!!");
            }
            catch (Exception ex)
            {
                Serilog.Log.Information(ex.InnerException.ToString(), ex);
                return Json("Unable to upload the Excel File, Something wents wrong please contact admin !");
            }
        }
    }
}
