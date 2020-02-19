using MySql.Data.MySqlClient;
using RMapi.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RMapi.Models
{
    public class Driver_Withdrawal_Request
    {
        public int WITHREQID {get; set;}
        public int REQUESTEDAMOUNT {get; set;}
        public DateTime DATEREQ {get; set;}
        public char REQSTATUS {get; set;}
        public int DRIVERID {get; set;}

    }
    public class DWithdrawalManager : BaseManager
    {
        public static List<Driver_Withdrawal_Request> GetWithReq(string whereclause, MySqlConnection conn = null)
        {
            Driver_Withdrawal_Request objWReq = null;
            List<Driver_Withdrawal_Request> lstWReq = new List<Driver_Withdrawal_Request>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql = "select * from Driver_Withdrawal_Request";
                if (!string.IsNullOrEmpty(whereclause))
                    sql += " where " + whereclause;
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                objWReq = ReaderDataUser(reader);
                                lstWReq.Add(objWReq);
                            }
                        }
                        else
                        {
                        }
                    }
                    if (isConnArgNull == true)
                    {
                        connection.Dispose();
                    }


                }
            }
            //endtry
            catch (Exception ex)
            {

            }
            return lstWReq;
        }


        private static User ReaderDataWithReq(MySqlDataReader reader)
        {

            User objWReq = new User();
            objWReq.WITHREQID = Converter.IsValidInt(reader["WITHREQID"]);
            objWReq.REQUESTEDAMOUNT = Converter.IsValidString(reader["REQUESTEDAMOUNT"]);
            objWReq.DATEREQ = Converter.IsValidString(reader["DATEREQ"]);
            objWReq.REQSTATUS = Converter.IsValidString(reader["REQSTATUS"]);
            objWReq.DRIVERID = Converter.IsValidInt(reader["DRIVERID"]);


            return objWReq;

        }

        public static string SaveUser(Driver_Withdrawal_Request objWReq, MySqlConnection conn = null, MySqlTransaction trans = null)
        {
            string returnMessage = "";
            string sWITHREQID = "";
            sWITHREQID = objWReq.WITHREQID.ToString();
            var templstWReq = GetUser("WITHREQID = '" + sWITHREQID + "'", conn);
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                using (MySqlCommand command = new MySqlCommand())
                {
                    string sql;
                    bool isEdit = true;
                    if (templstWReq.Count <= 0)
                    {
                        isEdit = false;
                        sql = @"INSERT INTO DRIVER_WITHDRAWAL_REQUEST (
                                                    
                                                  REQUESTEDAMOUNT,
                                                  DATEREQ,
                                                  REQSTATUS,
                                                  DRIVERID

                                                 
                                                    )
                                                    VALUES(
                                                     @REQUESTEDAMOUNT,
                                                  @DATEREQ,
                                                  @REQSTATUS,
                                                  @DRIVERID
                                                    )";
                    }
                    else
                    {
                        sql = @"Update DRIVER_WITHDRAWAL_REQUEST set
                                                  WITHREQID=@WITHREQID,
                                                  REQUESTEDAMOUNT=@REQUESTEDAMOUNT,
                                                  DATEREQ=@DATEREQ,
                                                  REQSTATUS=@REQSTATUS,
                                                  DRIVERID=@DRIVERID
                                                    Where WITHREQID=@WITHREQID";
                    }
                    if (trans != null)
                    {
                        command.Transaction = trans;
                    }
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    if (isEdit)
                    {
                        command.Parameters.AddWithValue("@WITHREQID",objUser.WITHREQID);
                    }
                    command.Parameters.AddWithValue("@REQUESTEDAMOUNT", objWReq.REQUESTEDAMOUNT);
                    command.Parameters.AddWithValue("@DATEREQ", objWReq.DATEREQ);
                    command.Parameters.AddWithValue("@REQSTATUS", objWReq.REQSTATUS);
                    command.Parameters.AddWithValue("@DRIVERID", objWReq.DRIVERID);


                    int affectedRows = command.ExecuteNonQuery();
                    var lastInsertID = command.LastInsertedId;
                    if (affectedRows > 0)
                    {
                        //    if (!isEdit)
                        //    {
                        //        returnMessage = lastInsertID.ToString();
                        //    }
                        //    else
                        {
                            returnMessage = Shared.Constants.MSG_OK_DBSAVE.Text;
                        }

                    }
                    else
                    {
                        returnMessage = Shared.Constants.MSG_ERR_DBSAVE.Text;
                    }
                }

                if (isConnArgNull == true)
                {
                    connection.Dispose();
                }
            }
            catch (Exception ex)
            {

            }

            return returnMessage;
        }

        public static string DeleteWithReq(string WITHREQID, MySqlConnection conn = null)
        {
            string returnMessage = "";
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                using (MySqlCommand command = new MySqlCommand())
                {
                    string sql;
                    sql = @"DELETE from DRIVER_WITHDRAWAL_REQUEST Where WITHREQID = @WITHREQID";
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@WITHREQID", WITHREQID);
                    int affectedRows = command.ExecuteNonQuery();
                    if (affectedRows > 0)
                    {
                        returnMessage = Shared.Constants.MSG_OK_DBSAVE.Text;
                    }
                    else
                    {
                        returnMessage = Shared.Constants.MSG_ERR_DBSAVE.Text;
                    }
                }

                if (isConnArgNull == true)
                {
                    connection.Dispose();
                }
            }
            catch (Exception ex)
            {

            }

            return returnMessage;
        }

    }
}