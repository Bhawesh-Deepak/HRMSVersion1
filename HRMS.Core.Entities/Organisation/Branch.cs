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
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Branch  name is required.")]
        [Display(Prompt = "Branch Name")]
        public string Name { get; set; }
      
        [Display(Prompt = "Branch code")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Branch logo is required.")]
        public string Logo { get; set; }
        public string Address { get; set; }
        public int ZipCode { get; set; }
        [Display(Prompt = "Branch email")]
        public string Email { get; set; }
        [Display(Prompt = "Branch phone")]
        public string Phone { get; set; }
        public string ContactPerson { get; set; }
    }
}
