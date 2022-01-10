using HRMS.Core.Entities.Posting;
using System.Collections.Generic;

namespace HRMS.Core.ReqRespVm.Response.Posting
{
    public class ReferCandidateDetailVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CandidateName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ResumePath { get; set; }
        public string EmployeeName { get; set; }
        public string EmpCode { get; set; }

    }
}
