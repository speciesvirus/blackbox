using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Awecent.Back.Serial.Models
{

    public class ReportActiveUserResult
    {
        public bool result { get; set; }
        public string message { get; set; }
        public List<ReportActiveUser> data { get; set; }
    }

    //m
    public class ReportActiveUser
    {
        public string gameID { get; set; }
        public DateTime? timeStart { get; set; }
        public DateTime? timeEnd { get; set; }
        //return result
        public bool result { get; set; }
        public string message { get; set; }
        public List<ActiveUserList> data { get; set; }
    }
    //1
    public class ActiveUserList
    {
        public int gameID { get; set; }
        public int count { get; set; }
        public DateTime? curTime { get; set; }
    }

    
   

}