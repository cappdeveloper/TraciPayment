using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class PaymentAccountModel
    {
        public int PaymentAccountKey { get; set; }
        public Nullable<int> AccountKey { get; set; }
        public Nullable<decimal> PaymentAccountAmount { get; set; }
        public string PaymentAccountNote { get; set; }
        public string AccountName { get; set; }
    }
}