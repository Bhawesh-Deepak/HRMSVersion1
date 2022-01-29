using HRMS.Core.Entities.HR;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.Helpers.ExcelHelper;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Reporting
{
    public class SalaryPaidRegisterController : Controller
    {
        private readonly IDapperRepository<PaidRegister> _IPaidRegisterRepository;
        //private readonly IDapperRepository<SalaryRegisterParams> _ISalaryRegisterParamsRepository;
        //private readonly IDapperRepository<SalaryRegisterByEmployeeCodeParams> _ISalaryRegisterByEmployeeCodeParamsRepository;
        private readonly IHostingEnvironment _IHostingEnviroment;

        public SalaryPaidRegisterController(IDapperRepository<PaidRegister> PaidRegisterRepository, IHostingEnvironment hostingEnvironment)
        {
            _IPaidRegisterRepository = PaidRegisterRepository;
            _IHostingEnviroment = hostingEnvironment;
            //_ISalaryRegisterByEmployeeCodeParamsRepository = SalaryRegisterByEmployeeCodeParamsRepository;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                return await Task.Run(() => View(ViewHelper.GetViewPathDetails("SalaryPaidRegister", "_SalaryPaidRegister")));
            }
            catch (Exception ex)
            {
                string template = $"Controller name {nameof(SalaryPaidRegisterController)} action name {nameof(Index)} exception is {ex.Message}";
                Serilog.Log.Error(ex, template);
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpPost]
        public async Task<IActionResult> ExportSalaryRegister(PaidRegister model)
        {

            List<PaidRegister> response = null;
            if (model.UploadFilePath == null)
            {
                var request = new PaidRegister()
                {
                    DateMonth = model.DateMonth,
                    DateYear = model.DateYear,
                };
                response = (await Task.Run(() => _IPaidRegisterRepository.GetAll<PaidRegister>(SqlQuery.GetEmployeeSalary, request))).ToList();
            }
            else
            {
                var empresponse = new ReadEmployeeCode().GetSalaryRegisterEmpCodeDetails(model.UploadFilePath);
                var request = new SalaryRegisterByEmployeeCodeParams()
                {
                    DateMonth = model.DateMonth,
                    DateYear = model.DateYear,
                    EmployeeCode = empresponse
                };
                response = (await Task.Run(() => _IPaidRegisterRepository.GetAll<PaidRegister>(SqlQuery.GetEmployeeSalaryByCode, request))).ToList();
            }

            string sWebRootFolder = _IHostingEnviroment.WebRootPath;
            string sFileName = @"EmployeeSalaryRegister.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            ExcelPackage Eps = new ExcelPackage();
            ExcelWorksheet Sheets = Eps.Workbook.Worksheets.Add("EmployeeSalary");
            Sheets.View.FreezePanes(1, 4);
            Sheets.Cells["A1:DD1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            Sheets.Cells["A1:DD1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);
            Eps.Encryption.Password = "sqy" + model.DateMonth + "" + model.DateYear;
            Sheets.Cells["A1"].Value = "Month";
            Sheets.Cells["B1"].Value = "Year";
            Sheets.Cells["C1"].Value = "Employee_Code";
            Sheets.Cells["D1"].Value = "Employee_Name";
            Sheets.Cells["E1"].Value = "AccountName";
            Sheets.Cells["F1"].Value = "Status_Description";
            Sheets.Cells["G1"].Value = "Date_of_Birth";
            Sheets.Cells["H1"].Value = "Date_Of_Joining";
            Sheets.Cells["I1"].Value = "Date_of_Confirmation";
            Sheets.Cells["J1"].Value = "Date_Of_Leaving";
            Sheets.Cells["K1"].Value = "Band";
            Sheets.Cells["L1"].Value = "BioMetricCode";
            Sheets.Cells["M1"].Value = "Branch";
            Sheets.Cells["N1"].Value = "Department";
            Sheets.Cells["O1"].Value = "Designation ";
            Sheets.Cells["P1"].Value = "Function";
            Sheets.Cells["Q1"].Value = "LegalEntity";
            Sheets.Cells["R1"].Value = "PandLName";
            Sheets.Cells["S1"].Value = "SubDepartment";
            Sheets.Cells["T1"].Value = "Zone";
            Sheets.Cells["U1"].Value = "PT_StateName";
            Sheets.Cells["V1"].Value = "UAN_No";
            Sheets.Cells["W1"].Value = "EFD_PFAcctNo";
            Sheets.Cells["X1"].Value = "EFD_ESICAcctNo";
            Sheets.Cells["Y1"].Value = "PAN";
            Sheets.Cells["Z1"].Value = "adhaarNo";
            Sheets.Cells["AA1"].Value = "IFSCode";
            Sheets.Cells["AB1"].Value = "Bank_name";
            Sheets.Cells["AC1"].Value = "Bank_Account_Number";
            Sheets.Cells["AD1"].Value = "Branch_name";
            Sheets.Cells["AE1"].Value = "Days_Worked";
            Sheets.Cells["AF1"].Value = "Arrears_Days";
            Sheets.Cells["AG1"].Value = "LOP";
            Sheets.Cells["AH1"].Value = "LFDAYS";
            Sheets.Cells["AI1"].Value = "OTHRS";

            string[] CellArray = { "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ", "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ", "DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO" };
            int A = 0;
            
            //Sheets.Cells["CY1"].Value = "PI_Tax";
            Sheets.Cells["DC1"].Value = "Total Deduction";
            Sheets.Cells["DD1"].Value = "Net Salary";
            int row = 2;
           
            var stream = new MemoryStream(Eps.GetAsByteArray());
            return File(stream.ToArray(), "application/vnd.ms-excel", sFileName);
        }
    }
}
