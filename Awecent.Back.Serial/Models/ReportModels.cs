﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    public class ReportActiveUser
    {
        public string gameID { get; set; }
        public string serverID { get; set; }

        public DateTime? timeStart { get; set; }
        public DateTime? timeEnd { get; set; }

        //return result
        public int _page { get; set; }
        public int _pageSize { get; set; }
        public bool result { get; set; }
        public string message { get; set; }
        public List<ActiveUserList> data { get; set; }

        // registration
        public int _sumCountActive { get; set; }
        public int _sumCountRegister { get; set; }
    }

    public class ActiveUserList
    {
        public int gameID { get; set; }
        public int serverID { get; set; }
        public int count { get; set; }
        public DateTime? curTime { get; set; }
        public string curTimeString { get; set; }

        // registration
        public int countRegisterFacebook { get; set; }
        public int countRegisterGuest { get; set; }
        public int countRegisterTotal { get; set; }

        public int countActiveFacebook { get; set; }
        public int countActiveGuest { get; set; }
        public int countActiveTotal { get; set; }

        public int countActive { get; set; }
        public int countRegister { get; set; }
    }



    public class ReportUser
    {
        public string gameID { get; set; }
        public string serverID { get; set; }
        public string promotionID { get; set; }

        public DateTime? timeStart { get; set; }
        public DateTime? timeEnd { get; set; }

        //return result
        public int _page { get; set; }
        public int _pageSize { get; set; }
        public bool result { get; set; }
        public string message { get; set; }
        public List<ReportUserList> data { get; set; }
    }


    public class ReportUserList
    {
        public string gameID { get; set; }
        public string serverID { get; set; }
        public string promotionID { get; set; }

        public int count { get; set; }
        public DateTime? curTime { get; set; }
        public string curTimeString { get; set; }

    }

    public class ReportUserResult
    {
        public bool result { get; set; }
        public string message { get; set; }
        public List<ReportUser> data { get; set; }
    }



    public class ReportGashapon
    {
        public string gameID { get; set; }
        public string serverID { get; set; }
        public string promotionID { get; set; }

        public DateTime? timeStart { get; set; }
        public DateTime? timeEnd { get; set; }

        //return result
        public bool result { get; set; }
        public string message { get; set; }
        public List<ReportGashaponList> data { get; set; }
    }
    public class ReportGashaponList
    {
        public int gashaponID { get; set; }
        public string name { get; set; }

        public int count { get; set; }
        public DateTime? curTime { get; set; }
        public string curTimeString { get; set; }

    }

    public class ReportGashaponResult
    {
        public bool result { get; set; }
        public string message { get; set; }
        public List<ReportGashapon> data { get; set; }
    }
   

}