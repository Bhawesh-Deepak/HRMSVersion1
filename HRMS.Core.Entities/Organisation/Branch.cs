using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Organisation
{
    [Table("Branch", Schema = "Organisation")]
    public class Branch : BaseModel<int>
    {
        [Required(ErrorMessage = "Company is required.")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Branch  name is required.")]
        [Display(Prompt = "Branch Name")]
        public string Name { get; set; }
        //[Required(ErrorMessage = "Branch  code is required.")]
        [Display(Prompt = "code")]
        public string Code { get; set; }
        //[Required(ErrorMessage = "Branch logo is required.")]
        [Display(Prompt = "logo")]
        public string Logo { get; set; }
        //[Required(ErrorMessage = "Branch Address is required.")]
        [Display(Prompt = "Address")]
        public string Address { get; set; }

        //[Required(ErrorMessage = "ZipCode  is required.")]
        [Display(Prompt = "ZipCode")]
        public int ZipCode { get; set; }

        //[Required(ErrorMessage = "Email  is required.")]
        [Display(Prompt = "Email")]
        public string Email { get; set; }
        //[Required(ErrorMessage = "phone  is required.")]
        [Display(Prompt = "phone")]
        public string Phone { get; set; }
        //[Required(ErrorMessage = "ContactPerson  is required.")]
        [Display(Prompt = "ContactPerson")]
        public string ContactPerson { get; set; }
    }
}
