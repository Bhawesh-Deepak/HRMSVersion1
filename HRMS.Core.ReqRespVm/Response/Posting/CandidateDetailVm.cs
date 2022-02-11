using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Posting
{
    public class CandidateDetailVm
    {
        public int Id { get; set; }
        public int JobTitleId { get; set; }
        public string CandidateName { get; set; }
        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }
        public string ResumePath { get; set; }
        public string CandidateStatus { get; set; }
        public string ReferedBy { get; set; }
        public string JobTitle { get; set; }
    }
}
