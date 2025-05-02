using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BussinessLayer
{
    public class Users
    {
        private enum _enMode
        {
            _enUpdate = 0, _enAddNew = 1
        }
        _enMode _Mode;
        public int ID;
        public string UserName;
        public string PassWord;
        public bool IsAdmin;
        public static List<Users> GetAllUsers()
        {
            DataTable dt = clsUsers.GetAllUsers();
            List<Users> users = new List<Users>();
            foreach (DataRow dr in dt.Rows)
            {
                users.Add(
                    Find((int)dr["ID"])
                    );
            }
            return users;
        }
        private Users(int id, string userName, string password, bool isadmin)
        {
            ID = id;
            UserName = userName;
            PassWord = password;
            IsAdmin = isadmin;
            _Mode = _enMode._enUpdate;
        }
        public Users()
        {
            ID = -99;
            UserName = "";
            PassWord = "";
            IsAdmin = false;
            _Mode = _enMode._enAddNew;
        }
        public static Users Find(int id)
        {
            string username = "";
            string password = "";
            bool isadmin = false;
            if (clsUsers.GetUserByID(id, ref username, ref password, ref isadmin))
            {
                return new Users(id, username, password, isadmin);
            }
            return null;
        }
        public static Users Find(string username)
        {
            int id = -99;
            string password = "";
            bool isadmin = false;
            if (clsUsers.GetUserByUserName(username, ref id, ref password, ref isadmin))
            {
                return new Users(id, username, password, isadmin);
            }
            return null;
        }
        private bool _AddUsers()
        {
            this.ID = clsUsers.AddUsers(this.UserName, this.PassWord, this.IsAdmin);
            return (this.ID != -99);
        }
        private bool _UpdateUsers()
        {
            return clsUsers.UpdateUsers(this.ID, this.UserName, this.PassWord, this.IsAdmin);
        }
        public static bool IsUserExists(string username)
        {
            return clsUsers.IsExsistUser(username);
        }
        public static bool IsUserExists(string username, string password)
        {
            return clsUsers.IsExsistUser(username, password);
        }
        public bool Save()
        {
            switch (_Mode)
            {
                case _enMode._enAddNew:
                    if (_AddUsers())
                    {
                        _Mode = _enMode._enUpdate;
                        return true;
                    }
                    return false;
                case _enMode._enUpdate:
                    return _UpdateUsers();
            }
            return false;
        }
        public static bool Deleted(int id)
        {
            return clsUsers.DeleteUsers(id);
        }
    }
}
