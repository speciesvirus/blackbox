using Awecent.Back.Serial.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Awecent.Back.Serial.Controllers
{
    public class ReportController : Controller
    {
        private MySQLContext context = new MySQLContext();
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult ItemCode()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            // Get the claims values
            var name = identity.Claims.Where(c => c.Type == "GameList")
                               .Select(c => c.Value).SingleOrDefault();

            List<Game> list = JsonConvert.DeserializeObject<List<Game>>(name);
            ViewBag.Games = list;

            return View();
        }

        [HttpPost]
        public JsonResult GetRecord(ReportItemCodeInput model) {
            var result = context.GetItemCode(model);
            return Json(result , JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPromotions(string id) {
            PromotionsList list = context.GetPromotions(id);
            return Json(list , JsonRequestBehavior.AllowGet);
        }
    }

}