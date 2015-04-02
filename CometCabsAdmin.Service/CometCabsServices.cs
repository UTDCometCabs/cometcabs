using System;
using System.Linq;
using System.ServiceModel;
using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Service.Contracts;
using System.Collections.Generic;

namespace CometCabsAdmin.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class CometCabsServices : IUser
    {
        private IUserService _userService;

        public CometCabsServices(IUserService userService)
        {
            _userService = userService;
        }

        #region IUser Members

        public Model.Entities.User Login(string userName, string password)
        {
            return _userService.GetUsers()
                .Where(s => s.Username.Equals(userName) && s.Password.Equals(password))
                .SingleOrDefault();
        }

        public Dictionary<int, string> SaveUser(UserData data)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            try
            {
                Model.Entities.User user = _userService.GetUsers()
                    .Where(s => s.Username.Equals(data.Name) && s.EmailAddress.Equals(data.EmailAddress))
                    .SingleOrDefault();

                if (user == null)
                {
                    user = new Model.Entities.User
                    {
                        Username = data.Name,
                        Password = data.Password,
                        EmailAddress = data.EmailAddress,
                        UserProfile = new Model.Entities.UserProfile
                        {
                            FirstName = data.FirstName,
                            LastName = data.LastName,
                            Address = data.Address,
                            CreateDate = DateTime.Now,
                            CreatedBy = data.Name,
                            IPAddress = data.IPAddress,

                        },
                        UserRole = new Model.Entities.UserRoles
                        {
                            RoleName = "User",
                            CreateDate = DateTime.Now,
                            CreatedBy = data.Name,
                            IPAddress = data.IPAddress,
                        },
                        CreateDate = DateTime.Now,
                        CreatedBy = data.Name,
                        IPAddress = data.IPAddress,
                    };

                    _userService.InsertUser(user);
                }
                else
                {
                    user.UserProfile.FirstName = data.FirstName;
                    user.UserProfile.LastName = data.LastName;
                    user.UserProfile.Address = data.Address;
                    user.UserProfile.UpdateDate = DateTime.Now;
                    user.UserProfile.UpdatedBy = data.Name;
                    user.UserProfile.IPAddress = data.IPAddress;

                    _userService.UpdateUser(user);
                }

                result.Add(0, string.Empty);
            }
            catch (Exception ex)
            {
                result.Add(1000, string.Format("An error has occured: {0}, stack trace: {1}", ex.Message, ex.StackTrace));
            }

            return result;
        }

        #endregion
    }
}
