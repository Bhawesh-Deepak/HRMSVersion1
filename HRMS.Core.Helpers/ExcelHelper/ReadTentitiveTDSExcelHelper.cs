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
    public class ReadTentitiveTDSExcelHelper
    {
        public List<EmployeeTentitiveTDS> GetEmployeeTentitiveTDs(IFormFile inputFile)
        {
            var dataResult = ReadExcelDataHelper.GetDataTableFromExcelFile(inputFile);
            var model = new List<EmployeeTentitiveTDS>();

            for (int i = 1; i < dataResult.dtResult.Rows.Count; i++)
            {
                var TentitiveTDS = new EmployeeTentitiveTDS();
                TentitiveTDS.EmpCode = dataResult.dtResult.Rows[i][0].ToString().GetDefaultDBNull<string>();
                TentitiveTDS.DateYear = dataResult.dtResult.Rows[i][1].ToString().GetDefaultDBNull<int>();
                TentitiveTDS.DateMonth = dataResult.dtResult.Rows[i][2].ToString().GetDefaultDBNull<int>();
                TentitiveTDS.GrossSalary = dataResult.dtResult.Rows[i][3].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.HRADeclared = dataResult.dtResult.Rows[i][4].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.HRAExamption = dataResult.dtResult.Rows[i][5].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.Sec80CExamption = dataResult.dtResult.Rows[i][6].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.Sec80CCD1B = dataResult.dtResult.Rows[i][7].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.Sec80CCD2 = dataResult.dtResult.Rows[i][8].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.Sec80D = dataResult.dtResult.Rows[i][9].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.Sec80DD = dataResult.dtResult.Rows[i][10].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.Sec80E = dataResult.dtResult.Rows[i][11].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.Sec80EE = dataResult.dtResult.Rows[i][12].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.Sec80EEB = dataResult.dtResult.Rows[i][13].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.Sec80G = dataResult.dtResult.Rows[i][14].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.Sec80GG = dataResult.dtResult.Rows[i][15].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.Sec80U = dataResult.dtResult.Rows[i][16].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.Sec24 = dataResult.dtResult.Rows[i][17].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.Sec10 = dataResult.dtResult.Rows[i][18].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.Sec16 = dataResult.dtResult.Rows[i][19].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.PreviousEmployerSalary = dataResult.dtResult.Rows[i][20].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.Age = dataResult.dtResult.Rows[i][21].ToString().GetDefaultDBNull<int>();
                TentitiveTDS.TotalExamptAmount = dataResult.dtResult.Rows[i][22].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.TaxableAmount = dataResult.dtResult.Rows[i][23].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.StanderedDeduction = dataResult.dtResult.Rows[i][24].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.HECAmount = dataResult.dtResult.Rows[i][25].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.Surcharge = dataResult.dtResult.Rows[i][26].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.FinalTDSAmountYearly = dataResult.dtResult.Rows[i][27].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.FinalTDSAmountMonthly = dataResult.dtResult.Rows[i][28].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.PaidTax = dataResult.dtResult.Rows[i][29].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.RemainingTax = dataResult.dtResult.Rows[i][30].ToString().GetDefaultDBNull<decimal>();
                TentitiveTDS.FinancialYear = dataResult.dtResult.Rows[i][31].ToString().GetDefaultDBNull<int>();
                model.Add(TentitiveTDS);
                 
            }
            return model;
        }
    }
}
