using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Master
{
    public class EmployeeAssetsDetails
    {
        public int EmployeeAssetId { get; set; }
        public string EmployeeDetailsName { get; set; }
        public string CategoryAssetsName { get; set; }
        public string Branch { get; set; }
        public string AssetsTag { get; set; }
        public string SerialNumber { get; set; }
        public string ModelNumber { get; set; }
        public string MEID { get; set; }
        public string AssetsStatus { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Make { get; set; }
        public string DongleNumber { get; set; }


    }
}
