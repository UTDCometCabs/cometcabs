using System.Linq;

namespace CometCabsAdmin.Model.Contracts
{
    public interface IRepository<T> where T : BaseEntity
    {
        T GetById(object Id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> Table { get; }
    }
}
