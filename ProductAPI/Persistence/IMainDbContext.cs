using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace ProductAPI.Persistence
{
    public interface IMainDbContext : IDisposable
    {
        EntityEntry Entry(object entity);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
