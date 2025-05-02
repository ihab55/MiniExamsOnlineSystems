using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BussinessLayer
{
    public class Questions
    {
        private enum _enMode
        {
            _enUpdate = 0, _enAddNew = 1
        }
        _enMode _Mode;
        public int ID;
        public string Text;
        public List<string> Options;
        public int CorrectAnswer;
        public int ExamID;
        public static DataTable GetAllQuestions()
        {
            return clsQuestions.GetAllQuestions();
        }
        public static List<Questions> GetAllQuestionsInExam(int examid)
        {
            DataTable dt = clsQuestions.GetAllQuestions();
            List<Questions> questions = new List<Questions>();
            foreach (DataRow dr in dt.Rows)
            {
                if ((int)dr["ExamID"] == examid)
                {

                    questions.Add(

                        Find((int)dr["ID"])
                        );
                }
            }
            return questions;

        }
        private Questions(int id, string text, List<string> options, int correctanswer, int examid)
        {
            ID = id;
            Options = options;
            CorrectAnswer = correctanswer;
            ExamID = examid;
            Text = text;
            _Mode = _enMode._enUpdate;
        }
        public Questions()
        {
            ID = -99;
            Options = new List<string> { "", "", "", "" };
            CorrectAnswer = -99;
            ExamID = 0;
            _Mode = _enMode._enAddNew;
        }
        public static Questions Find(int id)
        {
            List<string> option = new List<string> { "", "", "", "" };
            int correctanswer = 0;
            int examid = 0;
            string text = "";
            if (clsQuestions.GetQuestionByID(id, ref text, option, ref correctanswer, ref examid))
            {
                return new Questions(id, text, option, correctanswer, examid);
            }
            return null;
        }
        private bool _AddQuestions()
        {
            this.ID = clsQuestions.AddQuestions(this.Text, this.Options, this.CorrectAnswer, this.ExamID);
            return (this.ID != -99);
        }
        private bool _UpdateQuestions()
        {
            return clsQuestions.UpdateQuestions(this.ID, this.Text, this.Options, this.CorrectAnswer, this.ExamID);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case _enMode._enAddNew:
                    if (_AddQuestions())
                    {
                        _Mode = _enMode._enUpdate;
                        return true;
                    }
                    return false;
                case _enMode._enUpdate:
                    return _UpdateQuestions();
            }
            return false;
        }
        public static bool Deleted(int id)
        {
            return clsQuestions.DeleteQuestions(id);
        }
    }
}
