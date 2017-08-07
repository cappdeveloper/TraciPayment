using OneClickHSE.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{

    public class PaymentScreenController : Controller
    {
        NYFSEntities1 obj = new NYFSEntities1();

        //
        // GET: /PaymentScreen/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetPaymentDllData()
        {
            var paymentTypes = obj.PaymentTypes.ToList();

            var paymentTypesModel = paymentTypes.Select(x => new PaymentTypeModel()
            {
                PaymentTypeKey = x.PaymentTypeKey,
                PaymentTypeName = x.PaymentTypeName
            }).ToList();

            var persons = obj.People.ToList();
            var personsModel = persons.Select(x => new PersonModel()
            {
                PersonKey = x.PersonKey,
                PersonName = x.PersonName
            }).ToList();

            var accounts = obj.Accounts.ToList();
            var accountsModel = accounts.Select(x => new AccountModel()
            {
                AccountKey = x.AccountKey,
                AccountName = x.AccountName,
            }).ToList();

            var programs = obj.Programs.ToList();
            var programsModel = programs.Select(x => new ProgramModel()
            {
                ProgramKey = x.ProgramKey,
                ProgramName = x.ProgramName,
            }).ToList();

            //return new JsonNetResult(new { PaymentTypes = paymentTypes, Persons = personModel }, JsonRequestBehavior.AllowGet);
            return Json(new { PaymentTypes = paymentTypesModel, Persons = personsModel, Accounts = accountsModel, Programs = programsModel }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SavePayment(Payment payment)
        {
            var paymentObj = obj.Payments.Add(payment);
            obj.SaveChanges();
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
            return Json(payment, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllPayments()
        {
            var payments = obj.Payments.ToList();
            return Json(payments, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Deletepayment(int paymentKey)
        {
            var payment = obj.Payments.SingleOrDefault(x => x.PaymentKey == paymentKey);
            if(payment !=null)
            {
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
            var paymentProgram = obj.PaymentPrograms.SingleOrDefault(x => x.PaymentKey == paymentProgramKey);
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