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

        public PaymentTransactionList SearchPaymentTransactionList(PaymentTransaction model)
        {
            using (MySqlConnection con = new MySqlConnection(PaymentConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("GetGashaponHeaderList", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("pi_referenceId", model.ReferenceId));
                    cmd.Parameters.Add(new MySqlParameter("pi_gameCode", model.GameCode));
                    cmd.Parameters.Add(new MySqlParameter("pi_serverCode", model.ServerCode));
                    cmd.Parameters.Add(new MySqlParameter("pi_userId", model.UserId));
                    cmd.Parameters.Add(new MySqlParameter("pi_providerId", model.ProviderId));
                    cmd.Parameters.Add(new MySqlParameter("pi_dealerId", model.DealerId));
                    cmd.Parameters.Add(new MySqlParameter("pi_startDate", model.StartDate));
                    cmd.Parameters.Add(new MySqlParameter("pi_endDate", model.EndDate));
                    cmd.Parameters.Add(new MySqlParameter("pi_statusTransaction", model.TransactionStatus));
                    cmd.Parameters.Add(new MySqlParameter("pi_Page", model.Page));
                    cmd.Parameters.Add(new MySqlParameter("pi_PageSize", model.PageSize));
                    cmd.Parameters.Add(new MySqlParameter("po_CountRow", MySqlDbType.Int32) { Direction = ParameterDirection.InputOutput });

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    int row = Convert.ToInt32(cmd.Parameters["_CountRow"].Value.ToString());
                    if (row <= 0) return new PaymentTransactionList { Result = false };
                    PaymentTransactionList paymentTransactionList = new PaymentTransactionList();
                    paymentTransactionList.Result = true;
                    paymentTransactionList.Total = row;
                    var q = dt.AsEnumerable().Select(x => new PaymentTransaction()
                    {
                        ReferenceId = x.Field<string>("GashaponHdrId"),
                        ProviderName = x.Field<string>("GameId"),
                        DealerName = x.Field<string>("GameName"),
                        ServerName = x.Field<string>("GashaponName"),
                        RcvPaymentCode = x.Field<string>("VisibleStartDate"),
                        RcvPrice = x.Field<string>("VisibleEndDate"),
                        RcvCurrencyUnit = x.Field<string>("StartDate"),
                        RcvRespCode = x.Field<string>("EndDate"),
                        RcvRespMsg = x.Field<string>("Status"),
                        ExceptionCode = x.Field<string>("ReturnType"),
                        ExceptionMsg = x.Field<string>("CreateUser"),
                        TransactionStatus = x.Field<string>("CreateDate"),
                        CreateUser = x.Field<string>("UpdateUser"),
                        CreateDate = x.Field<DateTime?>("UpdateDate").Value,
                        UpdateUser = x.Field<string>("FreePlay"),
                        UpdateDate = x.Field<DateTime?>("UsedPoint").Value,
                    }).ToList();
                    paymentTransactionList.Data = q;
                    return paymentTransactionList;
                }
                catch (Exception ex)
                {
                    new LogFile().WriterError(new LogModel { Data = model, Exception = ex.Message, Function = "SearchPaymentTransactionList" });
                    return new PaymentTransactionList { Result = false, Message = ex.Message };
                }
            }
        }
	}
}