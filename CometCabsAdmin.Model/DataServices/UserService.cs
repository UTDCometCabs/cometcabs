using System.Linq;
using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Model.Entities;

namespace CometCabsAdmin.Model.DataServices
{
    public class UserService : IUserService
    {
        private IRepository<User> _userRepository;
        private IRepository<UserProfile> _userProfileRepository;
        private IRepository<UserRoles> _userRolesRepository;

        public UserService(IRepository<User> userRepository
            , IRepository<UserProfile> userProfileRepository
            , IRepository<UserRoles> userRolesRepository)
        {
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
            _userRolesRepository = userRolesRepository;
        }

        #region IUserService Members

        public IQueryable<User> GetUsers()
        {
            return _userRepository.Table;
        }

        public User GetUser(long id)
        {
            return _userRepository.GetById(id);
        }

        public void InsertUser(User user)
        {
            _userRepository.Insert(user);
        }

        public void UpdateUser(User user)
        {
            _userRepository.Update(user);
        }

        public void DeleteUser(User user)
        {
            _userRolesRepository.Delete(user.UserRole);
            _userProfileRepository.Delete(user.UserProfile);
            _userRepository.Delete(user);
        }

        #endregion
    }
}
