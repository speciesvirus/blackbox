using Awecent.Back.Serial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Awecent.Back.Serial.Controllers
{
    [RoutePrefix("api/report")]
    public class ReportApiController : ApiController
    {
        MemberContext context = new MemberContext();

        [Route("active/user")]
        [HttpPost]
        public HttpResponseMessage userActive(ReportActiveUser model)
        {
            //ReportActiveUser result = context.GetActiveUser(model);
            var gameID = model.gameID;
            string[] words = gameID.Split(',');
            //System.Console.WriteLine("{0} words in text:", words.Length);

            ReportActiveUserResult result = new ReportActiveUserResult();
            result.data = new List<ReportActiveUser>();
            foreach (var element in words)
            {
                string[] server = element.Split('-');
                model.gameID = server[0];
                model.serverID = server[1];

                result.data.Add(context.GetActiveUser(model));
            }


            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
                
        }


        [Route("register/user")]
        [HttpPost]
        public HttpResponseMessage userRegister(ReportActiveUser model)
        {
            
            string[] server = model.gameID.Split('-');
            model.gameID = server[0];
            model.serverID = server[1];

            var result = context.GetRegisterUser(model);

            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);

        }


        //[Route("gashapon/user")]
        //[HttpPost]
        //public HttpResponseMessage userGashapon(ReportUser model)
        //{

        //    ReportUserResult result = new ReportUserResult();
        //    var promotionID = model.promotionID;
        //    string[] words = promotionID.Split(',');
            
        //    foreach (var element in words)
        //    {
        //        model.promotionID = element;
        //        model.data.Add(context.GetGashaponUser(model));
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, model, Configuration.Formatters.JsonFormatter);

        //}




        [Route("gashapon/count")]
        [HttpPost]
        public HttpResponseMessage gashaponUesd(ReportGashapon model)
        {

            ReportGashaponResult reportResult = new ReportGashaponResult();
            reportResult.data = new List<ReportGashapon>();

            var promotionID = model.promotionID;
            string[] words = promotionID.Split(',');

            foreach (var element in words)
            {
                model.promotionID = element;
                ReportGashapon result = context.GetGashaponReport(model);
                result.gameID = model.gameID;
                result.serverID = model.serverID;
                result.promotionID = element;
                reportResult.data.Add(result);
                //model.data = reportResult.data;
                //model.data.Add(context.GetGashaponReport(model));
            }

            return Request.CreateResponse(HttpStatusCode.OK, reportResult, Configuration.Formatters.JsonFormatter);

        }
         

         
    }
}
