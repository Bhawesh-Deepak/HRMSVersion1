using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Reimbursement
{
    public class EmployeeReimbursementVM
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string Category { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Invocie { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string Attachment { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }

    }
}
