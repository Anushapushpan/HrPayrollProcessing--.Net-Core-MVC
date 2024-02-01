using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Master
{
    public class DepartmentMasterEntity
    {
        public string? deptNo { get; set; }
        public string? deptName { get; set; }
        public string? deptCrBy { get; set; }
        public DateTime deptCrDt { get; set; }
        public string? deptUpBy { get; set; }
        public DateTime deptUpDt { get; set; }
    }
}
