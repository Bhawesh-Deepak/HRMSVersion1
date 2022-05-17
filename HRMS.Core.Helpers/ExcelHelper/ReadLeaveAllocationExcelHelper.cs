using HRMS.Core.Entities.HR;
using HRMS.Core.Entities.Leave;
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
                leaveallocationModels.Add(new LeaveAllocation()
                {
                    EmpCode = dataResult.dtResult.Rows[i][0].ToString().GetDefaultDBNull<string>(),
                    AnnualLeave = dataResult.dtResult.Rows[i][1].ToString().GetDefaultDBNull<int>(),
                    MandatoryLeave = dataResult.dtResult.Rows[i][2].ToString().GetDefaultDBNull<int>(),
                    OptionalLeave = dataResult.dtResult.Rows[i][3].ToString().GetDefaultDBNull<int>(),
                    SickLeaves = dataResult.dtResult.Rows[i][4].ToString().GetDefaultDBNull<int>(),
                    MaternityLeave = dataResult.dtResult.Rows[i][5].ToString().GetDefaultDBNull<int>(),
                    PaternityLeave = dataResult.dtResult.Rows[i][6].ToString().GetDefaultDBNull<int>(),
                    BereavementLeave = dataResult.dtResult.Rows[i][7].ToString().GetDefaultDBNull<int>(),
                });
            }
            return leaveallocationModels;
        }
    }
}
