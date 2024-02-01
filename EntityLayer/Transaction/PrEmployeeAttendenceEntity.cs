using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Transaction
{
    public class PrEmployeeAttendenceEntity
    {
        public string? attEmpNo { get; set; }
        public string? attYyyyMm { get; set; }
        public int attDaysPresent { get; set; }
        public int attDaysAbsent { get; set; }
        public string? attCrBy { get; set; }
        public DateTime attCrDt { get; set; }
        public string? attUpBy { get; set; }
        public DateTime attUpDt { get; set; }
        public string? month {  get; set; }  
    }
}
