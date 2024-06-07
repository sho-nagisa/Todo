using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Todo.Models;
using Todo.Models.Home;
using Dapper;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Todo.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly string _connectionString;


        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("DefaulteConnection");
        }


        [HttpGet]
        public IActionResult Index()
        {

            var taskForm = new TaskForm();

            using var con = new SqlConnection(_connectionString);

            //読み取り
            var sql = "select * from taskTable order by limitTime";
            var task = con.Query<Tasks>(sql).ToList(); //タスクをリストとして取得する
            taskForm.Tasks = task;
            //Execute()select文以外
            using var con2 = new SqlConnection(_connectionString);
            //書き込み
            var sql2 = "SELECT DISTINCT teamName FROM taskTable ORDER BY teamName";
            var team = con.Query<Teams>(sql2).ToList(); //チームをリストとして取得する
            taskForm.Teams = team;
            return View("Index", taskForm);

        }

        [HttpPost]
        public IActionResult filterTeam(IEnumerable<String> filterTeams)
        {
            var taskform = new TaskForm();
            if (!filterTeams.Any())
            {
                return RedirectToAction("Index");
            }

            taskform.Tasks = new List<Tasks>();

            using var con = new SqlConnection(_connectionString);
            foreach (String teamname in filterTeams)
            {
                var sql = "select * from taskTable where teamName=@teamName order by limitTime";
                var param = new DynamicParameters();
                param.Add("teamName", teamname);
                var task = con.Query<Tasks>(sql, param).ToList(); //タスクをリストとして取得する
                taskform.Tasks.AddRange(task);
            }
            //書き込み
            var sql2 = "SELECT DISTINCT teamName FROM taskTable ORDER BY teamName";
            var team = con.Query<Teams>(sql2).ToList(); //チームをリストとして取得する
            taskform.Teams = team;

            return View("Index", taskform);
        }




        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult valtemp()
        {
            return View();
        }

        [HttpGet]
        public IActionResult addTask(TaskForm taskForm)
        {
            if (ModelState.IsValid)
            {
                return View(new TaskForm());
            }


            using var con = new SqlConnection(_connectionString);

            //読み取り
            var sql = "select * from templateTable";
            var template = con.Query<Templates2>(sql).ToList(); //タスクをリストとして取得する
            taskForm.Templates2 = template;
            return View("addTask", taskForm);


        }



        [HttpPost]
        public IActionResult addTask2(TaskForm taskForm)
        {
            using var con = new SqlConnection(_connectionString);
            //書き込み
            var sql = @"
insert into taskTable (taskName, teamName,managerName,limitTime,comment) 
values(@taskName,@teamName,@managerName,@limitTime,@comment)
";
            var param = new DynamicParameters();
            param.Add("taskName", taskForm.taskName);
            param.Add("teamName", taskForm.teamName);
            param.Add("managerName", taskForm.managerName);
            param.Add("limitTime", taskForm.limitTime);
            param.Add("comment", taskForm.comment);
            con.Execute(sql, param);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult finishTask(IEnumerable<int> finishNum)
        {

            using var con = new SqlConnection(_connectionString);
            var param = new DynamicParameters();
            var sql = "DELETE FROM taskTable WHERE taskId = @finishNum";
            param.Add("finishNum", finishNum);

            con.Execute(sql, param);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult useTemplate(int flexRadioDefault, TaskForm taskForm)
        {

            using var con = new SqlConnection(_connectionString);
            var param = new DynamicParameters();
            //読み取り

            var sql = "select * from templateTable";
            var template = con.Query<Templates2>(sql).ToList(); //タスクをリストとして取得する
            taskForm.Templates2 = template;

            //今適用したいテンプレートを選ぶ
            var selected = taskForm.Templates2.First(a => a.templateId == flexRadioDefault);


            taskForm.managerName = selected.managerName;
            taskForm.taskName = selected.taskName;
            taskForm.teamName = selected.teamName;
            taskForm.comment = selected.comment;
            //Execute()select文以外
            return View("addTask", taskForm);

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}