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
    [Table("Location", Schema = "Master")]
    public class Location: BaseModel<int>
    {
        [Required(ErrorMessage = "This field is required.")]
        public int LocationTypeid { get; set; }

 
        [Required(ErrorMessage = "This field is required.")]
        [Display(Prompt = "Location Name")]
        public string Name { get; set; }
    }
}
