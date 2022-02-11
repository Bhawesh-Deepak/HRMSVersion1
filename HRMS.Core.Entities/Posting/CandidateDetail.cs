using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Posting
{
    [Table("CandidateDetail",Schema = "Posting")]
    public class CandidateDetail: BaseModel<int>
    {
        public int JobTitleId { get; set; }
        public string CandidateName { get; set; }
        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }
        public string ResumePath { get; set; }
        public string CandidateStatus { get; set; }
        public string ReferedBy { get; set; }
    }
}
