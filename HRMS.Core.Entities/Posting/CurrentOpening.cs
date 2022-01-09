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
        [MaxLength(1000), MinLength(10)]
        [Display(Prompt ="Please Enter Job Title")]
        public string Title { get; set; }

        [Required()]
        [MaxLength(500), MinLength(5)]
        [Display(Prompt = "Please Enter Required Exprience")]
        public string RequiredExprience { get; set; }

        [Required()]
        [MaxLength(1500), MinLength(5)]
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
        [MaxLength(2500), MinLength(10)]
        [Display(Prompt = "Please Enter Job Location")]
        public string Location { get; set; }
        public string DescriptionPath { get; set; }
    }
}
