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
    public class RouteCoordinatesMap : EntityTypeConfiguration<RouteCoordinates>
    {
        public RouteCoordinatesMap()
        {
            HasKey(t => t.Id);
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Latitude).IsRequired().HasColumnType("float");
            Property(t => t.Longitude).IsRequired().HasColumnType("float");
            Property(t => t.CreatedBy).IsRequired().HasColumnType("nvarchar").HasMaxLength(20);
            Property(t => t.CreateDate).IsRequired();
            Property(t => t.UpdatedBy).HasColumnType("nvarchar").HasMaxLength(20);
            Property(t => t.UpdateDate);
            Property(t => t.IPAddress).HasColumnType("nvarchar").HasMaxLength(15);

            HasRequired<Route>(t => t.Route)
                .WithMany(t => t.RouteCoordinates);

            ToTable("RouteCoordinates");
        }
    }
}
