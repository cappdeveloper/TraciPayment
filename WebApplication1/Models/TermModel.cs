using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class TermModel
    {
        public int TermKey { get; set; }
        public string TermName { get; set; }
        public Nullable<System.DateTime> TermStartDate { get; set; }
        public Nullable<System.DateTime> TermEndDate { get; set; }
        public string TermNote { get; set; }
    
    }
}