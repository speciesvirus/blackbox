﻿using Awecent.Back.Serial.Models;
using ClosedXML.Excel;
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
        [ClaimsAuthorize(ClaimTypes.Role, "Administrator", "Product")]
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
        [ClaimsAuthorize(ClaimTypes.Role, "Administrator", "Product")]
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

        [Authorize]
        [ClaimsAuthorize(ClaimTypes.Role, "Administrator", "Product")]
        public ActionResult Generate(string id, string name)
        {
            if (id == null) return RedirectToAction("Index", "Master");
            ViewBag.GameID = id;
            ViewBag.PromotionID = name;
            return View();
        }

        [Authorize]
        [ClaimsAuthorize(ClaimTypes.Role, "Administrator", "Product")]
        public ActionResult Lot(string id)
        {
            if (id == null) return RedirectToAction("Index","Master");
            ViewBag.PromotionID = id;
            return View();
        }

        [HttpPost]
        public JsonResult GetGameServer(string GameID)
        {
            Game list = context.GetGameServer(GameID);
            return Json(list);
        }

        [Authorize]
        public ActionResult Import(HttpPostedFileBase excelFile, int? degit, string id, string name)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.GameID = id;
                ViewBag.PromotionID = name;
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

                string utc = ConvertToUnixTimestamp(DateTime.UtcNow).ToString();
                string timestamp = name + "" + utc;
                string createName = ClaimName();
                string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                StringBuilder sb = new StringBuilder();

                var countTemp = 1;
                foreach (ItemCode item in list)
                {
                    //if (item.Code.Length != degit)
                    //{
                    //    valid++;
                    //    validlist.Add("" + item.Code);
                    //}
                    sb.AppendFormat("INSERT INTO `gameitemcode_temp`(`id`,`iPromotionID`,`digitCode`,`itemCode`,`statusProcess`,`fileName`,`dtCreateUser`,`dtCreateDate`) "
                    + "VALUES('{0}','{1}','{2}','{3}','0',{4},'{5}','{6}');"
                    , countTemp, name, degit, item.Code, timestamp, createName, date);

                    countTemp++;
                }
                string sql = sb.ToString();
                MasterCode master = context.InsertExecute(sql);
                int countValid = context.ValidateImportItemCode(timestamp);
                //if (master.Result)
                //{
                //    master.Result = context.UpdateStatusToProgress(name);
                //    if (!master.Result) master.Message = "Update status fail.";
                //}

                int total = list.ToList().Count;
                ViewBag.GameID = id;
                ViewBag.PromotionID = name;
                ViewBag.TempData = list;
                ViewBag.Total = total;
                ViewBag.TotalValid = countValid;// valid;
                ViewBag.Validlist = validlist;
                ViewBag.FileName = excelFile.FileName;
                ViewBag.Degit = degit;
                ViewBag.key = timestamp;
                return View();

            }
            ViewBag.GameID = id;
            ViewBag.PromotionID = name;
            return View();
        }
        static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }


        [Authorize]
        [ClaimsAuthorize(ClaimTypes.Role, "Administrator", "Product")]
        public ActionResult ExportCode(string id, string name)
        {
            ItemCode model = new ItemCode { PromotionID = Convert.ToInt64(name), LotID = Convert.ToInt64(id) };
            model.CreateBy = ClaimName();
            ItemCodeList list = context.ExportItemCode(model);

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("itemcode");
            for (int i = 0; i < list.Data.Count; i++)
            {
                ItemCode item = list.Data[i];
                worksheet.Cell((i+1), 1).SetValue(item.Code);
                worksheet.Cell((i + 1), 2).SetValue(item.IsUsed);
                worksheet.Cell((i + 1), 3).SetValue(item.TimeCreate);
            }

            string filename = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "-" + id + "" + name;

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", string.Format("attachment;filename=\"{0}.xlsx\"", filename));

            // Flush the workbook to the Response.OutputStream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                memoryStream.Close();
            }

            Response.End();

            return View();
        }

        #region json function

        [HttpPost]
        [Authorize]
        public JsonResult GetLotList(Lot model) {
            LotList list = context.GetLotList(model);
            return Json(list);
        }

        [HttpPost]
        [Authorize]
        public JsonResult GetItemCodeList(ItemCode model)
        {
            ItemCodeList list = context.GetItemCodeList(model);
            return Json(list);
        }

        [HttpPost]
        [Authorize]
        public JsonResult SaveImport(string id, string promotionid, string serverid, string itemid, string key)
        {
            string file = Server.MapPath("~/File/" + id);
            if (!System.IO.File.Exists(file)) return Json(new MasterCode { Result = false, Message = "file not found." });
            if (promotionid == null || string.IsNullOrEmpty(promotionid)) return Json(new MasterCode { Result = false, Message = "PromotionId not found." });

            var excel = new ExcelQueryFactory(file);
            var list = from c in excel.Worksheet<ItemCode>()
                       where c.Code != null
                       select c;
            var total = list.ToList().Count;
            string name = ClaimName();
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            MasterCode master = context.SaveTemptoTable(new OutputTempMaster { Key = key, GenereateBy = name }, new MasterCode { ServerID = Convert.ToInt32(serverid), ItemID = Convert.ToInt32(itemid) });
            if (master == null) return Json(new MasterCode { Result = false, Message = "Can't insert to temp." });

            //MasterCode master = context.GenerateCodeLot(new MasterCode { PromotionID = Convert.ToInt64(promotionid), ServerID = Convert.ToInt32(serverid), ItemID = Convert.ToInt32(itemid), CreateBy = name, CodeAmount = total });
            // old by ker
            //MasterCode master = context.CreateLotItemCode(new MasterCode { PromotionID = Convert.ToInt64(promotionid), CreateBy = name, CodeAmount = total });
            //if (master == null) return Json(new MasterCode { Result = false, Message = "can not create lot." });

            //StringBuilder sb = new StringBuilder();

            //foreach (ItemCode item in list)
            //{
            //    sb.AppendFormat("INSERT INTO `gameitemcode`(`iPromotionID`,`iLotID`,`vCode`,`cIsUse`,`dtCreateUser`,`dtCreateDate`) "
            //        + "VALUES('{0}','{1}','{2}','N','{3}','{4}');"
            //        , promotionid, master.Lot, item.Code, name, date);
            //}

            //string sql = sb.ToString();
            //master = context.InsertExecute(sql);
            //if (master.Result)
            //{
            //    master.Result = context.UpdateStatusToProgress(promotionid);
            //    if (!master.Result) master.Message = "Update status fail.";
            //}
            return Json(master);
        }

        [HttpPost]
        [Authorize]
        public JsonResult GetItemCodeTemp(OutputTempMaster model) {
            ItemCodeList list = context.GetItemCodeListTemp(model);
            return Json(list);
        }

        [HttpPost]
        [Authorize]
        public JsonResult SaveGenerate(OutputTempMaster model,MasterCode masterModel)
        {
            if (model == null || model.Key == null) return Json(new MasterCode { Result = false, Message = "Can not reference code in temp table." });
            model.GenereateBy = ClaimName();
            try
            {
                string[] arrstr = model.GenereateBy.Split('@');
                model.GenereateBy = arrstr[0];
            }
            catch (Exception ex)
            {

            }
            MasterCode master = context.SaveTemptoTable(model, masterModel);
            return Json(master);
        }

        [HttpPost]
        [Authorize]
        public JsonResult GenerateToTemp(InputTempMaster model)
        {
            if (!ModelState.IsValid) return Json(new MasterCode { Result = false, Message = "Model state is valid" });
            model.GenereateBy = ClaimName();
            try
            {
                string[] arrstr = model.GenereateBy.Split('@');
                model.GenereateBy = arrstr[0];
            }
            catch (Exception ex) { 
                
            }
            OutputTempMaster output = context.GenerateCodeToTempAndValidateion(model);
            return Json(output);
        }

        [HttpPost]
        [Authorize]
        public JsonResult GenerateLot(MasterCode model)
        {
            model.CreateBy = ClaimName();
            try
            {
                string[] arrstr = model.CreateBy.Split('@');
                model.CreateBy = arrstr[0];
            }
            catch (Exception ex)
            {

            }
            MasterCode list = context.GenerateCodeLot(model);
            return Json(list);
        }


        public JsonResult SearchMaster(MasterCode model)
        {
            if (model.GameID == null)
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
            model.SerialPrefix = model.SerialPrefix != null ? model.SerialPrefix.ToLower().ToString() : null; 
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
            return Json(master, JsonRequestBehavior.AllowGet);
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