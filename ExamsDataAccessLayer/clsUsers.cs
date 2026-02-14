using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.DTOs;

namespace DataAccessLayer
{
    public static class clsUsers
    {
        public static List<UserDTO> GetAllUsers()
        {
            var userList = new List<UserDTO>();
            using (SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName))
            {
                string query = "SELECT * FROM Users";
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        userList.Add(new UserDTO
                        {
                            ID = (int)reader["ID"],
                            UserName = (string)reader["UserName"],
                            PassWord = (string)reader["Password"],
                            IsAdmin = (bool)reader["IsAdmin"]
                        });
                    }
                    reader.Close();
                }
                catch (Exception ex) { }
            }
            return userList;
        }

        public static UserDTO GetUserByID(int ID)
        {
            UserDTO user = null;
            using (SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName))
            {
                string query = "SELECT * FROM Users WHERE ID = @ID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", ID);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        user = new UserDTO
                        {
                            ID = (int)reader["ID"],
                            UserName = (string)reader["UserName"],
                            PassWord = (string)reader["Password"],
                            IsAdmin = (bool)reader["IsAdmin"]
                        };
                    }
                    reader.Close();
                }
                catch (Exception ex) { }
            }
            return user;
        }

        public static UserDTO GetUserByUserName(string UserName)
        {
            UserDTO user = null;
            using (SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName))
            {
                string query = "SELECT * FROM Users WHERE UserName = @UserName";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserName", UserName);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        user = new UserDTO
                        {
                            ID = (int)reader["ID"],
                            UserName = (string)reader["UserName"],
                            PassWord = (string)reader["Password"],
                            IsAdmin = (bool)reader["IsAdmin"]
                        };
                    }
                    reader.Close();
                }
                catch (Exception ex) { }
            }
            return user;
        }

        public static int AddUsers(string username, string password, bool IsAdmin = false)
        {
            int id = -99;
            SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName);
            string query = @"INSERT INTO Users(
           UserName,PassWord,IsAdmin)
           VALUES (@UserName, @PassWord, @IsAdmin);
           SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserName", username);
            command.Parameters.AddWithValue("@PassWord", password);
            command.Parameters.AddWithValue("@IsAdmin", IsAdmin);
            try
            {
                connection.Open();
                object value = command.ExecuteScalar();
                id = ((int.TryParse(value.ToString(), out int resualt) && value != null) ? resualt : -99);
            }
            catch (Exception ex) { id = -99; }
            finally { connection.Close(); }
            return id;
        }
        public static bool UpdateUsers(int id, string username, string password, bool IsAdmin)
        {
            int IsAffected = 0;
            SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName);
            string query = @"UPDATE Users
                 SET UserName = @UserName ,PassWord = @PassWord , IsAdmin = @IsAdmin
            WHERE ID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);
            command.Parameters.AddWithValue("@UserName", username);
            command.Parameters.AddWithValue("@PassWord", password);
            command.Parameters.AddWithValue("@IsAdmin", IsAdmin);
            try
            {
                connection.Open();
                IsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex) { IsAffected = 0; }
            finally { connection.Close(); }
            return (IsAffected > 0);
        }
        public static bool DeleteUsers(int id)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName);
            string query = "DELETE FROM Users WHERE ID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);
            try
            {
                connection.Open();
                RowAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                RowAffected = 0;
            }
            finally { connection.Close(); }
            return (RowAffected > 0);
        }
        public static bool IsExsistUser(string username, string password)
        {
            bool IsExsist = false;
            SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName);
            string query = "SELECT X=1 FROM Users WHERE UserName = @UserName AND PassWord = @PassWord";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserName", username);
            command.Parameters.AddWithValue("@PassWord", password);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                IsExsist = reader.HasRows;
                reader.Close();
            }
            catch (Exception ex) { IsExsist = false; }
            finally { connection.Close(); }
            return IsExsist;
        }
        public static bool IsExsistUser(string username)
        {
            bool IsExsist = false;
            SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName);
            string query = "SELECT X=1 FROM Users WHERE UserName = @UserName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserName", username);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                IsExsist = reader.HasRows;
                reader.Close();
            }
            catch (Exception ex) { IsExsist = false; }
            finally { connection.Close(); }
            return IsExsist;
        }
    }
}
