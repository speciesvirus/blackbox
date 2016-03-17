using Awecent.Back.Serial.Models;
using LinqToExcel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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
        private GashaponContext gashaponContext = new GashaponContext();
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        [ClaimsAuthorize(ClaimTypes.Role, "Administrator", "Product", "Reporter")]
        public ActionResult Active()
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
            ViewBag.ServerName = "";

            return View();
        }

        [Authorize]
        [ClaimsAuthorize(ClaimTypes.Role, "Administrator", "Product", "Reporter")]
        public ActionResult Registration()
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
        public ActionResult Import(HttpPostedFileBase excelFile, int? degit, string id)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PromotionID = id;
                return View();
            }

            if (excelFile != null && (excelFile.FileName.EndsWith("xls") || excelFile.FileName.EndsWith("xlsx")))
            {
                var folder = AppDomain.CurrentDomain.BaseDirectory + "File";

                string file = Server.MapPath("~/File/" + excelFile.FileName);
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }
                excelFile.SaveAs(file);

                var excel = new ExcelQueryFactory(file);
                var list = from c in excel.Worksheet<ItemCode>()
                           where c.Code != null
                           select c;

                int valid = 0;
                List<string> validlist = new List<string>();
                foreach (ItemCode item in list)
                {
                    if (item.Code.Length != degit)
                    {
                        valid++;
                        validlist.Add("" + item.Code);
                    }
                }

                int total = list.ToList().Count;
                ViewBag.PromotionID = id;
                ViewBag.TempData = list;
                ViewBag.Total = total;
                ViewBag.TotalValid = valid;
                ViewBag.Validlist = validlist;
                ViewBag.FileName = excelFile.FileName;
                ViewBag.Degit = degit;
                return View();

            }
            ViewBag.PromotionID = id;
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



        [Authorize]
        public ActionResult Gashapon()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            // Get the claims values

            var server = identity.Claims.Where(c => c.Type == "ServerList")
                   .Select(c => c.Value).SingleOrDefault();
            List<Game> ServerList = JsonConvert.DeserializeObject<List<Game>>(server);
            ViewBag.Server = ServerList;
            ViewBag.ServerName = "";

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

        [HttpGet]
        public JsonResult GetGPN(string id)
        {
            GashaponHeaderList list = gashaponContext.GetGashaponHeaderListDDL(new GashaponHeader { GameId = int.Parse(id) });
            return Json(list, JsonRequestBehavior.AllowGet);
        }



    }

}