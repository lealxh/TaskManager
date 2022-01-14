namespace TaskManager.Models
{
    public class ThreadState
    {
        public int TaskId { get; set; }
        public CancellationTokenSource Source { get; set; }
    }
}
