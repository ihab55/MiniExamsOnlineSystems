using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.DTOs;

namespace DataAccessLayer
{
    public class clsUsersExam
    {
        public static List<UserExamDTO> GetAllUsersExam()
        {
            var results = new List<UserExamDTO>();
            using (SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName))
            {
                string query = @"
                    SELECT UE.ID, U.UserName, E.Title as ExamTitle, UE.NumOfPoint, UE.TotalPoint, UE.IsPass, UE.TakenDate
                    FROM UsersExam UE
                    JOIN Users U ON UE.UserID = U.ID
                    JOIN Exams E ON UE.ExamID = E.ID";
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        results.Add(new UserExamDTO
                        {
                            ID = (int)reader["ID"],
                            UserName = (string)reader["UserName"],
                            ExamTitle = (string)reader["ExamTitle"],
                            Score = (int)reader["NumOfPoint"],
                            TotalPoint = (int)reader["TotalPoint"],
                            IsPass = (bool)reader["IsPass"],
                            TakenDate = (DateTime)reader["TakenDate"]
                        });
                    }
                    reader.Close();
                }
                catch (Exception ex) { }
            }
            return results;
        }
        public static bool GetUserExamByID(int id, ref int NumOfPoint, ref int Total, ref bool Ispass, ref DateTime TakenDate,
            ref int userid, ref int examid)
        {
            bool IsFouned = false;
            SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName);
            string query = "SELECT * Where ID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);
            try
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    NumOfPoint = (int)reader["NumOfPoint"];
                    Total = (int)reader["TotalPoint"];
                    Ispass = (bool)reader["IsPass"];
                    TakenDate = (DateTime)reader["TakenDate"];
                    userid = (int)reader["UserID"];
                    examid = (int)reader["ExamID"];
                    IsFouned = true;
                }
                reader.Close();
            }
            catch (Exception ex) { IsFouned = false; }
            finally { connection.Close(); }
            return IsFouned;
        }
        public static int AddUsersExam(int NumOfPoint, int Total, bool Ispass, DateTime TakenDate, int userid, int examid)
        {
            int id = -99;
            SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName);
            string query = @"INSERT INTO UsersExam(
          NumOfPoint,TotalPoint,IsPass,TakenDate,UserID,ExamID)
           VALUES (@NumOfPoint,@TotalPoint,@IsPass,@TakenDate,@UserID,@ExamID);
           SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NumOfPoint", NumOfPoint);
            command.Parameters.AddWithValue("@TotalPoint", Total);
            command.Parameters.AddWithValue("@IsPass", Ispass);
            command.Parameters.AddWithValue("@TakenDate", TakenDate);
            command.Parameters.AddWithValue("@UserID", userid);
            command.Parameters.AddWithValue("@ExamID", examid);
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
        public static bool UpdateUsersExam(int id, int NumOfPoint, int Total, bool Ispass, DateTime TakenDate, int userid, int examid)
        {
            int IsAffected = 0;
            SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName);
            string query = @"UPDATE UsersExam
                 SET NumOfPoint = @NumOfPoint ,TotalPoint = @TotalPoint , IsPass = @IsPass, TakenDate = @TakenDate, 
            UserID=@UserID ,ExamID = @ExamID
            WHERE ID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);
            command.Parameters.AddWithValue("@NumOfPoint", NumOfPoint);
            command.Parameters.AddWithValue("@TotalPoint", Total);
            command.Parameters.AddWithValue("@IsPass", Ispass);
            command.Parameters.AddWithValue("@TakenDate", TakenDate);
            command.Parameters.AddWithValue("@UserID", userid);
            command.Parameters.AddWithValue("@ExamID", examid);
            try
            {
                connection.Open();
                IsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex) { IsAffected = 0; }
            finally { connection.Close(); }
            return (IsAffected > 0);
        }
        public static bool DeleteUsersExam(int id)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName);
            string query = "DELETE FROM UsersExam WHERE ID = @ID";
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
        public static DataTable GetViewData()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName);
            string query = @"SELECT UsersExam.ID, Users.Username, Exams.Title AS [Exam Title], UsersExam.NumOfPoint AS [Point Earned], UsersExam.TotalPoint AS [Total Point],
(NumOfPoint/TotalPoint)*100 AS Percentage , UsersExam.IsPass AS [Status]
FROM     UsersExam INNER JOIN
                  Users ON UsersExam.UserID = Users.ID INNER JOIN
                  Exams ON UsersExam.ExamID = Exams.ID AND Users.ID = Exams.CreatedByAdminID";
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                dt = null;
            }
            finally { connection.Close(); }
            return dt;
        }
    }
}
