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

namespace HRMS.API.Controllers.Compensation
{
    [Route("api/HRMS/[controller]/[action]")]
    [ApiController]
    public class PaymentDeductionController : ControllerBase
    {
        private readonly IDapperRepository<PaymentDeductionParams> _IPaymentDeductionParamsRepository;
        public PaymentDeductionController(IDapperRepository<PaymentDeductionParams> paymentDeductionParamsRepository)
        {
            _IPaymentDeductionParamsRepository = paymentDeductionParamsRepository;
        }
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetEmployeePaymentDeduction(string EmpCode, int FinancialYear)
        {
            try
            {
                var parameter = new PaymentDeductionParams()
                {

                    EmpCode = EmpCode,
                    FinancialYear = FinancialYear
                };
                var response = _IPaymentDeductionParamsRepository.GetAll<PaymentDeductionVM>(SqlQuery.GetEmployeePaymentDeduction, parameter);
                return Ok(response);


            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<PaymentDeductionVM>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }
    }
}
