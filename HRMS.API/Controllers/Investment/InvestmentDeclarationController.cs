using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Investment;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.ReqRespVm.RequestVm.Investment;
using HRMS.Core.ReqRespVm.Response.Investment;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
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
    public class InvestmentDeclarationController : ControllerBase
    {
        private readonly IDapperRepository<InvestmentDeclarationOldParams> _IInvestmentDeclarationOldParamsRepository;
        private readonly IDapperRepository<CalculateTDSParams> _ICalculateTDSParamsRepository;
        private readonly IGenericRepository<EmployeeInvestmentDecalaration, int> _IEmployeeInvestmentDecalarationRepository;
        public InvestmentDeclarationController(IDapperRepository<InvestmentDeclarationOldParams> investmentDeclarationOldParamsRepository,
            IDapperRepository<CalculateTDSParams> calculateTDSParamsRepository,
            IGenericRepository<EmployeeInvestmentDecalaration, int> employeeInvestmentDecalarationRepository)
        {
            _IInvestmentDeclarationOldParamsRepository = investmentDeclarationOldParamsRepository;
            _IEmployeeInvestmentDecalarationRepository = employeeInvestmentDecalarationRepository;
            _ICalculateTDSParamsRepository = calculateTDSParamsRepository;
        }
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetInvestmentDeclarationOldMethod(string EmpCode, int FinancialYear)
        {
            try
            {
                var decParams = new InvestmentDeclarationOldParams()
                {
                    EmpCode = EmpCode,
                    FinancialYear = FinancialYear
                };

                var response = _IInvestmentDeclarationOldParamsRepository.GetAll<InvestmentDeclarationOldMethodVM>
                    (SqlQuery.GetInvestmentDeclarationOldMethod, decParams);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<InvestmentDeclarationOldMethodVM>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> PostInvestmentDeclarationOldMethod(List<InvestmentDeclarationVM> declarationVMs)
        {
            try
            {
                var request = new List<EmployeeInvestmentDecalaration>();
                foreach (var item in declarationVMs)
                {
                    request.Add(new EmployeeInvestmentDecalaration()
                    {
                        InvestmentMasterId = item.InvestmentMasterId,
                        InvestmentParticularId = item.InvestmentParticularId,
                        InvestmentChildNodeId = item.InvestmentChildNodeId,
                        DeclaredAmount = item.DeclaredAmount,
                        LocatonId = item.LocatonId,
                        EmpCode = item.EmpCode,
                        FinancialYear = item.FinancialYear,
                        CreatedBy = item.CreatedBy,
                        MaxAmount = item.MaxAmount,
                        VerifiedAmount = item.VerifiedAmount,
                        NoOfChildren = item.NoOfChildren,
                        SubmitedAmount = item.SubmitedAmount,
                        CreatedDate = DateTime.Now
                    });
                }
                var updateModel = await _IEmployeeInvestmentDecalarationRepository.GetAllEntities(x => x.EmpCode.Trim() == request.FirstOrDefault().EmpCode.Trim());
                updateModel.Entities.ToList().ForEach(data =>
                {
                    data.IsActive = false;
                    data.IsDeleted = true;
                    data.UpdatedDate = DateTime.Now;

                });
                var deleteResponse = await _IEmployeeInvestmentDecalarationRepository.UpdateMultipleEntity(updateModel.Entities.ToArray());
                var response = await _IEmployeeInvestmentDecalarationRepository.CreateEntities(request.ToArray());
                return Ok(true);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<InvestmentDeclarationOldMethodVM>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> CalculateTDS(string EmpCode, int FinancialYear)
        {
            try
            {
                var decParams = new CalculateTDSParams()
                {
                    EmpCode = EmpCode,
                    FinancialYear = FinancialYear
                };

                var response = _ICalculateTDSParamsRepository.Execute<int>
                    (SqlQuery.CalculateTDS, decParams);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<InvestmentDeclarationOldMethodVM>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }
    }
}
