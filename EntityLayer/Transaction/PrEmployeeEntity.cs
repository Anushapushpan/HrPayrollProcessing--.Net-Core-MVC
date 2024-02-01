using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Transaction
{
    public class PrEmployeeEntity
    {
        public string? empNo { get; set; }
        public string? empPwd { get; set; }
       
        public string? empName { get; set; }
        public DateTime empDob { get; set; }
        public DateTime empJoinDate { get; set; }
        public double empSalary { get; set; }
        public string? empDeptNo { get; set; }
        public string? empMgrNo { get; set; }
        public string? empStatus { get; set; }
        public string? empActiveYn { get; set; }
        public bool isEmpActiveYn { get => empActiveYn == "Y"; set => empActiveYn = (value) ? "Y" : "N"; }
        public string? empCrBy { get; set; }
        public DateTime empCrDt { get; set; }
        public string? empUpBy { get; set; }
        public DateTime empUpDt { get; set; }
    }
}
