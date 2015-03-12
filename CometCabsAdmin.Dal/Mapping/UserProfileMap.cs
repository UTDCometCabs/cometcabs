using CometCabsAdmin.Model.Entities;
using System.Data.Entity.ModelConfiguration;

namespace CometCabsAdmin.Dal.Mapping
{
    public class UserProfileMap : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileMap()
        {
            HasKey(t => t.Id);

            Property(t => t.FirstName).IsRequired().HasMaxLength(20).HasColumnType("nvarchar");
            Property(t => t.LastName).IsRequired().HasMaxLength(20).HasColumnType("nvarchar");
            Property(t => t.Address).HasColumnType("nvarchar").HasMaxLength(150);
            Property(t => t.CreatedBy).IsRequired().HasColumnType("nvarchar").HasMaxLength(20); ;
            Property(t => t.CreateDate).IsRequired();
            Property(t => t.UpdatedBy).HasColumnType("nvarchar").HasMaxLength(20); ;
            Property(t => t.UpdateDate);
            Property(t => t.IPAddress).HasColumnType("nvarchar").HasMaxLength(15); ;
            ToTable("UsersProfiles");
            HasRequired(t => t.User).WithRequiredDependent(u => u.UserProfile);
        }
    }
}
