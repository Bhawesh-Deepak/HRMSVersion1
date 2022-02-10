using HRMS.Core.Entities.Payroll;
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
    public class ReadIncrementExcelHelper
    {
        public EmployeeSalaryVm GetEmployeeIncrementComponent(IFormFile inputFile)
        {

            var dataResult = ReadExcelDataHelper.GetDataTableFromExcelFile(inputFile);
            var columnresult = dataResult.dtResult.Columns;
            var model = new EmployeeSalaryVm();
            var salaryModels = new List<EmployeeSalary>();
            var ctccomponentModels = new List<EmployeeCtcComponent>();

            for (int i = 1; i < dataResult.dtResult.Rows.Count; i++)
            {
                var empSalaryModel = new EmployeeSalary();
                empSalaryModel.EmpCode = dataResult.dtResult.Rows[i][0].ToString().GetDefaultDBNull<string>();
                empSalaryModel.StartDate = dataResult.dtResult.Rows[i][1].ToString().GetDefaultDBNull<DateTime>();
                empSalaryModel.CTC = dataResult.dtResult.Rows[i][2].ToString().GetDefaultDBNull<decimal>();
                empSalaryModel.CreatedDate = DateTime.Now;
                salaryModels.Add(empSalaryModel);
                List<int> ComponentID = new List<int>();
                List<decimal> ComponentValue = new List<decimal>();
                //-------------Start Earning------------------------------------------------
                ComponentValue.Add(dataResult.dtResult.Rows[i][3].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(1);
                ComponentValue.Add(dataResult.dtResult.Rows[i][3].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(2);

                ComponentValue.Add(dataResult.dtResult.Rows[i][4].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(3);
                ComponentValue.Add(dataResult.dtResult.Rows[i][4].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(4);

                ComponentValue.Add(dataResult.dtResult.Rows[i][5].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(5);
                ComponentValue.Add(dataResult.dtResult.Rows[i][5].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(6);

                ComponentValue.Add(dataResult.dtResult.Rows[i][6].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(7);
                ComponentValue.Add(dataResult.dtResult.Rows[i][6].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(8);

                ComponentValue.Add(dataResult.dtResult.Rows[i][7].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(9);
                ComponentValue.Add(dataResult.dtResult.Rows[i][7].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(10);

                ComponentValue.Add(dataResult.dtResult.Rows[i][8].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(11);
                ComponentValue.Add(dataResult.dtResult.Rows[i][8].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(12);

                ComponentValue.Add(dataResult.dtResult.Rows[i][9].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(13);
                ComponentValue.Add(dataResult.dtResult.Rows[i][9].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(14);

                ComponentValue.Add(dataResult.dtResult.Rows[i][10].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(15);
                ComponentValue.Add(dataResult.dtResult.Rows[i][10].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(16);

                ComponentValue.Add(dataResult.dtResult.Rows[i][11].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(17);
                ComponentValue.Add(dataResult.dtResult.Rows[i][11].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(18);

                ComponentValue.Add(dataResult.dtResult.Rows[i][12].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(19);
                ComponentValue.Add(dataResult.dtResult.Rows[i][12].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(20);

                ComponentValue.Add(dataResult.dtResult.Rows[i][13].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(21);
                ComponentValue.Add(dataResult.dtResult.Rows[i][13].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(22);

                ComponentValue.Add(dataResult.dtResult.Rows[i][14].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(23);
                ComponentValue.Add(dataResult.dtResult.Rows[i][14].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(24);

                ComponentValue.Add(dataResult.dtResult.Rows[i][15].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(25);
                ComponentValue.Add(dataResult.dtResult.Rows[i][15].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(26);

                ComponentValue.Add(dataResult.dtResult.Rows[i][16].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(27);
                ComponentValue.Add(dataResult.dtResult.Rows[i][16].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(28);
                //---------------------------NON-CTC --------------------------------------------------
                ComponentValue.Add(0);
                ComponentID.Add(29);
                ComponentValue.Add(0);
                ComponentID.Add(30);
                ComponentValue.Add(0);
                ComponentID.Add(31);
                ComponentValue.Add(0);
                ComponentID.Add(32);
                ComponentValue.Add(0);
                ComponentID.Add(33);
                ComponentValue.Add(0);
                ComponentID.Add(34);
                ComponentValue.Add(0);
                ComponentID.Add(35);
                ComponentValue.Add(0);
                ComponentID.Add(36);
                ComponentValue.Add(0);
                ComponentID.Add(37);
                ComponentValue.Add(0);
                ComponentID.Add(38);
                ComponentValue.Add(0);
                ComponentID.Add(39);
                ComponentValue.Add(0);
                ComponentID.Add(40);
                ComponentValue.Add(0);
                ComponentID.Add(41);
                ComponentValue.Add(0);
                ComponentID.Add(42);
                //---------------------------End Earning ------Start Deduction -----------------------------------------
                ComponentValue.Add(dataResult.dtResult.Rows[i][17].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(43);
                ComponentValue.Add(dataResult.dtResult.Rows[i][17].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(44);

                ComponentValue.Add(dataResult.dtResult.Rows[i][18].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(45);
                ComponentValue.Add(dataResult.dtResult.Rows[i][18].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(46);
                //----------------------------NON CTC-----------------------------
                ComponentValue.Add(0);
                ComponentID.Add(47);
                ComponentValue.Add(0);
                ComponentID.Add(48);
                ComponentValue.Add(0);
                ComponentID.Add(49);
                ComponentValue.Add(0);
                ComponentID.Add(50);
                ComponentValue.Add(0);
                ComponentID.Add(51);
                ComponentValue.Add(0);
                ComponentID.Add(52);
                ComponentValue.Add(0);
                ComponentID.Add(53);
                ComponentValue.Add(0);
                ComponentID.Add(54);
                ComponentValue.Add(0);
                ComponentID.Add(55);
                ComponentValue.Add(0);
                ComponentID.Add(56);
                ComponentValue.Add(0);
                ComponentID.Add(57);
                ComponentValue.Add(0);
                ComponentID.Add(58);
                ComponentValue.Add(0);
                ComponentID.Add(59);
                ComponentValue.Add(dataResult.dtResult.Rows[i][19].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(60);
                ComponentValue.Add(dataResult.dtResult.Rows[i][19].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(61);
                ComponentValue.Add(0);
                ComponentID.Add(62);
                ComponentValue.Add(0);
                ComponentID.Add(63);
                ComponentValue.Add(dataResult.dtResult.Rows[i][20].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(64);
                ComponentValue.Add(dataResult.dtResult.Rows[i][20].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(65);
                ComponentValue.Add(dataResult.dtResult.Rows[i][21].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(66);
                ComponentValue.Add(dataResult.dtResult.Rows[i][21].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(67);
                ComponentValue.Add(dataResult.dtResult.Rows[i][22].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(68);
                ComponentValue.Add(dataResult.dtResult.Rows[i][22].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(69);
                ComponentValue.Add(0);
                ComponentID.Add(70);
                for (int J = 0; J < ComponentValue.Count(); J++)
                {
                    var employeectccomponent = new EmployeeCtcComponent();
                    employeectccomponent.EmployeeSalaryId = 0;
                    employeectccomponent.EmpCode = dataResult.dtResult.Rows[i][0].ToString().GetDefaultDBNull<string>();
                    employeectccomponent.ComponentId = ComponentID[J];
                    employeectccomponent.ComponentValue = ComponentValue[J];
                    ctccomponentModels.Add(employeectccomponent);
                }
            }
            model.EmployeeSalaryDetails = salaryModels;
            model.EmployeeCtcComponentDetails = ctccomponentModels;
            return model;
        }
    }
}
