using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Core.Entities.Posting
{
    [Table("ReferCandidate", Schema = "Posting")]
    public class ReferCandidate : BaseModel<int>
    {
        [Required(ErrorMessage = "Please select Current Opening")]
        [Display(Prompt = "Please select Current Opening")]
        public int OpeningId { get; set; }

        [Required(ErrorMessage = "Candidate name is required.")]
        [Display(Prompt ="Please enter candidate name")]
        [MaxLength(500)]
        public string CandidateName { get; set; }

        [Required(ErrorMessage ="Candidate email is required.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage ="Candidate phone is required.")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        public string ResumePath { get; set; }
    }
}
