﻿using HRMS.Core.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Core.Entities.Payroll
{
    [Table("Attandence", Schema= "Payroll")]
    public class EmployeeAttendance: BaseModel<int>
    {
        public string EmployeeCode { get; set; }
        public int DateYear { get; set; }
        public int DateMonth { get; set; }
        public int TotalDays { get; set; }
        public decimal LOPDays { get; set; }
        public decimal PresentDays { get; set; }
    }
}
