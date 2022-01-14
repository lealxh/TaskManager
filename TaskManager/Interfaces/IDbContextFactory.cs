using TaskManager.Models;

namespace TaskManager.Interfaces
{
    public interface IDbContextFactory
    {
        DataContext Create();
    }
}
