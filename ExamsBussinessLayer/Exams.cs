using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataAccessLayer.DTOs;

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
            var examDTOs = clsExams.GetAllExams();
            return examDTOs.Select(dto => new Exams(dto)).ToList();
        }

        public string Admin { get; private set; }

        private Exams(ExamDTO dto)
        {
            ID = dto.ID;
            Title = dto.Title;
            Description = dto.Description;
            QuizTime = dto.QuizTime;
            CreatedDate = dto.CreatedDate;
            CreateByAdmin = dto.CreatedByAdminID;
            Admin = dto.AdminName;
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
            var dto = clsExams.GetExamByID(id);
            return dto != null ? new Exams(dto) : null;
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
