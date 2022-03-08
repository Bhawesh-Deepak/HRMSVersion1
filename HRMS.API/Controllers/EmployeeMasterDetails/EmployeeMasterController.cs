﻿using HRMS.Core.Entities.Common;
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
    [Route("HRMS/[controller]/[action]")]
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
            try
            {
                var empParams = new EmployeeSingleDetailParam()
                {
                    Id = Id
                };

                var response = _IEmployeeSingleDetailRepository.GetAll<EmployeeDetail>
                    (SqlQuery.GetEmployeeSingleDetails, empParams);

                return await Task.Run(()=> Ok(new Helpers.ResponseEntityList<EmployeeDetail>
                    (System.Net.HttpStatusCode.OK, ResponseStatus.Success.ToString(), response).GetResponseEntityList())) ;
            }
            catch (Exception ex) 
            {
                return await Task.Run(()=> BadRequest(new Helpers.ResponseEntityList<EmployeeDetail>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }
    }
}
