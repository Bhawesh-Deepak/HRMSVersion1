using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.ReqRespVm.Response.Master
{
    public class HolidaysDetails
    {
        public int HolidayId { get; set; }
        public string StateName { get; set; }
        public string HolidayName { get; set; }
        public DateTime Holidate{ get; set; }
        public string Desscription { get; set; }
    }
}
