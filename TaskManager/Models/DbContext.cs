using Microsoft.EntityFrameworkCore;

namespace TaskManager.Models
{
    //DataContext for EF core 
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
        }
        public DbSet<TaskData> Tasks { get; set; }
    }
}
