using OneClickHSE.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class PaymentScreenController : Controller
    {
        NYFSEntities2 obj = new NYFSEntities2();

        //
        // GET: /PaymentScreen/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetPayments(int page, int pagesize)
        {
            var paymentCount = obj.Payments.Count();
            var skip = (page - 1) * pagesize;
            var payments = obj.Payments.OrderBy(x => x.PaymentKey).Skip(skip).Take(pagesize).ToList(); // for listing

            var paymentsModel = payments.Select(x => new PaymentModel()
            {
                PaymentKey = x.PaymentKey,
                 PaymentTo=(Int32)x.PaymentTo,
                 PaymentToClient=obj.People.Where(m=>m.PersonKey==x.PaymentTo).SingleOrDefault().PersonName,
              // PaymentTo=obj.People.Where(x=>x)
                PaymentCheckNumber = x.PaymentCheckNumber,
                PaymentTypeName = x.PaymentType.PaymentTypeName,
                PaymentNote = x.PaymentNote,
             
                PaymentAccountsModel = x.PaymentAccounts.Count() > 0 ? x.PaymentAccounts.Select(y => new PaymentAccountModel()
                {
                    PaymentAccountAmount = y.PaymentAccountAmount,
                    AccountName = y.Account.AccountName,
                  
                }).ToList() : null,
                PaymentProgramsModel = x.PaymentPrograms.Count() > 0 ? x.PaymentPrograms.Select(y => new PaymentProgramModel()
                {
                    PaymentProgramAmount = y.PaymentProgramAmount,
                    ProgramName = y.Program.ProgramName
                }).ToList() : null,

                PaymentTotalAmount = x.PaymentAccounts.Sum(z => z.PaymentAccountAmount) + x.PaymentPrograms.Sum(z => z.PaymentProgramAmount)
            }).ToList();

            var paymentTypes = obj.PaymentTypes.ToList(); // for payment type dll
            var paymentTypesModel = paymentTypes.Select(x => new PaymentTypeModel()
            {
                PaymentTypeKey = x.PaymentTypeKey,
                PaymentTypeName = x.PaymentTypeName
            }).ToList();

            var persons = obj.People.ToList(); // for persons dll
            var personsModel = persons.Select(x => new PersonModel()
            {
                PersonKey = x.PersonKey,
                PersonName = x.PersonName
            }).ToList();

            var accounts = obj.Accounts.ToList(); // for accounts dll
            var accountsModel = accounts.Select(x => new AccountModel()
            {
                AccountKey = x.AccountKey,
                AccountName = x.AccountName,
            }).ToList();

            var programs = obj.Programs.ToList(); // for program dll
            var programsModel = programs.Select(x => new ProgramModel()
            {
                ProgramKey = x.ProgramKey,
                ProgramName = x.ProgramName,
            }).ToList();

            return Json(new { Payments = paymentsModel, PaymentTypes = paymentTypesModel, Persons = personsModel, Accounts = accountsModel, Programs = programsModel, PaymentCount = paymentCount }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SavePayment(Payment payment)
        {
            if (payment.PaymentKey == 0)
            {
             obj.Payments.Add(payment);
             obj.SaveChanges();
            }
            else
            {
                var paymentObj = obj.Payments.SingleOrDefault(x => x.PaymentKey == payment.PaymentKey);
                if (paymentObj != null)
                {
                    paymentObj.PaymentDate = payment.PaymentDate;
                    paymentObj.PaymentCheckNumber = payment.PaymentCheckNumber;
                    paymentObj.PaymentTypeKey = payment.PaymentTypeKey;
                    paymentObj.PaymentTo = payment.PaymentTo;
                  
                    paymentObj.PaymentVendorInvoiceNumber = payment.PaymentVendorInvoiceNumber;
                    paymentObj.PaymentNote = payment.PaymentNote;
                    obj.SaveChanges();
                }
            }

            return Json(payment, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SavePerson(Person person)
        {
            obj.People.Add(person);
            obj.SaveChanges();
            return Json(person, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SavePaymentAccount(PaymentAccount paymentAccount)
        {
            obj.PaymentAccounts.Add(paymentAccount);
            obj.SaveChanges();
            return Json(paymentAccount, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SavePaymentProgram(PaymentProgram paymentProgram)
        {
            obj.PaymentPrograms.Add(paymentProgram);
            obj.SaveChanges();
            return Json(paymentProgram, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPayment(int paymentKay)
        {
            var payment = obj.Payments.SingleOrDefault(x => x.PaymentKey == paymentKay);
            //Accessing client name
            int client_id =Convert.ToInt32(payment.PaymentTo);
            PaymentModel paymentModel = new PaymentModel ();
            
                paymentModel.PaymentKey = payment.PaymentKey;
                paymentModel.PaymentDate = payment.PaymentDate;               
                paymentModel.PaymentCheckNumber = payment.PaymentCheckNumber;
                paymentModel.PaymentTypeKey =payment.PaymentTypeKey;
                paymentModel.PaymentTo =(Int32)payment.PaymentTo;
                //Accessing client name
                paymentModel.PaymentToClient = obj.People.Where(m => m.PersonKey==client_id).SingleOrDefault().PersonName;
                paymentModel.PaymentVendorInvoiceNumber = payment.PaymentVendorInvoiceNumber;
                paymentModel.PaymentNote = payment.PaymentNote;
                paymentModel.PaymentTypeName = payment.PaymentType.PaymentTypeName;
                paymentModel.PaymentAccountsModel = payment.PaymentAccounts.Select(y => new PaymentAccountModel()
                {
                    PaymentAccountKey = y.PaymentAccountKey,
                    PaymentAccountAmount =y.PaymentAccountAmount,
                    AccountName = y.Account.AccountName
                }).ToList();
                paymentModel.PaymentProgramsModel = payment.PaymentPrograms.Select(y => new PaymentProgramModel()
                {
                    PaymentProgramKey = y.PaymentProgramKey,
                    PaymentProgramAmount = y.PaymentProgramAmount,
                    ProgramName = y.Program.ProgramName
                }).ToList();

                return Json(paymentModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Deletepayment(int paymentKey)
        {
            var payment = obj.Payments.SingleOrDefault(x => x.PaymentKey == paymentKey);
            if (payment != null)
            {

                var paymentAccounts = obj.PaymentAccounts.Where(y=>y.PaymentKey ==paymentKey).ToList();
                if (paymentAccounts.Count > 0)
                    obj.PaymentAccounts.RemoveRange(paymentAccounts);

                var paymentPrograms = obj.PaymentPrograms.Where(y => y.PaymentKey == paymentKey).ToList();
                if (paymentPrograms.Count > 0)
                    obj.PaymentPrograms.RemoveRange(paymentPrograms);

                obj.Payments.Remove(payment);
                obj.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeletePaymentAccount(int paymentAccountKey)
        {
            var paymentAccount = obj.PaymentAccounts.SingleOrDefault(x => x.PaymentAccountKey == paymentAccountKey);
            if (paymentAccount != null)
            {
                obj.PaymentAccounts.Remove(paymentAccount);
                obj.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeletePaymentProgram(int paymentProgramKey)
        {
            var paymentProgram = obj.PaymentPrograms.SingleOrDefault(x => x.PaymentProgramKey == paymentProgramKey);
            if (paymentProgram != null)
            {
                obj.PaymentPrograms.Remove(paymentProgram);
                obj.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}