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
    [Table("AnnouncementAndUpdate", Schema = "Master")]
    public class AnnouncementAndUpdate : BaseModel<int>
    {
        [Required(ErrorMessage = "this field is required")]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "this field is required")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "this field is required")]
        [Display(Prompt = "Announcement")]
        public string Announcement { get; set; }


        [Required(ErrorMessage = "this field is required")]
        public DateTime AnnouncementDate { get; set; }
        [Required(ErrorMessage = "this field is required")]
        public DateTime ApplicableDate { get; set; }

       
    }
}
