using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DataAccessLayer.DTOs;

namespace DataAccessLayer
{
    public static class clsExams
    {
        public static List<ExamDTO> GetAllExams()
        {
            var examsList = new List<ExamDTO>();
            using (SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName))
            {
                string query = @"
                    SELECT E.*, U.Username as AdminName 
                    FROM Exams E 
                    LEFT JOIN Users U ON E.CreatedByAdminID = U.ID";
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        examsList.Add(new ExamDTO
                        {
                            ID = (int)reader["ID"],
                            Title = (string)reader["Title"],
                            Description = (string)reader["Description"],
                            QuizTime = (int)reader["QuizTime"],
                            CreatedDate = (DateTime)reader["CreatedDate"],
                            CreatedByAdminID = (int)reader["CreatedByAdminID"],
                            AdminName = reader["AdminName"] != DBNull.Value ? (string)reader["AdminName"] : "Unknown"
                        });
                    }
                    reader.Close();
                }
                catch (Exception ex) { }
            }
            return examsList;
        }

        public static ExamDTO GetExamByID(int ID)
        {
            ExamDTO exam = null;
            using (SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName))
            {
                string query = @"
                    SELECT E.*, U.Username as AdminName 
                    FROM Exams E 
                    LEFT JOIN Users U ON E.CreatedByAdminID = U.ID 
                    WHERE E.ID = @ID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", ID);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        exam = new ExamDTO
                        {
                            ID = (int)reader["ID"],
                            Title = (string)reader["Title"],
                            Description = (string)reader["Description"],
                            QuizTime = (int)reader["QuizTime"],
                            CreatedDate = (DateTime)reader["CreatedDate"],
                            CreatedByAdminID = (int)reader["CreatedByAdminID"],
                            AdminName = reader["AdminName"] != DBNull.Value ? (string)reader["AdminName"] : "Unknown"
                        };
                    }
                    reader.Close();
                }
                catch (Exception ex) { }
            }
            return exam;
        }

        public static int AddExam(string Title, string Description, int QuizTime, DateTime CreateDate, int CreatedByAdmin)
        {
            int ID = -99;
            SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName);
            string query = @"INSERT INTO Exams(
           Title,Description,QuizTime,CreatedDate,CreatedByAdminID)
           VALUES (@Title, @Description, @QuizTime, @CreatedDate, @CreatedByAdminID);SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Title", Title);
            command.Parameters.AddWithValue("@Description", Description);
            command.Parameters.AddWithValue("@QuizTime", QuizTime);
            command.Parameters.AddWithValue("@CreatedDate", CreateDate);
            command.Parameters.AddWithValue("@CreatedByAdminID", CreatedByAdmin);
            try
            {
                connection.Open();
                object value = command.ExecuteScalar();
                ID = ((int.TryParse(value.ToString(), out int resualt) && value != null) ? resualt : -99);
            }
            catch (Exception ex) { ID = -99; }
            finally { connection.Close(); }
            return ID;
        }
        public static bool UpdateExam(int id, string Title, string Description, int QuizTime, DateTime CreateDate, int CreatedByAdmin)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName);
            string query = @"UPDATE Exams
                 SET Title = @Title
                 ,Description = @Description
                 ,QuizTime = @QuizTime
                 ,CreatedDate = @CreatedDate
                 ,CreatedByAdminID = @CreatedByAdminID
            WHERE ID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Title", Title);
            command.Parameters.AddWithValue("@Description", Description);
            command.Parameters.AddWithValue("@QuizTime", QuizTime);
            command.Parameters.AddWithValue("@CreatedDate", CreateDate);
            command.Parameters.AddWithValue("@CreatedByAdminID", CreatedByAdmin);
            command.Parameters.AddWithValue("@ID", id);
            try
            {
                connection.Open();
                RowAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex) { RowAffected = 0; }
            finally { connection.Close(); }
            return (RowAffected > 0);
        }
        public static bool DeleteExam(int id)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName);
            string query = "DELETE FROM Exams WHERE ID = @ID";
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
        public static bool GetAdminName(int id, ref string name)
        {
            bool IsFounded = false;
            SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName);
            string query = "SELECT DISTINCT Users.Username AS Name FROM Exams INNER JOIN Users ON Exams.CreatedByAdminID = Users.ID Where Exams.CreatedByAdminID =@ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    name = (string)reader["Name"];
                    IsFounded = true;
                }
                reader.Close();
            }
            catch (Exception ex) { IsFounded = false; }
            finally { connection.Close(); }
            return IsFounded;
        }

    }
}
