using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Microsoft.Practices.Unity;
using CometCabsAdmin.Dal;
using CometCabsAdmin.Model.Common;
using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Model.DataServices;

namespace CometCabsAdmin.Service
{
    public class UnityServiceHostFactory : ServiceHostFactory
    {
        private readonly IUnityContainer _container;

        public UnityServiceHostFactory()
        {
            _container = new UnityContainer();
            RegisterTypes(_container);
        }

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return new UnityServiceHost(this._container, serviceType, baseAddresses);
        }

        private void RegisterTypes(IUnityContainer container)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CometCabsConnectionString"].ConnectionString.ToString();

            container.RegisterType<IConnections, Connections>(new InjectionConstructor(connectionString));
            container.RegisterType<IEncryption, Encryption>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IRouteService, RouteService>();
            container.RegisterType<IDbContext, CometCabsDbContext>();
            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
