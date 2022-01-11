using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.CommonHelper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Helpers.ExcelHelper
{
    public class ReadEmployeeArrear
    {
        public List<EmployeeArrears> GetEmployeeArrearsDetails(IFormFile inputFile)
        {
            var dataResult = ReadExcelDataHelper.GetDataTableFromExcelFile(inputFile);
            var arrearsModels = new List<EmployeeArrears>();
            for (int i = 1; i < dataResult.dtResult.Rows.Count; i++)
            {
                var arrearsModel = new EmployeeArrears();
                arrearsModel.EmployeeCode = dataResult.dtResult.Rows[i][0].ToString().GetDefaultDBNull<string>();
                arrearsModel.DateYear = dataResult.dtResult.Rows[i][1].ToString().GetDefaultDBNull<int>();
                arrearsModel.DateMonth = dataResult.dtResult.Rows[i][2].ToString().GetDefaultDBNull<int>();
                arrearsModel.ArrearMonth4 = dataResult.dtResult.Rows[i][3].ToString().GetDefaultDBNull<int>();
                arrearsModel.ArrearYear4 = dataResult.dtResult.Rows[i][4].ToString().GetDefaultDBNull<int>();
                arrearsModel.ArrearDays4 = dataResult.dtResult.Rows[i][5].ToString().GetDefaultDBNull<decimal>();
                arrearsModel.ArrearMonth3 = dataResult.dtResult.Rows[i][6].ToString().GetDefaultDBNull<int>();
                arrearsModel.ArrearYear3 = dataResult.dtResult.Rows[i][7].ToString().GetDefaultDBNull<int>();
                arrearsModel.ArrearDays3 = dataResult.dtResult.Rows[i][8].ToString().GetDefaultDBNull<decimal>();
                arrearsModel.ArrearMonth2 = dataResult.dtResult.Rows[i][9].ToString().GetDefaultDBNull<int>();
                arrearsModel.ArrearYear2 = dataResult.dtResult.Rows[i][10].ToString().GetDefaultDBNull<int>();
                arrearsModel.ArrearDays2 = dataResult.dtResult.Rows[i][11].ToString().GetDefaultDBNull<decimal>();
                arrearsModel.ArrearMonth1 = dataResult.dtResult.Rows[i][12].ToString().GetDefaultDBNull<int>();
                arrearsModel.ArrearYear1 = dataResult.dtResult.Rows[i][13].ToString().GetDefaultDBNull<int>();
                arrearsModel.ArrearDays1 = dataResult.dtResult.Rows[i][14].ToString().GetDefaultDBNull<decimal>();
                arrearsModels.Add(arrearsModel);
            }
            return arrearsModels;
        }
    }
}
