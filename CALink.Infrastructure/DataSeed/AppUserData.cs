using CALink.Domain.Entities.Super_Admin;
using Microsoft.EntityFrameworkCore;
using CALink.Infrastructure.DataSeederConstant;


namespace CALink.Infrastructure.DataSeed
{
    public class AppUserData : IEntityTypeConfiguration<AppUser>
    {
       

        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<AppUser> builder)
        {

            string generatedPassword = "Admin@123";

            builder.HasData(new AppUser
            {
                Id = GuidConstant.AppUserGuid,
                FirstName = "Kanha",
                LastName = "Yadav",
                Email = "kanha@gmail.com",
                Password = "Admin@123",
                IsActive = true,
                CreatedBy = GuidConstant.AppUserGuid,   // static value
                UpdatedBy = GuidConstant.AppUserGuid,   // static value
                CreatedAt = new DateTime(2025, 09, 09), // static value
                UpdatedAt = new DateTime(2025, 09, 09), // static value
                LastLogin = null,
                ProfilePictureUrl = null
            });

        }
    }
}
