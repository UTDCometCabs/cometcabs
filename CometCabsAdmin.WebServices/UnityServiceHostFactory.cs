using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Model.Common;
using CometCabsAdmin.Model.DataServices;
using CometCabsAdmin.Dal;
using CometCabsAdmin.Model.Entities;

namespace CometCabsAdmin.WebServices
{
    public class UnityServiceHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            UnityServiceHost serviceHost = new UnityServiceHost(serviceType, baseAddresses);
            UnityContainer container = new UnityContainer();

            //configure container
            string connectionString = ConfigurationManager.ConnectionStrings["CometCabsConnectionString"].ConnectionString.ToString();

            container.RegisterType<IConnections, Connections>(new InjectionConstructor(connectionString));            
            container.RegisterType<IEncryption, Encryption>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IRouteService, RouteService>();
            container.RegisterType<IDbContext, CometCabsDbContext>();
            container.RegisterType<IRepository<User>, Repository<User>>();
            container.RegisterType<IRepository<UserProfile>, Repository<UserProfile>>();
            container.RegisterType<IRepository<UserRoles>, Repository<UserRoles>>();
            container.RegisterType<IRepository<Route>, Repository<Route>>();
            container.RegisterType<IRepository<RouteCoordinates>, Repository<RouteCoordinates>>();
            container.RegisterType<IRepository<RouteDirections>, Repository<RouteDirections>>();
            
            //UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            //section.Configure(serviceHost.Container);

            serviceHost.Container = container;

            return serviceHost;
        }
    }
}