using HRMS.API.Helpers;
using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.API.Controllers.Master
{
    [Route("api/HRMS/[controller]/[action]")]
    [ApiController]
    public class FinancialYearAPIController : ControllerBase
    {
        private readonly IGenericRepository<AssesmentYear, int> _IAssesmentYearRepository;
        public FinancialYearAPIController(IGenericRepository<AssesmentYear, int> assesmentYearRepository)
        {
            _IAssesmentYearRepository = assesmentYearRepository;
        }
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetAllFinancialYear()
        {
            try
            {
                var models = await _IAssesmentYearRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

                return Ok(models.Entities); //new APIResponseHelper<AssesmentYear, int>().GetResponse(models);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<AssesmentYear>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }
    }
}
