using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Helpers.ExcelHelper
{
    public class ReadUtilityExcelHelper
    {
        public List<ReadUtilityExcelDataVM> GetUtilityDetails(IFormFile inputFile)
        {
            var dataResult = ReadExcelDataHelper.GetDataTableFromExcelFile(inputFile);
            var utilityModels = new List<ReadUtilityExcelDataVM>();
            for (int i = 1; i < dataResult.dtResult.Rows.Count; i++)
            {
                var utility = new ReadUtilityExcelDataVM();
                utility.EmpCode = dataResult.dtResult.Rows[i][0].ToString().GetDefaultDBNull<string>();
                utility.ResponseValue = dataResult.dtResult.Rows[i][1].ToString().GetDefaultDBNull<string>();
                utilityModels.Add(utility);
            }
            return utilityModels;
        }
        public List<ReadUtilityExcelDataVM> GetUtilityDetailsDate(IFormFile inputFile)
        {
            var dataResult = ReadExcelDataHelper.GetDataTableFromExcelFile(inputFile);
            var utilityModels = new List<ReadUtilityExcelDataVM>();
            for (int i = 1; i < dataResult.dtResult.Rows.Count; i++)
            {
                var utility = new ReadUtilityExcelDataVM();
                utility.EmpCode = dataResult.dtResult.Rows[i][0].ToString().GetDefaultDBNull<string>();
                utility.ResponseValueDate = dataResult.dtResult.Rows[i][1].ToString().GetDefaultDBNull<DateTime>();
                utilityModels.Add(utility);
            }
            return utilityModels;
        }
        public List<ReadUtilityExcelDataVM> GetUtilityDetailsInt(IFormFile inputFile)
        {
            var dataResult = ReadExcelDataHelper.GetDataTableFromExcelFile(inputFile);
            var utilityModels = new List<ReadUtilityExcelDataVM>();
            for (int i = 1; i < dataResult.dtResult.Rows.Count; i++)
            {
                var utility = new ReadUtilityExcelDataVM();
                utility.EmpCode = dataResult.dtResult.Rows[i][0].ToString().GetDefaultDBNull<string>();
                utility.ResponseValueInt = dataResult.dtResult.Rows[i][1].ToString().GetDefaultDBNull<Int32>();
                utilityModels.Add(utility);
            }
            return utilityModels;
        }

    }
}
