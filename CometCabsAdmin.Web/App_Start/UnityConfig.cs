using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using CometCabsAdmin.Model.Contracts;
using System.Configuration;
using CometCabsAdmin.Model.Common;
using CometCabsAdmin.Model.DataServices;
using CometCabsAdmin.Dal;
using Unity.WebApi;
using System.Web.Http;

namespace CometCabsAdmin.Web
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            string connectionString = ConfigurationManager.ConnectionStrings["CometCabsConnectionString"].ConnectionString.ToString();

            container.RegisterType<IConnections, Connections>(new InjectionConstructor(connectionString));
            container.RegisterType<IEncryption, Encryption>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IRouteService, RouteService>();
            container.RegisterType<ICabService, CabService>();
            container.RegisterType<IInterestsService, InterestsService>();
            container.RegisterType<IDbContext, CometCabsDbContext>();
            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
