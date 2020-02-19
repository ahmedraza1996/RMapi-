using MySql.Data.MySqlClient;
using RMapi.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RMapi.Models
{
    public class Passenger_Ride
    {
        public int PASSENGERREQUESTID {get; set;}
        public int RIDEAMOUNT {get; set;}
        public int REQUESTID {get; set;}
        public int ROUTEID {get; set;}
    }
    public class PRideManager : BaseManager
    {
        public static List<Passenger_Ride> GetPRide(string whereclause, MySqlConnection conn = null)
        {
            Passenger_Ride objPRide = null;
            List<Passenger_Ride> lstPRide = new List<Passenger_Ride>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql = "select * from Passenger_Ride";
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
                                objPRide = ReaderDataUser(reader);
                                lstPRide.Add(objPRide);
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
            return lstPRide;
        }


        private static Passenger_Ride ReaderDataPRide(MySqlDataReader reader)
        {

            Passenger_Ride objPRide = new Passenger_Ride();
            objPRide.PASSENGERREQUESTID = Converter.IsValidInt(reader["PASSENGERREQUESTID"]);
            objPRide.RIDEAMOUNT = Converter.IsValidInt(reader["RIDEAMOUNT"]);
            objPRide.REQUESTID = Converter.IsValidInt(reader["REQUESTID"]);
            objPRide.ROUTEID = Converter.IsValidInt(reader["ROUTEID"]);


            //will add for image
            return objPRide;

        }

        public static string SaveUser(Passenger_Ride objPRide, MySqlConnection conn = null, MySqlTransaction trans = null)
        {
            string returnMessage = "";
            string sPASSENGERREQUESTID= "";
            sPASSENGERREQUESTID = objPRide.PASSENGERREQUESTID.ToString();
            var templstPRide = GetPRide("PASSENGERREQUESTID = '" + sPASSENGERREQUESTID + "'", conn);
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                using (MySqlCommand command = new MySqlCommand())
                {
                    string sql;
                    bool isEdit = true;
                    if (templstPRide.Count <= 0)
                    {
                        isEdit = false;
                        sql = @"INSERT INTO PASSENGER_RIDE(
                                                    
                                                  RIDEAMOUNT,
                                                  REQUESTID,
                                                  ROUTEID
                                                 
                                                    )
                                                    VALUES(
                                                     @RIDEAMOUNT,
                                                  @REQUESTID,
                                                  @ROUTEID
                                                    )";
                    }
                    else
                    {
                        sql = @"Update PASSENGER_RIDE set
                                                  PASSENGERREQUESTID=@PASSENGERREQUESTID,
                                                  RIDEAMOUNT=@RIDEAMOUNT,
                                                  REQUESTID=@REQUESTID,
                                                  ROUTEID=@ROUTEID
                                                    Where PASSENGERREQUESTID=@PASSENGERREQUESTID";
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
                        command.Parameters.AddWithValue("@PASSENGERREQUESTID",objPRide.PASSENGERREQUESTID);
                    }
                    command.Parameters.AddWithValue("@RIDEAMOUNT", objPRide.RIDEAMOUNT);
                    command.Parameters.AddWithValue("@REQUESTID", objPRide.REQUESTID);
                    command.Parameters.AddWithValue("@ROUTEID", objPRide.ROUTEID);


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

        public static string DeletePRide(string PASSENGERREQUESTID, MySqlConnection conn = null)
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
                    sql = @"DELETE from PASSENGER_RIDE Where PASSENGERREQUESTID = @PASSENGERREQUESTID";
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@PASSENGERREQUESTID", PASSENGERREQUESTID);
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