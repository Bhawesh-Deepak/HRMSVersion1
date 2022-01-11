using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Master
{
    [Table("AssesmentYear", Schema = "Master")]
    public class AssesmentYear : BaseModel<int>
    {
        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "Start Year")]
        public int StartYear { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "Start Month")]
        public int StartMonth { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "End Year")]
        public int EndYear { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "End Month")]
        public int EndMonth { get; set; }
        public string Name { get; set; }

    }
}
