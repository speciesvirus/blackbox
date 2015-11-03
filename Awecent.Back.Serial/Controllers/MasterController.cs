using Awecent.Back.Serial.Models;
using LinqToExcel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
//using Excel = Microsoft.Office.Interop.Excel;

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

        public ActionResult Import(HttpPostedFileBase excelFile , int? degit , string id)
        {
            if (!ModelState.IsValid) {
                ViewBag.PromotionID = id;
                return View(); 
            }

            if (excelFile != null && (excelFile.FileName.EndsWith("xls") || excelFile.FileName.EndsWith("xlsx"))) {
                var folder = AppDomain.CurrentDomain.BaseDirectory + "File";

                string file = Server.MapPath("~/File/" + excelFile.FileName);
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }
                excelFile.SaveAs(file);

                var excel = new ExcelQueryFactory(file);
                var list  = from c in excel.Worksheet<ItemCode>()
                       where c.Code != null
                       select c;

                int valid = 0;
                foreach (ItemCode item in list) { 
                    if(item.Code.Length != degit){
                        valid++;
                    }
                }

                int total = list.ToList().Count;
                ViewBag.PromotionID = id;
                ViewBag.TempData = list;
                ViewBag.Total = total;
                ViewBag.TotalValid = valid;
                ViewBag.FileName = excelFile.FileName;
                ViewBag.Degit = degit;
                return View();
                    
            }
            ViewBag.PromotionID = id;
            return View();
        }

        public JsonResult SaveImport(string id, string promotionid)
        {
            string file = Server.MapPath("~/File/" + id);
            if (!System.IO.File.Exists(file)) return Json(new MasterCode { Result = false , Message = "file not found." });
            if (promotionid == null || string.IsNullOrEmpty(promotionid)) return Json(new MasterCode { Result = false, Message = "file not found." });
            
            var excel = new ExcelQueryFactory(file);
            var list = from c in excel.Worksheet<ItemCode>()
                       where c.Code != null
                       select c;
            var total = list.ToList().Count;
            string name = ClaimName();
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            MasterCode master = context.CreateLotItemCode(new MasterCode { PromotionID = Convert.ToInt64(promotionid) ,CreateBy = name , CodeAmount = total  });
            if (master == null) return Json(new MasterCode { Result = false, Message = "can not create lot." });
            StringBuilder sb = new StringBuilder();

            foreach (ItemCode item in list)
            {
                sb.AppendFormat("INSERT INTO `gameitemcode`(`iPromotionID`,`iLotID`,`vCode`,`cIsUse`,`dtCreateUser`,`dtCreateDate`) "
                    +"VALUES('{0}','{1}','{2}','N','{3}','{4}');"
                    , promotionid, master.Lot, item.Code, name, date);
            }

            string sql = sb.ToString();
            master =  context.InsertExecute(sql);
            if(master.Result){
                master.Result = context.UpdateStatusToProgress(promotionid);
                if (!master.Result) master.Message = "Update status fail.";
            }
            return Json(master);
        }

        public ActionResult Generate(string id)
        {
            ViewBag.PromotionID = id;
            return View();
        }

        #region json function

        
        public JsonResult generateToTemp(MasterCode model) {
            if (!ModelState.IsValid)
            {
                model.Result = false;
                model.Message = "Model state valid fail.";
                return Json(model);
            }

            return Json(null);
        }

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