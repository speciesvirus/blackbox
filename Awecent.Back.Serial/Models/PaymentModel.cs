using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Awecent.Back.Serial.Models
{

    public class ReportRefund
    {
        [Required]
        public string gameId { get; set; }
        public string serverId { get; set; }
        public string UID { get; set; }

        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }

        public int pageSize { get; set; }
        public int page { get; set; }
    }



    public class RefundError
    {
        public string productTransactionId { get; set; }
        public string referenceId { get; set; }
        public string providerName { get; set; }
        public string dealerName { get; set; }
        public string gameName { get; set; }
        public string serverName { get; set; }
        public string userId { get; set; }
        public string pointGame { get; set; }
        public string pointUnit { get; set; }
        public string productCode { get; set; }
        public string paymentTransactionId { get; set; }
        public string rcvRespCode { get; set; }
        public string rcvRespMsg { get; set; }
        public string exceptionCode { get; set; }
        public string exceptionMsg { get; set; }
        public string createUser { get; set; }
        public string createDate { get; set; }


        //page
        public int PageSize { get; set; }
        public int Page { get; set; }

    }

    public class RefundErrorList : ResultModel
    {
        public List<RefundError> Data { get; set; }
        public int Total { get; set; }
    }


}