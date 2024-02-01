using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Master
{
    public class ErrorCodeMasterEntity
    {
        public string? errCode { get; set; }
        public string? errType { get; set; }
        public string? errDesc { get; set; }
        public string? errCrBy { get; set; }
        public DateTime errCrDt { get; set; }
        public string? errUpBy { get; set; }
        public DateTime errUpDt { get; set; }
    }
}
