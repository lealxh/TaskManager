namespace TaskManager.Models
{
    /// <summary>
    /// Data of the Tasks to run in thread
    /// TotalWork: is the number of iterations of this Task
    /// CurrentWork: is the currently executed iterations
    /// State is the Thread State: NotRunning, Running and Finished
    /// </summary>
    public class TaskData
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int TotalWork { get; set; }
        public int CurrentWork { get; set; }
        public string State { get; set; }
    }
}
