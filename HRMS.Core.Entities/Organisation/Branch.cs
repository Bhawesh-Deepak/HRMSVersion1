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
        [Required(ErrorMessage = "This field is required.")]
        public int CompanyId { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public int RegionId { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public int LocationTypeId { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Display(Prompt = "Branch Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Display(Prompt = "Code")]
        public string Code { get; set; }
        

    }
}
