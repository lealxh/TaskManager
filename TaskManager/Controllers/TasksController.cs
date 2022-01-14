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
                        taskData.State = "Running";
                        await _myContext.SaveChangesAsync();
                
                    }
                    await Task.Delay(1000);

                } while (!thread.Source.IsCancellationRequested && taskData.TotalWork > taskData.CurrentWork);
            }
            catch (System.Threading.Tasks.TaskCanceledException)
            {
                throw;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // GET: Processes
        public async Task<IActionResult> TasksPartial()
        {
            using (var _context = dbcontextFactory.Create())
            {
                return PartialView(await _context.Tasks.ToListAsync());
            }
        }

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

        // GET: Processes
        public async Task<IActionResult> StartAll()
        {
            using (var _context = dbcontextFactory.Create())
            {
                var tasks = await _context.Tasks.ToListAsync();
                foreach (var item in tasks)
                    this.threadManager.CreateTask(CallBack, item).Start();

                return Json(new { isValid = true });
            }
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Tasks/Create
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
