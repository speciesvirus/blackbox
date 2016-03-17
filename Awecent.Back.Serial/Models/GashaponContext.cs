using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Awecent.Back.Serial.Models
{
    public class GashaponContext
    {
        string BackOfficeConnection = WebConfigurationManager.ConnectionStrings["BackOfficeConnection"].ToString();
        string ItemConnection = WebConfigurationManager.ConnectionStrings["ItemConnection"].ToString();

        public GashaponHeaderList GetGashaponHeaderListDDL(GashaponHeader model)
        {
            using (MySqlConnection con = new MySqlConnection(ItemConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("GetGashaponHeaderListDDL", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.Add(new MySqlParameter("_GameID", model.GameId));
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    GashaponHeaderList list = new GashaponHeaderList();
                    list.Result = true;
                    list.Message = "Get Data DDL Success.";
                    var q = dt.AsEnumerable().Select(row => new GashaponHeader()
                    {
                        GashaponHdrId = (int)row["GashaponHdrId"],
                        GashaponName = row["GashaponName"].ToString()
                    }).ToList();

                    list.Data = q;
                    return list;
                }
                catch (Exception ex)
                {
                    new LogFile().WriterError(new LogModel { Data = model, Exception = ex.Message, Function = "GetGashaponHeaderListDDL" });
                    return null;
                }
            }
        }

        public GashaponHeaderList GetGashaponHeaderList(GashaponHeader model)
        {
            using (MySqlConnection con = new MySqlConnection(ItemConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("GetGashaponHeaderList", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("_GameId", model.GameId));
                    cmd.Parameters.Add(new MySqlParameter("_GashaponHdrId", model.GashaponHdrId));
                    cmd.Parameters.Add(new MySqlParameter("_Status", model.StatusId));
                    cmd.Parameters.Add(new MySqlParameter("_Page", model.Page));
                    cmd.Parameters.Add(new MySqlParameter("_PageSize", model.PageSize));
                    cmd.Parameters.Add(new MySqlParameter("_CountRow", MySqlDbType.Int32) { Direction = ParameterDirection.InputOutput });
                    
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    int row = Convert.ToInt32(cmd.Parameters["_CountRow"].Value.ToString());
                    if (row <= 0) return new GashaponHeaderList { Result = false };
                    GashaponHeaderList list = new GashaponHeaderList();
                    list.Result = true;
                    list.Total = row;
                    var q = dt.AsEnumerable().Select(x => new GashaponHeader()
                    {
                        GashaponHdrId = x.Field<int?>("GashaponHdrId"),
                        GameId = x.Field<int?>("GameId"),
                        GameName = x.Field<string>("GameName"),
                        GashaponName = x.Field<string>("GashaponName"),
                        VisibleStartDate = x.Field<DateTime?>("VisibleStartDate").Value,
                        VisibleEndDate = x.Field<DateTime?>("VisibleEndDate").Value,
                        StartDate = x.Field<DateTime?>("StartDate").Value,
                        EndDate = x.Field<DateTime?>("EndDate").Value,
                        StatusId = x.Field<string>("Status"),
                        ReturnTypeId = x.Field<string>("ReturnType"),
                        CreateUser = x.Field<string>("CreateUser"),
                        CreateDate = x.Field<DateTime?>("CreateDate").Value,
                        UpdateUser = x.Field<string>("UpdateUser"),
                        UpdateDate = x.Field<DateTime?>("UpdateDate").Value,
                        free = x.Field<int?>("FreePlay").Value,
                        point = x.Field<int?>("UsedPoint").Value,
                    }).ToList();
                    GashaponHeaderList list2 = new GashaponHeaderList();
                    list.Data = q;
                    return list;
                }
                catch (Exception ex)
                {
                    new LogFile().WriterError(new LogModel { Data = model, Exception = ex.Message, Function = "GashaponHeaderList" });
                    return new GashaponHeaderList { Result = false, Message = ex.Message };
                }

            }
        }

        public GashaponDetailList GetGashaponDetailList(GashaponDetail model)
        {
            using (MySqlConnection con = new MySqlConnection(ItemConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("GetGashaponDetailList", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("_GashaponHdrId", model.GashaponHdrId));
                    cmd.Parameters.Add(new MySqlParameter("_Page", model.Page));
                    cmd.Parameters.Add(new MySqlParameter("_PageSize", model.PageSize));
                    cmd.Parameters.Add(new MySqlParameter("_CountRow", MySqlDbType.Int32) { Direction = ParameterDirection.InputOutput });

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    int row = Convert.ToInt32(cmd.Parameters["_CountRow"].Value.ToString());
                    if (row <= 0) return new GashaponDetailList { Result = false };
                    GashaponDetailList list = new GashaponDetailList();
                    list.Result = true;
                    list.Total = row;
                    var q = dt.AsEnumerable().Select(x => new GashaponDetail()
                    {
                        GashaponDtlId = x.Field<int>("GashaponDtlId"),
                        GashaponHdrId= x.Field<int>("GashaponHdrId"),
                        ItemId = x.Field<int>("ItemId"),
                        ItemName = x.Field<string>("ItemName"),
                        DropRate = x.Field<decimal>("DropRate"),
                        LimitItem = x.Field<int?>("LimitItem"),
                        LimitItemPerId = x.Field<string>("LimitItemPerId"),
                        RunningItem = x.Field<int>("RunningItem"),
                        CreateUser = x.Field<string>("CreateUser"),
                        CreateDate = x.Field<DateTime>("CreateDate"),
                        UpdateUser = x.Field<string>("UpdateUser"),
                        UpdateDate = x.Field<DateTime>("UpdateDate"),
                    }).ToList();
                    list.Data = q;
                    return list;
                }
                catch (Exception ex)
                {
                    new LogFile().WriterError(new LogModel { Data = model, Exception = ex.Message, Function = "GashaponDetailList" });
                    return new GashaponDetailList { Result = false, Message = ex.Message };
                }

            }
        }

        public GashaponHeader CreateGashaponHeader(GashaponHeader model)
        {
            using (MySqlConnection con = new MySqlConnection(ItemConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("awe_createGashaponHeader", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("_GameId", model.GameId));
                    cmd.Parameters.Add(new MySqlParameter("_GashaponName", model.GashaponName));
                    cmd.Parameters.Add(new MySqlParameter("_VisibleStartDate", model.VisibleStartDate.Value));
                    cmd.Parameters.Add(new MySqlParameter("_VisibleEndDate", model.VisibleEndDate.Value));
                    cmd.Parameters.Add(new MySqlParameter("_StartDate", model.StartDate.Value));
                    cmd.Parameters.Add(new MySqlParameter("_EndDate", model.EndDate.Value));
                    cmd.Parameters.Add(new MySqlParameter("_freePlay", model.free));
                    cmd.Parameters.Add(new MySqlParameter("_usedPoint", model.point));

                    cmd.Parameters.Add(new MySqlParameter("_ReturnTypeId", model.ReturnTypeId));
                    cmd.Parameters.Add(new MySqlParameter("_CreateUser", model.CreateUser));

                    cmd.Parameters.Add(new MySqlParameter("_ReturnCode", MySqlDbType.Int32) { Direction = ParameterDirection.InputOutput });
                    cmd.Parameters.Add(new MySqlParameter("_ReturnMsg", MySqlDbType.VarChar) { Direction = ParameterDirection.InputOutput });
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    int code = Convert.ToInt32(cmd.Parameters["_ReturnCode"].Value.ToString());
                    string result = cmd.Parameters["_ReturnMsg"].Value.ToString();

                    if (string.IsNullOrEmpty(result) || code != 200)
                    {
                        model.Result = false; model.Message = "Create Gashapon Header fail : " + result;
                    }
                    else
                    {
                        model.Result = true; model.Message = "Create Gashapon Header success.";
                    }

                    return model;
                }
                catch (Exception ex)
                {
                    new LogFile().WriterError(new LogModel { Data = model, Exception = ex.Message, Function = "CreateGashaponHeader" });
                    model.Result = false; model.Message = ex.Message;
                    return model;
                }
            }

        }

        public GashaponDetail CreateGashaponDetail(GashaponDetail model)
        {
            using (MySqlConnection con = new MySqlConnection(ItemConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("awe_createGashaponDetail", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("_GashaponHdrId", model.GashaponHdrId));
                    cmd.Parameters.Add(new MySqlParameter("_ItemId", model.ItemId));
                    cmd.Parameters.Add(new MySqlParameter("_ItemName", model.ItemName));
                    cmd.Parameters.Add(new MySqlParameter("_DropRate", model.DropRate));
                    cmd.Parameters.Add(new MySqlParameter("_LimitItem", model.LimitItem));
                    cmd.Parameters.Add(new MySqlParameter("_LimitItemPerId", model.LimitItemPerId));
                    cmd.Parameters.Add(new MySqlParameter("_CreateUser", model.CreateUser));

                    cmd.Parameters.Add(new MySqlParameter("_ReturnCode", MySqlDbType.Int32) { Direction = ParameterDirection.InputOutput });
                    cmd.Parameters.Add(new MySqlParameter("_ReturnMsg", MySqlDbType.VarChar) { Direction = ParameterDirection.InputOutput });
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    int code = Convert.ToInt32(cmd.Parameters["_ReturnCode"].Value.ToString());
                    string result = cmd.Parameters["_ReturnMsg"].Value.ToString();

                    if (string.IsNullOrEmpty(result) || code != 200)
                    {
                        model.Result = false; model.Message = "Create Gashapon Detail fail : " + result;
                    }
                    else
                    {
                        model.Result = true; model.Message = "Create Gashapon Detail success.";
                    }

                    return model;
                }
                catch (Exception ex)
                {
                    new LogFile().WriterError(new LogModel { Data = model, Exception = ex.Message, Function = "CreateGashaponHeader" });
                    model.Result = false; model.Message = ex.Message;
                    return model;
                }
            }

        }

        public GashaponHeader SaveChangeGashaponHeader(GashaponHeader model)
        {
            using (MySqlConnection con = new MySqlConnection(ItemConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("awe_updateGashaponHeader", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("_GashaponHdrId", model.GashaponHdrId));
                    cmd.Parameters.Add(new MySqlParameter("_GashaponName", model.GashaponName));
                    cmd.Parameters.Add(new MySqlParameter("_VisibleStartDate", model.VisibleStartDate.Value));
                    cmd.Parameters.Add(new MySqlParameter("_VisibleEndDate", model.VisibleEndDate.Value));
                    cmd.Parameters.Add(new MySqlParameter("_StartDate", model.StartDate.Value));
                    cmd.Parameters.Add(new MySqlParameter("_EndDate", model.EndDate.Value));
                    cmd.Parameters.Add(new MySqlParameter("_StatusId", model.StatusId));
                    cmd.Parameters.Add(new MySqlParameter("_freePlay", model.free));
                    cmd.Parameters.Add(new MySqlParameter("_usedPoint", model.point));
                    cmd.Parameters.Add(new MySqlParameter("_ReturnTypeId", model.ReturnTypeId));
                    cmd.Parameters.Add(new MySqlParameter("_CreateUser", model.CreateUser));

                    cmd.Parameters.Add(new MySqlParameter("_ReturnCode", MySqlDbType.Int32) { Direction = ParameterDirection.InputOutput });
                    cmd.Parameters.Add(new MySqlParameter("_ReturnMsg", MySqlDbType.VarChar) { Direction = ParameterDirection.InputOutput });
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    int code = Convert.ToInt32(cmd.Parameters["_ReturnCode"].Value.ToString());
                    string result = cmd.Parameters["_ReturnMsg"].Value.ToString();

                    if (string.IsNullOrEmpty(result) || code != 200)
                    {
                        model.Result = false; model.Message = "Update Gashapon Header fail : " + result;
                    }
                    else
                    {
                        model.Result = true; model.Message = "Update Gashapon Header success.";
                    }

                    return model;
                }
                catch (Exception ex)
                {
                    new LogFile().WriterError(new LogModel { Data = model, Exception = ex.Message, Function = "CreateGashaponHeader" });
                    model.Result = false; model.Message = ex.Message;
                    return model;
                }
            }
        }

        public GashaponDetail SaveChangeGashaponDetail(GashaponDetail model)
        {
            using (MySqlConnection con = new MySqlConnection(ItemConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("awe_updateGashaponDetail", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("_GashaponDtlId", model.GashaponDtlId));
                    cmd.Parameters.Add(new MySqlParameter("_ItemId", model.ItemId));
                    cmd.Parameters.Add(new MySqlParameter("_ItemName", model.ItemName));
                    cmd.Parameters.Add(new MySqlParameter("_DropRate", model.DropRate));
                    cmd.Parameters.Add(new MySqlParameter("_LimitItem", model.LimitItem));
                    cmd.Parameters.Add(new MySqlParameter("_LimitItemPerId", model.LimitItemPerId));
                    cmd.Parameters.Add(new MySqlParameter("_CreateUser", model.CreateUser));

                    cmd.Parameters.Add(new MySqlParameter("_ReturnCode", MySqlDbType.Int32) { Direction = ParameterDirection.InputOutput });
                    cmd.Parameters.Add(new MySqlParameter("_ReturnMsg", MySqlDbType.VarChar) { Direction = ParameterDirection.InputOutput });
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    int code = Convert.ToInt32(cmd.Parameters["_ReturnCode"].Value.ToString());
                    string result = cmd.Parameters["_ReturnMsg"].Value.ToString();

                    if (string.IsNullOrEmpty(result) || code != 200)
                    {
                        model.Result = false; model.Message = "Create Gashapon Detail fail : " + result;
                    }
                    else
                    {
                        model.Result = true; model.Message = "Create Gashapon Detail success.";
                    }

                    return model;
                }
                catch (Exception ex)
                {
                    new LogFile().WriterError(new LogModel { Data = model, Exception = ex.Message, Function = "CreateGashaponHeader" });
                    model.Result = false; model.Message = ex.Message;
                    return model;
                }
            }

        }

        public GashaponDetail DeleteGashapon(string ItemIdListString)
        {
            GashaponDetail model = new GashaponDetail();
            using (MySqlConnection con = new MySqlConnection(ItemConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("awe_deleteItemGashapon", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("_GashaponHdrId", ItemIdListString));

                    cmd.Parameters.Add(new MySqlParameter("_ReturnCode", MySqlDbType.Int32) { Direction = ParameterDirection.InputOutput });
                    cmd.Parameters.Add(new MySqlParameter("_ReturnMsg", MySqlDbType.VarChar) { Direction = ParameterDirection.InputOutput });
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    int code = Convert.ToInt32(cmd.Parameters["_ReturnCode"].Value.ToString());
                    string result = cmd.Parameters["_ReturnMsg"].Value.ToString();

                    
                    if (string.IsNullOrEmpty(result) || code != 200)
                    {
                        model.Result = false; model.Message = "Delete Gashapon Detail fail.";
                    }
                    else
                    {
                        model.Result = true; model.Message = "Delete Gashapon Detail success.";
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
    }
}