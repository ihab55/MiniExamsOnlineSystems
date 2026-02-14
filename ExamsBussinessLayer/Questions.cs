using DataAccessLayer;
using DataAccessLayer.DTOs;
using System.Collections.Generic;
using System.Linq;

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
        public static List<Questions> GetAllQuestions()
        {
            var questionDTOs = clsQuestions.GetAllQuestions();
            return questionDTOs.Select(dto => new Questions(dto)).ToList();
        }
        public static List<Questions> GetAllQuestionsInExam(int examid)
        {
            var questionDTOs = clsQuestions.GetAllQuestionsInExam(examid);
            return questionDTOs.Select(dto => new Questions(dto)).ToList();
        }
        private Questions(QuestionDTO dto)
        {
            ID = dto.ID;
            Options = dto.Options;
            CorrectAnswer = dto.CorrectAnswer;
            ExamID = dto.ExamID;
            Text = dto.Text;
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
            var dto = clsQuestions.GetQuestionByID(id);
            return dto != null ? new Questions(dto) : null;
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
