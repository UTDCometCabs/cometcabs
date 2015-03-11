using CometCabsAdmin.Model.Contracts;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;

namespace CometCabsAdmin.Dal
{
    public class CometCabsDbContext : DbContext, IDbContext
    {
        public CometCabsDbContext(IConnections connections) : base(connections.CometCabsConnectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => !string.IsNullOrEmpty(type.Namespace))
                .Where(
                    type => (type.BaseType != null)
                    && (type.BaseType.IsGenericType)
                    && (type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>))
                );

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            base.OnModelCreating(modelBuilder);
        }

        #region IDbContext Members

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : Model.BaseEntity
        {
            return base.Set<TEntity>();
        }

        #endregion
    }
}
