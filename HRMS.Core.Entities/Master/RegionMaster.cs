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
    [Table("Region", Schema = "Master")]
    public class RegionMaster: BaseModel<int>
    {
        [Required(ErrorMessage = "This field is required.")]
        [Display(Prompt = "Region")]
        public string Name { get; set; }
    }
}
