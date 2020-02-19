using MySql.Data.MySqlClient;
using RMapi.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RMapi.Models
{
    public class Passenger_Wallet
    {
        public int USERID { get; set;}
        public int RMC { get; set; }
    }
    public class PassengerWalletManager : BaseManager
    {
        public static List<Passenger_Wallet> GetPWallet(string whereclause, MySqlConnection conn = null)
        {
            Passenger_Wallet objWall = null;
            List<Passenger_Wallet> lstPWall = new List<Passenger_Wallet>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql = "select * from Passenger_Wallet";
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
                                objWall = ReaderDataUser(reader);
                                lstPWall.Add(objWall);
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
            return lstPWall;
        }


        private static Passenger_Wallet ReaderDataPwall(MySqlDataReader reader)
        {

            Passenger_Wallet objWall = new Passenger_Wallet();
            objWall.USERID = Converter.IsValidInt(reader["USERID"]);
            objWall.RMC = Converter.IsValidInt(reader["RMC"]);




            return objWall;

        }

        public static string SavePwall(Passenger_Wallet objWall, MySqlConnection conn = null, MySqlTransaction trans = null)
        {
            string returnMessage = "";
            string sUSERID = "";
            sUSERID = objWall.DRIVERID.ToString();
            var templstWall = GetPWallet("USERID = '" + sUSERID + "'", conn);
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                using (MySqlCommand command = new MySqlCommand())
                {
                    string sql;
                    bool isEdit = true;
                    if (templstWall.Count <= 0)
                    {
                        isEdit = false;
                        sql = @"INSERT INTO PASSENGER_WALLET(
                                                    
                                                  RMC

                                                 
                                                    )
                                                    VALUES(
                                                     @RMC
                                                    )";
                    }
                    else
                    {
                        sql = @"Update DRIVER_WALLET set
                                                  USERID=@USERID,
                                                  RMC=@RMC
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
                        command.Parameters.AddWithValue("@USERID",objWall.USERID);
                    }
                    command.Parameters.AddWithValue("@RMC", objWall.RMC);



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

        public static string DeletePWall(string USERID, MySqlConnection conn = null)
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
                    sql = @"DELETE from PASSENGER_WALLET Where USERID = @USERID";
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