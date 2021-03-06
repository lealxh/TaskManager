using TaskManager.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Models
{
    /// <summary>
    /// Sevice to create a new DbContext when needed.
    /// </summary>
    public class DbContextFactory : IDbContextFactory
    {

        public DbContextFactory(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }

        public IConfiguration Configuration { get; }

        public DataContext Create()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
               .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")).Options;

            return new DataContext(options);
        }
    }
}
