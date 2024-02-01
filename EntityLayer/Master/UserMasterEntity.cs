using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Master
{
    public class UserMasterEntity
    {
        public string? userId { get; set; }
        public string? userName { get; set; }
        public string? userPassword { get; set; }
        public string? userCrBy { get; set; }
        public DateTime userCrDt { get; set; }
        public string? userUpBy { get; set; }
        public DateTime userUpDt { get; set; }
        public string? userActiveYn { get; set; }
        public bool isUserActiveYn { get => userActiveYn == "Y"; set => userActiveYn = (value) ? "Y" : "N"; }
    }
}
