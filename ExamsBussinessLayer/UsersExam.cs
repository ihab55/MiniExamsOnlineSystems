using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BussinessLayer
{
    public class UsersExam
    {
        private enum _enMode
        {
            _enUpdate = 0, _enAddNew = 1
        }
        _enMode _Mode;
        public int ID;
        public int NumOfPoint;
        public int TotalPoint;
        public bool IsPass;
        public DateTime TakenDate;
        public int UserID;
        public int ExamID;

        public static List<UsersExam> GetAllUsersExam()
        {
            List<UsersExam> lstUsersExam = new List<UsersExam>();
            DataTable dt = clsUsersExam.GetAllUsersExam();
            foreach (DataRow dr in dt.Rows) {
                lstUsersExam.Add(
                    Find((int)dr["ID"])
                    );
            }
            return lstUsersExam;
        }
        private UsersExam(int id, int numofpoint, int total, bool ispass, DateTime takenday, int userid, int examid)
        {
            ID = id;
            NumOfPoint = numofpoint;
            TotalPoint = total;
            IsPass = ispass;
            TakenDate = takenday;
            UserID = userid;
            ExamID = examid;
            _Mode = _enMode._enUpdate;
        }
        public UsersExam()
        {
            ID = -99;
            NumOfPoint = 0;
            TotalPoint = 0;
            IsPass = false;
            TakenDate = DateTime.Now;
            UserID = 0;
            ExamID = 0;
            _Mode = _enMode._enAddNew;
        }
        public static UsersExam Find(int id)
        {
            int numOfPoint = 0;
            int totalPoint = 0;
            bool isPass = false;
            DateTime takenDate = DateTime.Now;
            int userID = 0;
            int examID = 0;
            if (clsUsersExam.GetUserExamByID(id, ref numOfPoint, ref totalPoint, ref isPass, ref takenDate, ref userID, ref examID))
            {
                return new UsersExam(id, numOfPoint, totalPoint, isPass, takenDate, userID, examID);
            }
            return null;
        }
        private bool _AddUsersExam()
        {
            this.ID = clsUsersExam.AddUsersExam(this.NumOfPoint, this.TotalPoint, this.IsPass, this.TakenDate, this.UserID, this.ExamID);
            return (this.ID != -99);
        }
        private bool _UpdateUsersExam()
        {
            return clsUsersExam.UpdateUsersExam(this.ID, this.NumOfPoint, this.TotalPoint, this.IsPass, this.TakenDate, this.UserID, this.ExamID);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case _enMode._enAddNew:
                    if (_AddUsersExam())
                    {
                        _Mode = _enMode._enUpdate;
                        return true;
                    }
                    return false;
                case _enMode._enUpdate:
                    return _UpdateUsersExam();
            }
            return false;
        }
        public static bool Deleted(int id)
        {
            return clsUsersExam.DeleteUsersExam(id);
        }
    }
}
