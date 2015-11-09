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
                model.gameID = element;
                result.data.Add(context.GetActiveUser(model));
            }


            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
                
        }


        [Route("register/user")]
        [HttpPost]
        public HttpResponseMessage userRegister(ReportActiveUser model)
        {

            var result = context.GetRegisterUser(model);

            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);

        }
         
    }
}
