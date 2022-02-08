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
    [Table("P&LMaster", Schema = "Master")]
    public class PAndLMaster : BaseModel<int>
    {
        [Required(ErrorMessage = "This field is required.")]
        [Display(Prompt = "P and L Name")]
        public string Name { get; set; }
    }
}
