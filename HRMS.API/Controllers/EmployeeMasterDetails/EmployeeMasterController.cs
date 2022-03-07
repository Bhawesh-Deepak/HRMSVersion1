using HRMS.Core.Entities.Payroll;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.API.Controllers.EmployeeMasterDetails
{
    [Route("HRMS/V1/[controller]/[action]")]
    [ApiController]
    public class EmployeeMasterController : ControllerBase
    {
        private readonly IDapperRepository<EmployeeSingleDetailParam> _IEmployeeSingleDetailRepository;
        public EmployeeMasterController(IDapperRepository<EmployeeSingleDetailParam> EmployeeSingleDetailRepository)
        {
            _IEmployeeSingleDetailRepository = EmployeeSingleDetailRepository;
        }

        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> EmployeeProfile(int Id)
        {
            var empParams = new EmployeeSingleDetailParam()
            {
                Id = Id
            };
            var response = _IEmployeeSingleDetailRepository.GetAll<EmployeeDetail>(SqlQuery.GetEmployeeSingleDetails, empParams);
            return Ok(response);
        }
    }
}
