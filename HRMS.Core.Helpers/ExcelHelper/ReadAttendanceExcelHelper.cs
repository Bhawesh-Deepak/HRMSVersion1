﻿using HRMS.Core.Entities.Payroll;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;


namespace HRMS.Core.Helpers.ExcelHelper
{
    public class ReadAttendanceExcelHelper
    {
        public IEnumerable<EmployeeAttendance> GetAttendanceDetails(IFormFile inputFile)
        {
            var dataResult = ReadExcelDataHelper.GetDataTableFromExcelFile(inputFile);
            var models = new List<EmployeeAttendance>();

            for (int i = 1; i < dataResult.dtResult.Rows.Count; i++)
            {
                var model = new EmployeeAttendance();
                model.DateMonth = Convert.ToInt32(dataResult.dtResult.Rows[i][0]);
                model.DateYear = Convert.ToInt32(dataResult.dtResult.Rows[i][1]);
                model.EmployeeCode = Convert.ToString(dataResult.dtResult.Rows[i][2]);
                model.LOPDays = Convert.ToDecimal(dataResult.dtResult.Rows[i][3]);
                model.TotalDays = DateTime.DaysInMonth(model.DateYear, model.DateMonth);
                model.PresentDays = Convert.ToDecimal(model.TotalDays - model.LOPDays);
                model.FinancialYear = 1;
                models.Add(model);
            }

            return models;
        }
        public IEnumerable<EmployeeAttendance> GetAttendanceDetailsBackData(IFormFile inputFile1)
        {
            var dataResult = ReadExcelDataHelper.GetDataTableFromExcelFile(inputFile1);
            var models = new List<EmployeeAttendance>();

            for (int i = 1; i < dataResult.dtResult.Rows.Count; i++)
            {
                var model = new EmployeeAttendance();
                model.DateMonth = Convert.ToInt32(dataResult.dtResult.Rows[i][0]);
                model.DateYear = Convert.ToInt32(dataResult.dtResult.Rows[i][1]);
                model.EmployeeCode = Convert.ToString(dataResult.dtResult.Rows[i][2]);
                model.LOPDays = Convert.ToDecimal(dataResult.dtResult.Rows[i][3]);
                model.PresentDays = Convert.ToDecimal(dataResult.dtResult.Rows[i][4]);
                model.TotalDays = DateTime.DaysInMonth(model.DateYear, model.DateMonth);
               // model.PresentDays = model.TotalDays - model.LOPDays;
                model.FinancialYear = Convert.ToInt32(dataResult.dtResult.Rows[i][5]);
                models.Add(model);
            }

            return models;
        }

    }
}
