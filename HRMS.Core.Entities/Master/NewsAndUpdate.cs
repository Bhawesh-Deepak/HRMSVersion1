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
    [Table("NewsAndUpdate", Schema = "Master")]
    public class NewsAndUpdate : BaseModel<int>
    {
        [Required(ErrorMessage = "This field is required.")]
        public int BranchId { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [Display(Prompt = "News and Update")]
        public string News { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime CalenderDate { get; set; }
        public string UploadFile { get; set; }

    }
}
