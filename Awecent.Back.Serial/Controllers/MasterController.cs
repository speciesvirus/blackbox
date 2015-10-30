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
    public class MasterController : Controller
    {
        private MasterCodeContext context = new MasterCodeContext();

        // GET: Master
        [Authorize]
        public ActionResult Index()
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
        public ActionResult Create()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            // Get the claims values
            var name = identity.Claims.Where(c => c.Type == "GameList")
                               .Select(c => c.Value).SingleOrDefault();
            List<Game> list = JsonConvert.DeserializeObject<List<Game>>(name);
            ViewBag.Games = list;



            return View();
        }

        public ActionResult Import()
        {
            return View();
        }

        public ActionResult Generate()
        {
            return View();
        }

        #region json function
        public JsonResult SearchMaster(MasterCode model)
        {
            if (!ModelState.IsValid)
            {
                model.Result = false;
                model.Message = "Model state valid fail.";
                return Json(model);
            }
            model.CreateUser = ClaimName();
            MasterCodeList list = context.GetMasterCodeList(model);
            return Json(list);
        }

        public JsonResult CreateMaster(MasterCode model)
        {
            if (!ModelState.IsValid)
            {
                model.Result = false;
                model.Message = "Model state valid fail.";
                return Json(model);
            }
            model.CreateUser = ClaimName();
            MasterCode master = context.CreateMasterCode(model);
            return Json(master);
        }

        [HttpPost]
        public JsonResult UpdateMaster(MasterCode model)
        {
            if (model.PromotionID == null)
            {
                model.Result = false;
                model.Message = "Model state valid fail.";
                return Json(model);
            }
            model.CreateUser = ClaimName();
            MasterCode master = context.SaveChangeMasterCode(model);
            return Json(master);
        }

      
        [HttpGet]
        public JsonResult ChangeStatus(string id)
        {
            if (id == null) return Json(new MasterCode { Result = false, Message = "Model state valid fail." });
            MasterCode master = context.PendingAndActiveMasterCode(new MasterCode { PromotionID = Convert.ToInt64(id), CreateUser = ClaimName() });
            return Json(master, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult Delete(string id)
        {
            if (id == null) return Json(new MasterCode { Result = false, Message = "Model state valid fail." });
            MasterCode master = context.DeleteMasterCode(new MasterCode { PromotionID = Convert.ToInt64(id), CreateUser = ClaimName() });
            return Json(master , JsonRequestBehavior.AllowGet);
        }

        #endregion


        private string ClaimName()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var name = identity.Claims.Where(c => c.Type == ClaimTypes.Name)
                                 .Select(c => c.Value).SingleOrDefault();
            return name;
        }


    }
}