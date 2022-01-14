namespace TaskManager.Models
{
    /// <summary>
    /// Thread data to keep in memory of the running threads
    /// TaskId: is the Id in the TaskData table
    /// Source: Contains the Cancelation token of the Thread created
    /// </summary>
    public class ThreadState
    {
        public int TaskId { get; set; }
        public CancellationTokenSource Source { get; set; }
    }
}
