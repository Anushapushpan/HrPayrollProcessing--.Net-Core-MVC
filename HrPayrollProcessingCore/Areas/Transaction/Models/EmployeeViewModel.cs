using EntityLayer.Transaction;
using EntityLayer.Std;

namespace HrPayrollProcessingCore
{
    public class EmployeeViewModel
    {
        public PrEmployeeEntity Employee { get; set; }
        public List<DeptDdlSelect> deptList { get; set; }
        public List<ManagerDdlSelect> mgrList { get; set; }
        public List<DropdownSelect> statusList { get; set; }
        public string? CurrentPage { get; set; }
        public string? AllowancePage { get; set; }
    }
}
