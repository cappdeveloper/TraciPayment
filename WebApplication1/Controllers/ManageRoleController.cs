using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ManageRoleController : Controller
    {
        // GET: ManageRole
        NYFSEntities2 oDB = new NYFSEntities2();
        public ActionResult Index()
        {
            List<Roles> oRoles = new List<Roles>();
            oRoles = (new RoleDb()).GetRoles();
            return View(oRoles);
        }
        public ActionResult CreateEdit(string id)
        {
            Roles model = new Roles();
            if (id != null)
            {
                model = (new RoleDb()).GetRolebyId(id);
                ViewBag.Action = "Edit";
            }
            else
            {
                ViewBag.Action = "Create";
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateEdit(Roles model)
        {
            string returnId = "";
            try
            {


                if (model.Id != null)
                {
                    var user = oDB.AspNetRoles.Where(m => m.Id == model.Id).FirstOrDefault();

                    user.Name = model.Name;

                    oDB.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    oDB.SaveChanges();



                    returnId = model.Id;
                }
                else
                {
                    Guid IdGen = Guid.NewGuid();
                    var user = new WebApplication1.Models.AspNetRole
                    {
                        Id = IdGen.ToString(),
                        Name = model.Name,
                    };
                    oDB.AspNetRoles.Add(user);
                    oDB.SaveChanges();
                }

                return Json(new
                {
                    returnId = returnId,

                }, JsonRequestBehavior.AllowGet);



            }
            catch (Exception ex)
            {

            }

            ViewBag.Action = (model.Id != null) ? "Edit" : "Create";

            return Json(new
            {
                returnId = returnId,

            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            try
            {
                var q = oDB.AspNetRoles.Where(m => m.Id == id).SingleOrDefault();
                oDB.AspNetRoles.Remove(q);
                oDB.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);

        }
    }
}