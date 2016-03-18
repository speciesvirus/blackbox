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
    public class GashaponController : Controller
    {
        private GashaponContext context = new GashaponContext();
        // GET: Gashapon
        [Authorize]
        [ClaimsAuthorize(ClaimTypes.Role, "Administrator", "Product")]
        public ActionResult GashaponHeader()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            // Get the claims values
            var name = identity.Claims.Where(c => c.Type == "GameList")
                               .Select(c => c.Value).SingleOrDefault();
            List<Game> list = JsonConvert.DeserializeObject<List<Game>>(name);
            ViewBag.Games = list;
            return View();
        }

        public JsonResult SearchGashaponDDL(GashaponHeader model)
        {
            if (String.IsNullOrEmpty(model.GameId.ToString()))
            {
                model.Result = false;
                model.Message = "model state valid fail.";
                return Json(model);
            }
            GashaponHeaderList list = context.GetGashaponHeaderListDDL(model);
            return Json(list);
        } 

        public JsonResult SearchGashapon(GashaponHeader model)
        {
            if (model.GameId == null)
            {
                model.Result = false;
                model.Message = "Model state valid fail.";
                return Json(model);
            }
            model.CreateUser = ClaimName();
            GashaponHeaderList list = context.GetGashaponHeaderList(model);
            return Json(list);
        }

        public JsonResult SearchGashaponDetail(GashaponDetail model)
        {
            if (model.GashaponHdrId == null)
            {
                model.Result = false;
                model.Message = "Model state valid fail.";
                return Json(model);
            }
            model.CreateUser = ClaimName();
            GashaponDetailList list = context.GetGashaponDetailList(model);
            return Json(list);
        }

        private string ClaimName()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var name = identity.Claims.Where(c => c.Type == ClaimTypes.Name)
                                 .Select(c => c.Value).SingleOrDefault();
            return name;
        }

        [Authorize]
        [ClaimsAuthorize(ClaimTypes.Role, "Administrator", "Product")]
        public ActionResult GashaponDetail(string id)
        {
            GashaponHeader model = new GashaponHeader();
            model.GashaponHdrId = Convert.ToInt32(id);
            if (id == null) return RedirectToAction("Index", "Master");
            GashaponHeaderList list = context.GetGashaponHeaderList(model);
            model = list.Data[0];
            ViewBag.GashaponHdrId = id;
            return View(model);
        }

        [Authorize]
        [ClaimsAuthorize(ClaimTypes.Role, "Administrator", "Product")]
        public ActionResult CreateGashaponHeaderView(GashaponHeader model)
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
        public JsonResult CreateGashaponHeader(GashaponHeader model)
        {
            if (!ModelState.IsValid)
            {
                model.Result = false;
                model.Message = "Model state valid fail.";
                return Json(model);
            }
            model.CreateUser = ClaimName();
            GashaponHeader gashaponHeader = context.CreateGashaponHeader(model);
            return Json(gashaponHeader);
        }

        [HttpPost]
        public JsonResult CreateGashaponDetail(GashaponDetail model)
        {
            if (!ModelState.IsValid)
            {
                model.Result = false;
                model.Message = "Model state valid fail.";
                return Json(model);
            }
            model.CreateUser = ClaimName();
            GashaponDetail gashaponDetail = context.CreateGashaponDetail(model);
            return Json(gashaponDetail);
        }

        [HttpPost]
        public JsonResult UpdateGashaponHeader(GashaponHeader model)
        {
            if (model.GashaponHdrId == null)
            {
                model.Result = false;
                model.Message = "Model state valid fail.";
                return Json(model);
            }
            model.CreateUser = ClaimName();
            GashaponHeader gashaponHeader = context.SaveChangeGashaponHeader(model);
            return Json(gashaponHeader);
        }

        [HttpPost]
        public JsonResult UpdateGashaponDetail(GashaponDetail model)
        {
            if (model.GashaponDtlId == null)
            {
                model.Result = false;
                model.Message = "Model state valid fail.";
                return Json(model);
            }
            model.CreateUser = ClaimName();
            GashaponDetail gashaponDetail = context.SaveChangeGashaponDetail(model);
            return Json(gashaponDetail);
        }

        [HttpGet]
        public JsonResult DeleteItemGashapon(String id)
        {
            if (id == null) return Json(new GashaponDetail { Result = false, Message = "Model state valid fail." });
            GashaponDetail detail = context.DeleteGashapon(id);
          return Json(detail, JsonRequestBehavior.AllowGet);
        }
    }
}
