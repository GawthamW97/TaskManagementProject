namespace TaskManagementApp.Models.DTOs
{
    public class GetTaskCommentDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int TaskId { get; set; }
    }
}
