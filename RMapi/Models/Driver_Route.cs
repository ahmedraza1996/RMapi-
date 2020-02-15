using MySql.Data.MySqlClient;
using RMapi.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RMapi.Models
{
    public class Driver_Route
    {
        public int ROUTEID { get; set; }
        public int DRIVERID { get; set; }
        public DateTime? DATE { get; set; }
        public string FROM { get; set; }
        public string TO { get; set; }
        public string TIMESLAB { get; set; }
        public int AVAILABLESEATS { get; set; }

    }
    public class DriverRouteManager : BaseManager
    {
        public static List<Driver_Route> GetRoute(string whereclause, MySqlConnection conn = null)
        {
            Driver_Route objDriverRoute = null;
            List<Driver_Route> lstRoute = new List<Driver_Route>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql = "select * from DRIVER_ROUTE";
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
                                objDriverRoute = ReaderDataRoute(reader);
                                lstRoute.Add(objDriverRoute);
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
            return lstRoute;
        }


        private static Driver_Route ReaderDataRoute(MySqlDataReader reader)
        {

            Driver_Route objRoute = new Driver_Route();

            objRoute.ROUTEID = Converter.IsValidInt(reader["ROUTEID"]);
            objRoute.DRIVERID = Converter.IsValidInt(reader["DRIVERID"]);
            objRoute.DATE = Converter.IsValidDateTime(reader["DATE"]);
            objRoute.FROM = Converter.IsValidString(reader["FROM"]);
            objRoute.TO = Converter.IsValidString(reader["TO"]);
            objRoute.TIMESLAB = Converter.IsValidString(reader["TIMESLAB"]);
            objRoute.AVAILABLESEATS = Converter.IsValidInt(reader["AVAILABLESEATS"]);
            return objRoute;

        }

        public static string SaveRoute(Driver_Route objRoute, MySqlConnection conn = null, MySqlTransaction trans = null)
        {
            string returnMessage = "";
            string sROUTEID = "";
            sROUTEID = objRoute.ROUTEID.ToString(); 
            var templstDriver = GetRoute("ROUTEID = '" + sROUTEID + "'", conn);
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                using (MySqlCommand command = new MySqlCommand())
                {
                    string sql;
                    bool isEdit = true;
                    if (templstDriver.Count <= 0)
                    {
                        isEdit = false;
                        sql = @"INSERT INTO DRIVER_ROUTE(
                                                 
                                                DRIVERID,
                                                DATE,
                                                FROM ,
                                                TO ,
                                                TIMESLAB, 
                                                AVAILABLESEATS,
                                                    )
                                                    VALUES(
                                                      @DRIVERID,
                                                        @DATE,
                                                        @FROM ,
                                                        @TO ,
                                                        @TIMESLAB, 
                                                        @AVAILABLESEATS,
                                                    )";
                    }
                    else
                    {
                        sql = @"Update DRIVER_ROUTE set
                                                    ROUTEID=@ROUTEID,  
                                                    DRIVERID=@DRIVERID,                                               
                                                    DATE=@DATE,
                                                    FROM=@FROM,
                                                    TO=@TO,
                                                    TIMESLAB=@TIMESLAB, 
                                                    AVAILABLESEATS=@AVAILABLESEATS
                                                    

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
                        command.Parameters.AddWithValue("@ROUTEID", objRoute.ROUTEID);
                    }
                    command.Parameters.AddWithValue("@DRIVERID", objRoute.DRIVERID);
                    command.Parameters.AddWithValue("@DATE", objRoute.DATE);
                    command.Parameters.AddWithValue("@FROM", objRoute.FROM);
                    command.Parameters.AddWithValue("@TO", objRoute.TO);
                    command.Parameters.AddWithValue("@TIMESLAB", objRoute.TIMESLAB);
                    command.Parameters.AddWithValue("@AVAILABLESEATS", objRoute.AVAILABLESEATS);
                  


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

        public static string DeleteRoute(string ROUTEID, MySqlConnection conn = null)
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
                    sql = @"DELETE from DRIVER_ROUTE Where ROUTE = @ROUTEID";
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@ROUTEID", ROUTEID);
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