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

        public MasterCode CreateLotItemCode(MasterCode model)
        {
            using (MySqlConnection con = new MySqlConnection(ItemConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("awe_generateLot", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("_PromotionId", model.PromotionID));
                    cmd.Parameters.Add(new MySqlParameter("_CodeAmount", model.CodeAmount));
                    cmd.Parameters.Add(new MySqlParameter("_CreateUser", model.CreateBy));
                    cmd.Parameters.Add(new MySqlParameter("_ReturnCode", MySqlDbType.Int32) { Direction = ParameterDirection.InputOutput });
                    cmd.Parameters.Add(new MySqlParameter("_ReturnMsg", MySqlDbType.VarChar) { Direction = ParameterDirection.InputOutput });
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    int code = int.Parse(cmd.Parameters["_ReturnCode"].Value.ToString());
                    if (code == 200)
                    {
                        model.Result = true;
                        model.Lot = cmd.Parameters["_ReturnMsg"].Value.ToString();
                        return model;
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    new LogFile().WriterError(new LogModel { Data = model, Exception = ex.Message, Function = "CreateLotItemCode" });
                    return null;
                    //hendle exception
                }
            }

        }

        public MasterCode CreateMasterCode(MasterCode model)
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
                    //_StartDate,_EndDate,_ItemID,_CreateUser,_GenerateType,_PrefixCode,_ContentUrl
                    cmd.Parameters.Add(new MySqlParameter("_StartDate", model.StartDate.Value));
                    cmd.Parameters.Add(new MySqlParameter("_EndDate", model.EndDate.Value));
                    cmd.Parameters.Add(new MySqlParameter("_ItemID", model.ItemID));
                    cmd.Parameters.Add(new MySqlParameter("_CreateUser", model.CreateUser));
                    cmd.Parameters.Add(new MySqlParameter("_GenerateType", model.GenerateType));
                    cmd.Parameters.Add(new MySqlParameter("_PrefixCode", model.SerialPrefix));
                    cmd.Parameters.Add(new MySqlParameter("_ContentUrl", model.URLShared));

                    cmd.Parameters.Add(new MySqlParameter("_ReturnCode", MySqlDbType.Int32) { Direction = ParameterDirection.InputOutput });
                    cmd.Parameters.Add(new MySqlParameter("_ReturnMsg", MySqlDbType.VarChar) { Direction = ParameterDirection.InputOutput });
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    int code = Convert.ToInt32(cmd.Parameters["_ReturnCode"].Value.ToString());
                    string result = cmd.Parameters["_ReturnMsg"].Value.ToString();

                    if (string.IsNullOrEmpty(result) || code != 200)
                    {
                        model.Result = false; model.Message = "Create Master Code fail " + result;
                    }
                    else
                    {
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

        public MasterCode SaveChangeMasterCode(MasterCode model)
        {
            using (MySqlConnection con = new MySqlConnection(ItemConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("awe_editMasterCode", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("_PromotionID", model.PromotionID));
                    cmd.Parameters.Add(new MySqlParameter("_PromotionName", model.PromotionName));
                    cmd.Parameters.Add(new MySqlParameter("_PromotionDetail", model.PromotionDescription));
                    cmd.Parameters.Add(new MySqlParameter("_SerailFlag", model.SerialType));
                    //_StartDate,_EndDate,_ItemID,_CreateUser,_GenerateType,_PrefixCode
                    cmd.Parameters.Add(new MySqlParameter("_StartDate", model.StartDate.Value));
                    cmd.Parameters.Add(new MySqlParameter("_EndDate", model.EndDate.Value));
                    cmd.Parameters.Add(new MySqlParameter("_ItemID", model.ItemID));
                    cmd.Parameters.Add(new MySqlParameter("_UpdateUser", model.CreateUser));
                    cmd.Parameters.Add(new MySqlParameter("_GenerateType", model.GenerateType));
                    cmd.Parameters.Add(new MySqlParameter("_PrefixCode", model.SerialPrefix));
                    cmd.Parameters.Add(new MySqlParameter("_ContentUrl", model.URLShared));

                    cmd.Parameters.Add(new MySqlParameter("_ReturnCode", MySqlDbType.Int32) { Direction = ParameterDirection.InputOutput });
                    cmd.Parameters.Add(new MySqlParameter("_ReturnMsg", MySqlDbType.VarChar) { Direction = ParameterDirection.InputOutput });
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    int code = Convert.ToInt32(cmd.Parameters["_ReturnCode"].Value.ToString());
                    string result = cmd.Parameters["_ReturnMsg"].Value.ToString();

                    if (string.IsNullOrEmpty(result) || code != 200)
                    {
                        model.Result = false; model.Message = "Update Master Code fail.";
                    }
                    else
                    {
                        model.Result = true; model.Code = result; model.Message = "Update Master Code success.";
                    }

                    return model;
                }
                catch (Exception ex)
                {
                    new LogFile().WriterError(new LogModel { Data = model, Exception = ex.Message, Function = "SaveChangeMasterCode" });
                    model.Result = false; model.Message = ex.Message;
                    return model;
                }
            }

        }

        public MasterCode DeleteMasterCode(MasterCode model)
        {
            using (MySqlConnection con = new MySqlConnection(ItemConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("awe_deleteMasterCode", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("_PromotionID", model.PromotionID));
                    cmd.Parameters.Add(new MySqlParameter("_UserName", model.CreateUser));

                    cmd.Parameters.Add(new MySqlParameter("_ReturnCode", MySqlDbType.Int32) { Direction = ParameterDirection.InputOutput });
                    cmd.Parameters.Add(new MySqlParameter("_ReturnMsg", MySqlDbType.VarChar) { Direction = ParameterDirection.InputOutput });
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    int code = Convert.ToInt32(cmd.Parameters["_ReturnCode"].Value.ToString());
                    string result = cmd.Parameters["_ReturnMsg"].Value.ToString();

                    if (string.IsNullOrEmpty(result) || code != 200)
                    {
                        model.Result = false; model.Message = "Delete Master Code fail.";
                    }
                    else
                    {
                        model.Result = true; model.Code = result; model.Message = "Delete Master Code success.";
                    }

                    return model;
                }
                catch (Exception ex)
                {
                    new LogFile().WriterError(new LogModel { Data = model, Exception = ex.Message, Function = "DeleteMasterCode" });
                    model.Result = false; model.Message = ex.Message;
                    return model;
                }
            }

        }

        public MasterCode PendingAndActiveMasterCode(MasterCode model)
        {
            using (MySqlConnection con = new MySqlConnection(ItemConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("awe_pendingAndActiveMasterCode", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("_PromotionID", model.PromotionID));
                    cmd.Parameters.Add(new MySqlParameter("_UserName", model.CreateUser));
                    cmd.Parameters.Add(new MySqlParameter("_ReturnCode", MySqlDbType.Int32) { Direction = ParameterDirection.InputOutput });
                    cmd.Parameters.Add(new MySqlParameter("_ReturnMsg", MySqlDbType.VarChar) { Direction = ParameterDirection.InputOutput });
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    int code = Convert.ToInt32(cmd.Parameters["_ReturnCode"].Value.ToString());
                    string result = cmd.Parameters["_ReturnMsg"].Value.ToString();

                    if (string.IsNullOrEmpty(result) || code != 200)
                    {
                        model.Result = false; model.Message = "Update Master Code fail.";
                    }
                    else
                    {
                        model.Result = true;
                        model.Code = result;
                        model.Message = "Update Master Code success.";
                    }

                    return model;
                }
                catch (Exception ex)
                {
                    new LogFile().WriterError(new LogModel { Data = model, Exception = ex.Message, Function = "PendingMasterCode" });
                    model.Result = false; model.Message = ex.Message;
                    return model;
                }
            }

        }

        public MasterCodeList GetMasterCodeList(MasterCode model)
        {
            using (MySqlConnection con = new MySqlConnection(ItemConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("awe_getPromotionByGameId", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("_GameID", model.GameID));
                    cmd.Parameters.Add(new MySqlParameter("_Page", model.Page));
                    cmd.Parameters.Add(new MySqlParameter("_PageSize", model.PageSize));

                    cmd.Parameters.Add(new MySqlParameter("_row", MySqlDbType.Int32) { Direction = ParameterDirection.InputOutput });
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    int row = Convert.ToInt32(cmd.Parameters["_row"].Value.ToString());
                    if (row <= 0) return new MasterCodeList { Result = false };
                    MasterCodeList list = new MasterCodeList();
                    list.Result = true;
                    list.Total = row;
                    var q = dt.AsEnumerable().Select(x => new MasterCode()
                    {
                        GameID = x.Field<int?>("GameID"),
                        PromotionID = x.Field<long?>("PromotionID") == null ? 0 : x.Field<long?>("PromotionID"),
                        PromotionName = x.Field<string>("PromotionName"),
                        PromotionDescription = x.Field<string>("PromotionDescription"),
                        SerialType = x.Field<string>("SerialType"),
                        StartDate = x.Field<DateTime?>("StartDate").Value,
                        EndDate = x.Field<DateTime?>("EndDate").Value,
                        Status = x.Field<string>("Status"),
                        ItemID = x.Field<long>("ItemID") == null ? "" : x.Field<long>("ItemID").ToString(),
                        GenerateType = x.Field<string>("GenerateType"),
                        SerialPrefix = x.Field<string>("SerialPrefix"),
                        CodeAmount = x.Field<int?>("CodeAmount") == null ? 0 : x.Field<int?>("CodeAmount"),
                        URLShared = x.Field<string>("URLShared"),
                        CreateUser = x.Field<string>("CreateUser"),
                        Code = x.Field<string>("Code"),
                        CreateDate = x.Field<DateTime?>("CreateDate").Value,
                    }).ToList();
                    list.Data = q;
                    return list;
                }
                catch (Exception ex)
                {
                    new LogFile().WriterError(new LogModel { Data = model, Exception = ex.Message, Function = "GetMasterCodeList" });
                    return new MasterCodeList { Result = false, Message = ex.Message };
                }

            }
        }

        public MasterCode InsertExecute(string sql)
        {
            using (MySqlConnection con = new MySqlConnection(ItemConnection))
            {
                MySqlTransaction tx = null;
                try
                {
                    MySqlCommand cmd = new MySqlCommand();
                    con.Open();
                    tx = con.BeginTransaction();
                    cmd.Connection = con;
                    cmd.Transaction = tx;
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    tx.Commit();
                    return new MasterCode { Result = true , Message="Import success." };
                }
                catch (Exception ex) {
                    new LogFile().WriterError(new LogModel { Exception = ex.Message, Function = "InsertExecute" });
                    tx.Rollback();
                    return new MasterCode { Result = false, Message =ex.Message};
                }
            }
        }

    }
}