namespace Todo.Models.Home
{
    public class TaskForm
    {
        public List<Tasks>? Tasks { get; set; }
        public List<Teams>? Teams { get; set; }

        public List<Templates2>? Templates2 { get; set; }


        public int taskId { get; set; }

        public string taskName { get; set; }

        public string teamName { get; set; }

        public string managerName { get; set; }

        public DateTime? limitTime { get; set; }

        public string comment { get; set; }

    }
    public class Tasks
    {

        
        public int taskId { get; set; }
    

        public string taskName { get; set; }

        public string teamName { get; set; }

        public string managerName { get; set; }

        public DateTime? limitTime { get; set; }

        public string comment { get; set; }
    }

    public class Teams
    {
        public string teamName { get; set; }

    }

    public class Templates2
    {
        public int templateId { get; set; }

        public string taskName { get; set; }

        public string teamName { get; set; }

        public string managerName { get; set; }

        public string comment { get; set; }
    }

}
