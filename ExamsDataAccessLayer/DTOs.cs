using System;
using System.Collections.Generic;

namespace DataAccessLayer.DTOs
{
    public class ExamDTO
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int QuizTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByAdminID { get; set; }
        public string AdminName { get; set; }
    }

    public class UserDTO
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class QuestionDTO
    {
        public int ID { get; set; }
        public int ExamID { get; set; }
        public string Text { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public int CorrectAnswer { get; set; }

        public List<string> Options => new List<string> { Option1, Option2, Option3, Option4 };
    }

    public class UserExamDTO
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string ExamTitle { get; set; }
        public int Score { get; set; }
        public int TotalPoint { get; set; }
        public int Percentage => TotalPoint > 0 ? (Score * 100) / TotalPoint : 0;
        public bool IsPass { get; set; }
        public DateTime TakenDate { get; set; }
    }
}
