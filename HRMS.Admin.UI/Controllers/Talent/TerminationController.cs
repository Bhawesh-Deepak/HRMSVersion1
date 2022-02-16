using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Admin.UI.AuthenticateService;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Entities.Talent;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Admin.UI.Controllers.Talent
{
    [CustomAuthenticate]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class TerminationController : Controller
    {
        private readonly IGenericRepository<EmployeeTermination, int> _IEmployeeTerminationRepository;
        private readonly IDapperRepository<EmployeeSingleDetailParam> _IEmployeeSingleDetailRepository;
        public TerminationController(IGenericRepository<EmployeeTermination, int> employeeTerminationRepo, IDapperRepository<EmployeeSingleDetailParam> EmployeeSingleDetailRepository)
        {
            _IEmployeeTerminationRepository = employeeTerminationRepo;
            _IEmployeeSingleDetailRepository = EmployeeSingleDetailRepository;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Termination", "_TerminationIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(TerminationController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> CreateTermination(int id)
        {
            try
            {
               // await PopulateViewBag();

                if (id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("Termination", "_CreateTermination"));
                }
                else
                {
                    var response = await _IEmployeeTerminationRepository.GetAllEntities(x => x.Id == id);
                    return PartialView(ViewHelper.GetViewPathDetails("Termination", "_CreateTermination"), response.Entities.First());
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(TerminationController)} action name {nameof(CreateTermination)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        public async Task<IActionResult> GetEmployeeDetails(int Id)
        {
            try
            {
                var empParams = new EmployeeSingleDetailParam()
                {
                    Id = Id
                };
                var response = _IEmployeeSingleDetailRepository.GetAll<EmployeeDetail>(SqlQuery.GetEmployeeSingleDetails, empParams);

                return PartialView(ViewHelper.GetViewPathDetails("Termination", "_EmployeeDetails"), response.FirstOrDefault());
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(TerminationController)} action name {nameof(GetEmployeeDetails)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
