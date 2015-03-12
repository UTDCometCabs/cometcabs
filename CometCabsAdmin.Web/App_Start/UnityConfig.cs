﻿using System.Configuration;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using CometCabsAdmin.Dal;
using CometCabsAdmin.Model.Common;
using CometCabsAdmin.Model.Contracts;
using CometCabsAdmin.Model.DataServices;

namespace CometCabsAdmin.Web
{
    public class UnityConfig
    {
        public static IUnityContainer GetConfiguredContainer()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityServiceLocator(container));

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            RegisterTypes(container);

            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CometCabsConnectionString"].ConnectionString.ToString();

            container.RegisterType<IConnections, Connections>(new InjectionConstructor(connectionString));
            container.RegisterType<IEncryption, Encryption>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IDbContext, CometCabsDbContext>();
            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}