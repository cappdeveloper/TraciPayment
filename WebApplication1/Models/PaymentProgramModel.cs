using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class PaymentProgramModel
    {
        public int PaymentProgramKey { get; set; }
        public Nullable<int> ProgramKey { get; set; }
        public Nullable<decimal> PaymentProgramAmount { get; set; }
        public string PaymentProgramNote { get; set; }
        public string ProgramName { get; set; }
    }
}