using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataAccessLayer.DTOs;

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
            var userDTOs = clsUsers.GetAllUsers();
            return userDTOs.Select(dto => new Users(dto)).ToList();
        }
        private Users(UserDTO dto)
        {
            ID = dto.ID;
            UserName = dto.UserName;
            PassWord = dto.PassWord;
            IsAdmin = dto.IsAdmin;
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
            var dto = clsUsers.GetUserByID(id);
            return dto != null ? new Users(dto) : null;
        }
        public static Users Find(string username)
        {
            var dto = clsUsers.GetUserByUserName(username);
            return dto != null ? new Users(dto) : null;
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
