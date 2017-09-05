using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;

using WebApplication1;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private EmailService.ApplicationSignInManager _signInManager;
        private EmailService.ApplicationUserManager _userManager;
        NYFSEntities2 oDB= new NYFSEntities2();
       

        public UserController()
        {
        }

        public UserController(EmailService.ApplicationUserManager userManager, EmailService.ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public EmailService.ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<EmailService.ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public EmailService.ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<EmailService.ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


         //Get User Data
        public ActionResult Index()
        {
            List<Users> oUsers = new List<Users>();
           oUsers = (new UserDB()).GetUsers();
            return View(oUsers);
         
        }
        public ActionResult CreateEdit(string id)
        {
            Users model = new Users();
            if (id!=null)
            {
            model = (new UserDB()).GetUsersbyId(id);
                ViewBag.Action = "Edit";
            }
            else
            {
                ViewBag.Action = "Create";
            }


       //     ViewBag.Contractor = (new UserDB()).LoadContractor();

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            NYFSEntities2 _entities = new NYFSEntities2();
            try
            {
                var q = _entities.AspNetUsers.Where(m => m.Id == id).SingleOrDefault();
                _entities.AspNetUsers.Remove(q);
                _entities.SaveChanges();




                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateEdit(Users model)
        {
           // RequestResultModel requestModel = new RequestResultModel();

           string returnId ="";
            try
            {
                model.CreatedDate = DateTime.UtcNow;
                model.ModifiedDate = DateTime.UtcNow;

                if (model.Ids != null)
                {

                   // ViewBag.Contractor = (new UserDB()).LoadContractor();
                    var user = oDB.AspNetUsers.Where(m => m.Id == model.Ids).FirstOrDefault();

                    user.Name = model.Name;
                    user.UserName = model.UserName;
                    user.CompanyEmail = model.CompanyEmail;
                    user.Address = model.Address;
                    user.City = model.City;
                    //  user.ModifiedDate = model.ModifiedDate;
                    user.Zip = model.Zip;
                  //  user.ContractorId = model.ContractorId.Value;
                    user.PhoneNumber = model.Contact;
                    user.Notes = model.Notes;
                    user.isActive = true;

                    oDB.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    oDB.SaveChanges();



                    returnId = model.Ids;
                }
                else
                {
                  //  ViewBag.Contractor = (new UserDB()).LoadContractor();


                    //tblUser user = new tblUser();
                    //user.Name = model.Name;
                    //user.UserName = model.UserName;
                    //user.CompanyEmail = model.CompanyEmail;
                    //user.Address = model.Address;
                    //user.City = model.City;
                    //user.ModifiedDate = model.ModifiedDate;
                    //user.CreatedDate = model.CreatedDate;
                    //user.Zip = model.Zip;
                    //user.ContractorId = model.ContractorId;
                    //user.Contact = model.Contact;
                    //user.Notes = model.Notes;
                    //user.Password = model.Password;
                    //user.RoleID = 2;
                    //user.IsActive = true;

                    var user = new ApplicationUser
                    {

                        UserName = model.CompanyEmail,
                      //  Email = model.CompanyEmail,
                        Name = model.Name,
                        CompanyEmail = model.CompanyEmail,
                        Address = model.Address,
                        City = model.City,
                        CreatedDate = model.CreatedDate.Value,
                        Zip = model.Zip,
                    //    ContractorId = model.ContractorId.Value,
                    //    PhoneNumber = model.Contact,
                        Notes = model.Notes,
                        isActive = true,






                    };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        var currentUser = UserManager.FindByName(user.UserName);

                        var roleresult = UserManager.AddToRole(currentUser.Id, "User");
                        //  returnId = currentUser.Id;
                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771

                    }


                    //  oDB.tblUsers.Add(user);
                    //   oDB.SaveChanges();


                }

                return Json(new
                {
                    returnId = returnId,
                 
                }, JsonRequestBehavior.AllowGet);
          
                
              
            }
            catch (Exception ex)
            {
              
            }

            ViewBag.Action = (model.Id>0) ? "Edit" : "Create";

             return Json(new
            {
                returnId = returnId,
           
            }, JsonRequestBehavior.AllowGet);
        }


    }
}