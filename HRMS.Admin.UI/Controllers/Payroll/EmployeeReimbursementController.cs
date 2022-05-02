using HRMS.Core.Entities.Common;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Entities.Reimbursement;
using HRMS.Core.Helpers.BlobHelper;
using HRMS.Core.Helpers.CommonCRUDHelper;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Core.ReqRespVm.Response.Reimbursement;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Payroll
{
    public class EmployeeReimbursementController : Controller
    {
        private readonly IGenericRepository<EmployeeReimbursement, int> _IEmployeeReimbursementRepo;
        private readonly IGenericRepository<ReimbursementCategory, int> _IReimbursementCategoryRepo;
        private readonly IGenericRepository<EmployeeDetail, int> _IEmployeeDetailRepo;
        private readonly IHostingEnvironment _IHostingEnviroment;

        public EmployeeReimbursementController(IGenericRepository<EmployeeReimbursement, int> iEmployeeReimbursementRepo,
            IGenericRepository<ReimbursementCategory, int> reimbursementCategoryRepo,
             IGenericRepository<EmployeeDetail, int> employeeDetailRepo,
            IHostingEnvironment hostingEnvironment)
        {
            _IEmployeeReimbursementRepo = iEmployeeReimbursementRepo;
            _IHostingEnviroment = hostingEnvironment;
            _IReimbursementCategoryRepo = reimbursementCategoryRepo;
            _IEmployeeDetailRepo = employeeDetailRepo;
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

                var reimbursementResponse = await _IEmployeeReimbursementRepo.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var categoryResponse = await _IReimbursementCategoryRepo.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var employeeResponse = await _IEmployeeDetailRepo.GetAllEntities(x => x.IsActive && !x.IsDeleted);
                var responseDetails = (from reimbursement in reimbursementResponse.Entities
                                       join category in categoryResponse.Entities
                                       on reimbursement.CategoryId equals category.Id
                                       join employee in employeeResponse.Entities
                                     on reimbursement.EmpCode equals employee.EmpCode
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

                return PartialView(ViewHelper.GetViewPathDetails("Reimbursement", "ReimbursementDetail"), responseDetails);
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
                await PopulateViewBag();
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
                model.FilePath = await new BlobHelper().UploadImageToFolder(InvoiceFile, _IHostingEnviroment);
                if (model.Id == 0)
                {
                    model.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                    model.FinancialYear = Convert.ToInt32(HttpContext.Session.GetString("financialYearId"));
                    model.CreatedDate = DateTime.Now;
                    var response = await _IEmployeeReimbursementRepo.CreateEntity(model);
                    return Json(response.Message);
                }
                else
                {
                    model.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                    model.UpdatedDate = DateTime.Now;
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
        [HttpGet]
        public async Task<IActionResult> AcceptReimbursement(int id)
        {
            try
            {
                var acceptModel = await _IEmployeeReimbursementRepo.GetAllEntityById(x => x.Id == id);
                acceptModel.Entity.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                acceptModel.Entity.UpdatedDate = DateTime.Now;
                acceptModel.Entity.Status = "Accepted";
                var response = await _IEmployeeReimbursementRepo.UpdateEntity(acceptModel.Entity);
                return Json(response.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeReimbursement)} action name {nameof(AcceptReimbursement)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> RejectReimbursement(int id)
        {
            try
            {
                var acceptModel = await _IEmployeeReimbursementRepo.GetAllEntityById(x => x.Id == id);

                return PartialView(ViewHelper.GetViewPathDetails("Reimbursement", "RejectReimbursement"), acceptModel.Entity);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeReimbursement)} action name {nameof(RejectReimbursement)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostRejectReimbursement(EmployeeReimbursement reimbursement)
        {
            try
            {
                var acceptModel = await _IEmployeeReimbursementRepo.GetAllEntityById(x => x.Id == reimbursement.Id);
                acceptModel.Entity.UpdatedBy = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
                acceptModel.Entity.UpdatedDate = DateTime.Now;
                acceptModel.Entity.Status = "Rejected";
                acceptModel.Entity.Description = reimbursement.Description;
                var response = await _IEmployeeReimbursementRepo.UpdateEntity(acceptModel.Entity);
                return Json(response.Message);
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeReimbursement)} action name {nameof(PostRejectReimbursement)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }
        private async Task PopulateViewBag()
        {
            var categoryResponse = await _IReimbursementCategoryRepo.GetAllEntities(x => x.IsActive && !x.IsDeleted);
            if (categoryResponse.ResponseStatus == ResponseStatus.Success)
                ViewBag.CategoryList = categoryResponse.Entities.ToList();
        }
        [HttpGet]
        public async Task<IActionResult> ExportReiembursement()
        {
            try
            {
                await PopulateViewBag();
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("Reimbursement", "_ExportReiembursement")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeReimbursement)} action name {nameof(ExportReiembursement)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }
        [HttpPost]
        public async Task<IActionResult> PostExportReiembursement(ExportReimbursementVM exportReimbursementVM)
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
                                       where reimbursement.DateMonth.Trim() == exportReimbursementVM.DateMonth.Trim() && reimbursement.DateYear.Trim() == exportReimbursementVM.DateYear.Trim()
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
                                           Description = reimbursement.Description,
                                           CategoryId = category.Id
                                       }).ToList();
                if (exportReimbursementVM.CategoryId != null && exportReimbursementVM.Status == "")
                {
                    responseDetails.Where(x => x.CategoryId == exportReimbursementVM.CategoryId);
                }
                else if (exportReimbursementVM.CategoryId == null && exportReimbursementVM.Status != "")
                {
                    if (exportReimbursementVM.Status == "Active")
                    {
                        responseDetails.Where(x => x.Status.Trim() == null);
                    }
                    else
                    {
                        responseDetails.Where(x => x.Status.Trim() == exportReimbursementVM.Status.Trim());
                    }

                }
                else if(exportReimbursementVM.CategoryId != null && exportReimbursementVM.Status != "")
                {
                    if (exportReimbursementVM.Status == "Active")
                    {
                        responseDetails.Where(x => x.CategoryId == exportReimbursementVM.CategoryId && x.Status.Trim() == null);
                    }
                    else
                    {
                        responseDetails.Where(x => x.CategoryId == exportReimbursementVM.CategoryId && x.Status.Trim() == exportReimbursementVM.Status.Trim());
                    }
                }
               

                var sWebRootFolder = _IHostingEnviroment.WebRootPath;
                var sFileName = @"EmployeeReiembursement.xlsx";
                var URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                if (file.Exists)
                {
                    file.Delete();
                    file = new FileInfo(fileName: Path.Combine(sWebRootFolder, sFileName));
                }
                ExcelPackage Eps = new ExcelPackage();
                ExcelWorksheet Sheets = Eps.Workbook.Worksheets.Add("Reiembursement");
                Sheets.Cells["A1:H1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                Sheets.Cells["A1:H1"].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);

                Sheets.Cells["A1"].Value = "Employee";
                Sheets.Cells["B1"].Value = "Category";
                Sheets.Cells["C1"].Value = "Month";
                Sheets.Cells["D1"].Value = "Year";
                Sheets.Cells["E1"].Value = "Invocie";
                Sheets.Cells["F1"].Value = "Currency";
                Sheets.Cells["G1"].Value = "Amount";
                Sheets.Cells["H1"].Value = "Status";
                
                int row = 2;
                foreach (var data in responseDetails)
                {
                    Sheets.Cells[string.Format("A{0}", row)].Value = data.EmployeeName;
                    Sheets.Cells[string.Format("B{0}", row)].Value = data.Category;
                    Sheets.Cells[string.Format("C{0}", row)].Value = data.Month;
                    Sheets.Cells[string.Format("D{0}", row)].Value = data.Year;
                    Sheets.Cells[string.Format("E{0}", row)].Value = data.Invocie;
                    Sheets.Cells[string.Format("F{0}", row)].Value = data.Currency;
                    Sheets.Cells[string.Format("G{0}", row)].Value = data.Amount;
                    Sheets.Cells[string.Format("H{0}", row)].Value = data.Status;
                    
                    row++;
                }

                var stream = new MemoryStream(Eps.GetAsByteArray());
                return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);


            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(EmployeeReimbursement)} action name {nameof(ExportReiembursement)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }

        }
    }
}
