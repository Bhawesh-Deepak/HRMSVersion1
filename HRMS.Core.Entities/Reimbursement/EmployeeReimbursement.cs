using HRMS.Core.Entities.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Reimbursement
{
    [Table("EmployeeReimbursement", Schema = "Reimbursement")]
    public class EmployeeReimbursement : BaseModel<int>
    {
        [Required(ErrorMessage ="this field is required")]
        public string EmpCode { get; set; }
        [Required(ErrorMessage = "this field is required")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "this field is required")]
        public string DateMonth { get; set; }
        [Required(ErrorMessage = "this field is required")]
        public string DateYear { get; set; }
        [Required(ErrorMessage = "this field is required")]
        [Display(Prompt = "Currency")]
        public string Currency { get; set; }
        [Required(ErrorMessage = "this field is required")]
        [Display(Prompt = "Invoice Number")]
        public string InvoiceNumber { get; set; }
        [Required(ErrorMessage = "this field is required")]
        [Display(Prompt = "Invoice Amount")]
        public decimal InvoiceAmount { get; set; }
        [Required(ErrorMessage = "this field is required")]
        public string FilePath { get; set; }
        

        public string Description { get; set; }
        public string Status { get; set; }
    }
}
