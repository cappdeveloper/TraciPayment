using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PersonController : Controller
    {
        NYFSEntities2 obj = new NYFSEntities2();
        //
        // GET: /Person/
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetPersons(int page, int pagesize)
        {
            var personCount = obj.People.Count();
            var skip = (page - 1) * pagesize;
            var persons = obj.People.OrderBy(x => x.PersonKey).Skip(skip).Take(pagesize).ToList();
            var personsModel = persons.Select(x => new PersonModel()
            {
                PersonKey = x.PersonKey,
                PersonName = x.PersonName,
                PersonAddress = x.PersonAddress,
                PersonCity = x.PersonCity,
                PersonStateKey = x.PersonStateKey,
                PersonZipCode = x.PersonZipCode,
                PersonNote = x.PersonNote,
                VendorFederalEIN = x.VendorFederalEIN,
                VendorDefaultTerms = x.VendorDefaultTerms
            }).ToList();

            return Json(new { Persons = personsModel, PersonCount = personCount }, JsonRequestBehavior.AllowGet);
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