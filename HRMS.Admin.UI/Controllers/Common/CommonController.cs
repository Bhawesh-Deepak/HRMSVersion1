using HRMS.Admin.UI.AuthenticateService;
using HRMS.Core.ReqRespVm.Response.Common;
using HRMS.Core.ReqRespVm.Response.Employee;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Common
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class CommonController : Controller
    {
        private readonly IDapperRepository<FilteredEmployeeParams> _IFilteredEmployeeRepository;
        private readonly IDapperRepository<EmployeeAutoCompleteParams> _IEmployeeInformationRepository;
        public CommonController(IDapperRepository<FilteredEmployeeParams> filteredEmployeeRepository, IDapperRepository<EmployeeAutoCompleteParams> employeeinformationRepository)
        {
            _IFilteredEmployeeRepository = filteredEmployeeRepository;
            _IEmployeeInformationRepository = employeeinformationRepository;
        }
        public async Task<IActionResult> GetFilteredEmployee(string name, string empCode, string department, string designation)
        {
            try
            {
                var model = new FilteredEmployeeParams()
                {
                    Name = name,
                    empCode = empCode,
                    designation = designation,
                    department = department
                };

                var response = await Task.Run(() => _IFilteredEmployeeRepository.GetAll<FilteredEmployee>(SqlQuery.GetFileteredEmployee, model));

                return Json(response);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(CommonController)} action name {nameof(GetFilteredEmployee)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> getEmployeeAutoComplete(string term)
        {
            var requestmodel = new EmployeeAutoCompleteParams()
            {
                prefix = term,
            };
            var employeeResponse = await Task.Run(() => _IEmployeeInformationRepository.GetAll<EmployeeAutoCompleteVM>(SqlQuery.GetemployeeAutoComplete, requestmodel));

            return Json(employeeResponse);
        }
    }
}
