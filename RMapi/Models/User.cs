using MySql.Data.MySqlClient;
using RMapi.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RMapi.Models
{
    public class User
    {
        public int USERID { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string NU_ID { get; set; }
        public string PASSWORD { get; set; }
        public string CONFIRM_PASSWORD { get; set;  }
        public string USERNAME { get; set;  }
        public string GENDER { get; set;  }
        public string PHONENUMBER { get; set;  }
    }
    public class UserManager : BaseManager
    {
        public static List<User> GetUser(string whereclause, MySqlConnection conn = null)
        {
            User objUser = null;
            List<User> lstUser = new List<User>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql = "select * from User";
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
                                objUser = ReaderDataUser(reader);
                                lstUser.Add(objUser);
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
            return lstUser;
        }


        private static User ReaderDataUser(MySqlDataReader reader)
        {

            User objUser = new User();
            objUser.USERID = Converter.IsValidInt(reader["USERID"]);
            objUser.FIRSTNAME = Converter.IsValidString(reader["FIRSTNAME"]);
            objUser.LASTNAME = Converter.IsValidString(reader["LASTNAME"]);
            objUser.GENDER = Converter.IsValidString(reader["GENDER"]);
            objUser.NU_ID = Converter.IsValidString(reader["NU_ID"]);
            objUser.PASSWORD = Converter.IsValidString(reader["PASSWORD"]);
            objUser.PHONENUMBER = Converter.IsValidString(reader["PHONENUMBER"]);
            objUser.USERNAME = Converter.IsValidString(reader["USERNAME"]);


            //will add for image
            return objUser;

        }

        public static string SaveUser(User objUser, MySqlConnection conn = null, MySqlTransaction trans = null)
        {
            string returnMessage = "";
            string sUSERID = "";
            sUSERID = objUser.USERID.ToString();
            var templstDriver = GetUser("USERID = '" + sUSERID + "'", conn);
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
                        sql = @"INSERT INTO USER(
                                                    
                                                  FIRSTNAME,
                                                  LASTNAME,
                                                  GENDER,
                                                  NU_ID,
                                                  PASSWORD,
                                                  PHONENUMBER,
                                                  USERNAME

                                                 
                                                    )
                                                    VALUES(
                                                     @FIRSTNAME,
                                                  @LASTNAME,
                                                  @GENDER,
                                                  @NU_ID,
                                                  @PASSWORD,
                                                  @PHONENUMBER,
                                                  @USERNAME
                                                    )";
                    }
                    else
                    {
                        sql = @"Update USER set
                                                  USERID=@USERID,
                                                  FIRSTNAME=@FIRSTNAME,
                                                  LASTNAME=@LASTNAME,
                                                  GENDER=@GENDER,
                                                  NU_ID=@NU_ID,
                                                  PASSWORD=@PASSWORD,
                                                  PHONENUMBER=@PHONENUMBER,
                                                  USERNAME=@USERNAME
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
                        command.Parameters.AddWithValue("@USERID",objUser.USERID);
                    }
                    command.Parameters.AddWithValue("@FIRSTNAME", objUser.FIRSTNAME);
                    command.Parameters.AddWithValue("@LASTNAME", objUser.LASTNAME);
                    command.Parameters.AddWithValue("@USERNAME", objUser.USERNAME);
                    command.Parameters.AddWithValue("@PASSWORD", objUser.PASSWORD);
                    command.Parameters.AddWithValue("@GENDER", objUser.GENDER);
                    command.Parameters.AddWithValue("@NU_ID", objUser.NU_ID);
                    command.Parameters.AddWithValue("@PHONENUMBER", objUser.PHONENUMBER);


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

        public static string DeleteUser(string USERID, MySqlConnection conn = null)
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
                    sql = @"DELETE from USER Where USERID = @USERID";
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