using MySql.Data.MySqlClient;
using RMapi.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RMapi.Models
{
    public class Driver_Transaction_History
    {
        public int DRIVERID { get; set; }
        public char TRANSACTIONTYPE { get; set; }
        public int AMOUNT { get; set; }
        public DateTime? DATE { get; set; }
    }

    public class DriverTransactionHistoryManager : BaseManager
    {
        public static List<Driver_Transaction_History> GetHistory(string whereclause, MySqlConnection conn = null)
        {
            User objHist = null;
            List<Driver_Transaction_History> lstHist = new List<Driver_Transaction_History>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql = "select * from Driver_transaction_history";
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
                                objHist = ReaderDataHistory(reader);
                                lstHist.Add(objHist);
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
            return lstHist;
        }


        private static Driver_Transaction_History ReaderDataHistory(MySqlDataReader reader)
        {

            Driver_Transaction_History objHist = new Driver_Transaction_History();
            objHist.DRIVERID = Converter.IsValidInt(reader["DRIVERID"]);
            objHist.TRANSACTIONTYPE = Converter.IsValidString(reader["TRANSACTIONTYPE"]);
            objHist.AMOUNT = Converter.IsValidString(reader["AMOUNT"]);
            objHist.DATE = Converter.IsValidString(reader["DATE"]);




            return objHist;

        }

        public static string SaveHistory(User objHist, MySqlConnection conn = null, MySqlTransaction trans = null)
        {
            string returnMessage = "";
            string sDRIVERID = "";
            sDRIVERID = objHist.DRIVERID.ToString();
            var templstHist = GetHistory("DRIVERID = '" + sDRIVERID + "'", conn);
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                using (MySqlCommand command = new MySqlCommand())
                {
                    string sql;
                    bool isEdit = true;
                    if (templstHist.Count <= 0)
                    {
                        isEdit = false;
                        sql = @"INSERT INTO DRIVER_TRANSACTION_HISTORY(
                                                    
                                                  TRANSACTIONTYPE,
                                                  AMOUNT,
                                                  DATE


                                                 
                                                    )
                                                    VALUES(
                                                     @TRANSACTIONTYPE,
                                                  @AMOUNT,
                                                  @DATE
                                                    )";
                    }
                    else
                    {
                        sql = @"Update DRIVER_TRANSACTION_HISTORY set
                                                  DRIVERID=@DRIVERID,
                                                  TRANSACTIONTYPE=@TRANSACTIONTYPE,
                                                  AMOUNT=@AMOUNT,
                                                  DATE=@DATE
                                                    Where DRIVERID=@DRIVERID";
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
                        command.Parameters.AddWithValue("@DRIVERID",objHist.DRIVERID);
                    }
                    command.Parameters.AddWithValue("@TRANSACTIONTYPE", objHist.TRANSACTIONTYPE);
                    command.Parameters.AddWithValue("@AMOUNT", objHist.AMOUNT);
                    command.Parameters.AddWithValue("@DATE", objHist.DATE);



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

        public static string DeleteHistory(string DRIVERID, MySqlConnection conn = null)
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
                    sql = @"DELETE from DRIVER_TRANSACTION_HISTORY Where DRIVERID = @DRIVERID";
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@DRIVERID", DRIVERID);
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