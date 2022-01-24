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
        [Required(ErrorMessage = "this field is required.")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "Branch Name")]
        public string Name { get; set; }
        [Display(Prompt = "code")]
        public string Code { get; set; }
        [Display(Prompt = "logo")]
        public string Logo { get; set; }
        [Display(Prompt = "Address")]
        public string Address { get; set; }

        [Display(Prompt = "Zip Code")]
        public int ZipCode { get; set; }

        [Display(Prompt = "Email")]

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Incorrect Email Format")]

        public string Email { get; set; }
        [Display(Prompt = "phone")]
        public string Phone { get; set; }
        [Display(Prompt = "Contact Person")]
        public string ContactPerson { get; set; }
    }
}
