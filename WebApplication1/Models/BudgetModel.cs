using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class BudgetModel
    {
        public int BudgetKey { get; set; }
        public Nullable<int> BudgetProgramKey { get; set; }
        public Nullable<int> BudgetTermKey { get; set; }
        public Nullable<int> BudgetAccountKey { get; set; }
        public Nullable<decimal> BudgetAmount { get; set; }
        public string BudgetNote { get; set; }
        public virtual AccountModel Account { get; set; }
        public virtual ProgramModel Program { get; set; }
        public virtual TermModel Term { get; set; }
    }
}