using System.Data.Entity;

namespace CometCabsAdmin.Model.Contracts
{
    public interface IDbContext
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;
        int SaveChanges();
    }
}
