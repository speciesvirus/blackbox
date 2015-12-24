using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Awecent.Back.Serial.Models
{
    public class MemberContext
    {
        string ReportConnection = WebConfigurationManager.ConnectionStrings["ReportConnection"].ToString();


        public ReportActiveUser GetActiveUser(ReportActiveUser model)
        {
            using (MySqlConnection con = new MySqlConnection(ReportConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("awe_activeUserHour", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("_gameID", model.gameID));
                    cmd.Parameters.Add(new MySqlParameter("_dateStart", model.timeStart));
                    cmd.Parameters.Add(new MySqlParameter("_dateEnd", model.timeEnd));
                    cmd.Parameters.Add(new MySqlParameter("_serverID", model.serverID));

                    cmd.Parameters.Add(new MySqlParameter("_ReturnCode", MySqlDbType.Int32) { Direction = ParameterDirection.InputOutput });
                    cmd.Parameters.Add(new MySqlParameter("_ReturnMsg", MySqlDbType.VarChar) { Direction = ParameterDirection.InputOutput });
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    int code = Convert.ToInt32(cmd.Parameters["_ReturnCode"].Value);
                    int count = dt.Rows.Count;
                    ReportActiveUser list = new ReportActiveUser();

                    if (code == 200)
                    {
                        var q = dt.AsEnumerable().Select(row => new ActiveUserList()
                        {
                            gameID = row["_ReturnId"] == null ? 0 : Convert.ToInt32(row["_ReturnId"]),
                            serverID = row["_ReturnServerId"] == null ? 0 : Convert.ToInt32(row["_ReturnServerId"]),
                            count = row["_ReturnCount"] == null ? 0 : int.Parse(row["_ReturnCount"].ToString()),
                            curTime = row.Field<DateTime?>("_ReturnDate").Value
                            //(DateTime?)(row["_ReturnDate"] == DBNull.Value ? new DateTime?() : Convert.ToDateTime(row["_ReturnDate"])),
                            //row.Field<DateTime?>("_ReturnDate").Value
                        }).ToList();
                        list.result = true;
                        list.data = q;
                        return list;
                    }
                    else
                    {
                        list.result = false;
                        list.message = "0 rows";
                        return list;
                    }
//Convert.ToInt32(cmd.Parameters["_rows"].Value.ToString());
                    

                }
                catch (Exception ex)
                {
                    return new ReportActiveUser { message = ex.Message ,result = false };
                }
            }
        }


        public ReportActiveUser GetRegisterUser(ReportActiveUser model)
        {
            using (MySqlConnection con = new MySqlConnection(ReportConnection))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand("awe_registerUserNew", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("_gameID", model.gameID));
                    cmd.Parameters.Add(new MySqlParameter("_serverID", model.serverID));
                    cmd.Parameters.Add(new MySqlParameter("_dateStart", model.timeStart));
                    cmd.Parameters.Add(new MySqlParameter("_dateEnd", model.timeEnd));
                    //cmd.Parameters.Add(new MySqlParameter("_page", model._page));
                    //cmd.Parameters.Add(new MySqlParameter("_pageSize", model._pageSize));


                    cmd.Parameters.Add(new MySqlParameter("_sumCountActive", MySqlDbType.Int32) { Direction = ParameterDirection.InputOutput });
                    cmd.Parameters.Add(new MySqlParameter("_sumCountRegister", MySqlDbType.Int32) { Direction = ParameterDirection.InputOutput });
                    cmd.Parameters.Add(new MySqlParameter("_ReturnCode", MySqlDbType.Int32) { Direction = ParameterDirection.InputOutput });
                    cmd.Parameters.Add(new MySqlParameter("_ReturnMsg", MySqlDbType.VarChar) { Direction = ParameterDirection.InputOutput });
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);


                    int code = Convert.ToInt32(cmd.Parameters["_ReturnCode"].Value);
                    int count = dt.Rows.Count;

                    ReportActiveUser list = new ReportActiveUser();
                    list._sumCountActive = Convert.ToInt32(cmd.Parameters["_sumCountActive"].Value);
                    list._sumCountRegister = Convert.ToInt32(cmd.Parameters["_sumCountRegister"].Value);

                    if (code == 200)
                    {

                        var q = dt.AsEnumerable().Select(row => new ActiveUserList()
                        {
                            gameID = row["_ReturnId"] == null ? 0 : Convert.ToInt32(row["_ReturnId"]),

                            countActiveFacebook = row["_ReturnActiveFacebook"] == null ? 0 : int.Parse(row["_ReturnActiveFacebook"].ToString()),
                            countActiveGuest = row["_ReturnActiveGuest"] == null ? 0 : int.Parse(row["_ReturnActiveGuest"].ToString()),
                            countActiveTotal = row["_ReturnActiveTotal"] == null ? 0 : int.Parse(row["_ReturnActiveTotal"].ToString()),

                            countRegisterFacebook = row["_ReturnRegisterFacebook"] == null ? 0 : int.Parse(row["_ReturnRegisterFacebook"].ToString()),
                            countRegisterGuest = row["_ReturnRegisterGuest"] == null ? 0 : int.Parse(row["_ReturnRegisterGuest"].ToString()),
                            countRegisterTotal = row["_ReturnRegisterTotal"] == null ? 0 : int.Parse(row["_ReturnRegisterTotal"].ToString()),

                            //curTime = row.Field<DateTime?>("Date_Active").Value
                            curTime = (DateTime?)(row["_ReturnDate"] == DBNull.Value ? new DateTime?() : Convert.ToDateTime(row["_ReturnDate"])),
                        }).ToList();
                        list.result = true;
                        list.data = q;
                        return list;
                    }
                    else
                    {
                        list.result = false;
                        list.message = "0 rows";
                        return list;
                    }
                    //Convert.ToInt32(cmd.Parameters["_rows"].Value.ToString());


                }
                catch (Exception ex)
                {
                    return new ReportActiveUser { message = ex.Message, result = false };
                }
            }
        }



    }
}