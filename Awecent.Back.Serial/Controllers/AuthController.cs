using Awecent.Back.Serial.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Awecent.Back.Serial.Controllers
{
    public class AuthController : Controller
    {
        private MySQLContext context = new MySQLContext();

         private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        // GET: Auth
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult SignIn()
        {
            return View();
        }

        
        public ActionResult ValidateAccount(LoginViewModel model)
        {
            if(string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.Email))
                return RedirectToAction("SignIn", "Auth", new { error = "3t40q-s0dgh234" });
            if (SignAuthenticationManager(model)) {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("SignIn", "Auth", new { error = "23651fahsdba235"});
        }

        public bool SignAuthenticationManager(LoginViewModel model) {
            new LogFile().Writer(new LogModel { Data = model, Function = "SignAuthenticationManager" });
            UserandRole user = context.ValidateAccount(model.Email , model.Password);
            new LogFile().Writer(new LogModel { Data = user , Function = "SignAuthenticationManager" , Exception = "user return" });
            if (user == null) return false;
            //get server list
            UserandRole server = context.ValidateAccountServer(model.Email, model.Password);
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
 
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Sid, "" + user.UserId));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, "Awecent"));
            claims.Add(new Claim(ClaimTypes.Role, user.UserRole));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.Email, user.UserName));
            claims.Add(new Claim("GameList", JsonConvert.SerializeObject(user.UserGameList)));
            claims.Add(new Claim("ServerList", JsonConvert.SerializeObject(server.UserGameList)));
            var id = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            

            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignIn(id);
            return true;
        }


        public ActionResult SignOut() {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("SignIn", "Auth");
        }
    }
}