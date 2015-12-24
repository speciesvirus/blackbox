
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
    public class MySQLContext
    {
        //string DefaultConnection = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        string BackOfficeConnection = WebConfigurationManager.ConnectionStrings["BackOfficeConnection"].ToString();
        string ItemConnection = WebConfigurationManager.ConnectionStrings["ItemConnection"].ToString();

        #region report

        public PromotionsList GetPromotions(string id) {
            using (MySqlConnection con = new MySqlConnection(ItemConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("awe_getPromotionName", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("_GameID",id));
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    PromotionsList list = new PromotionsList();
                    var q = dt.AsEnumerable().Select(row => new Promotion()
                    {
                        ID = Convert.ToInt32(row["idx"].ToString()),
                        Name = row["vPromotionName"].ToString()
                    }).ToList();
                    list.data = q;
                    if (q != null) list.result = true;
                    else list.result = false;
                    return list;

                }
                catch (Exception ex)
                {
                    new LogFile().WriterError(new LogModel { Exception = ex.Message, Function = "GetPromotions" });
                    return new PromotionsList { result = false };
                }
            }

        }

        public ItemCodeList GetItemCode(ReportItemCodeInput model) {
            using (MySqlConnection con = new MySqlConnection(ItemConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("awe_getItemCodeUsed", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("_GameId", model.gameId));
                    cmd.Parameters.Add(new MySqlParameter("_StartDate", model.startDate.Value));
                    cmd.Parameters.Add(new MySqlParameter("_EndDate", model.endDate.Value));
                    cmd.Parameters.Add(new MySqlParameter("_PromotionID", model.promotionId));
                    //_PageSize , _Page , _row , _PromotionID
                    cmd.Parameters.Add(new MySqlParameter("_PageSize", model.pageSize));
                    cmd.Parameters.Add(new MySqlParameter("_Page", model.page));
                    cmd.Parameters.Add(new MySqlParameter("_rows", MySqlDbType.Int32) { Direction = ParameterDirection.InputOutput });
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    ItemCodeList list = new ItemCodeList();
                    list.Total = Convert.ToInt32(cmd.Parameters["_rows"].Value.ToString());

                    var q = dt.AsEnumerable().Select(row => new ItemCode()
                    {
                        PromotionID = Convert.ToInt64(row["iPromotionID"] == null ? "0" : row["iPromotionID"].ToString()),
                        LotID = Convert.ToInt64(row["iLotID"].ToString()),
                        Code = row["vCode"].ToString(),
                        IsUsed = row["cIsUse"].ToString(),
                        FacebookID = row["userID"].ToString(),
                        TimeUse = row["dtUsedDate"].ToString(),
                        TimeCreate = row["dtCreateDate"].ToString(),
                        CreateBy = row["dtCreateUser"].ToString(),
                    }).ToList();

                    if (q != null)
                    {
                        list.Result = true;
                        list.Data = q;
                    }
                    else {
                        list.Result = false;
                        list.Data = q; 
                    }

                    return list;
                }
                catch (Exception ex)
                {
                    new LogFile().WriterError(new LogModel { Exception = ex.Message, Data = model });
                    return new ItemCodeList{ Result = false  ,Message  = ex.Message };
                    //hendle exception
                }
            }
        }

        #endregion

        #region Auth
        public UserandRole ValidateAccount(string email, string password)
        {
            var encrypassword = Encryption.HMACSHA256(password);
            new LogFile().Writer(new LogModel { Data = new LoginViewModel { Email = email, Password = encrypassword }, Function = "ValidateAccount" });

            using (MySqlConnection con = new MySqlConnection(BackOfficeConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("ValidateAccountAndGetRoleName", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("_Email", email));
                    cmd.Parameters.Add(new MySqlParameter("_Password", encrypassword));
                    cmd.Parameters.Add(new MySqlParameter("_UserId", MySqlDbType.Int64) { Direction = ParameterDirection.InputOutput });
                    cmd.Parameters.Add(new MySqlParameter("_Role", MySqlDbType.VarChar) { Direction = ParameterDirection.InputOutput });
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    UserandRole user = new UserandRole();
                    string strId = cmd.Parameters["_UserId"].Value.ToString();
                    if (string.IsNullOrEmpty(strId)) { return null; }
                    user.UserId = Convert.ToInt32(cmd.Parameters["_UserId"].Value.ToString());
                    user.UserName = email;
                    user.UserRole = cmd.Parameters["_Role"].Value.ToString();

                    new LogFile().Writer(new LogModel { Data = user , Function = "ValidateAccount" });

                    var q = dt.AsEnumerable().Select(row => new Game()
                    {
                        GameId = row["GameId"].ToString(),
                        GameName = row["GameName"].ToString()
                    }).ToList();


                    user.UserGameList = q;

                    return user;
                }
                catch (Exception ex)
                {
                    new LogFile().WriterError(new LogModel { Data = new LoginViewModel { Email = email, Password = encrypassword } , Exception = ex.Message , Function = "ValidateAccount" });
                    return null;
                    //hendle exception
                }
            }

        }


        public UserandRole ValidateAccountServer(string email, string password)
        {
            var encrypassword = Encryption.HMACSHA256(password);
            new LogFile().Writer(new LogModel { Data = new LoginViewModel { Email = email, Password = encrypassword }, Function = "ValidateAccount" });

            using (MySqlConnection con = new MySqlConnection(BackOfficeConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("ValidateAccountAndGetRoleNameAndServer", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("_Email", email));
                    cmd.Parameters.Add(new MySqlParameter("_Password", encrypassword));
                    cmd.Parameters.Add(new MySqlParameter("_UserId", MySqlDbType.Int64) { Direction = ParameterDirection.InputOutput });
                    cmd.Parameters.Add(new MySqlParameter("_Role", MySqlDbType.VarChar) { Direction = ParameterDirection.InputOutput });
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    UserandRole user = new UserandRole();
                    string strId = cmd.Parameters["_UserId"].Value.ToString();
                    if (string.IsNullOrEmpty(strId)) { return null; }
                    user.UserId = Convert.ToInt32(cmd.Parameters["_UserId"].Value.ToString());
                    user.UserName = email;
                    user.UserRole = cmd.Parameters["_Role"].Value.ToString();

                    new LogFile().Writer(new LogModel { Data = user, Function = "ValidateAccount" });

                    var q = dt.AsEnumerable().Select(row => new Game()
                    {
                        GameId = row["GameId"].ToString(),
                        GameName = row["GameName"].ToString(),
                        ServerId = (int)row["ServerId"],
                        ServerName = row["ServerName"].ToString()
                    }).ToList();


                    user.UserGameList = q;

                    return user;
                }
                catch (Exception ex)
                {
                    new LogFile().WriterError(new LogModel { Data = new LoginViewModel { Email = email, Password = encrypassword }, Exception = ex.Message, Function = "ValidateAccount" });
                    return null;
                    //hendle exception
                }
            }

        }

        #endregion
    }
}