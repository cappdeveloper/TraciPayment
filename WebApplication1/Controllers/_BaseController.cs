using WebApplication1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
	public class BaseController : Controller
	{

		protected string currentUserId
		{
			get
			{
				if (User.Identity.IsAuthenticated)
				{
					return User.Identity.GetUserId<string>();
				}
				else
				{
					return null;
				}
			}
		}

		protected string currentUserEmail
		{
			get
			{
				if (User.Identity.IsAuthenticated)
				{
					return User.Identity.Name;
				}
				else
				{
					return string.Empty;
				}
			}
		}

		protected bool IsInRole(string role)
		{
			if (User.Identity.IsAuthenticated)
			{
				return User.IsInRole(role);
			}
			else
			{
				return false;
			}
		}

		public void IdentitySignIn(string userId, string userName, bool rememberMe, List<AspNetRole> roles)
		{
			var claims = new List<Claim>();
			claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
			claims.Add(new Claim(ClaimTypes.Name, userName));

			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role.Name));
			}


			var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
			AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = rememberMe }, identity);
		}

		public void IdentitySignout()
		{
			Session.RemoveAll();
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie, DefaultAuthenticationTypes.ExternalCookie);
		}

		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}


        public string RenderRazorViewToString(string viewName, object model)
        {

            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);

                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);

                viewResult.View.Render(viewContext, sw);

                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);

                return sw.GetStringBuilder().ToString();
            }
        }

    }
}