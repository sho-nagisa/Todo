using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Todo.Models;
using Todo.Models.Home;
using Dapper;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Reflection.Metadata;

namespace Todo.Controllers
{
    public class TemplateController : Controller
    {
        private readonly string _connectionString;
        public TemplateController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaulteConnection");
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Index(TemplateForm templateform)
        {
            if (ModelState.IsValid)
            {
                return View(new TemplateForm());
            }

            using var con = new SqlConnection(_connectionString);

            //読み取り
            var sql = "select * from templateTable";
            var temp = con.Query<Templates>(sql).ToList(); //タスクをリストとして取得する
            templateform.Templates = temp;
            //Execute()select文以外
            return View("Index", templateform);
        }
        public IActionResult TemplateAdd()
        {
            return View();
        }
        [HttpPost]
        public IActionResult TemplateAdd(TemplateForm templateform) 
        {
            if (templateform.taskName == null && templateform.teamName == null && templateform.managerName == null & templateform.comment == null)
            {
                ViewData["error1"] = "どれか一つ入力してください";
                return View("TemplateAdd");
            }

            using var con = new SqlConnection(_connectionString);
            
            var sql = @"
insert into templateTable (taskName, teamName,managerName,comment) 
values(@taskName,@teamName,@managerName,@comment)
";
            var param = new DynamicParameters();
            param.Add("taskName", templateform.taskName);
            param.Add("teamName", templateform.teamName);
            param.Add("managerName", templateform.managerName);
            param.Add("comment", templateform.comment);
  
            con.Execute(sql, param);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult deleteTemplate(IEnumerable<int> targetId)
        {
            using var con = new SqlConnection(_connectionString);
            var param = new DynamicParameters();
            foreach (int id in targetId)
            {
                param.Add("id", id);
                var sql = "DELETE FROM templateTable WHERE templateId = @id";
                con.Execute(sql,param);
            }          
            return RedirectToAction(nameof(Index));
        }

       
            
    

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }

}
