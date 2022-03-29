using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Master;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.ReqRespVm.RequestVm;
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
    public class MonthlyEarningAPIController : ControllerBase
    {
        private readonly IDapperRepository<GrossVsNetSalaryParams> _IGrossVsNetSalaryParamsRepository;
        private readonly IDapperRepository<AttendanceStatusParams> _IAttendanceStatusParamsRepository;
        private readonly IGenericRepository<EmployeeForm16Detail, int> _IEmployeeForm16DetailRepository;
        private readonly IDapperRepository<EmployeePaySlipParams> _IEmployeePaySlipRepository;
        private readonly IGenericRepository<AssesmentYear, int> _IAssesmentYearRepository;
        private readonly IDapperRepository<EmployeeSingleDetailParam> _IEmployeeSingleDetailRepository;
        public MonthlyEarningAPIController(IDapperRepository<GrossVsNetSalaryParams> grossVsNetSalaryParamsRepository,
            IDapperRepository<AttendanceStatusParams> attendanceStatusParamsRepository,
            IGenericRepository<EmployeeForm16Detail, int> employeeForm16DetailRepository,
            IGenericRepository<AssesmentYear, int> assesmentYearRepository,
             IDapperRepository<EmployeePaySlipParams> employeepayslipRepository,
             IDapperRepository<EmployeeSingleDetailParam> EmployeeSingleDetailRepository)
        {
            _IGrossVsNetSalaryParamsRepository = grossVsNetSalaryParamsRepository;
            _IAttendanceStatusParamsRepository = attendanceStatusParamsRepository;
            _IEmployeeForm16DetailRepository = employeeForm16DetailRepository;
            _IAssesmentYearRepository = assesmentYearRepository;
            _IEmployeePaySlipRepository = employeepayslipRepository;
            _IEmployeeSingleDetailRepository = EmployeeSingleDetailRepository;
        }
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetNetVsGrossSalary(string EmpCode)
        {
            try
            {
                var responseDetail = new List<GrossVsNetSalaryVM>();
                List<int> lastthreemonth = new List<int>();
                List<int> lastthreeyear = new List<int>();
                var lastthreeMonths = Enumerable.Range(0, 4).Select(i => DateTime.Now.AddMonths(i - 4)).Select(date => date.ToString("MM-yyyy"));
                foreach (var item in lastthreeMonths)
                {
                    string[] splitvalue = item.Split('-');
                    var parameter = new GrossVsNetSalaryParams()
                    {
                        DateMonth = Convert.ToInt32(splitvalue[0]),
                        DateYear = Convert.ToInt32(splitvalue[1]),
                        EmpCode = EmpCode
                    };
                    var response = _IGrossVsNetSalaryParamsRepository.GetAll<GrossVsNetSalaryVM>(SqlQuery.GetNetVsGrossSalary, parameter);
                    foreach (var data in response)
                    {
                        responseDetail.Add(new GrossVsNetSalaryVM()
                        {
                            GrossSalary = data.GrossSalary,
                            NetSalary = data.NetSalary,
                            MonthsName = data.MonthsName
                        });
                    }
                }

                return await Task.Run(() => Ok(new Helpers.ResponseEntityList<GrossVsNetSalaryVM>
                  (System.Net.HttpStatusCode.OK, ResponseStatus.Success.ToString(), responseDetail).GetResponseEntityList()));
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<GrossVsNetSalaryVM>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetAttendanceStatus(int DateMonth, int DateYear, string EmpCode)
        {
            try
            {
                var parameter = new AttendanceStatusParams()
                {
                    DateMonth = DateMonth,
                    DateYear = DateYear,
                    EmpCode = EmpCode
                };
                var response = _IAttendanceStatusParamsRepository.GetAll<AttendanceStatusVM>(SqlQuery.GetAttendanceStatus, parameter);
                return await Task.Run(() => Ok(new Helpers.ResponseEntityList<AttendanceStatusVM>
                  (System.Net.HttpStatusCode.OK, ResponseStatus.Success.ToString(), response).GetResponseEntityList()));
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<GrossVsNetSalaryVM>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> DownloadForm16(string EmpCode, int FinancialYear)
        {
            try
            {

                var response = await _IEmployeeForm16DetailRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.EmpCode == EmpCode && x.FinancialYear == FinancialYear);
                return await Task.Run(() => Ok(new Helpers.ResponseEntityList<EmployeeForm16Detail>
                  (System.Net.HttpStatusCode.OK, ResponseStatus.Success.ToString(), response.Entities.ToList()).GetResponseEntityList()));
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<EmployeeForm16Detail>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> EmployeePaySlip(int Id)
        {
            try
            {
                var response = await _IAssesmentYearRepository.GetAllEntities(x => x.IsActive && !x.IsDeleted && x.isCurrentFinancialYear == true);
                var empParams = new EmployeeSingleDetailParam()
                {
                    Id = Id
                };

                var employeeResponse = _IEmployeeSingleDetailRepository.GetAll<EmployeeDetail>
                    (SqlQuery.GetEmployeeSingleDetails, empParams);



                var payslipResponse = new List<PaySlipVM>();
                string[] AssesmentYears = response.Entities.First().Name.Split('-');
                for (int i = 1; i < 13; i++)
                {
                    System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                    string MonthName = mfi.GetMonthName(i).ToString().Substring(0, 3); 
                    if (i > 3)
                    {
                        payslipResponse.Add(new PaySlipVM()
                        {
                            DateMonth = Convert.ToInt32(i),
                            DateYear = Convert.ToInt32(AssesmentYears[0]),
                            MonthsName = MonthName,
                            EmployeeName=employeeResponse.FirstOrDefault().EmployeeName,
                            EmployeeId = employeeResponse.FirstOrDefault().Id,
                        });
                    }
                    else
                    {
                        payslipResponse.Add(new PaySlipVM()
                        {
                            DateMonth = Convert.ToInt32(i),
                            DateYear = Convert.ToInt32(AssesmentYears[1]),
                            MonthsName = MonthName,
                            EmployeeName = employeeResponse.FirstOrDefault().EmployeeName,
                            EmployeeId= employeeResponse.FirstOrDefault().Id,
                        });
                    }
                }
                return Ok(payslipResponse);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<PaySlipVM>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetEmployeePaySlip(int DateMonth, int DateYear, string EmpCode)
        {
            try
            {
                var payslipparams = new EmployeePaySlipParams()
                {
                    DateMonth = DateMonth,
                    DateYear = DateYear,
                    EmployeeCode = EmpCode
                };

                var response = await Task.Run(() => _IEmployeePaySlipRepository.GetAll<EmployeePaySlipVM>(SqlQuery.GetPaySlip, payslipparams));
                return await Task.Run(() => Ok(new Helpers.ResponseEntityList<EmployeePaySlipVM>
                  (System.Net.HttpStatusCode.OK, ResponseStatus.Success.ToString(), response).GetResponseEntityList()));
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<EmployeePaySlipVM>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }
    }
}
