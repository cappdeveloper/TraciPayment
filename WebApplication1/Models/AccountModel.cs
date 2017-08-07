using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class AccountModel
    {
        public int AccountKey { get; set; }
        public string AccountName { get; set; }
        public string GLAccountCode { get; set; }
        public Nullable<int> AccountTypeKey { get; set; }
        public string AccountNote { get; set; }
    }
}