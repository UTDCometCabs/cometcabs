using CometCabsAdmin.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CometCabsAdmin.Dal.Mapping
{
    public class RouteMap : EntityTypeConfiguration<Route>
    {
        public RouteMap()
        {
            HasKey(t => t.Id);
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.RouteName).IsRequired().HasMaxLength(50).HasColumnType("nvarchar");
            Property(t => t.RouteDesc).IsRequired().HasColumnType("text");
            Property(t => t.IsActive).HasColumnType("bit");
            Property(t => t.RouteColor).IsRequired().HasMaxLength(10).HasColumnType("nvarchar");
            Property(t => t.CreatedBy).IsRequired().HasColumnType("nvarchar").HasMaxLength(20);
            Property(t => t.CreateDate).IsRequired();
            Property(t => t.UpdatedBy).HasColumnType("nvarchar").HasMaxLength(20);
            Property(t => t.UpdateDate);
            Property(t => t.IPAddress).HasColumnType("nvarchar").HasMaxLength(15);

            HasMany<RouteCoordinates>(t => t.RouteCoordinates)
                .WithRequired(t => t.Route);

            ToTable("Routes");
        }
    }
}
