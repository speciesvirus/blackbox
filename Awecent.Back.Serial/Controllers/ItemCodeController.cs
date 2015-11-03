using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Awecent.Back.Serial.Controllers
{
    public class ItemCodeController : Controller
    {
        // GET: ItemCode
        public ActionResult Index()
        {
            return RedirectToAction("Index","Home");
            //return View();
        }
    }
}