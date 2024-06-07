namespace Todo.Models.Home
{
    public class TemplateForm
    {
        public List<Templates>? Templates { get; set; }
        public int templateId { get; set; }

        public string taskName { get; set; }

        public string teamName { get; set; }

        public string managerName { get; set; }

        public string comment { get; set; }
    }
    public class Templates
    {
        public int templateId { get; set; }

        public string taskName { get; set; }

        public string teamName { get; set; }

        public string managerName { get; set; }

        public string comment { get; set; }
    }
}
