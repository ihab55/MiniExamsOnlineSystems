using System.Diagnostics;
using BussinessLayer;
using ExamsBussinessLayer;
using Microsoft.AspNetCore.Mvc;
using MiniExamsWeb.Models;

namespace MiniExamsWeb.Controllers
{
    public class HomeController : Controller
    {
        private static Users _CurrentUser ;   
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult ViewGrades()
        {
            List<ViewModelData> usersExams = ViewModelData.GetAllUsersExams();
            return View(usersExams);
        }
        public IActionResult Index()
        {
            List<Exams> examsList = BussinessLayer.Exams.GetAllExam();
            return View(examsList);
        }
        // Action submit for login
        [HttpPost]
        public IActionResult ActionLogin(string UserName, string PassWord)
        {
            if (Users.IsUserExists(UserName, PassWord))
            {
                _CurrentUser = Users.Find(UserName);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid username or password.";
                return View("Login");
            }
        }
        public IActionResult Login()
        {
            return View();  
        }
        public IActionResult TakeExam(int id)
        {
            Exams exams = BussinessLayer.Exams.Find(id);
            exams.questions = Questions.GetAllQuestionsInExam(exams.ID);
            return View(exams);
        }
        public IActionResult ExamSubmit(int ExamID, Dictionary<int, int> UserAnswers)
        {
            Exams exam = Exams.Find(ExamID);
            exam.questions = Questions.GetAllQuestionsInExam(exam.ID);
            int score = 0;

            foreach (Questions question in exam.questions)
            {
                if (UserAnswers.TryGetValue(question.ID, out int userAnswer))
                {
                    bool isCorrect = (userAnswer == question.CorrectAnswer);
                    
                    if (isCorrect) score++;
                }
            }
            UsersExam usersExam = new UsersExam();
            usersExam.ExamID = exam.ID;
            usersExam.UserID = _CurrentUser.ID;
            usersExam.NumOfPoint = score;
            usersExam.TotalPoint = exam.questions.Count;
            usersExam.TakenDate = DateTime.Now;
            ViewBag.Percent =  ((float)usersExam.NumOfPoint / (float)usersExam.TotalPoint)*100;
            usersExam.IsPass = ViewBag.Percent > 60;
            usersExam.Save();
            return View(usersExam);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
