using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Controllers
{

    public class TasksController : Controller
    {
       
        private readonly IDbContextFactory dbcontextFactory;
        private readonly IThreadManager threadManager;

        public TasksController(IDbContextFactory dbcontextFactory, IThreadManager threadManager)
        {
            this.dbcontextFactory = dbcontextFactory;
            this.threadManager = threadManager;
        }
        public async Task<IActionResult> Index()
        {
            using (var _context = dbcontextFactory.Create())
            {
                return View(await _context.Tasks.ToListAsync());
            }
            
        }
        private async void UpdateState(Object state, string newState)
        {
            Models.ThreadState thread = state as Models.ThreadState;
            using (var _myContext = dbcontextFactory.Create())
            {
                var taskData = await _myContext.Tasks.FindAsync(thread.TaskId);
                taskData.State = newState;
                await _myContext.SaveChangesAsync();

            }
        }
        //The code executed by the Thread created
        public async void CallBack(Object state)
        {
            try
            {
                Models.ThreadState thread = state as Models.ThreadState;
                TaskData taskData = new TaskData();
                do
                {   
                    using (var _myContext = dbcontextFactory.Create())
                    {
                        taskData = await _myContext.Tasks.FindAsync(thread.TaskId);
                        taskData.CurrentWork++;
                        if(taskData.TotalWork>taskData.CurrentWork)
                            taskData.State = "Running";
                        else
                            taskData.State = "Finished";

                        await _myContext.SaveChangesAsync();
                
                    }
                    await Task.Delay(3000);

                } while (!thread.Source.IsCancellationRequested && taskData.TotalWork > taskData.CurrentWork);
            }
            catch (System.Threading.Tasks.TaskCanceledException)
            {
                UpdateState(state, "NotRunning");

            }
           
        }

        // GET method to update the Tasks visual state
        public async Task<IActionResult> TasksPartial()
        {
            using (var _context = dbcontextFactory.Create())
            {
                return PartialView(await _context.Tasks.ToListAsync());
            }
        }

        // Get method to delete the completed Task
        public async Task<IActionResult> Delete(int id)
        {
            using (var _context = dbcontextFactory.Create())
            {
                var task = await _context.Tasks.SingleOrDefaultAsync(x => x.Id == id);
                if (task != null)
                {
                    threadManager.RemoveTask(task.Id);
                    _context.Tasks.Remove(task);
                    await _context.SaveChangesAsync();
                    
                    return Json(new { Success = true });
                }

                return Json(new { Success = false });
            }
        }

        //Get method to Cancel a Thread curently running
        public async Task<IActionResult> CancelThread(int id)
        {
            using (var _context = dbcontextFactory.Create())
            {
                var task = await _context.Tasks.SingleOrDefaultAsync(x=>x.Id==id);
                if (task != null)
                {
                    task.State = "NotRunning";
                    await _context.SaveChangesAsync();
                    threadManager.CancelTask(task.Id);
                    return Json(new { Success = true });
                }


                return Json(new { Success = false });
            }
        }

        // GET method to start a Single Thread 
        public async Task<IActionResult> StartSingle(int id)
        {
            using (var _context = dbcontextFactory.Create())
            {
                var task = await _context.Tasks.SingleOrDefaultAsync(t=>t.Id==id);
                if(task!=null)
                this.threadManager.CreateTask(CallBack, task).Start();
                else
                    return Json(new { Success = false });


                return Json(new { isValid = true });
            }
        }

        // GET method to Start all the Task in state NotRunning
        public async Task<IActionResult> StartAll()
        {
            using (var _context = dbcontextFactory.Create())
            {
                var tasks = await _context.Tasks.ToListAsync();
                foreach (var item in tasks)
                {
                    if(item.State=="NotRunning")
                    this.threadManager.CreateTask(CallBack, item).Start();
                }

                return Json(new { isValid = true });
            }
        }

        // GET method to Show the Form of Task creation
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: to save the data of the Task sent
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,TotalWork,CurrentWork,State")] TaskData taskData)
        {
           
            if (ModelState.IsValid)
            {
                using (var _context = dbcontextFactory.Create())
                {
                    _context.Add(taskData);
                    await _context.SaveChangesAsync();
                    return Json(new { isValid = true });
                }
            }
             return Json(new { isValid = false });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
