using HRMS.Core.Entities.Common;
using HRMS.Core.ReqRespVm.Response.Reporting;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.API.Controllers.DashBoard
{
    [Route("api/HRMS/[controller]/[action]")]
    [ApiController]
    public class DashBoardAPIController : ControllerBase
    {
        private readonly IDapperRepository<GrossAndPIReportParams> _IGrossAndPIReportParamsRepository;

        public DashBoardAPIController(IDapperRepository<GrossAndPIReportParams> grossAndPIReportParamsRepository)
        {
            _IGrossAndPIReportParamsRepository = grossAndPIReportParamsRepository;

        }
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetEmployeeGrossAndPerformanceInsentive(string EmpCode, int FinancialYear)
        {
            try
            {
                var parameter = new GrossAndPIReportParams()
                {
                    EmpCode = EmpCode,
                    FinancialYear = FinancialYear
                };
                var response = await Task.Run(() => _IGrossAndPIReportParamsRepository.GetAll<GrossAndPIReportVM>(SqlQuery.GetGrossAndPIReport, parameter));
                return await Task.Run(() => Ok(new Helpers.ResponseEntityList<GrossAndPIReportVM>
                  (System.Net.HttpStatusCode.OK, ResponseStatus.Success.ToString(), response).GetResponseEntityList()));
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<GrossAndPIReportVM>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }
    }
}
