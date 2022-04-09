using HRMS.Admin.UI.AuthenticateService;
using HRMS.Core.Entities.History;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.Helpers.ExcelHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Utility
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class UtilityController : Controller
    {
        private readonly IGenericRepository<EmployeeUpdateHistory, int> _IEmployeeUpdateHistoryRepository;
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepository;
        private readonly IDapperRepository<EmpDetailByCodeParams> _IEmpDetailByCodeParamsRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;

        public UtilityController(IGenericRepository<EmployeeUpdateHistory, int> employeeUpdateHistoryRepository,
            IGenericRepository<EmployeeDetail, int> employeeDetailRepository,
            IDapperRepository<EmpDetailByCodeParams> empDetailByCodeParamsRepository,
            IHostingEnvironment hostingEnvironment)
        {
            _IEmployeeUpdateHistoryRepository = employeeUpdateHistoryRepository;
            _IEmployeeDetailRepository = employeeDetailRepository;
            _IEmpDetailByCodeParamsRepository = empDetailByCodeParamsRepository;
            _IHostingEnviroment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            try
            {

                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Utility", "_UtilityIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateDepartment(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    //var empResponse = _IEmployeeDetailRepository.GetAllEntities(x => x.EmpCode.Trim() == data.EmpCode.Trim());
                    var historyModel = new EmployeeUpdateHistory()
                    {
                        FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId")),
                        CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId")),
                        DepartmentName = empResponse.Result.FirstOrDefault().DepartmentName,
                        CreatedDate = DateTime.Now,
                        EmpCode = data.EmpCode
                    };
                    var updateHistoryResponse = _IEmployeeUpdateHistoryRepository.CreateEntity(historyModel);
                    empResponse.Result.FirstOrDefault().DepartmentName = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });

                return Json("Department Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateDepartment)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateDesignation(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    //var empResponse = _IEmployeeDetailRepository.GetAllEntities(x => x.EmpCode.Trim() == data.EmpCode.Trim());
                    var historyModel = new EmployeeUpdateHistory()
                    {
                        FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId")),
                        CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId")),
                        DepartmentName = empResponse.Result.FirstOrDefault().DesignationName,
                        CreatedDate = DateTime.Now,
                        EmpCode = data.EmpCode
                    };
                    var updateHistoryResponse = _IEmployeeUpdateHistoryRepository.CreateEntity(historyModel);
                    empResponse.Result.FirstOrDefault().DesignationName = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });

                return Json("Designation Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateDesignation)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateLocation(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    //var empResponse = _IEmployeeDetailRepository.GetAllEntities(x => x.EmpCode.Trim() == data.EmpCode.Trim());
                    var historyModel = new EmployeeUpdateHistory()
                    {
                        FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId")),
                        CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId")),
                        DepartmentName = empResponse.Result.FirstOrDefault().Location,
                        CreatedDate = DateTime.Now,
                        EmpCode = data.EmpCode
                    };
                    var updateHistoryResponse = _IEmployeeUpdateHistoryRepository.CreateEntity(historyModel);
                    empResponse.Result.FirstOrDefault().Location = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });

                return Json("Location Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateLocation)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateLegalEntity(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    //var empResponse = _IEmployeeDetailRepository.GetAllEntities(x => x.EmpCode.Trim() == data.EmpCode.Trim());
                    var historyModel = new EmployeeUpdateHistory()
                    {
                        FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId")),
                        CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId")),
                        DepartmentName = empResponse.Result.FirstOrDefault().LegalEntity,
                        CreatedDate = DateTime.Now,
                        EmpCode = data.EmpCode
                    };
                    var updateHistoryResponse = _IEmployeeUpdateHistoryRepository.CreateEntity(historyModel);
                    empResponse.Result.FirstOrDefault().LegalEntity = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });

                return Json("LegalEntity Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateLegalEntity)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSuperVisorCode(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    //var empResponse = _IEmployeeDetailRepository.GetAllEntities(x => x.EmpCode.Trim() == data.EmpCode.Trim());
                    var historyModel = new EmployeeUpdateHistory()
                    {
                        FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId")),
                        CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId")),
                        DepartmentName = empResponse.Result.FirstOrDefault().SuperVisorCode,
                        CreatedDate = DateTime.Now,
                        EmpCode = data.EmpCode
                    };
                    var updateHistoryResponse = _IEmployeeUpdateHistoryRepository.CreateEntity(historyModel);
                    empResponse.Result.FirstOrDefault().SuperVisorCode = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });

                return Json("SuperVisorCode Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateSuperVisorCode)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateLevel(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    //var empResponse = _IEmployeeDetailRepository.GetAllEntities(x => x.EmpCode.Trim() == data.EmpCode.Trim());
                    var historyModel = new EmployeeUpdateHistory()
                    {
                        FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId")),
                        CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId")),
                        DepartmentName = empResponse.Result.FirstOrDefault().Level,
                        CreatedDate = DateTime.Now,
                        EmpCode = data.EmpCode
                    };
                    var updateHistoryResponse = _IEmployeeUpdateHistoryRepository.CreateEntity(historyModel);
                    empResponse.Result.FirstOrDefault().Level = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });

                return Json("Level Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateLevel)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBranchOffice(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    //var empResponse = _IEmployeeDetailRepository.GetAllEntities(x => x.EmpCode.Trim() == data.EmpCode.Trim());
                    var historyModel = new EmployeeUpdateHistory()
                    {
                        FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId")),
                        CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId")),
                        DepartmentName = empResponse.Result.FirstOrDefault().BranchOfficeId,
                        CreatedDate = DateTime.Now,
                        EmpCode = data.EmpCode
                    };
                    var updateHistoryResponse = _IEmployeeUpdateHistoryRepository.CreateEntity(historyModel);
                    empResponse.Result.FirstOrDefault().BranchOfficeId = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });

                return Json("BranchOffice Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateBranchOffice)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBankAccountNumber(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().BankAccountNumber = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("BankAccountNumber Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateBranchOffice)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePanNumber(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().PanCardNumber = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("Update PaN Number Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdatePanNumber)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePNLHead(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().PAndLHeadName = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("Update PaN Number Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdatePNLHead)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBankName(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().BankName = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("Update PaN Number Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateBankName)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateIFSCCode(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().IFSCCode = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("IFSCCode Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateIFSCCode)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAadharNumber(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().AadharCardNumber = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("AadharNumber Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateAadharNumber)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePassportNumber(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().PassportNumber = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("PassportNumber Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdatePassportNumber)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePersonalEmail(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().PersonalEmailId = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("PersonalEmailId Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdatePersonalEmail)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateNameOnBankAccount(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().BankAccountName = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("NameOnBankAccount Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateNameOnBankAccount)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateESICNew(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().ESICNew = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("ESICNew Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateESICNew)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUANNumber(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().UANNumber = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("UANNumber Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateUANNumber)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePTState(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().PTStateName = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("PT State Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdatePTState)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateMobileNumber(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().ContactNumber = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("MobileNumber Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateMobileNumber)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateGender(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().Gender = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("Gender Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateGender)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSalutation(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().Salutation = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("Salutation Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateSalutation)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOfficeEmailId(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().OfficeEmailId = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("OfficeEmailId Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateOfficeEmailId)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateEmployeementStatus(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().EmployementStatus = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("EmployementStatus Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateEmployeementStatus)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRegion(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().Region = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("Region Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateRegion)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCurrentAddress(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().CurrentAddress = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("CurrentAddress Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateCurrentAddress)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePermanentAddress(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().PermanentAddress = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("PermanentAddress Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdatePermanentAddress)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateFatherName(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().FatherName = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("FatherName Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateFatherName)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateMaritalStatus(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().MaritalStatus = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("MaritalStatus Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateMaritalStatus)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateNationality(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().Nationality = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("Nationality Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateNationality)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePFAccountNumber(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().PAndFBankAccountNumberx = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("PFAccountNumber Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdatePFAccountNumber)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateEmployeeName(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().EmployeeName = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("EmployeeName Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateEmployeeName)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateShift(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetails(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().ShiftTiming = data.ResponseValue;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("ShiftTiming Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateShift)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateExitDate(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetailsDate(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().ExitDate = data.ResponseValueDate;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("ExitDate Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateExitDate)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateIsPFEligible(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetailsInt(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().IsPFEligible = data.ResponseValueInt;
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("IsPFEligible Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateIsPFEligible)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateIsESICEligible(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetailsInt(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().IsESICEligible = data.ResponseValueInt.ToString();
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("IsESICEligible Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateIsESICEligible)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UploadSubbatical(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetailsInt(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().IsSabbatical = Convert.ToBoolean(data.ResponseValueInt);
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("Subbatical Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UpdateIsESICEligible)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> UploadFNFInitiated(UtilityRequestVM utilityRequest)
        {
            try
            {
                var response = new ReadUtilityExcelHelper().GetUtilityDetailsInt(utilityRequest.UploadFile);
                response.ToList().ForEach(data =>
                {
                    var param = new EmpDetailByCodeParams() { EmpCode = data.EmpCode };
                    var empResponse = Task.Run(() => _IEmpDetailByCodeParamsRepository.GetAll<EmployeeDetail>(SqlQuery.GetSingleEmployeeDetailByEmpCode, param));
                    empResponse.Result.FirstOrDefault().IsFNFinitiated = Convert.ToBoolean(data.ResponseValueInt);
                    var updateempModel = empResponse.Result.FirstOrDefault();
                    var empUpdateResponse = _IEmployeeDetailRepository.UpdateEntity(updateempModel);

                });
                return Json("FNFinitiated Update Successfully..");
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(UtilityController)} action name {nameof(UploadFNFInitiated)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
