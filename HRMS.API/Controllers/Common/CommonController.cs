using HRMS.Core.ReqRespVm.Response.Employee;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.API.Controllers.Common
{
    [Route("api/HRMS/[controller]/[action]")]
    [ApiController]
    public class CommonController : ControllerBase
    {

        private readonly IDapperRepository<EmployeeAutoCompleteParams> _IEmployeeInformationRepository;
        public CommonController(IDapperRepository<EmployeeAutoCompleteParams> employeeinformationRepository)
        {
            _IEmployeeInformationRepository = employeeinformationRepository;
        }
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> getEmployeeAutoComplete(string term)
        {
            var requestmodel = new EmployeeAutoCompleteParams()
            {
                prefix = term,
            };
            var employeeResponse = await Task.Run(() => _IEmployeeInformationRepository.GetAll<EmployeeAutoCompleteVM>(SqlQuery.GetemployeeAutoComplete, requestmodel));

            return Ok(employeeResponse);
        }
    }
}
