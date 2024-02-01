using EntityLayer.Std;
using EntityLayer.Transaction;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace HrPayrollProcessingCore
{
    public class AttendenceViewModel
    {
        public PrEmployeeAttendenceEntity Attendence { get; set; }
        public List<EmpNoDropdown> EmpNoList { get; set; }
        public List<DropdownSelect> MonthList { get; set; }
        public List<DropdownSelect> statusList { get; set; }
        public string? EditAttPage { get; set; }
        public bool? PayrollStatus { get; set; }
        public List<SelectListItem> YearList { get; set; }
        public List<DdlEmpNo> EmployeeList { get; set; }

    }
}
