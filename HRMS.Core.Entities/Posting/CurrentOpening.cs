using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Posting
{
    [Table("CurrentOpening", Schema = "Posting")]
    public class CurrentOpening:BaseModel<int>
    {
        [Required()]
        [Display(Prompt ="Please Enter Job Title")]
        public string Title { get; set; }

        [Required()]
        [Display(Prompt = "Please Enter Required Experience")]
        public string RequiredExprience { get; set; }

        [Required()]
        [Display(Prompt = "Please Enter Required Key Skills")]
        public string KeySkills { get; set; }

        [Required()]
        [Display(Prompt = "Please Enter Vacancies")]
        public int Vacancy { get; set; }

        [Required()]
        [Display(Prompt = "Please Enter Last Apply Date")]
        [DataType(DataType.Date)]
        public DateTime LastApplyDate { get; set; }

        [Required()]
        [Display(Prompt = "Please Enter Job Location")]
        public string Location { get; set; }
        public string DescriptionPath { get; set; }
        public int BranchId { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime OpeningDate { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime ClosingDate { get; set; }
        public string JobDescription { get; set; }
    }
}
