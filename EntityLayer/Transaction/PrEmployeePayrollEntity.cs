using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Transaction
{
    public class PrEmployeePayrollEntity
    {
        public string? prYyyMm { get; set; }
        public string? prEmpNo { get; set; }
        public string? prDesignation { get; set; }
        public int prDaysPresent { get; set; }
        public int prDaysAbsent { get; set; }
        public double prBasic { get; set; }
        public double prHra { get; set; }
        public double prConv { get; set; }
        public double prDa { get; set; }
        public double prTds { get; set; }
        public double prEsi { get; set; }
        public double prTotEarnings { get; set; }
        public double prTotDedu { get; set; }
        public double prNetPayable { get; set; }
        public string? ehCrBy { get; set; }
        public DateTime ehCrDt { get; set; }
        public string? ehUpBy { get; set; }
        public DateTime ehUpDt { get; set; }
    }
}
