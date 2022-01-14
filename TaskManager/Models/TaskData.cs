namespace TaskManager.Models
{
    public enum state { Running, Canceled};
    public class TaskData
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int TotalWork { get; set; }
        public int CurrentWork { get; set; }
        public string State { get; set; }
    }
}
