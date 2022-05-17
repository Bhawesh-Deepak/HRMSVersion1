using HRMS.Core.Entities.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Investment
{
    public class ComputationOfTaxReportVM
    {

        public int TentitiveTDSId { get; set; }
        public int DateMonth { get; set; }
        public int DateYear { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal HRADeclared { get; set; }
        public decimal HRAExamption { get; set; }
        public decimal Sec80CExamption { get; set; }
        public decimal Sec80CCD1B { get; set; }
        public decimal Sec80CCD2 { get; set; }
        public decimal Sec80D { get; set; }
        public decimal Sec80DD { get; set; }
        public decimal Sec80E { get; set; }
        public decimal Sec80EE { get; set; }
        public decimal Sec80EEB { get; set; }
        public decimal Sec80G { get; set; }
        public decimal Sec80GG { get; set; }
        public decimal Sec80U { get; set; }
        public decimal Sec24 { get; set; }
        public decimal Sec10 { get; set; }
        public decimal Age { get; set; }
        public decimal TotalExamptAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal StanderedDeduction { get; set; }
        public decimal HECAmount { get; set; }
        public decimal Surcharge { get; set; }
        public decimal FinalTDSAmountYearly { get; set; }
        public decimal FinalTDSAmountMonthly { get; set; }
        public decimal RemainingTax { get; set; }
        public decimal PaidTax { get; set; }
        public decimal Sec16 { get; set; }
        public decimal previousemployersal { get; set; }
        public decimal PerformanceIncentive { get; set; }
        public List<DeclarationVM> Investments { get; set; }
        public List<EmployeeSalaryVM> EmployeSalary { get; set; }
    }
    public class  DeclarationVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal DeclaredAmount { get; set; }
    }
    public class EmployeeSalaryVM
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string PanCardNumber { get; set; }
        public string EmpCode { get; set; }
        public decimal CTC { get; set; }
        public decimal ComponentValue { get; set; }
        public int ComponentID { get; set; }
        public string ComponentName { get; set; }
        public string OfficeEmailId { get; set; }
        public string PersonalEmailId { get; set; }

    }
}
