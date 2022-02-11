using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.BlobHelper;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Payroll
{
    public class EmployeeReimbursementController : Controller
    {
        private readonly IGenericRepository<EmployeeReimbursement, int> _IEmployeeReimbursementRepo;
        private readonly IHostingEnvironment _IHostingEnviroment;

        public EmployeeReimbursementController(IGenericRepository<EmployeeReimbursement, int> iEmployeeReimbursementRepo, 
            IHostingEnvironment hostingEnvironment)
        {
            _IEmployeeReimbursementRepo = iEmployeeReimbursementRepo;
            _IHostingEnviroment = hostingEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Reimbursement", "ReimbursementIndex")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeReimbursement)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> GetReimbursementDetails()
        {
            try
            {
                var response = await _IEmployeeReimbursementRepo.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                return PartialView(ViewHelper.GetViewPathDetails("Reimbursement", "ReimbursementDetail"), response.Entities);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeReimbursement)} action name {nameof(GetReimbursementDetails)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> CreateReimbursement(int id)
        {
            try
            {
                var response = await _IEmployeeReimbursementRepo.GetAllEntities(x => x.Id == id);
                if (id == 0)
                {
                    return PartialView(ViewHelper.GetViewPathDetails("Reimbursement", "ReimbursementCreate"));
                }
                else
                {
                    return PartialView(ViewHelper.GetViewPathDetails("Reimbursement", "ReimbursementCreate"), response.Entities.First());
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeReimbursement)} action name {nameof(CreateReimbursement)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpsertReiembursement(EmployeeReimbursement model, IFormFile InvoiceFile)
        {
            try
            {
                model.EmpCode = HttpContext.Session.GetString("EmpCode");
                model.InvoicePDFPath = await new BlobHelper().UploadImageToFolder(InvoiceFile, _IHostingEnviroment);
                if (model.Id == 0)
                {

                    var response = await _IEmployeeReimbursementRepo.CreateEntity(model);
                    return Json(response.Message);
                }
                else
                {
                    var response = await _IEmployeeReimbursementRepo.UpdateEntity(model);
                    return Json(response.Message);
                }
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeReimbursement)} action name {nameof(UpsertReiembursement)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteReimbursement(int id)
        {
            try
            {
                var deleteModel = await _IEmployeeReimbursementRepo.GetAllEntityById(x => x.Id == id);
                var deleteDbModel = CrudHelper.DeleteHelper<EmployeeReimbursement>(deleteModel.Entity, 1);
                var deleteResponse = await _IEmployeeReimbursementRepo.DeleteEntity(deleteDbModel);
                if (deleteResponse.ResponseStatus == Core.Entities.Common.ResponseStatus.Deleted)
                {
                    return Json(deleteResponse.Message);
                }
                return Json(deleteResponse.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeReimbursement)} action name {nameof(DeleteReimbursement)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
