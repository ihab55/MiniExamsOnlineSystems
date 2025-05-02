using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BussinessLayer
{
    public class Exams
    {
        private enum _enMode { _enUpdate = 0, _enAddNew = 1 };
        public int ID;
        public string Title;
        public string Description;
        public int QuizTime;
        public DateTime CreatedDate;
        public int CreateByAdmin;
        public List<Questions>questions;
        _enMode _Mode;
        public static List<Exams> GetAllExam()
        {
            DataTable dt = clsExams.GetAllExams();
            List<Exams> examsList = new List<Exams>();
            foreach (DataRow dr in dt.Rows)
            {
                examsList.Add(
                    Find((int)dr["ID"])
                    );
            }
            return examsList;
        }
        public string AdminName()
        {
            string name = "";
            if (DataAccessLayer.clsExams.GetAdminName(CreateByAdmin, ref name))
            {
                return name;
            }
            return "";
        }
        private Exams(int id, string title, string description, int quizetime, DateTime createdate, int creatbyadmin)
        {
            ID = id;
            Title = title;
            Description = description;
            QuizTime = quizetime;
            CreatedDate = createdate;
            CreateByAdmin = creatbyadmin;
            _Mode = _enMode._enUpdate;
        }
        public Exams()
        {
            ID = -99;
            Title = "";
            Description = "";
            QuizTime = 0;
            CreatedDate = DateTime.Now;
            CreateByAdmin = 0;
            _Mode = _enMode._enAddNew;
        }
        public static Exams Find(int id)
        {
            string title = "";
            string description = "";
            int quizTime = 0;
            DateTime createdDate = DateTime.Now;
            int createByAdmin = 0;
            if (DataAccessLayer.clsExams.GetExamByID(id, ref title, ref description, ref quizTime, ref createdDate, ref createByAdmin))
            {
                return new Exams(id, title, description, quizTime, createdDate, createByAdmin);
            }
            return null;
        }

        private bool _AddExams()
        {
            ID = DataAccessLayer.clsExams.AddExam(Title, Description, QuizTime, CreatedDate, CreateByAdmin);
            return (ID != -99);
        }
        private bool _UpdateExams()
        {
            return clsExams.UpdateExam(ID, Title, Description, QuizTime, CreatedDate, CreateByAdmin);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case _enMode._enAddNew:
                    if (_AddExams())
                    {
                        _Mode = _enMode._enUpdate;
                        return true;
                    }
                    return false;
                    break;
                case _enMode._enUpdate:
                    return _UpdateExams();
                    break;
            }
            return false;
        }
        public static bool Deleted(int id)
        {
            return clsExams.DeleteExam(id);
        }
    }
}
