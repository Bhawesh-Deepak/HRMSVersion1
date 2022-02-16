using HRMS.Core.ReqRespVm.Response.Attendance;
using HRMS.Core.ReqRespVm.Response.Reporting;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Reporting
{
    public class AttendanceDayWiseReportController : Controller
    {
        private readonly IDapperRepository<AttendanceReportParams> _IEmployeeAttendanceRepository;

        public AttendanceDayWiseReportController(IDapperRepository<AttendanceReportParams> attendanceDapper)
        {
            _IEmployeeAttendanceRepository = attendanceDapper;
        }
        public async Task<IActionResult> Index()
        {
            var sqlParams = new AttendanceReportParams()
            {
                DateMonthId = 4,
                DateYearId = 2020
            };

            var response = _IEmployeeAttendanceRepository.GetAll<AttendanceReportVm>(SqlQuery.GetEmployeeAttendanceDetails, sqlParams);

            var models = new List<AttendanceVm>();
            response.ForEach(data =>
            {
                var model = new AttendanceVm();
                model.EmployeeName = data.EmployeeName;
                model.EmployeeCode = data.EmpCode;
                model.EmployeeLevel = data.Level;
                model.Month = data.DateMonth;
                model.Year = data.DateYear;
                model.TotalDays = data.TotalDays;
                model.LOPDays = data.LOPDays;
                model.PresentDays = data.PresentDays;

                IDictionary<DateTime, string> attendanceDates = new Dictionary<DateTime, string>();

                for (int i = 1; i <= data.TotalDays; i++)
                {
                    if (i <= data.PresentDays)
                    {
                        attendanceDates.Add(data.StartDate.AddDays(i-1), "Present");
                    }
                    else
                    {
                        attendanceDates.Add(data.StartDate.AddDays(i - 1), "Absent");
                    }
                    model.DatWiseAttendance = attendanceDates;
                }

                models.Add(model);

            });

            return Json(models);
        }
    }
}
