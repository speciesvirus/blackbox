using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Awecent.Back.Serial.Models
{
    public class MasterCodeContext
    {
        string BackOfficeConnection = WebConfigurationManager.ConnectionStrings["BackOfficeConnection"].ToString();
        string ItemConnection = WebConfigurationManager.ConnectionStrings["ItemConnection"].ToString();

        #region MasterCode

        public MasterCodeList GetMasterCode(MasterCode model)
        {
            using (MySqlConnection con = new MySqlConnection(ItemConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("ValidateAccountAndGetRoleName", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    cmd.Parameters.Add(new MySqlParameter("_Password", ""));
                    cmd.Parameters.Add(new MySqlParameter("_UserId", MySqlDbType.Int64) { Direction = ParameterDirection.InputOutput });
                    cmd.Parameters.Add(new MySqlParameter("_Role", MySqlDbType.VarChar) { Direction = ParameterDirection.InputOutput });
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    string strId = cmd.Parameters["_UserId"].Value.ToString();

                    //new LogFile().Writer(new LogModel { Data = model, Function = "ValidateAccount" });

                    var q = dt.AsEnumerable().Select(row => new MasterCode()
                    {
                        PromotionID = (int)row["GameId"],
                        PromotionName = row["PromotionName"].ToString()
                    }).ToList();

                   // user.UserGameList = q;

                    return null;
                }
                catch (Exception ex)
                {
                    new LogFile().WriterError(new LogModel { Data = model, Exception = ex.Message, Function = "GetMasterCode" });
                    return null;
                    //hendle exception
                }
            }

        }


        public MasterCode CreateMasterCode(MasterCode model, string userName)
        {
            using (MySqlConnection con = new MySqlConnection(ItemConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("awe_generateMasterCode", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("_GameID", model.GameID));
                    cmd.Parameters.Add(new MySqlParameter("_PromotionName", model.PromotionName));
                    cmd.Parameters.Add(new MySqlParameter("_PromotionDetail", model.PromotionDescription));
                    cmd.Parameters.Add(new MySqlParameter("_SerailFlag", model.SerialType));
                    //_StartDate,_EndDate,_ItemID,_CreateUser,_GenerateType,_PrefixCode
                    cmd.Parameters.Add(new MySqlParameter("_StartDate", model.StartDate.Value));
                    cmd.Parameters.Add(new MySqlParameter("_EndDate", model.EndDate.Value));
                    cmd.Parameters.Add(new MySqlParameter("_ItemID", model.ItemID));
                    cmd.Parameters.Add(new MySqlParameter("_CreateUser", userName));
                    cmd.Parameters.Add(new MySqlParameter("_GenerateType", model.GenerateType));
                    cmd.Parameters.Add(new MySqlParameter("_PrefixCode", model.SerialPrefix));

                    cmd.Parameters.Add(new MySqlParameter("_ReturnCode", MySqlDbType.Int32) { Direction = ParameterDirection.InputOutput });
                    cmd.Parameters.Add(new MySqlParameter("_ReturnMsg", MySqlDbType.VarChar) { Direction = ParameterDirection.InputOutput });
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    int code = Convert.ToInt32(cmd.Parameters["_ReturnCode"].Value.ToString());
                    string result = cmd.Parameters["_ReturnMsg"].Value.ToString();

                    if (string.IsNullOrEmpty(result) || code != 200)
                    {
                        model.Result = false; model.Message = "Create Master Code fail.";
                    }
                    else {
                        model.Result = true; model.Code = result; model.Message = "Create Master Code success.";
                    }

                    return model;
                }
                catch (Exception ex)
                {
                    new LogFile().WriterError(new LogModel { Data = model, Exception = ex.Message, Function = "GetMasterCode" });
                    model.Result = false; model.Message = ex.Message;
                    return model;
                }
            }

        }


        #endregion



    }
}