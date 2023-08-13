using Infrastructure.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.DTO_s;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce_App.Areas.Identity.Data;

public class Ecommerce_AppContext : IdentityDbContext<Ecommerce_AppUser>
{
    public Ecommerce_AppContext(DbContextOptions<Ecommerce_AppContext> options)
        : base(options)
    {
    }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
    }

    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<Ecommerce_AppUser>
    {
        public void Configure(EntityTypeBuilder<Ecommerce_AppUser> builder)
        {
            builder.Property(u => u.FirstName).HasMaxLength(255);
            builder.Property(u => u.LastName).HasMaxLength(255);
            builder.Property(u => u.Address).HasMaxLength(255);
            builder.Property(u => u.Image).HasMaxLength(255);
        }
    }

    public DbSet<Domain.DTO_s.RoomImage> RoomImage { get; set; } = default!;

}
