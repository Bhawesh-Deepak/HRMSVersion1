using HRMS.Admin.UI.AuthenticateService;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.Helpers.ExcelHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Core.ReqRespVm.Response.Salary;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Salary
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class SalaryCalculationController : Controller
    {
        private readonly IHostingEnvironment _IHostingEnviroment;
        private readonly IGenericRepository<AssesmentYear, int> _IAssesmentYearRepository;
        private readonly IDapperRepository<SalaryRegisterByEmployeeCodeParams> _ISalaryRegisterByEmployeeCodeParamsRepository;
        public SalaryCalculationController(IGenericRepository<AssesmentYear, int> assesmentyearRepo,
            IDapperRepository<SalaryRegisterByEmployeeCodeParams> SalaryRegisterByEmployeeCodeParamsRepository,
            IHostingEnvironment hostingEnvironment)
        {
            _IHostingEnviroment = hostingEnvironment;
            _ISalaryRegisterByEmployeeCodeParamsRepository = SalaryRegisterByEmployeeCodeParamsRepository;
            _IAssesmentYearRepository = assesmentyearRepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                await PopulateViewBag();
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("SalaryCalculation", "_SalaryCalculationIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(SalaryCalculationController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CalculateSalary(EmployeeSalaryRegisterVM model, IFormFile UploadFile)
        {
            try
            {
                string empresponse = null;
                if (UploadFile != null) { empresponse = new ReadEmployeeCode().GetSalaryRegisterEmpCodeDetails(UploadFile); }
                var request = new SalaryRegisterByEmployeeCodeParams()
                {
                    DateMonth = model.DateMonth,
                    DateYear = model.DateYear,
                    EmployeeCode = empresponse
                };
                var uploadResponse = _ISalaryRegisterByEmployeeCodeParamsRepository.Execute<SalaryRegisterByEmployeeCodeParams>(SqlQuery.calculateSalary, request);

                return Json($"Salary Calculated  successfully !!");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(SalaryCalculationController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        private async Task PopulateViewBag()
        {
            var assesmentyearResponse = await _IAssesmentYearRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            if (assesmentyearResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.AssesmentYearList = assesmentyearResponse.Entities;

        }
    }
}
