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
    [Table("CompanyNews", Schema = "Master")]
    public class CompanyNews : BaseModel<int>
    {

        [Required(ErrorMessage = "This field   is required.")]
        [DataType(DataType.Date)]
        public DateTime NewsDate { get; set; }

        [Required(ErrorMessage = "This field   is required.")]
        [Display(Prompt = "News Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int DepartmentId { get; set; }
        [NotMapped]
        public string DepartmentName { get; set; }

    }
}
