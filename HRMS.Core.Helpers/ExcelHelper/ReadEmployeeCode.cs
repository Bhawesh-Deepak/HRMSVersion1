using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.Response.Salary;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Helpers.ExcelHelper
{
    public class ReadEmployeeCode
    {
        public string GetSalaryRegisterEmpCodeDetails(IFormFile inputFile)
        {
            var dataResult = ReadExcelDataHelper.GetDataTableFromExcelFile(inputFile);
            StringBuilder builder = new StringBuilder();
            for (int i = 1; i < dataResult.dtResult.Rows.Count; i++)
            {
                builder.Append(dataResult.dtResult.Rows[i][0].ToString().GetDefaultDBNull<string>()).Append(",");
            }
            return builder.ToString().TrimEnd(new char[] { ',' });
        }
    }
}
