using MySql.Data.MySqlClient;
using RMapi.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RMapi.Models
{
    public class Passenger_Transaction_History
    {
        public int USERID { get; set; }
        public char TRANSACTIONTYPE { get; set; }
        public int AMOUNT { get; set; }
        public DateTime? DATE { get; set; }
    }
    public class PassTransactionHistoryManager : BaseManager
    {
        public static List<Passenger_Transaction_History> GetHistory(string whereclause, MySqlConnection conn = null)
        {
            Passenger_Transaction_History objHist = null;
            List<Passenger_Transaction_History> lstHist = new List<Passenger_Transaction_History>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql = "select * from Passenger_Transaction_History";
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


        private static Passenger_Transaction_History ReaderDataHistory(MySqlDataReader reader)
        {

            Passenger_Transaction_History objHist = new Passenger_Transaction_History();
            objHist.DRIVERID = Converter.IsValidInt(reader["USERID"]);
            objHist.TRANSACTIONTYPE = Converter.IsValidString(reader["TRANSACTIONTYPE"]);
            objHist.AMOUNT = Converter.IsValidString(reader["AMOUNT"]);
            objHist.DATE = Converter.IsValidString(reader["DATE"]);




            return objHist;

        }

        public static string SaveHistory(Passenger_Transaction_History objHist, MySqlConnection conn = null, MySqlTransaction trans = null)
        {
            string returnMessage = "";
            string sUSERID = "";
            sUSERID = objHist.USERID.ToString();
            var templstHist = GetHistory("USERID = '" + sUSERID + "'", conn);
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
                        sql = @"INSERT INTO PASSENGER_TRANSACTION_HISTORY(
                                                    
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
                        sql = @"Update PASSENGER_TRANSACTION_HISTORY set
                                                  USERID=@USERID,
                                                  TRANSACTIONTYPE=@TRANSACTIONTYPE,
                                                  AMOUNT=@AMOUNT,
                                                  DATE=@DATE
                                                    Where USERID=@USERID";
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
                        command.Parameters.AddWithValue("@USERID",objHist.USERID);
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

        public static string DeleteHistory(string USERID, MySqlConnection conn = null)
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
                    sql = @"DELETE from PASSENGER_TRANSACTION_HISTORY Where USERID = @USERID";
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@USERID", USERID);
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