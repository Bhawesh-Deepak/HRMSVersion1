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
    [Table("StateMaster", Schema = "Master")]
    public class StateMaster: BaseModel<int>
    {
        public int CountryId { get; set; }
        [Required(ErrorMessage = "State name is required.")]
        [Display(Prompt = "State Name")]
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
