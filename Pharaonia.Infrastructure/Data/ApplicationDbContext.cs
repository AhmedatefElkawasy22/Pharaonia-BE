
namespace Pharaonia.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<DestinationImages> DestinationIamges { get; set; }
        public DbSet<ContactUS> ContactUS { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }
        public DbSet<BookOffer> BookOffer { get; set; }
        public DbSet<Gallery> Gallery { get; set; }
        public DbSet<OfferImages> OfferImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "1",
                Name = "Admin",
                NormalizedName = "ADMIN"
            });

            builder.Entity<AboutUs>().HasKey(x => x.Id);
            builder.Entity<User>().Property(x => x.OTPResetPassword).HasDefaultValue(null);
            builder.Entity<User>().Property(x => x.OTPResetPasswordExpiration).HasDefaultValue(null);

            base.OnModelCreating(builder);
        }
    }
}
