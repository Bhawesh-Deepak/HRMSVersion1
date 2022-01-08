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
    [Table("Shift", Schema = "Master")]
    public class Shift: BaseModel<int>
    {
      
        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "Shift")]
        public string Name { get; set; }

        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "StartTime")]
        public string ShiftStartTime { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "EndTime")]
        public string ShiftEndTime { get; set; }
        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "LateTime")]
        public string ShiftLateTime { get; set; }

    }
}
