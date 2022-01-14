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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
