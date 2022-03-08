using HRMS.Core.Entities.LeadManagement;
using HRMS.Core.Entities.Master;
using HRMS.Core.Entities.Organisation;
using HRMS.Core.Entities.Payroll;
using HRMS.Core.Entities.Posting;
using HRMS.Core.Entities.UserManagement;
using HRMS.Core.Entities.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using HRMS.Core.Entities.Talent;

namespace HRMS.Core.Entities.Common
{
    /// <summary>
    /// This is the context class which is used to connect to database
    /// </summary>
    public class HRMSContext : DbContext
    {
        private readonly string _connectionString;

        public HRMSContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
        }

        public HRMSContext(IConfiguration configuration, string dbName)
        {
            _connectionString = configuration.GetSection($"ConnectionStrings:{dbName}").Value;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().Property("CreatedDate").HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<Company>().Property("CreatedDate").HasDefaultValue(DateTime.Now);
        }

        #region Master

        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<SalaryHeads> SalaryHeads { get; set; }
        public virtual DbSet<Designation> Designations { get; set; }
        public virtual DbSet<EmployeeAttendance> EmployeeAttendances { get; set; }
        public virtual DbSet<EmployeeDetail> EmployeeDetails { get; set; }
        public virtual DbSet<CustomerLeadDetail> CustomerLeadDetails { get; set; }
        public virtual DbSet<RoleMaster> RoleMasters { get; set; }
        public virtual DbSet<ModuleMaster> ModuleMasters { get; set; }
        public virtual DbSet<SubModuleMaster> SubModuleMasters { get; set; }
        public virtual DbSet<MenuChildNode> MenuChildNodes { get; set; }
        public virtual DbSet<RoleAccess> RoleAccesses { get; set; }
        public virtual DbSet<AuthenticateUser> AuthenticateUsers { get; set; }
        public virtual DbSet<CustomerDetail> CustomerDetails { get; set; }
        public virtual DbSet<LeadType> LeadTypes { get; set; }
        public virtual DbSet<CustomerSecondryDetail> CustomerSecondryDetails { get; set; }
        public virtual DbSet<CustomerLead> CustomerLeads { get; set; }
        public virtual DbSet<CustomerCallingDetails> CustomerCallingDetails { get; set; }
        public virtual DbSet<CustomerLeadCloserForm> CustomerLeadCloserForms { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<LegalEntity> LegalEntitys { get; set; }
        public virtual DbSet<CompanyHolidays> CompanyHolidays { get; set; }
        public virtual DbSet<StateMaster> StateMasters { get; set; }
        public virtual DbSet<EmployeeType> EmployeeTypes { get; set; }
        public virtual DbSet<LocationType> LocationTypes { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<PAndLMaster> PAndLMasters { get; set; }
        public virtual DbSet<RegionMaster> RegionMasters { get; set; }
        public virtual DbSet<CompanyNews> CompanyNews { get; set; }
        public virtual DbSet<Shift> Shifts { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<CompanyPolicy> CompanyPolicies { get; set; }
        public virtual DbSet<AssetsCategory> AssetsCategories { get; set; }
        public virtual DbSet<EmployeeAssets> EmployeeAssets { get; set; }
        public virtual DbSet<CurrentOpening> CurrentOpenings { get; set; }
        public virtual DbSet<ReferCandidate> ReferCandidates { get; set; }
        public virtual DbSet<AssesmentYear> AssesmentYears { get; set; }
        public virtual DbSet<EmployeeArrears> EmployeeArrearss { get; set; }
        public virtual DbSet<NewsAndUpdate> NewsAndUpdates { get; set; }
        public virtual DbSet<CtcComponentDetail> CtcComponentDetails { get; set; }
        public virtual DbSet<EmployeeSalary> EmployeeSalarys { get; set; }
        public virtual DbSet<EmployeeCtcComponent> EmployeeCtcComponents { get; set; }
        public virtual DbSet<EmployeeNonCTC> EmployeeNonCTCs { get; set; }
        public virtual DbSet<AdminEmployeeDetail> AdminEmployeeDetails { get; set; }
        public virtual DbSet<EmployeeSalaryPosted> EmployeeSalaryPosteds { get; set; }
        public virtual DbSet<EmployeeTDSSummery> EmployeeTDSSummerys { get; set; }
        public virtual DbSet<PaidRegister> PaidRegisters { get; set; }
        public virtual DbSet<LeaveType> LeaveTypes { get; set; }
        public virtual DbSet<LeaveAllocation> LeaveAllocations { get; set; }
        public virtual DbSet<EmployeeReimbursement> EmployeeReimbursements { get; set; }
        public virtual DbSet<CandidateDetail> CandidateDetails { get; set; }
        public virtual DbSet<LearningAndDevelopment> LearningAndDevelopments { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseMode> CourseModes { get; set; }
        public virtual DbSet<LAndDHour> LAndDHours { get; set; }
        public virtual DbSet<EmployeeTermination> EmployeeTerminations { get; set; }
        public virtual DbSet<AwardType> AwardTypes { get; set; }
        public virtual DbSet<Award> Awards { get; set; }
        public virtual DbSet<PerformanceIndication> PerformanceIndications { get; set; }
        public virtual DbSet<EmployeeTentitiveTDS> EmployeeTentitiveTDSs { get; set; }
        public virtual DbSet<EmployeeForm16Detail> EmployeeForm16Details { get; set; }
        #endregion
    }
}
