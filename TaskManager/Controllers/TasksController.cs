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

        public TasksController(IDbContextFactory dbcontextFactory)
        {
            this.dbcontextFactory = dbcontextFactory;
        }
        public async Task<IActionResult> Index()
        {
            using (var _context = dbcontextFactory.Create())
            {
                return View(await _context.Tasks.ToListAsync());
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
