using HRMS.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities.Payroll
{
    [Table("EmployeeArrears", Schema = "Payroll")]
    public class EmployeeArrears : BaseModel<int>
    {
        public string EmployeeCode { get; set; }
        public int DateYear { get; set; }
        public int DateMonth { get; set; }
        public int ArrearMonth4 { get; set; }
        public int ArrearYear4 { get; set; }
        public decimal ArrearDays4 { get; set; }
        public int ArrearMonth3 { get; set; }
        public int ArrearYear3 { get; set; }
        public decimal ArrearDays3 { get; set; }
        public int ArrearMonth2 { get; set; }
        public int ArrearYear2 { get; set; }
        public decimal ArrearDays2 { get; set; }
        public int ArrearMonth1 { get; set; }
        public int ArrearYear1 { get; set; }
        public decimal ArrearDays1 { get; set; }

    }
}
