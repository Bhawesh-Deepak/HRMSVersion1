using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Leave
{
    [Table("LeaveAllocation", Schema = "Leave")]
    public class LeaveAllocation : BaseModel<int>
    {
        [Required(ErrorMessage = "Enter Employee Code.")]
        [Display(Prompt ="Enter Employee Code")]
        public string EmpCode { get; set; }
        public int AnnualLeave { get; set; }
        public int MandatoryLeave { get; set; }
        public int OptionalLeave { get; set; }
        public int SickLeaves { get; set; }
        public int MaternityLeave { get; set; }
        public int PaternityLeave { get; set; }
        public int BereavementLeave { get; set; }

    }
}
