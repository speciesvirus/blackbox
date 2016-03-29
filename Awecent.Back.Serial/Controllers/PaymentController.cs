﻿using Awecent.Back.Serial.Models;
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


        [Authorize]
        [ClaimsAuthorize(ClaimTypes.Role, "Administrator", "Product", "Reporter")]
        public ActionResult PaymentRefund()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var server = identity.Claims.Where(c => c.Type == "ServerList")
                   .Select(c => c.Value).SingleOrDefault();
            List<Game> ServerList = JsonConvert.DeserializeObject<List<Game>>(server);
            ViewBag.Server = ServerList;
            ViewBag.ServerName = "";


            return View();
        }

        [Authorize]
        [ClaimsAuthorize(ClaimTypes.Role, "Administrator", "Product", "Reporter")]
        public ActionResult ItemRefund()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var server = identity.Claims.Where(c => c.Type == "ServerList")
                   .Select(c => c.Value).SingleOrDefault();
            List<Game> ServerList = JsonConvert.DeserializeObject<List<Game>>(server);
            ViewBag.Server = ServerList;
            ViewBag.ServerName = "";


            return View();
        }


        [HttpPost]
        public JsonResult GetRefund(ReportRefund model)
        {
            string[] element = model.gameId.Split('-');
            model.gameId = element[0];
            model.serverId = element[1] == "0" ? "1" : element[1];

            var result = paymentContext.GetRefundList(model);
            return Json(result);
        }

        [HttpPost]
        public JsonResult SearchTransaction(PaymentTransaction model)
        {
            if (model.GameCode == null)
            {
                model.Result = false;
                model.Message = "Model state valid fail.";
                return Json(model);
            }
            PaymentTransactionList paymentTransactionList = paymentContext.SearchPaymentTransactionList(model);
            return Json(paymentTransactionList);
        }

        //---------------------------------------------------Function private----------------------------------------------------------
        private string ClaimName()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var name = identity.Claims.Where(c => c.Type == ClaimTypes.Name)
                                 .Select(c => c.Value).SingleOrDefault();
            return name;
        }



	}
}