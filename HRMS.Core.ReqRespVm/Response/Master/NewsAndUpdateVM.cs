using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Master
{
    public class NewsAndUpdateVM
    {
        public int Id { get; set; }
        public string UploadFile { get; set; }
        public string News { get; set; }
        public string CalenderDate { get; set; }
        public string BranchName { get; set; }
        public string RegionName { get; set; }
        public string LocationTypeName { get; set; }

    }
}
