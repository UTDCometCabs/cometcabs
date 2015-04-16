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
    public class CabActivityMap : EntityTypeConfiguration<CabActivity>
    {
        public CabActivityMap()
        {
            HasKey(t => t.Id);
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.LoginTime).IsRequired();
            Property(t => t.CreatedBy).IsRequired().HasColumnType("nvarchar").HasMaxLength(20);
            Property(t => t.CreateDate).IsRequired();
            Property(t => t.UpdatedBy).HasColumnType("nvarchar").HasMaxLength(20);
            Property(t => t.UpdateDate);
            Property(t => t.IPAddress).HasColumnType("nvarchar").HasMaxLength(15);

            HasRequired<Cab>(t => t.Cab)
                .WithMany(t => t.CabActivity);

            HasRequired<User>(t => t.Driver)
                .WithMany(t => t.CabActivity);

            HasMany<CabCoordinate>(t => t.CabCoordinate)
                .WithRequired(t => t.CabActivity);

            ToTable("CabActivity");
        }
    }
}
