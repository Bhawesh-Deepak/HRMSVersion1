using HRMS.Core.Entities.HR;
using HRMS.Core.Helpers.CommonHelper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Helpers.ExcelHelper
{
    public class ReadLeaveAllocationExcelHelper
    {
        public IEnumerable<LeaveAllocation> GetLeaveAllocationComponent(IFormFile inputFile)
        {
            var dataResult = ReadExcelDataHelper.GetDataTableFromExcelFile(inputFile);
            var columnresult = dataResult.dtResult.Columns;
            var leaveallocationModels = new List<LeaveAllocation>();
            for (int i = 1; i < dataResult.dtResult.Rows.Count; i++)
            {
                for (int J = 1; J < dataResult.dtResult.Columns.Count; J++)
                {
                    var leaveallocation = new LeaveAllocation();
                    leaveallocation.EmpCode = dataResult.dtResult.Rows[i][0].ToString().GetDefaultDBNull<string>();
                    leaveallocation.LeaveCode = dataResult.dtResult.Rows[0][J].ToString().GetDefaultDBNull<string>();
                    leaveallocation.CountLeave = dataResult.dtResult.Rows[i][J].ToString().GetDefaultDBNull<decimal>();
                    leaveallocationModels.Add(leaveallocation);

                }
            }
            return leaveallocationModels;
        }
    }
}
