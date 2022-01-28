using HRMS.Core.Entities.Common;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Employee;
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
        public EmployeePaySlipController(IHostingEnvironment hostingEnvironment, IDapperRepository<EmployeeInformationParams> employeeinformationRepository)
        {
            _IHostingEnviroment = hostingEnvironment;
            _IEmployeeInformationRepository = employeeinformationRepository;

        }
        public async Task<IActionResult> Index()
        {
            await PopulateViewBag();
            return await Task.Run(() => View(ViewHelper.GetViewPathDetails("EmployeePaySlip", "_EmployeePaySlip")));
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
