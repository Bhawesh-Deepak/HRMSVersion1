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
    public class ReadEmployeeSalaryExcelHelper
    {
        public EmployeeSalaryVm GetEmployeeSalaryDetails(IFormFile inputFile)
        {
            var dataResult = ReadExcelDataHelper.GetDataTableFromExcelFile(inputFile);

            var model = new EmployeeSalaryVm();
            var employeeModels = new List<EmployeeDetail>();
            var salaryModels = new List<EmployeeSalary>();
            var ctccomponentModels = new List<EmployeeCtcComponent>();


            for (int i = 2; i < dataResult.dtResult.Rows.Count; i++)
            {
                var empModel = new EmployeeDetail();

                empModel.Salutation = dataResult.dtResult.Rows[i][0].ToString().GetDefaultDBNull<string>();
                empModel.EmployeeName = dataResult.dtResult.Rows[i][1].ToString().GetDefaultDBNull<string>();
                empModel.EmpCode = dataResult.dtResult.Rows[i][2].ToString().GetDefaultDBNull<string>();
                empModel.JoiningDate =Convert.ToDateTime( dataResult.dtResult.Rows[i][3].ToString()).ToString("yyyy-MM-dd").GetDefaultDBNull<DateTime>();
                empModel.EmployementStatus = dataResult.dtResult.Rows[i][4].ToString().GetDefaultDBNull<string>();
                empModel.OfficeEmailId = dataResult.dtResult.Rows[i][5].ToString().GetDefaultDBNull<string>();
                empModel.DepartmentName = dataResult.dtResult.Rows[i][6].ToString().GetDefaultDBNull<string>();
                empModel.DesignationName = dataResult.dtResult.Rows[i][7].ToString().GetDefaultDBNull<string>();
                empModel.Location = dataResult.dtResult.Rows[i][8].ToString().GetDefaultDBNull<string>();
                empModel.LegalEntity = dataResult.dtResult.Rows[i][9].ToString().GetDefaultDBNull<string>();
                empModel.PAndLHeadName = dataResult.dtResult.Rows[i][10].ToString().GetDefaultDBNull<string>();
                empModel.SuperVisorCode = dataResult.dtResult.Rows[i][11].ToString().GetDefaultDBNull<string>();
                empModel.Level = dataResult.dtResult.Rows[i][12].ToString().GetDefaultDBNull<string>();
                empModel.PanCardNumber = dataResult.dtResult.Rows[i][13].ToString().GetDefaultDBNull<string>();
                empModel.PassportNumber = dataResult.dtResult.Rows[i][14].ToString().GetDefaultDBNull<string>();
                empModel.AadharCardNumber = dataResult.dtResult.Rows[i][15].ToString().GetDefaultDBNull<string>();
                empModel.BankAccountName = dataResult.dtResult.Rows[i][16].ToString().GetDefaultDBNull<string>();
                empModel.BankAccountNumber= dataResult.dtResult.Rows[i][17].ToString().GetDefaultDBNull<string>();
                empModel.BankName = dataResult.dtResult.Rows[i][18].ToString().GetDefaultDBNull<string>();
                empModel.IFSCCode = dataResult.dtResult.Rows[i][19].ToString().GetDefaultDBNull<string>();
                empModel.PreviousOrganisation= dataResult.dtResult.Rows[i][20].ToString().GetDefaultDBNull<string>();
                empModel.WorkExprience = dataResult.dtResult.Rows[i][21].ToString().GetDefaultDBNull<string>();
                empModel.EducationalQualification = dataResult.dtResult.Rows[i][22].ToString().GetDefaultDBNull<string>();
                empModel.InstituteName = dataResult.dtResult.Rows[i][23].ToString().GetDefaultDBNull<string>();
                empModel.ConfirmationDate = dataResult.dtResult.Rows[i][24].ToString().GetDefaultDBNull<DateTime>();
                empModel.RecruitmentSource = dataResult.dtResult.Rows[i][25].ToString().GetDefaultDBNull<string>();
                empModel.RecruitmentName = dataResult.dtResult.Rows[i][26].ToString().GetDefaultDBNull<string>();
                empModel.FatherName = dataResult.dtResult.Rows[i][27].ToString().GetDefaultDBNull<string>();
                empModel.PersonalEmailId = dataResult.dtResult.Rows[i][28].ToString().GetDefaultDBNull<string>();
                empModel.ContactNumber = dataResult.dtResult.Rows[i][29].ToString().GetDefaultDBNull<string>();
                empModel.DateOfBirth = dataResult.dtResult.Rows[i][30].ToString().GetDefaultDBNull<DateTime>();
                empModel.CurrentAddress = dataResult.dtResult.Rows[i][31].ToString().GetDefaultDBNull<string>();
                empModel.PermanentAddress = dataResult.dtResult.Rows[i][32].ToString().GetDefaultDBNull<string>();
                empModel.BiometricCode = dataResult.dtResult.Rows[i][33].ToString().GetDefaultDBNull<string>();
                empModel.BloodGroup = dataResult.dtResult.Rows[i][34].ToString().GetDefaultDBNull<string>();
                empModel.Gender = dataResult.dtResult.Rows[i][35].ToString().GetDefaultDBNull<string>();
                empModel.MaritalStatus = dataResult.dtResult.Rows[i][36].ToString().GetDefaultDBNull<string>();
                empModel.Region = dataResult.dtResult.Rows[i][37].ToString().GetDefaultDBNull<string>();
                empModel.PIPStartDate = dataResult.dtResult.Rows[i][38].ToString().GetDefaultDBNull<string>();
                empModel.PIPEndDate = dataResult.dtResult.Rows[i][39].ToString().GetDefaultDBNull<string>();
                empModel.PIP = dataResult.dtResult.Rows[i][40].ToString().GetDefaultDBNull<string>();
                empModel.WhatsAppNumber = dataResult.dtResult.Rows[i][41].ToString().GetDefaultDBNull<string>();
                empModel.NoticePeriod = dataResult.dtResult.Rows[i][42].ToString().GetDefaultDBNull<string>();
                empModel.SpouceName = dataResult.dtResult.Rows[i][43].ToString().GetDefaultDBNull<string>();
                empModel.DateOfMairrage = dataResult.dtResult.Rows[i][44].ToString().GetDefaultDBNull<DateTime>();
                empModel.EmergencyNumber = dataResult.dtResult.Rows[i][45].ToString().GetDefaultDBNull<string>();
                empModel.EmergencyRelationWithEmployee = dataResult.dtResult.Rows[i][46].ToString().GetDefaultDBNull<string>();
                empModel.UANNumber = dataResult.dtResult.Rows[i][47].ToString().GetDefaultDBNull<string>();
                empModel.ESICNew = dataResult.dtResult.Rows[i][48].ToString().GetDefaultDBNull<string>();
                empModel.LeaveSupervisor = dataResult.dtResult.Rows[i][49].ToString().GetDefaultDBNull<string>();
                empModel.IJPLocation = dataResult.dtResult.Rows[i][50].ToString().GetDefaultDBNull<string>();
                empModel.ShiftTiming = dataResult.dtResult.Rows[i][51].ToString().GetDefaultDBNull<string>();
                empModel.ConfirmationStatus = dataResult.dtResult.Rows[i][52].ToString().GetDefaultDBNull<string>();
                empModel.Nationality = dataResult.dtResult.Rows[i][53].ToString().GetDefaultDBNull<string>();
                empModel.PAndFBankAccountNumberx = dataResult.dtResult.Rows[i][54].ToString().GetDefaultDBNull<string>();
                empModel.ESICPreviousNumber = dataResult.dtResult.Rows[i][55].ToString().GetDefaultDBNull<string>();
                empModel.Induction = dataResult.dtResult.Rows[i][56].ToString().GetDefaultDBNull<string>();
                empModel.VISANumber = dataResult.dtResult.Rows[i][57].ToString().GetDefaultDBNull<string>();
                empModel.VISADate= dataResult.dtResult.Rows[i][58].ToString().GetDefaultDBNull<DateTime>();
                empModel.TaxFileNumber = dataResult.dtResult.Rows[i][59].ToString().GetDefaultDBNull<string>();
                empModel.SupernationAccountNumber = dataResult.dtResult.Rows[i][60].ToString().GetDefaultDBNull<string>();
                empModel.SwiftCode = dataResult.dtResult.Rows[i][61].ToString().GetDefaultDBNull<string>();
                empModel.RoutingCode = dataResult.dtResult.Rows[i][62].ToString().GetDefaultDBNull<string>();
                empModel.AlternateMobileNumber = dataResult.dtResult.Rows[i][63].ToString().GetDefaultDBNull<string>();
                empModel.BranchOfficeId = dataResult.dtResult.Rows[i][64].ToString().GetDefaultDBNull<string>();
                empModel.ExitDate = dataResult.dtResult.Rows[i][65].ToString().GetDefaultDBNull<DateTime>();
                empModel.HolidayGroupId = dataResult.dtResult.Rows[i][66].ToString().GetDefaultDBNull<string>();
                empModel.IsESICEligible = dataResult.dtResult.Rows[i][67].ToString().GetDefaultDBNull<string>();
                empModel.LandLineNumber = dataResult.dtResult.Rows[i][68].ToString().GetDefaultDBNull<string>();
                empModel.LeaveApprover1 = dataResult.dtResult.Rows[i][69].ToString().GetDefaultDBNull<string>();
                empModel.LeaveApprover2 = dataResult.dtResult.Rows[i][70].ToString().GetDefaultDBNull<string>();
                empModel.PTStateName = dataResult.dtResult.Rows[i][71].ToString().GetDefaultDBNull<string>();
                empModel.IsPFEligible = dataResult.dtResult.Rows[i][72].ToString().GetDefaultDBNull<int>();
                empModel.CreatedDate = DateTime.Now;
                employeeModels.Add(empModel);

                var empSalaryModel = new EmployeeSalary();
                empSalaryModel.CTC = dataResult.dtResult.Rows[i][73].ToString().GetDefaultDBNull<decimal>();
                empSalaryModel.EmpCode = dataResult.dtResult.Rows[i][2].ToString().GetDefaultDBNull<string>();
                empSalaryModel.StartDate = dataResult.dtResult.Rows[i][3].ToString().GetDefaultDBNull<DateTime>();
                empSalaryModel.CreatedDate = DateTime.Now;
                salaryModels.Add(empSalaryModel);

                List<int> ComponentID = new List<int>();
                List<decimal> ComponentValue = new List<decimal>();
                //-------------Start Earning------------------------------------------------
                ComponentValue.Add(dataResult.dtResult.Rows[i][74].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(1);
                ComponentValue.Add(dataResult.dtResult.Rows[i][74].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(2);

                ComponentValue.Add(dataResult.dtResult.Rows[i][75].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(3);
                ComponentValue.Add(dataResult.dtResult.Rows[i][75].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(4);

                ComponentValue.Add(dataResult.dtResult.Rows[i][76].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(5);
                ComponentValue.Add(dataResult.dtResult.Rows[i][76].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(6);

                ComponentValue.Add(dataResult.dtResult.Rows[i][77].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(7);
                ComponentValue.Add(dataResult.dtResult.Rows[i][77].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(8);

                ComponentValue.Add(dataResult.dtResult.Rows[i][78].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(9);
                ComponentValue.Add(dataResult.dtResult.Rows[i][78].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(10);

                ComponentValue.Add(dataResult.dtResult.Rows[i][79].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(11);
                ComponentValue.Add(dataResult.dtResult.Rows[i][79].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(12);

                ComponentValue.Add(dataResult.dtResult.Rows[i][80].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(13);
                ComponentValue.Add(dataResult.dtResult.Rows[i][80].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(14);

                ComponentValue.Add(dataResult.dtResult.Rows[i][81].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(15);
                ComponentValue.Add(dataResult.dtResult.Rows[i][81].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(16);

                ComponentValue.Add(dataResult.dtResult.Rows[i][82].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(17);
                ComponentValue.Add(dataResult.dtResult.Rows[i][82].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(18);

                ComponentValue.Add(dataResult.dtResult.Rows[i][83].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(19);
                ComponentValue.Add(dataResult.dtResult.Rows[i][83].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(20);

                ComponentValue.Add(dataResult.dtResult.Rows[i][84].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(21);
                ComponentValue.Add(dataResult.dtResult.Rows[i][84].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(22);

                ComponentValue.Add(dataResult.dtResult.Rows[i][85].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(23);
                ComponentValue.Add(dataResult.dtResult.Rows[i][85].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(24);

                ComponentValue.Add(dataResult.dtResult.Rows[i][86].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(25);
                ComponentValue.Add(dataResult.dtResult.Rows[i][86].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(26);

                ComponentValue.Add(dataResult.dtResult.Rows[i][87].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(27);
                ComponentValue.Add(dataResult.dtResult.Rows[i][87].ToString().GetDefaultDBNull<decimal>());
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
                ComponentValue.Add(dataResult.dtResult.Rows[i][88].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(43);
                ComponentValue.Add(dataResult.dtResult.Rows[i][88].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(44);

                ComponentValue.Add(dataResult.dtResult.Rows[i][89].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(45);
                ComponentValue.Add(dataResult.dtResult.Rows[i][89].ToString().GetDefaultDBNull<decimal>());
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
                ComponentValue.Add(dataResult.dtResult.Rows[i][89].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(60);
                ComponentValue.Add(dataResult.dtResult.Rows[i][89].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(61);
                ComponentValue.Add(0);
                ComponentID.Add(62);
                ComponentValue.Add(0);
                ComponentID.Add(63);
                ComponentValue.Add(dataResult.dtResult.Rows[i][90].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(64);
                ComponentValue.Add(dataResult.dtResult.Rows[i][90].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(65);
                ComponentValue.Add(dataResult.dtResult.Rows[i][91].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(66);
                ComponentValue.Add(dataResult.dtResult.Rows[i][91].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(67);
                ComponentValue.Add(dataResult.dtResult.Rows[i][92].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(68);
                ComponentValue.Add(dataResult.dtResult.Rows[i][92].ToString().GetDefaultDBNull<decimal>());
                ComponentID.Add(69);
                ComponentValue.Add(0);
                ComponentID.Add(70);
                for (int J = 0; J < ComponentValue.Count(); J++)
                {
                    var employeectccomponent = new EmployeeCtcComponent();
                    employeectccomponent.EmployeeSalaryId = 0;
                    employeectccomponent.EmpCode = dataResult.dtResult.Rows[i][2].ToString().GetDefaultDBNull<string>();
                    employeectccomponent.ComponentId = ComponentID[J];                     
                    employeectccomponent.ComponentValue = ComponentValue[J];
                    ctccomponentModels.Add(employeectccomponent);
                }
                //for (int J = 74; J < dataResult.dtResult.Columns.Count; J++)
                //{
                //    var employeectccomponent = new EmployeeCtcComponent();
                //    employeectccomponent.EmployeeSalaryId = 0;
                //    employeectccomponent.EmpCode = dataResult.dtResult.Rows[i][2].ToString().GetDefaultDBNull<string>();
                //    employeectccomponent.ComponentName = dataResult.dtResult.Rows[0][J].ToString().GetDefaultDBNull<string>();
                //    employeectccomponent.ComponentValue = dataResult.dtResult.Rows[i][J].ToString().GetDefaultDBNull<decimal>();
                //    ctccomponentModels.Add(employeectccomponent);
                //}
            }
            model.EmployeeDetails = employeeModels;
            model.EmployeeSalaryDetails = salaryModels;
            model.EmployeeCtcComponentDetails = ctccomponentModels;
            return model;
        }
        
    }
}
