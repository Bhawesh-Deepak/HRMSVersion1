using HRMS.Core.Entities.Payroll;
using HRMS.Core.Helpers.CommonHelper;
using HRMS.Core.Helpers.ExcelHelper;
using HRMS.Core.ReqRespVm.RequestVm;
using HRMS.Core.ReqRespVm.SqlParams;
using HRMS.Services.Implementation.SqlConstant;
using HRMS.Services.Repository.GenericRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.UI.Controllers.Master
{
    public class EmployeeAttendanceController : Controller
    {

        private readonly IGenericRepository<EmployeeAttendance, int> _IEmployeeAttendanceRepository;
        private readonly IDapperRepository<AttendanceParams> _IEmployeeDapperRepository;

        public EmployeeAttendanceController(IGenericRepository<EmployeeAttendance, int> employeeAttendanceRepo,
            IDapperRepository<AttendanceParams> employeeDapperRepository)
        {
            _IEmployeeAttendanceRepository = employeeAttendanceRepo;
            _IEmployeeDapperRepository = employeeDapperRepository;
        }
        public IActionResult Index()
        {
            try
            {
                return View(ViewHelper.GetViewPathDetails("EmployeeAttendance", "EmployeeAttendanceCreate"));
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, $"controller name  is {nameof(EmployeeAttendanceController)} action name {nameof(UploadAttendance)}");
                return Json("Something wents wrong, Please contact admin !!!");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadAttendance(UploadExcelVm model)
        {
            try
            {
                var response = new ReadAttendanceExcelHelper().GetAttendanceDetails(model.UploadFile);
                response.ToList().ForEach(item =>
                {
                    var model = new AttendanceParams()
                    {
                        EmpCode = item.EmployeeCode,
                        LopDays = item.LOPDays,
                        MonthId = item.DateMonth,
                        YearId = item.DateYear
                    };

                    var uploadResponse = _IEmployeeDapperRepository
                    .Execute<AttendanceParams>(SqlQuery.UploadAttendance, model);
                });

                return Json($"Employee Attendance uploaded successfully !!");
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, $"controller name  is {nameof(EmployeeAttendanceController)} action name {nameof(UploadAttendance)}");
                return Json("Something wents wrong, Please contact admin !!!");
            }

        }
    }
}
