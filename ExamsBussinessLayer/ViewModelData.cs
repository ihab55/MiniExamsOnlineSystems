using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessLayer;
using DataAccessLayer;

using DataAccessLayer.DTOs;

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
            var dtos = clsUsersExam.GetAllUsersExam();
            return dtos.Select(dto => new ViewModelData
            {
                ID = dto.ID,
                Username = dto.UserName,
                ExamTitle = dto.ExamTitle,
                Score = dto.Score,
                TotalPoint = dto.TotalPoint,
                Percentage = dto.Percentage,
                Status = dto.IsPass
            }).ToList();
        }
    }
}
