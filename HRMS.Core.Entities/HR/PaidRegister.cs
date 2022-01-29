using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HRMS.Core.Entities.HR
{
    [Table("PaidRegister", Schema = "HR")]
    public class PaidRegister : BaseModel<int>
    {
        [Required(ErrorMessage = "this field is required.")]
        [Display(Prompt = "Month")]
        public int DateMonth{ get; set; }

        [Required(ErrorMessage = "this field is required.")]
        [RegularExpression(@"^(\d{4})$", ErrorMessage = "Enter a valid 4 digit Year")]
        [Display(Prompt = "Year")]
        public int DateYear{ get; set; }
        [Required(ErrorMessage = "this field is required.")]
        public string UploadFilePath{ get; set; }
        public int ComponentType { get; set; }

        public IFormFile UploadFile { get; set; }
    }
}
