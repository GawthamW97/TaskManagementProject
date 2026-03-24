namespace TaskManagementApp.Models.DomainModels
{
    public class BaseProject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public List<BaseTask> Tasks { get; set; }
        public List<BaseUser> Users { get; set; }

    }

    public class Project : BaseProject
    {
        public List<BaseTask> Tasks { get; set; }
        public List<BaseUser> TeamMembers { get; set; }
    }
}
