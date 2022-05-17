using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Investment;
using HRMS.Core.Entities.Master;
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
    public class InvestmentDeclarationPDFController : ControllerBase
    {
        private readonly IGenericRepository<InvestmentDeclarationPDFDetails, int> _IInvestmentDeclarationPDFRepository;
        private readonly IGenericRepository<AssesmentYear, int> _IAssesmentYearRepository;
        public InvestmentDeclarationPDFController(IGenericRepository<InvestmentDeclarationPDFDetails, int> investmentDeclarationPDFRepository,
            IGenericRepository<AssesmentYear, int> assesmentYearRepository)
        {
            _IInvestmentDeclarationPDFRepository = investmentDeclarationPDFRepository;
            _IAssesmentYearRepository = assesmentYearRepository;


        }
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetInvestmentDeclarationPDFDetails(string EmpCode, int FinancialYear)
        {
            try
            {
                var response = await _IInvestmentDeclarationPDFRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.EmpCode.Trim() == EmpCode && x.FinancialYear == FinancialYear);

                var assesmentYear = await _IAssesmentYearRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var responseDetails = (from pdf in response.Entities
                                       join assesment in assesmentYear.Entities
                                       on pdf.FinancialYear equals assesment.Id
                                       select new InvestmentDeclarationPDFDetails
                                       {
                                           Id = pdf.Id,
                                           EmpCode = pdf.EmpCode,
                                           AssesmentYear = assesment.Name,
                                           FilePath = pdf.FilePath,
                                           CreatedDate = pdf.CreatedDate
                                       }).ToList();

                return Ok(responseDetails);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<InvestmentDeclarationPDFDetails>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateInvestmentDeclarationPDFDetails(InvestmentDeclarationPDFDetails declarationPDFDetails)
        {
            try
            {
                var response = await _IInvestmentDeclarationPDFRepository.CreateEntity(declarationPDFDetails);
                return Ok(response.ResponseStatus);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<InvestmentDeclarationPDFDetails>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }
    }
}
