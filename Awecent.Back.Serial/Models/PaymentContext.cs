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
                    MySqlCommand cmd = new MySqlCommand("awe_storePaymentGetListDealerBackEnd", con);
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


        public RefundErrorList GetPaymentRefundList(ReportRefund model)
        {
            using (MySqlConnection con = new MySqlConnection(PaymentConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("awe_storePaymentGetListTransactionRefund", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add(new MySqlParameter("pi_gameCode", model.gameId));
                    cmd.Parameters.Add(new MySqlParameter("pi_serverCode", model.serverId));
                    cmd.Parameters.Add(new MySqlParameter("pi_userId", model.UID));
                    cmd.Parameters.Add(new MySqlParameter("pi_startDate", model.startDate.Value));
                    cmd.Parameters.Add(new MySqlParameter("pi_endDate", model.endDate.Value));
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
                        productTransactionId = row["productTransactionId"].ToString(),
                        referenceId = row["referenceId"].ToString(),
                        providerName = row["providerName"].ToString(),
                        dealerName = row["dealerName"].ToString(),
                        gameName = row["gameName"].ToString(),
                        serverName = row["serverName"].ToString(),
                        userId = row["userId"].ToString(),
                        pointGame = row["pointGame"].ToString(),
                        pointUnit = row["pointUnit"].ToString(),
                        productCode = row["productCode"].ToString(),
                        paymentTransactionId = row["paymentTransactionId"].ToString(),
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


        public RefundErrorList GetProductRefundList(ReportRefund model)
        {
            using (MySqlConnection con = new MySqlConnection(PaymentConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("awe_storeProductGetListTransactionRefund", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add(new MySqlParameter("pi_gameCode", model.gameId));
                    cmd.Parameters.Add(new MySqlParameter("pi_serverCode", model.serverId));
                    cmd.Parameters.Add(new MySqlParameter("pi_userId", model.UID));
                    cmd.Parameters.Add(new MySqlParameter("pi_startDate", model.startDate.Value));
                    cmd.Parameters.Add(new MySqlParameter("pi_endDate", model.endDate.Value));
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
                        productTransactionId = row["productTransactionId"].ToString(),
                        referenceId = row["referenceId"].ToString(),
                        providerName = row["providerName"].ToString(),
                        dealerName = row["dealerName"].ToString(),
                        gameName = row["gameName"].ToString(),
                        serverName = row["serverName"].ToString(),
                        userId = row["userId"].ToString(),
                        pointGame = row["pointGame"].ToString(),
                        pointUnit = row["pointUnit"].ToString(),
                        productCode = row["productCode"].ToString(),
                        paymentTransactionId = row["paymentTransactionId"].ToString(),
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


        public PaymentTransactionList SearchPaymentTransactionList(PaymentTransaction model)
        {
            using (MySqlConnection con = new MySqlConnection(PaymentConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("awe_storePaymentGetListTransaction", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("pi_gameCode", model.GameCode));
                    cmd.Parameters.Add(new MySqlParameter("pi_serverCode", model.ServerCode));
                    cmd.Parameters.Add(new MySqlParameter("pi_userId", model.UserId));
                    cmd.Parameters.Add(new MySqlParameter("pi_providerId", model.ProviderId));
                    cmd.Parameters.Add(new MySqlParameter("pi_dealerId", model.DealerId));
                    cmd.Parameters.Add(new MySqlParameter("pi_referenceId", model.ReferenceId));
                    cmd.Parameters.Add(new MySqlParameter("pi_startCretranDate", model.StartDate));
                    cmd.Parameters.Add(new MySqlParameter("pi_endCretranDate", model.EndDate));
                    cmd.Parameters.Add(new MySqlParameter("pi_statusTransaction", model.TransactionStatus));
                    cmd.Parameters.Add(new MySqlParameter("pi_page", model.Page));
                    cmd.Parameters.Add(new MySqlParameter("pi_pageSize", model.PageSize));
                    cmd.Parameters.Add(new MySqlParameter("po_countRow", MySqlDbType.Int32) { Direction = ParameterDirection.InputOutput });

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    int row = Convert.ToInt32(cmd.Parameters["po_CountRow"].Value.ToString());
                    if (row <= 0) return new PaymentTransactionList { Result = false };
                    PaymentTransactionList paymentTransactionList = new PaymentTransactionList();
                    paymentTransactionList.Result = true;
                    paymentTransactionList.Total = row;
                    var q = dt.AsEnumerable().Select(x => new PaymentTransaction()
                    {
                        PaymentTransactionId = x.Field<int>("paymentTransactionId"),
                        ReferenceId = x.Field<string>("referenceId"),
                        ProviderName = x.Field<string>("providerName"),
                        DealerName = x.Field<string>("dealerName"),
                        GameName = x.Field<string>("gameName"),
                        ServerName = x.Field<string>("serverName"),
                        UserId = x.Field<string>("userId"),
                        ItemName = x.Field<string>("itemName"),
                        Price = x.Field<string>("price"),
                        CurrencyUnit = x.Field<string>("currencyUnit"),
                        PaymentCode = x.Field<string>("paymentCode"),
                        CustomerIp = x.Field<string>("customerIp"),
                        CreateTransactionDate = x.Field<DateTime?>("createTransactionDate").Value,
                        Other = x.Field<string>("other"),
                        RcvTransactionId = x.Field<string>("rcvTransactionId"),
                        RcvReferenceId = x.Field<string>("rcvReferenceId"),
                        RcvProviderName = x.Field<string>("rcvProviderName"),
                        RcvPaymentCode = x.Field<string>("rcvPaymentCode"),
                        RcvPrice = x.Field<string>("rcvPrice"),
                        RcvCurrencyUnit = x.Field<string>("rcvCurrencyUnit"),
                        RcvTransactionDate = x.Field<DateTime?>("rcvTransactionDate"),
                        RcvRespCode = x.Field<string>("rcvRespCode"),
                        RcvRespMsg = x.Field<string>("rcvRespMsg"),
                        ExceptionCode = x.Field<string>("exceptionCode"),
                        ExceptionMsg = x.Field<string>("exceptionMsg"),
                        TransactionStatus = x.Field<string>("transactionStatus"),
                        CreateUser = x.Field<string>("createUser"),
                        CreateDate = x.Field<DateTime?>("createDate").Value,
                        UpdateUser = x.Field<string>("updateUser"),
                        UpdateDate = x.Field<DateTime?>("updateDate").Value,
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

        public PaymentTransactionList SearchPaymentList(PaymentTransaction model)
        {
            using (MySqlConnection con = new MySqlConnection(PaymentConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("awe_storePaymentGetListPayment", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("pi_gameCode", model.GameCode));
                    cmd.Parameters.Add(new MySqlParameter("pi_serverCode", model.ServerCode));
                    cmd.Parameters.Add(new MySqlParameter("pi_userId", model.UserId));
                    cmd.Parameters.Add(new MySqlParameter("pi_providerId", model.ProviderId));
                    cmd.Parameters.Add(new MySqlParameter("pi_dealerId", model.DealerId));
                    cmd.Parameters.Add(new MySqlParameter("pi_startDate", model.StartDate));
                    cmd.Parameters.Add(new MySqlParameter("pi_endDate", model.EndDate));
                    cmd.Parameters.Add(new MySqlParameter("pi_page", model.Page));
                    cmd.Parameters.Add(new MySqlParameter("pi_pageSize", model.PageSize));
                    cmd.Parameters.Add(new MySqlParameter("po_countRow", MySqlDbType.Int32) { Direction = ParameterDirection.InputOutput });

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    int row = Convert.ToInt32(cmd.Parameters["po_CountRow"].Value.ToString());
                    if (row <= 0) return new PaymentTransactionList { Result = false };
                    PaymentTransactionList paymentTransactionList = new PaymentTransactionList();
                    paymentTransactionList.Result = true;
                    paymentTransactionList.Total = row;
                    var q = dt.AsEnumerable().Select(x => new PaymentTransaction()
                    {
                        GameName = x.Field<string>("gameName"),
                        ServerName = x.Field<string>("serverName"),
                        ProviderName = x.Field<string>("providerName"),
                        DealerName = x.Field<string>("dealerName"),
                        TotalPrice = x.Field<double>("totalPrice"),
                        CurrencyUnit = x.Field<string>("rcvCurrencyUnit"),
                        TransactionDate = x.Field<DateTime?>("rcvTransactionDate").Value,
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