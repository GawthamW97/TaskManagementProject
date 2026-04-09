namespace TaskManagementApp.Models.DomainModels
{
    public class BaseUser
    {
        public int Id{ get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public int? ProjectId { get; set; }
    }
}
