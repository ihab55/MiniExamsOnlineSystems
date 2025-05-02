using BussinessLayer;
using Microsoft.AspNetCore.Mvc;

namespace MiniExamsWeb.Controllers
{
    public class AdminController : Controller
    {
        private static Users _currentUsers;
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ActionToLogin(string Username, string Password)
        {
            ViewBag.ErrorMessage = "";
            if (Users.IsUserExists(Username, Password))
            {
                _currentUsers = Users.Find(Username);
                if (_currentUsers.IsAdmin)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.ErrorMessage = "You are not an Admin ";
                return View("Login");
            }
            ViewBag.ErrorMessage = "Invalid UserName or Password";
            return View("Login");
        }
        public IActionResult EditExams()
        {
            List<Exams> examList = Exams.GetAllExam();
            return View(examList);
        }
        public IActionResult EditUser()
        {
            List<Users> users = Users.GetAllUsers();
            return View(users);
        }
        public IActionResult SaveExam(string Title, string Description, int QuizeTime = 60)
        {
            Exams exams = new Exams();
            exams.Title = Title;
            exams.Description = Description;
            exams.QuizTime = QuizeTime;
            exams.CreatedDate = DateTime.Now;
            exams.CreateByAdmin = _currentUsers.ID;
            exams.Save();
            return RedirectToAction("EditExams");
        }
        public IActionResult DeleteExam(int id)
        {
            Exams.Deleted(id);
            return RedirectToAction("EditExams");
        }
        public IActionResult EditQuestion(int id)
        {
            Exams exams = Exams.Find(id);
            List<Questions> questions = Questions.GetAllQuestionsInExam(exams.ID);
            ViewBag.Exam = exams;
            return View(questions);
        }
        public IActionResult DeleteQuestion(int questionId, int examId)
        {
            Questions.Deleted(questionId);
            return Redirect($"/Admin/EditQuestion?id={examId}");
        }
        public IActionResult SaveQuestion(int QuestionID, string Text, List<string> Options, int CorrectAnswer)
        {
            Questions questions = Questions.Find(QuestionID);
            questions.Text = Text;
            questions.Options = Options;
            questions.CorrectAnswer = CorrectAnswer;
            questions.Save();
            return Redirect($"/Admin/EditQuestion?id={questions.ExamID}");
        }
        public IActionResult AddQuestion(List<string> Options, int CorrectAnswer, int examId, string Text)
        {
            Questions questions = new Questions();
            questions.Text = Text;
            questions.Options = Options;
            questions.CorrectAnswer = CorrectAnswer;
            questions.ExamID = examId;
            questions.Save();
            return Redirect($"/Admin/EditQuestion?id={questions.ExamID}");
        }
        public IActionResult Login()
        {
            return View();
        }
        //  Users
        public IActionResult ActionAddOrEdit(string UserName, string PassWord, bool IsAdmin)
        {
            Users users;
            if (Users.IsUserExists(UserName))
            {
                users = Users.Find(UserName);
                users.PassWord = PassWord;
                users.IsAdmin = IsAdmin;
            }
            else
            {
                users = new Users();
                users.UserName = UserName;
                users.PassWord = PassWord;
                users.IsAdmin = IsAdmin;
            }
            users.Save();
            return RedirectToAction("EditUser");
        }
        public IActionResult DeleteUser(int id)
        {
            Users.Deleted(id);
            return RedirectToAction("EditUser");
        }
    }
}
