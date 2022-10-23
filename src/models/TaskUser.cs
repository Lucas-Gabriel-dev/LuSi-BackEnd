namespace LuSiBack.src.models
{
    public class TaskUser
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        

        public int UserTaskId { get; set; }
        public User User { get; set; }

        public DateTime DeadLine { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<TaskOption> TaskOptions { get; set; }
    }
}