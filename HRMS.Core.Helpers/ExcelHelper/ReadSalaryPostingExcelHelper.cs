using HRMS.Core.Entities.Posting;
using HRMS.Core.Helpers.CommonHelper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Helpers.ExcelHelper
{
    public class ReadSalaryPostingExcelHelper
    {
        public IEnumerable<EmployeeSalaryPosted> GetEmployeeSalaryPostingComponent(IFormFile inputFile)
        {
            var dataResult = ReadExcelDataHelper.GetDataTableFromExcelFile(inputFile);
            var columnresult = dataResult.dtResult.Columns;
            var models = new List<EmployeeSalaryPosted>();
            for (int i = 2; i < dataResult.dtResult.Rows.Count; i++)
            {
                for (int J = 3; J < dataResult.dtResult.Columns.Count; J++)
                {
                    var model = new EmployeeSalaryPosted();
                    model.DateMonth = dataResult.dtResult.Rows[i][0].ToString().GetDefaultDBNull<int>();
                    model.DateYear = dataResult.dtResult.Rows[i][1].ToString().GetDefaultDBNull<int>();
                    model.EmpCode = dataResult.dtResult.Rows[i][2].ToString().GetDefaultDBNull<string>();
                    model.ComponentId = dataResult.dtResult.Rows[0][J].ToString().GetDefaultDBNull<int>();
                    model.SalaryAmount = dataResult.dtResult.Rows[i][J].ToString().GetDefaultDBNull<decimal>();
                    model.FinancialYear = 0;
                    models.Add(model);
                }
            }
            return models;
        }
    }
}
