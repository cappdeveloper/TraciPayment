using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.Concrete_Classes;
using WebApplication1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using WebApplication1;


namespace WebApplication1.Controllers

{

    public class AccountController : BaseController
    {
        private EmailService.ApplicationSignInManager _signInManager;
        private EmailService.ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(EmailService.ApplicationUserManager userManager, EmailService.ApplicationSignInManager signInManager)
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

        //
        // GET: /Account/Login
        public ActionResult Login(string msg)
        {

            ViewBag.errorMsg = msg;
            return View();
        }

        
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(string Username, string Password)
        {
            var context =new NYFSEntities2();
            if (!ModelState.IsValid)
            {
                return View("Login");
            }

            var user = await UserManager.FindByNameAsync(Username);
            if (user != null)
            {
                var result = await SignInManager.PasswordSignInAsync(Username, Password, true, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        if (UserManager.IsInRole(user.Id, "Admin")) {
                            var allroles = context.AspNetRoles.ToList();
                            this.IdentitySignIn(user.Id, user.Name, false, allroles);
                        return RedirectToAction("Index", "PaymentScreen");
                        }
                        else
                            return RedirectToAction("Login", "Account", new { msg = "Username and Password is Not correct !" });
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return RedirectToAction("Login", "Account", new { msg = "Username and Password is Not correct !" });
                }
            }
            else
                return RedirectToAction("Login", "Account", new { msg = "Username and Password is Not correct !" });
        }
       
        [Authorize()]
        public ActionResult Logoff()
        {
            this.IdentitySignout();
            return RedirectToAction("login", "Account");
        }

        [Authorize]
        public ActionResult ManageAccount()
        {
            string Id = currentUserId.ToString();
            return View();
        }

    }
}
