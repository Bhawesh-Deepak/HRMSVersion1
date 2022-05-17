using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Investment;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.API.Controllers.Investment
{
    [Route("api/HRMS/[controller]/[action]")]
    [ApiController]
    public class DeclarationTypeController : ControllerBase
    {
        private readonly IGenericRepository<InvestmentDeclarationType, int> _IDeclarationTypeRepository;
        public DeclarationTypeController(IGenericRepository<InvestmentDeclarationType, int> declarationTypeRepository)
        {
            _IDeclarationTypeRepository = declarationTypeRepository;
        }
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetAllDeclarationType(string EmpCode, int FinancialYear)
        {
            try
            {
                var models = await _IDeclarationTypeRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.EmpCode.Trim() == EmpCode.Trim() && x.FinancialYear == FinancialYear);
                return Ok(models.Entities.FirstOrDefault());
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<InvestmentDeclarationType>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> PostDeclarationType(InvestmentDeclarationType declarationType)
        {
            try
            {
                var updatemodel = await _IDeclarationTypeRepository.GetAllEntities(x => x.EmpCode.Trim() == declarationType.EmpCode.Trim() && x.FinancialYear == declarationType.FinancialYear);
                updatemodel.Entities.ToList().ForEach(data =>
                {
                    data.IsActive = false;
                    data.IsDeleted = true;
                });
                var response = await _IDeclarationTypeRepository.CreateEntity(declarationType);
                return Ok(response.ResponseStatus);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<InvestmentDeclarationType>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }

    }
}
