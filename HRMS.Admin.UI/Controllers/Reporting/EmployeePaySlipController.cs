using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.Helpers.ExcelHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Core.ReqRespVm.Response.Employee;
using HRMS.Core.ReqRespVm.Response.Reporting;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Admin.UI.AuthenticateService;

namespace HRMS.Admin.UI.Controllers.Reporting
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class EmployeePaySlipController : Controller
    {
        private readonly IHostingEnvironment _IHostingEnviroment;
        private readonly IDapperRepository<EmployeeInformationParams> _IEmployeeInformationRepository;
        private readonly IDapperRepository<EmployeePaySlipParams> _IEmployeePaySlipRepository;
        private readonly IGenericRepository<AssesmentYear, int> _IAssesmentYearRepository;
        public EmployeePaySlipController(IHostingEnvironment hostingEnvironment,
            IDapperRepository<EmployeeInformationParams> employeeinformationRepository, IGenericRepository<AssesmentYear, int> assesmentyearRepo,
            IDapperRepository<EmployeePaySlipParams> employeepayslipRepository)
        {
            _IHostingEnviroment = hostingEnvironment;
            _IEmployeeInformationRepository = employeeinformationRepository;
            _IEmployeePaySlipRepository = employeepayslipRepository;
            _IAssesmentYearRepository = assesmentyearRepo;
        }
       
        public async Task<IActionResult> Index()
        {
            try
            {
                await PopulateViewBag();
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("EmployeePaySlip", "_EmployeePaySlip")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeePaySlipController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DownloadPaySlip(EmployeeSalaryRegisterVM model)
        {
            try
            {
                string empresponse = null;
            if (model.UploadFile != null && model.EmployeeCode == null)
                empresponse = new ReadEmployeeCode().GetSalaryRegisterEmpCodeDetails(model.UploadFile);

            else if (model.UploadFile == null && model.EmployeeCode != null)
                empresponse = model.EmployeeCode;

            var payslipparams = new EmployeePaySlipParams()
            {
                DateMonth = model.DateMonth,
                DateYear = model.DateYear,
                EmployeeCode = empresponse

            };

            var response = await Task.Run(() => _IEmployeePaySlipRepository.GetAll<EmployeePaySlipVM>(SqlQuery.GetPaySlip, payslipparams));

            return PartialView(ViewHelper.GetViewPathDetails("EmployeePaySlip", "_PaySlip"), response);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeePaySlipController)} action name {nameof(DownloadPaySlip)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var model = new EmployeeInformationParams() { };
            var employeeResponse = await Task.Run(() => _IEmployeeInformationRepository.GetAll<EmployeeInformationVM>(SqlQuery.EmployeeInformation, model));
            ViewBag.EmployeeList = employeeResponse.ToList();
            var assesmentyearResponse = await _IAssesmentYearRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            if (assesmentyearResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.AssesmentYearList = assesmentyearResponse.Entities;

        }

        #endregion
    }
}
