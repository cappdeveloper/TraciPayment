using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class PaymentModel
    {
        public int PaymentKey { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public  int PaymentTo { get; set; }
        public string PaymentToClient { get; set; }
        public string PaymentCheckNumber { get; set; }
        public Nullable<int> PaymentTypeKey { get; set; }
        public Nullable<decimal> PaymentAmount { get; set; }
        public string PaymentNote { get; set; }
        public Nullable<byte> PaymentReconciled { get; set; }
        public string PaymentVendorInvoiceNumber { get; set; }
        public string PaymentTypeName { get; set; }
        public List<PaymentAccountModel> PaymentAccountsModel { get; set; }
        public List<PaymentProgramModel> PaymentProgramsModel { get; set; }
    }
}