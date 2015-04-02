using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CometCabsAdmin.Model.Entities;

namespace CometCabsAdmin.Dal.Mapping
{
    public class UserRolesMap : EntityTypeConfiguration<UserRoles>
    {
        public UserRolesMap()
        {
            HasKey(t => t.Id);
            // enable identity for one-to-many relationship
            // Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity); 

            Property(t => t.RoleName).IsRequired().HasMaxLength(20).HasColumnType("nvarchar");
            Property(t => t.CreatedBy).IsRequired().HasColumnType("nvarchar").HasMaxLength(20);
            Property(t => t.CreateDate).IsRequired();
            Property(t => t.UpdatedBy).HasColumnType("nvarchar").HasMaxLength(20);
            Property(t => t.UpdateDate);
            Property(t => t.IPAddress).HasColumnType("nvarchar").HasMaxLength(15);

            ToTable("UsersRoles");

            // one-to-many relationship with user table
            // HasRequired(t => t.User).WithMany(c => c.UserRoles).HasForeignKey(t => t.UserId).WillCascadeOnDelete(false);

            // one-to-one relationship with user table
            HasRequired(t => t.User).WithRequiredDependent(u => u.UserRole);
        }
    }
}
