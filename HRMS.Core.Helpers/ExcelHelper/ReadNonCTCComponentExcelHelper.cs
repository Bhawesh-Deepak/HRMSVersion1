using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.CommonHelper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;


namespace HRMS.Core.Helpers.ExcelHelper
{
    public class ReadNonCTCComponentExcelHelper
    {
        public IEnumerable<EmployeeNonCTC> GetEmployeeNonCTCComponent(IFormFile inputFile)
        {

            var dataResult = ReadExcelDataHelper.GetDataTableFromExcelFile(inputFile);
            var columnresult = dataResult.dtResult.Columns;
            var models = new List<EmployeeNonCTC>();
            for (int i = 1; i < dataResult.dtResult.Rows.Count; i++)
            {
                for (int J = 3; J < dataResult.dtResult.Columns.Count; J++)
                {
                    var model = new EmployeeNonCTC();
                    model.DateMonth = dataResult.dtResult.Rows[i][0].ToString().GetDefaultDBNull<int>();
                    model.DateYear = dataResult.dtResult.Rows[i][1].ToString().GetDefaultDBNull<int>();
                    model.EmpCode = dataResult.dtResult.Rows[i][2].ToString().GetDefaultDBNull<string>();
                    model.ComponentName = dataResult.dtResult.Rows[0][J].ToString().GetDefaultDBNull<string>();
                    model.ComponentValue = dataResult.dtResult.Rows[i][J].ToString().GetDefaultDBNull<decimal>();
                    models.Add(model);
                }
            }

            return models;
        }
    }
}
