using MySql.Data.MySqlClient;
using RMapi.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RMapi.Models
{
    public class Passenger_Request
    {
        public int  REQUESTID { get; set; }
        public int USERID { get; set; }
        public DateTime? DATE { get; set; }
        public string FROM { get; set; }
        public string TO { get; set;  }
        public string TIME { get; set;  }
        public char STATUS { get; set;  }

    }
    public class PRequestManager : BaseManager
    {
        public static List<PRequestManager> GetPRequest(string whereclause, MySqlConnection conn = null)
        {
            Passenger_Request objPReq = null;
            List<PRequestManager> lstPreq = new List<PRequestManager>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql = "select * from Passenger_Request";
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
                                objPReq = ReaderDataUser(reader);
                                lstPreq.Add(objPReq);
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
            return lstPreq;
        }


        private static Passenger_Request ReaderDataPRequest(MySqlDataReader reader)
        {

            Passenger_Request objPReq = new Passenger_Request();
            objPReq.REQUESTID = Converter.IsValidInt(reader["REQUESTID"]);
            objPReq.USERID = Converter.IsValidInt(reader["USERID"]);
            objPReq.DATE = Converter.IsValidString(reader["DATE"]);
            objPReq.FROM = Converter.IsValidString(reader["FROM"]);
            objPReq.TO = Converter.IsValidString(reader["TO"]);
            objPReq.TIME = Converter.IsValidString(reader["TIME"]);
            objPReq.STATUS = Converter.IsValidString(reader["STATUS"]);


            //will add for image
            return objPReq;

        }

        public static string SavePRequest(Passenger_Request objPReq, MySqlConnection conn = null, MySqlTransaction trans = null)
        {
            string returnMessage = "";
            string sREQUESTID = "";
            sREQUESTID = objPReq.REQUESTID.ToString();
            var templstPReq = GetPRequest("REQUESTID = '" + sREQUESTID + "'", conn);
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                using (MySqlCommand command = new MySqlCommand())
                {
                    string sql;
                    bool isEdit = true;
                    if (templstPReq.Count <= 0)
                    {
                        isEdit = false;
                        sql = @"INSERT INTO PASSENGER_REQUEST(
                                                    
                                                  USERID,
                                                  DATE,
                                                  FROM,
                                                  TO,
                                                  TIME,
                                                  STATUS
                                                 
                                                    )
                                                    VALUES(
                                                     @USERID,
                                                  @DATE,
                                                  @FROM,
                                                  @TO,
                                                  @TIME,
                                                  @STATUS
                                                    )";
                    }
                    else
                    {
                        sql = @"Update PASSENGER_REQUEST set
                                                  REQUESTID=@REQUESTID,
                                                  USERID=@USERID,
                                                  DATE=@DATE,
                                                  FROM=@FROM,
                                                  TO=@TO,
                                                  TIME=@TIME,
                                                  STATUS=@STATUS
                                                    Where REQUESTID=@REQUESTID";
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
                        command.Parameters.AddWithValue("@REQUESTID",objPReq.REQUESTID);
                    }
                    command.Parameters.AddWithValue("@FIRSTNAME", objPReq.FIRSTNAME);
                    command.Parameters.AddWithValue("@USERID", objPReq.USERID);
                    command.Parameters.AddWithValue("@DATE", objPReq.DATE);
                    command.Parameters.AddWithValue("@FROM", objPReq.FROM);
                    command.Parameters.AddWithValue("@TO", objPReq.TO);
                    command.Parameters.AddWithValue("@TIME", objPReq.TIME);
                    command.Parameters.AddWithValue("@STATUS", objPReq.STATUS);


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

        public static string DeletePRequest(string REQUESTID, MySqlConnection conn = null)
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
                    sql = @"DELETE from PASSENGER_REQUEST Where REQUESTID = @REQUESTID";
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@REQUESTID", REQUESTID);
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