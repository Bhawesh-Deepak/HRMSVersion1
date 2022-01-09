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
    [Table("CompanyPolicy", Schema = "Master")]
    public class CompanyPolicy : BaseModel<int>
    {
        [Required(ErrorMessage = "Department is required.")]
        public int DepartmentId { get; set; }


        [Required(ErrorMessage = "Company Policy name is required.")]
        [Display(Prompt = "policy Name")]
        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime CalenderDate { get; set; }

        public string DocumentUrl { get; set; }
    }
}
