using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Model.Entities;
using CometCabsAdmin.Model.DataServices;
using CometCabsAdmin.Model.Common;
using Microsoft.Practices.Unity;
using System.Configuration;
using CometCabsAdmin.Dal;

namespace CometCabsAdmin.Services
{
    public class CometCabsServices : ICometCabsServices
    {
        private IEncryption _encryption;
        private IUserService _userService;

        public CometCabsServices()
        {
            _userService = DependencyInjection.RegisterTypes.Resolve<IUserService>();
            _encryption = DependencyInjection.RegisterTypes.Resolve<IEncryption>();
        }

        #region ICometCabsServices Members

        public bool Login(string userName, string password)
        {
            string pwd = _encryption.Encrypt(password);

            User result = _userService.GetUsers()
                .SingleOrDefault(s => (s.Username.Equals(userName)) && (s.Password.Equals(pwd)));

            return result != null;
        }

        #endregion
    }

    public static class DependencyInjection
    {
        private static IUnityContainer _container;

        static DependencyInjection()
        {
            _container = new UnityContainer();
        }

        public static IUnityContainer RegisterTypes
        {
            get
            {
                string connectionString = ConfigurationManager.ConnectionStrings["CometCabsConnectionString"].ConnectionString.ToString();

                _container.RegisterType<IConnections, Connections>(new InjectionConstructor(connectionString));
                _container.RegisterType<IEncryption, Encryption>();
                _container.RegisterType<IUserService, UserService>();
                _container.RegisterType<IRouteService, RouteService>();
                _container.RegisterType<IDbContext, CometCabsDbContext>();
                _container.RegisterType(typeof(IRepository<>), typeof(Repository<>));

                return _container;
            }
        }
    }
}
