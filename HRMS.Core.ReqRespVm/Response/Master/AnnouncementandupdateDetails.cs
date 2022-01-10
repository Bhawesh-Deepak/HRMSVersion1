using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Master
{
    public class AnnouncementandupdateDetails
    {
        public int AnnandupdId { get; set; }
        public string Department { get; set; }
        
        public string Branch { get; set; }
        public string Announcement { get; set; }
        public string AnnouncementDate { get; set; }
        public string ApplicableDate { get; set; } 
    }
}
