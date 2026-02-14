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
    public class clsQuestions
    {
        public static List<QuestionDTO> GetAllQuestions()
        {
            var questionList = new List<QuestionDTO>();
            using (SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName))
            {
                string query = "SELECT * FROM Questions";
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        questionList.Add(new QuestionDTO
                        {
                            ID = (int)reader["ID"],
                            ExamID = (int)reader["ExamID"],
                            Text = (string)reader["Text"],
                            Option1 = (string)reader["Option1"],
                            Option2 = (string)reader["Option2"],
                            Option3 = (string)reader["Option3"],
                            Option4 = (string)reader["Option4"],
                            CorrectAnswer = (int)reader["CorrectAnswer"]
                        });
                    }
                    reader.Close();
                }
                catch (Exception ex) { }
            }
            return questionList;
        }

        public static List<QuestionDTO> GetAllQuestionsInExam(int ExamID)
        {
            var questionList = new List<QuestionDTO>();
            using (SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName))
            {
                string query = "SELECT * FROM Questions WHERE ExamID = @ExamID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ExamID", ExamID);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        questionList.Add(new QuestionDTO
                        {
                            ID = (int)reader["ID"],
                            ExamID = (int)reader["ExamID"],
                            Text = (string)reader["Text"],
                            Option1 = (string)reader["Option1"],
                            Option2 = (string)reader["Option2"],
                            Option3 = (string)reader["Option3"],
                            Option4 = (string)reader["Option4"],
                            CorrectAnswer = (int)reader["CorrectAnswer"]
                        });
                    }
                    reader.Close();
                }
                catch (Exception ex) { }
            }
            return questionList;
        }

        public static QuestionDTO GetQuestionByID(int ID)
        {
            QuestionDTO question = null;
            using (SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName))
            {
                string query = "SELECT * FROM Questions WHERE ID = @ID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", ID);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        question = new QuestionDTO
                        {
                            ID = (int)reader["ID"],
                            ExamID = (int)reader["ExamID"],
                            Text = (string)reader["Text"],
                            Option1 = (string)reader["Option1"],
                            Option2 = (string)reader["Option2"],
                            Option3 = (string)reader["Option3"],
                            Option4 = (string)reader["Option4"],
                            CorrectAnswer = (int)reader["CorrectAnswer"]
                        };
                    }
                    reader.Close();
                }
                catch (Exception ex) { }
            }
            return question;
        }
        public static int AddQuestions(string Text, List<string> options, int correctans, int examid)
        {
            int id = -99;
            SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName);
            string query = @"INSERT INTO Questions
           (ExamID,Text,Option1,Option2,Option3,Option4,CorrectAnswer)
     VALUES(@ExamID,@Text,@Option1,@Option2,@Option3,@Option4,@CorrectAnswer);
		   SELECT SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ExamID", examid);
            command.Parameters.AddWithValue("@Text", Text);
            command.Parameters.AddWithValue("@Option1", options[0]);
            command.Parameters.AddWithValue("@Option2", options[1]);
            command.Parameters.AddWithValue("@Option3", options[2]);
            command.Parameters.AddWithValue("@Option4", options[3]);
            command.Parameters.AddWithValue("@CorrectAnswer", correctans);
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
        public static bool UpdateQuestions(int id, string text, List<string> options, int correctans, int examid)
        {
            int IsAffected = 0;
            SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName);
            string query = @"UPDATE Questions
                 SET Text = @Text ,Option1 = @Option1 ,Option2 = @Option2 , Option3 = @Option3, Option4 = @Option4, CorrectAnswer=@CorrectAnswer
            ,ExamID = @ExamID
            WHERE ID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);
            command.Parameters.AddWithValue("@Text", text);
            command.Parameters.AddWithValue("@Option1", options[0]);
            command.Parameters.AddWithValue("@Option2", options[1]);
            command.Parameters.AddWithValue("@Option3", options[2]);
            command.Parameters.AddWithValue("@Option4", options[3]);
            command.Parameters.AddWithValue("@CorrectAnswer", correctans);
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
        public static bool DeleteQuestions(int id)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(DataAccessSetting.ConnectingName);
            string query = "DELETE FROM Questions WHERE ID = @ID";
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
    }
}
