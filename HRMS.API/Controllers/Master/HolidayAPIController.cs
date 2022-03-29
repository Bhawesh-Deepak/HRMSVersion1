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
    public class HolidayAPIController : ControllerBase
    {
        private readonly IGenericRepository<CompanyHolidays, int> _ICompanyHolidaysRepository;
        public HolidayAPIController(IGenericRepository<CompanyHolidays, int> companyHolidaysRepository)
        {
            _ICompanyHolidaysRepository = companyHolidaysRepository;
        }
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetCompanyHolidays()
        {
            try
            {
                var models = await _ICompanyHolidaysRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                return Ok(models.Entities.ToList());
                    //new APIResponseHelper<CompanyHolidays, int>().GetResponse(models);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<CompanyHolidays>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }
    }
}
