using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Payroll
{
    [Table("EmployeeReimbursement", Schema = "Payroll")]
    public class EmployeeReimbursement : BaseModel<int>
    {
        public string EmpCode { get; set; }
        public string InvoiceMonth { get; set; }
        public string InvoiceYear { get; set; }
        public string Currency { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string InvoicePDFPath { get; set; }
    }
}
