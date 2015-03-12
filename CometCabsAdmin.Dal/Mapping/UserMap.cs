using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CometCabsAdmin.Model.Entities;

namespace CometCabsAdmin.Dal.Mapping
{
    public class UserMap:EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            HasKey(t => t.Id);
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity); 

            Property(t => t.Username).IsRequired().HasColumnType("nvarchar").HasMaxLength(20);
            Property(t => t.EmailAddress).IsRequired().HasColumnType("nvarchar").HasMaxLength(200);
            Property(t => t.Password).IsRequired().HasColumnType("nvarchar").HasMaxLength(100);
            Property(t => t.CreatedBy).IsRequired().HasColumnType("nvarchar").HasMaxLength(20);
            Property(t => t.CreateDate).IsRequired();
            Property(t => t.UpdatedBy).HasColumnType("nvarchar").HasMaxLength(20);
            Property(t => t.UpdateDate);
            Property(t => t.IPAddress).HasColumnType("nvarchar").HasMaxLength(15);

            ToTable("Users");
        }
    }
}
