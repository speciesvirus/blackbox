using Awecent.Back.Serial.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace Awecent.Back.Serial.Models
{
    public class PaymentContext
    {
        string PaymentConnection = WebConfigurationManager.ConnectionStrings["PaymentConnection"].ToString();

        public ServerList GetServerList(string GameId)
        {
            using (MySqlConnection con = new MySqlConnection(PaymentConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("awe_storePaymentGetListServer", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("pi_gameCode", GameId));
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    ServerList list = new ServerList();
                    var q = dt.AsEnumerable().Select(row => new Server()
                    {
                        ServerId = Convert.ToInt32(row["serverCode"].ToString()),
                        ServerName = row["serverName"].ToString()
                    }).ToList();
                    list.data = q;
                    if (q != null) list.Result = true;
                    else list.Result = false;
                    return list;
                }
                catch (Exception ex)
                {
                    new LogFile().WriterError(new LogModel { Exception = ex.Message, Function = "GetServerList" });
                    return new ServerList { Result = false };
                }
            }
        }

        public ProviderList GetProviderList(string GameId)
        {
            using (MySqlConnection con = new MySqlConnection(PaymentConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("awe_storePaymentGetListProvider", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("pi_gameCode", GameId));
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    ProviderList list = new ProviderList();
                    var q = dt.AsEnumerable().Select(row => new Provider()
                    {
                        ProviderId = Convert.ToInt32(row["providerId"].ToString()),
                        ProviderName = row["providerName"].ToString()
                    }).ToList();
                    list.data = q;
                    if (q != null) list.Result = true;
                    else list.Result = false;
                    return list;
                }
                catch (Exception ex)
                {
                    new LogFile().WriterError(new LogModel { Exception = ex.Message, Function = "GetProviderList" });
                    return new ProviderList { Result = false };
                }
            }

        }

        public DealerList GetDealerList(string ProviderId)
        {
            using (MySqlConnection con = new MySqlConnection(PaymentConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("awe_storePaymentGetListDealer", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("pi_providerId", ProviderId));
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    DealerList list = new DealerList();
                    var q = dt.AsEnumerable().Select(row => new Dealer()
                    {
                        DealerId = Convert.ToInt32(row["dealerId"].ToString()),
                        DealerName = row["dealerName"].ToString()
                    }).ToList();
                    list.data = q;
                    if (q != null) list.Result = true;
                    else list.Result = false;
                    return list;
                }
                catch (Exception ex)
                {
                    new LogFile().WriterError(new LogModel { Exception = ex.Message, Function = "GetDealerList" });
                    return new DealerList { Result = false };
                }
            }

        }






        public RefundErrorList GetRefundList(ReportRefund model)
        {
            using (MySqlConnection con = new MySqlConnection(PaymentConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("awe_storeProductGetListTransactionRefun", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add(new MySqlParameter("pi_gameCode", model.gameId));
                    cmd.Parameters.Add(new MySqlParameter("pi_serverCode", model.serverId));
                    cmd.Parameters.Add(new MySqlParameter("pi_userId", model.UID));
                    cmd.Parameters.Add(new MySqlParameter("pi_startData", model.startDate.Value));
                    cmd.Parameters.Add(new MySqlParameter("pi_endData", model.endDate.Value));
                    //_PageSize , _Page , _row
                    cmd.Parameters.Add(new MySqlParameter("pi_page", model.page));
                    cmd.Parameters.Add(new MySqlParameter("pi_pageSize", model.pageSize));
                    cmd.Parameters.Add(new MySqlParameter("po_countRow", MySqlDbType.Int32) { Direction = ParameterDirection.InputOutput });
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    RefundErrorList list = new RefundErrorList();
                    list.Total = Convert.ToInt32(cmd.Parameters["po_countRow"].Value.ToString());

                    var q = dt.AsEnumerable().Select(row => new RefundError()
                    {
                        paymentTransactionId = row["paymentTransactionId"].ToString(),
                        referenceId = row["referenceId"].ToString(),
                        providerName = row["providerName"].ToString(),
                        dealerName = row["dealerName"].ToString(),
                        gameCode = row["gameCode"].ToString(),
                        serverCode = row["serverCode"].ToString(),
                        userId = row["userId"].ToString(),
                        pointGame = row["pointGame"].ToString(),
                        pointUnit = row["pointUnit"].ToString(),
                        productCode = row["productCode"].ToString(),
                        rcvRespCode = row["rcvRespCode"].ToString(),
                        rcvRespMsg = row["rcvRespMsg"].ToString(),
                        exceptionCode = row["exceptionCode"].ToString(),
                        exceptionMsg = row["exceptionMsg"].ToString(),
                        createDate = row["createDate"].ToString()
                        //createDate = (DateTime?)(row["createDate"] == DBNull.Value ? new DateTime?() : Convert.ToDateTime(row["createDate"]))
                    }).ToList();



                    if (q != null)
                    {
                        list.Result = true;
                        list.Data = q;
                    }
                    else
                    {
                        list.Result = false;
                        list.Data = q;
                    }

                    return list;
                }
                catch (Exception ex)
                {
                    new LogFile().WriterError(new LogModel { Exception = ex.Message, Data = model });
                    return new RefundErrorList { Result = false, Message = ex.Message };
                    //hendle exception
                }
            }
        }


	}
}