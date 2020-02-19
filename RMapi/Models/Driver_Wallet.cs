using MySql.Data.MySqlClient;
using RMapi.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RMapi.Models
{
    public class Driver_Wallet
    {
        public int DRIVERID { get; set;}
        public int RMC { get; set; }
    }
    public class DriverWalletManager : BaseManager
    {
        public static List<Driver_Wallet> GetDWallet(string whereclause, MySqlConnection conn = null)
        {
            Driver_Wallet objWall = null;
            List<Driver_Wallet> lstDWall = new List<Driver_Wallet>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql = "select * from Driver_Wallet";
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
                                lstDWall.Add(objWall);
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
            return lstDWall;
        }


        private static Driver_Wallet ReaderDataDwall(MySqlDataReader reader)
        {

            Driver_Wallet objWall = new Driver_Wallet();
            objWall.DRIVERID = Converter.IsValidInt(reader["DRIVERID"]);
            objWall.RMC = Converter.IsValidInt(reader["RMC"]);




            return objWall;

        }

        public static string SaveDwall(Driver_Wallet objWall, MySqlConnection conn = null, MySqlTransaction trans = null)
        {
            string returnMessage = "";
            string sDRIVERID = "";
            sDRIVERID = objWall.DRIVERID.ToString();
            var templstWall = GetDWallet("DRIVERID = '" + sDRIVERID + "'", conn);
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
                        sql = @"INSERT INTO DRIVER_WALLET(
                                                    
                                                  RMC

                                                 
                                                    )
                                                    VALUES(
                                                     @RMC
                                                    )";
                    }
                    else
                    {
                        sql = @"Update DRIVER_WALLET set
                                                  DRIVERID=@DRIVERID,
                                                  RMC=@RMC
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
                        command.Parameters.AddWithValue("@DRIVERID",objWall.DRIVERID);
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

        public static string DeleteDWall(string DRIVERID, MySqlConnection conn = null)
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
                    sql = @"DELETE from DRIVER_WALLET Where DRIVERID = @DRIVERID";
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