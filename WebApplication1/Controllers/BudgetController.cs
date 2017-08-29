using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BudgetController : Controller
    {
        NYFSEntities2 obj = new NYFSEntities2();
        //
        // GET: /Budget/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetBudgets(int page, int pagesize)
        {
            var budgetCount = obj.Budgets.Count();
            var skip = (page - 1) * pagesize;
            var budgets = obj.Budgets.OrderBy(x => x.BudgetKey).Skip(skip).Take(pagesize).ToList();
            var budgetsModel = budgets.Select(x => new BudgetModel()
            {
                BudgetKey = x.BudgetKey,
                BudgetProgramKey = x.BudgetProgramKey,
                BudgetTermKey = x.BudgetTermKey,
                BudgetAccountKey = x.BudgetAccountKey,
                BudgetAmount = x.BudgetAmount,
                BudgetNote = x.BudgetNote,
                //Account = x.Account,
                Account = from a in x.Account
                          select new AccountModel()
                          {
                              AccountKey = a.AccountKey,
                              AccountName = a.AccountName
                          },
                Program = x.Program,
                Term = x.Term
            }).ToList();

            return Json(new { Budgets = budgetsModel, BudgetCount = budgetCount }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveUpdatePerson(Person person)
        {
            var personObj = obj.People.SingleOrDefault(x => x.PersonKey == person.PersonKey);
            if (personObj != null)
            {
                personObj.PersonName = person.PersonName;
                personObj.PersonAddress = person.PersonAddress;
                personObj.PersonCity = person.PersonCity;
                personObj.PersonStateKey = person.PersonStateKey;
                personObj.PersonZipCode = person.PersonZipCode;
                personObj.PersonNote = person.PersonNote;
                personObj.VendorFederalEIN = person.VendorFederalEIN;
                personObj.VendorDefaultTerms = person.VendorDefaultTerms;
                obj.SaveChanges();
            }

            return Json(person, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeletePerson(int personKey)
        {
            var person = obj.People.SingleOrDefault(x => x.PersonKey == personKey);
            if (person != null)
            {
                var payments = obj.Payments.Where(x => x.PaymentTo == personKey);
                if (payments.Count() == 0)
                {
                    var personWithRoles = obj.PersonWithRoles.Where(y => y.PersonKey == personKey).ToList();
                    if (personWithRoles.Count > 0)
                        obj.PersonWithRoles.RemoveRange(personWithRoles);

                    obj.People.Remove(person);
                    obj.SaveChanges();
                    return Json(new { Success = true, Message = "Person deleted successfully." }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { Success = false, Message = "You cannot delete this person, As this person is being used in some payments." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Success = false, Message = "Some error occured while deleting the person." }, JsonRequestBehavior.AllowGet);
            }
        }
	}
}