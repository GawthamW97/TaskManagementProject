namespace TaskManagementWeb.Models
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
