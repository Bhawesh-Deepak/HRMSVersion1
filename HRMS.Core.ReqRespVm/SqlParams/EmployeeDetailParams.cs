namespace HRMS.Core.ReqRespVm.SqlParams
{
    public class EmployeeDetailParams
    {
        public string LeagalEntity { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string PAndLHeadName { get; set; }
        public string JoiningDate { get; set; }
        public string Location { get; set; }
        public bool IsActive { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }

    }
}
