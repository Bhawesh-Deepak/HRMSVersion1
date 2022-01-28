using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.HR
{
    [Table("PaidRegister", Schema = "HR")]
    public class PaidRegister : BaseModel<int>
    {
        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "Month")]
        public int DateMonth{ get; set; }

        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "Year")]
        public int DateYear{ get; set; }
        public string UploadFilePath{ get; set; }
    }
}
