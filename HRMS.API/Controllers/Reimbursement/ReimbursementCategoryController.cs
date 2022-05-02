using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Reimbursement;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.API.Controllers.Reimbursement
{
    [Route("api/HRMS/[controller]/[action]")]
    [ApiController]
    public class ReimbursementCategoryController : ControllerBase
    {
        private readonly IGenericRepository<ReimbursementCategory, int> _IReimbursementCategoryRepo;
        public ReimbursementCategoryController(IGenericRepository<ReimbursementCategory, int> reimbursementCategoryRepo)
        {
            _IReimbursementCategoryRepo = reimbursementCategoryRepo;
        }
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetReimbursementCategory()
        {
            try
            {
                var categoryResponse = await _IReimbursementCategoryRepo.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                return Ok(categoryResponse.Entities.ToList());

            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<EmployeeReimbursement>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }
    }
}
