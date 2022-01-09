using HRMS.Core.ReqRespVm.Response.Common;
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
    public class CommonController : Controller
    {
        private readonly IDapperRepository<FilteredEmployeeParams> _IFilteredEmployeeRepository;

        public CommonController(IDapperRepository<FilteredEmployeeParams> filteredEmployeeRepository)
        {
            _IFilteredEmployeeRepository = filteredEmployeeRepository;
        }
        public async Task<IActionResult> GetFilteredEmployee(string name, string empCode, string department, string designation)
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
    }
}
