using System.Linq;
using CometCabsAdmin.Model.Entities;

namespace CometCabsAdmin.Model.Contracts
{
    public interface IUserService
    {
        IQueryable<User> GetUsers();
        User GetUser(long id);
        void InsertUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
    }
}
