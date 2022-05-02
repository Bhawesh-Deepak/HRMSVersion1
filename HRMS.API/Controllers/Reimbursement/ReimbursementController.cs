using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Entities.Reimbursement;
using HRMS.Core.Helpers.BlobHelper;
using HRMS.Core.ReqRespVm.Response.Reimbursement;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;

namespace HRMS.API.Controllers.Reimbursement
{
    [Route("api/HRMS/[controller]/[action]")]
    [ApiController]
    public class ReimbursementController : ControllerBase
    {
        private readonly IGenericRepository<EmployeeReimbursement, int> _IEmployeeReimbursementRepo;
        private readonly IGenericRepository<ReimbursementCategory, int> _IReimbursementCategoryRepo;
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepo;
        private readonly IHostingEnvironment _IHostingEnviroment;


        public ReimbursementController(IGenericRepository<EmployeeReimbursement, int> iEmployeeReimbursementRepo,
            IGenericRepository<ReimbursementCategory, int> reimbursementCategoryRepo,
             IGenericRepository<EmployeeDetail, int> employeeDetailRepo,
            IHostingEnvironment hostingEnvironment)
        {
            _IEmployeeReimbursementRepo = iEmployeeReimbursementRepo;
            _IHostingEnviroment = hostingEnvironment;
            _IReimbursementCategoryRepo = reimbursementCategoryRepo;
            _IEmployeeDetailRepo = employeeDetailRepo;


        }
        [HttpPost]
         
        public async Task<IActionResult> CreateReimbursement(EmployeeReimbursement reimbursement)
        {
            try
            {
                var response = await _IEmployeeReimbursementRepo.CreateEntity(reimbursement);
                return Ok(response);

            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                return await Task.Run(() => BadRequest(new Helpers.ResponseEntityList<EmployeeReimbursement>
                    (System.Net.HttpStatusCode.InternalServerError, ResponseStatus.DataBaseException.ToString(), null)
                    .GetResponseEntityList()));
            }
        }
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetReimbursement(string EmpCode)
        {
            try
            {
                var reimbursementResponse = await _IEmployeeReimbursementRepo.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var categoryResponse = await _IReimbursementCategoryRepo.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var employeeResponse = await _IEmployeeDetailRepo.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var responseDetails = (from reimbursement in reimbursementResponse.Entities
                                       join category in categoryResponse.Entities
                                       on reimbursement.CategoryId equals category.Id
                                       join employee in employeeResponse.Entities
                                       on reimbursement.EmpCode equals employee.EmpCode
                                       where reimbursement.EmpCode.Trim()==EmpCode.Trim()
                                       select new EmployeeReimbursementVM
                                       {
                                           Id = reimbursement.Id,
                                           EmployeeName = employee.EmployeeName + " ( " + employee.EmpCode + " ) ",
                                           Category = category.Name,
                                           Month = reimbursement.DateMonth.ToString(),
                                           Year = reimbursement.DateYear,
                                           Invocie = reimbursement.InvoiceNumber,
                                           Currency = reimbursement.Currency,
                                           Amount = reimbursement.InvoiceAmount,
                                           Attachment = reimbursement.FilePath,
                                           Status = reimbursement.Status,
                                           Description = reimbursement.Description
                                       }).ToList();
                return Ok(responseDetails);

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
