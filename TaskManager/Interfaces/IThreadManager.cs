using TaskManager.Models;

namespace TaskManager.Interfaces
{
    public interface IThreadManager
    {
        void CancelTask(int id);
        Task CreateTask(System.Action<object> action, TaskData task);
       
    }
}
