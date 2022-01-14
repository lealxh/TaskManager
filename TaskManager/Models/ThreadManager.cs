﻿using TaskManager.Interfaces;

namespace TaskManager.Models
{
    /// <summary>
    /// Service to create Threads and manage the Cancelation tokens of those Threads
    /// </summary>
    public class ThreadManager : IThreadManager
    {

        //List of state of Threads running
        public List<ThreadState> States { get; set; }
        public ThreadManager()
        {
            States = new List<ThreadState>();
        }
        public void CancelTask(int id)
        {
            var state = States.SingleOrDefault(t => t.TaskId == id);
            state.Source.Cancel();
            States.Remove(state);

        }

        public Task CreateTask(System.Action<object> action, TaskData task)
        {
            ThreadState state = new ThreadState()
            {
                TaskId = task.Id,
                Source = new System.Threading.CancellationTokenSource()
            };
            States.Add(state);
            return new Task(action, state, state.Source.Token);

        }
    }
}