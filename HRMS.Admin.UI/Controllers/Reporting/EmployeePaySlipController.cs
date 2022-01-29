using HRMS.Core.Entities.Common;
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

namespace HRMS.Admin.UI.Controllers.Reporting
{
    public class EmployeePaySlipController : Controller
    {
        private readonly IHostingEnvironment _IHostingEnviroment;
        private readonly IDapperRepository<EmployeeInformationParams> _IEmployeeInformationRepository;
        private readonly IDapperRepository<EmployeePaySlipParams> _IEmployeePaySlipRepository;
        public EmployeePaySlipController(IHostingEnvironment hostingEnvironment,
            IDapperRepository<EmployeeInformationParams> employeeinformationRepository,
            IDapperRepository<EmployeePaySlipParams> employeepayslipRepository)
        {
            _IHostingEnviroment = hostingEnvironment;
            _IEmployeeInformationRepository = employeeinformationRepository;
            _IEmployeePaySlipRepository = employeepayslipRepository;
        }
        public async Task<IActionResult> Index()
        {
            await PopulateViewBag();
            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("EmployeePaySlip", "_EmployeePaySlip")));
        }
        [HttpPost]
        public async Task<IActionResult> DownloadPaySlip(EmployeeSalaryRegisterVM model)
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
        #region PrivateFields
        private async Task PopulateViewBag()
        {
            var model = new EmployeeInformationParams() { };
            var employeeResponse = await Task.Run(() => _IEmployeeInformationRepository.GetAll<EmployeeInformationVM>(SqlQuery.EmployeeInformation, model));
            ViewBag.EmployeeList = employeeResponse.ToList();
        }

        #endregion
    }
}
