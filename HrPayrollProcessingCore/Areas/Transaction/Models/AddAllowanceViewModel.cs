using EntityLayer.Std;
using EntityLayer.Transaction;

namespace HrPayrollProcessingCore
{
    public class AddAllowanceViewModel
    {
        public PrEmployeeHrEntity PrEmployeeHrEntity { get; set; }
        public List<DropdownSelect> DesignationList { get; set; }
        public List<DropdownSelect> GradeList { get; set; }
        public string? CurrentAllowancePage { get; set; }
    }
}
