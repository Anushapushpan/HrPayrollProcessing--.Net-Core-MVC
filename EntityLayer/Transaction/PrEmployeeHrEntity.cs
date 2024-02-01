using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Transaction
{
    public class PrEmployeeHrEntity
    {
        public string? ehEmpNo { get; set; }
        public string? ehDesignation { get; set; }
        public string? ehGrade { get; set; }
        public double ehBasic { get; set; }
        public double ehHra { get; set; }
        public double ehConv { get; set; }
        public double ehDa { get; set; }
        public double ehTds { get; set; }
        public double ehEsi { get; set; }
        public string? ehCrBy { get; set; }
        public DateTime ehCrDt { get; set; }
        public string? ehUpBy { get; set; }
        public DateTime ehUpDt { get; set; }
    }
}
