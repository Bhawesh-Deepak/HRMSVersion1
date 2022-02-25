using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Payroll
{
    [Table("EmployeeTentitiveTDS", Schema = "Payroll")]
    public class EmployeeTentitiveTDS : BaseModel<int>
    {
        public string EmpCode { get; set; }
        public int DateYear { get; set; }
        public int DateMonth { get; set; }
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
        public decimal Sec16 { get; set; }
        public decimal PreviousEmployerSalary { get; set; }
        public int Age { get; set; }
        public decimal TotalExamptAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal StanderedDeduction { get; set; }
        public decimal HECAmount { get; set; }
        public decimal Surcharge { get; set; }
        public decimal FinalTDSAmountYearly { get; set; }
        public decimal FinalTDSAmountMonthly { get; set; }
        public decimal PaidTax { get; set; }
        public decimal RemainingTax { get; set; }

    }
}
