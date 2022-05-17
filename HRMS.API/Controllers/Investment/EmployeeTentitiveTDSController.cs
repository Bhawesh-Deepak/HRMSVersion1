using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Investment;
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
    public class EmployeeTentitiveTDSController : ControllerBase
    {
        private readonly IGenericRepository<EmployeeTentitiveTDS, int> _IEmployeeTentitiveTDSRepository;
        private readonly IGenericRepository<EmployeeInvestmentDecalaration, int> _IEmployeeInvestmentDecalarationRepository;
        private readonly IGenericRepository<InvestmentMaster, int> _IInvestmentMasterRepository;
        private readonly IDapperRepository<CalculateTDSParams> _IComputationOfTaxReportRepository;
        private readonly IDapperRepository<EmpSalaryParams> _IEmployeeSalaryRepository;
        public EmployeeTentitiveTDSController(IGenericRepository<EmployeeTentitiveTDS, int> employeeTentitiveTDSRepository,
            IGenericRepository<InvestmentMaster, int> InvestmentMasterRepository,
              IGenericRepository<EmployeeInvestmentDecalaration, int> employeeInvestmentDecalarationRepository,
            IDapperRepository<CalculateTDSParams> computationOfTaxReport,
            IDapperRepository<EmpSalaryParams> employeeSalaryRepository)
        {
            _IEmployeeTentitiveTDSRepository = employeeTentitiveTDSRepository;
            _IComputationOfTaxReportRepository = computationOfTaxReport;
            _IInvestmentMasterRepository = InvestmentMasterRepository;
            _IEmployeeInvestmentDecalarationRepository = employeeInvestmentDecalarationRepository;
            _IEmployeeSalaryRepository = employeeSalaryRepository;
        }
         
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> ComputationOfTaxReport(string EmpCode, int FinancialYear)
        {
            try
            {
                var investmentDetail = new ComputationOfTaxReportVM();
                var parameter = new CalculateTDSParams()
                {
                    EmpCode = EmpCode,
                    FinancialYear = FinancialYear
                };
                var empparameter = new EmpSalaryParams()
                {
                    EmpCode = EmpCode
                };
                investmentDetail = _IComputationOfTaxReportRepository.GetAll<ComputationOfTaxReportVM>(SqlQuery.ComputationOfTaxReport, parameter).FirstOrDefault();
                investmentDetail.EmployeSalary = _IEmployeeSalaryRepository.GetAll<EmployeeSalaryVM>(SqlQuery.EmployeeSalaryDetail, empparameter).ToList();
                var empInvestment = await _IEmployeeInvestmentDecalarationRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.EmpCode.Trim() == EmpCode.Trim() && x.FinancialYear == FinancialYear);
                var investment = await _IInvestmentMasterRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted);

                var responseDetails = (from empinvestment in empInvestment.Entities
                                       join invest in investment.Entities
                                       on empinvestment.InvestmentMasterId equals invest.Id
                                       select new DeclarationVM
                                       {
                                           Id = invest.Id,
                                           DeclaredAmount = empinvestment.DeclaredAmount,
                                           Name = invest.Name
                                       }).ToList();
                investmentDetail.Investments = responseDetails;
                 

                return Ok(investmentDetail);
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
