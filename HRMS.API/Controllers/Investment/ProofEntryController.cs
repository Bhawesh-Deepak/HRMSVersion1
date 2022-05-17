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
    public class ProofEntryController : ControllerBase
    {
        private readonly IGenericRepository<InvestmentProofEntry, int> _IInvestmentProofEntryRepository;
        public ProofEntryController(IGenericRepository<InvestmentProofEntry, int> investmentProofEntryRepository)
        {
            _IInvestmentProofEntryRepository = investmentProofEntryRepository;

        }
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> PostInvestmentProofEntry(InvestmentProofEntry investmentProof)
        {
            try
            {
                var response = await _IInvestmentProofEntryRepository.CreateEntity(investmentProof);
                return Ok(response.ResponseStatus);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<InvestmentProofEntry>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateInvestmentProofEntry(InvestmentProofEntry investmentProof)
        {
            try
            {
                var response = await _IInvestmentProofEntryRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.Id == investmentProof.Id);
                response.Entities.FirstOrDefault().ProofUrl = investmentProof.ProofUrl;
                var updateResponse = await _IInvestmentProofEntryRepository.UpdateEntity(response.Entities.FirstOrDefault());
                return Ok(response.ResponseStatus);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<InvestmentProofEntry>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetProofEntry(string EmpCode, int FinancialYear, int InvestmentChildNodeId)
        {
            try
            {
                var empInvestment = await _IInvestmentProofEntryRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.EmpCode.Trim() == EmpCode.Trim() && x.FinancialYear == FinancialYear && x.InvestmentChildNodeId == InvestmentChildNodeId);
                return Ok(empInvestment.Entities);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<EmployeeTentitiveTDS>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }

    }
}
