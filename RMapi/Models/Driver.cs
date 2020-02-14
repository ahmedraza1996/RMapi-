using MySql.Data.MySqlClient;
using RMapi.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RMapi.Models
{
    public class Driver
    {
        public int DRIVERID { get; set;  }
        public string CNIC { get; set;  }
        
        public string CNICIMAGE { get; set; } //relative path
        public string PROFILEPIC { get; set; } //relative path

        //IMAGE


        public HttpPostedFileBase CNICIMAGEFILE { get; set; }
        public HttpPostedFileBase PROFILEIMAGEFILE { get; set; }
    }
    public class DriverManager : BaseManager
    {
        public static List<Driver> GetDriver(string whereclause, MySqlConnection conn = null)
        {
            Driver objDriver = null;
            List<Driver> lstDriver = new List<Driver>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql = "select * from Driver";
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
                                objDriver = ReaderDataDriver(reader);
                                lstDriver.Add(objDriver);
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
            return lstDriver;
        }


        private static Driver ReaderDataDriver(MySqlDataReader reader)
        {

            Driver objDriver = new Driver();
            objDriver.DRIVERID = Converter.IsValidInt(reader["DRIVERID"]);
            objDriver.CNIC = Converter.IsValidString(reader["CNIC"]);
            objDriver.CNICIMAGE = Converter.IsValidString(reader["CNICIMAGE"]);
            objDriver.PROFILEPIC = Converter.IsValidString(reader["PROFILEPIC"]);
             
            //will add for image
            return objDriver;

        }

        public static string SaveActivity(Driver objDriver, MySqlConnection conn = null, MySqlTransaction trans = null)
        {
            string returnMessage = "";
            string sDRIVERID = "";
            sDRIVERID = objDriver.DRIVERID.ToString();
            var templstDriver = GetDriver("DRIVERID = '" + sDRIVERID + "'", conn);
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
                        sql = @"INSERT INTO Driver(
                                                    CNIC,
                                                    CNICIMAGE, 
                                                    PROFILEPIC
                                                    
                                                 
                                                    )
                                                    VALUES(
                                                     @CNIC,
                                                    @CNICIMAGE, 
                                                    @PROFILEPIC
                                                    )";
                    }
                    else
                    {
                        sql = @"Update DRIVER set
                                                    DRIVERID=@DRIVERID,                                               
                                                    CNIC=@CNIC,
                                                    CNICIMAGE=@CNICIMAGE,
                                                    PROFILEPIC=@PROFILEPIC,
                                                    

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
                        command.Parameters.AddWithValue("@DRIVERID", objDriver.DRIVERID);
                    }

                    command.Parameters.AddWithValue("@CNIC", objDriver.CNIC);
                    command.Parameters.AddWithValue("@CNICIMAGE", objDriver.CNICIMAGE);
                    command.Parameters.AddWithValue("@PROFILEPIC", objDriver.PROFILEPIC);
                    

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

        public static string DeleteActivity(string DRIVERID, MySqlConnection conn = null)
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
                    sql = @"DELETE from DRIVER Where DRIVERID = @DRIVERID";
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