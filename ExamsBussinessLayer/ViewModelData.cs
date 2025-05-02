using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessLayer;
using DataAccessLayer;

namespace ExamsBussinessLayer
{
    public  class ViewModelData
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string ExamTitle { get; set; }
        public int Score { get; set; }
        public int TotalPoint { get; set; }
        public int Percentage { get; set; }
        public bool Status { get; set; } 
        public static List<ViewModelData> GetAllUsersExams()
        {
            DataTable dtUserExam = clsUsersExam.GetAllUsersExam();
            List<ViewModelData> viewModelDataList = new List<ViewModelData>();
            foreach (DataRow row in dtUserExam.Rows)
            {
                viewModelDataList.Add(new ViewModelData
                {
                    ID = (int)row["ID"],
                    Username = Users.Find((int)row["UserID"]).UserName,
                    ExamTitle = Exams.Find((int)row["ExamID"]).Title,
                    Score = (int)row["NumOfPoint"],
                    TotalPoint = (int)row["TotalPoint"],
                    Percentage = ((int)row["NumOfPoint"] * 100) / ((int)row["TotalPoint"]),
                    Status = (bool)row["IsPass"]
                });
            }
            return viewModelDataList;
        }
    }
}
