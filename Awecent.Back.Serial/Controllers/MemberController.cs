using Awecent.Back.Serial.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Awecent.Back.Serial.Controllers
{
    public class MemberController : Controller
    {
        // GET: Member
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}