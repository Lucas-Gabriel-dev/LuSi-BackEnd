namespace LuSiBack.src.models
{
    public class TaskOption
    {
        public int Id { get; set; }
        public string Name  { get; set; }
        
        public int CurrentTaskId  { get; set; }
        public TaskUser TaskUser { get; set; }

        public bool Complete { get; set; }
    }
}