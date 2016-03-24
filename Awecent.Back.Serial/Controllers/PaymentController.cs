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
    public class PaymentController : Controller
    {
        private PaymentContext paymentContext = new PaymentContext();

        // GET: /Payment/
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        [ClaimsAuthorize(ClaimTypes.Role, "Administrator", "Product", "Reporter")]
        public ActionResult PaymentTransaction()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            // Get the claims values
            var name = identity.Claims.Where(c => c.Type == "GameList")
                               .Select(c => c.Value).SingleOrDefault();
            List<Game> list = JsonConvert.DeserializeObject<List<Game>>(name);
            ViewBag.Games = list;

            var server = identity.Claims.Where(c => c.Type == "ServerList")
                   .Select(c => c.Value).SingleOrDefault();
            List<Game> ServerList = JsonConvert.DeserializeObject<List<Game>>(server);
            ViewBag.Server = ServerList;

            return View();
        }

        [HttpGet]
        public JsonResult GetServer(string id)
        {
            ServerList list = paymentContext.GetServerList(id);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetProvider(string id)
        {
            ProviderList list = paymentContext.GetProviderList(id);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDealer(string id)
        {
            DealerList list = paymentContext.GetDealerList(id);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
	}
}