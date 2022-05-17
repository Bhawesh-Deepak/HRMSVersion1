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
    public class InvestmentDeclarationWindowController : ControllerBase
    {
        private readonly IGenericRepository<InvestmentDeclarationWindow, int> _IInvestmentDeclarationWindowRepository;
        public InvestmentDeclarationWindowController(IGenericRepository<InvestmentDeclarationWindow, int> investmentDeclarationWindowRepository)
        {
            _IInvestmentDeclarationWindowRepository = investmentDeclarationWindowRepository;
        }
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetInvestmentDeclarationWindow()
        {
            try
            {
                var response = await _IInvestmentDeclarationWindowRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                return Ok(response.Entities.FirstOrDefault());
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<InvestmentDeclarationWindow>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }
    }
}
