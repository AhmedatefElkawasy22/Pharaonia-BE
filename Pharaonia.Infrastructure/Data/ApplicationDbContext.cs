
namespace Pharaonia.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Destination> destinations { get; set; }
        public DbSet<Offer> offers { get; set; }
        public DbSet<DestinationImages> destinationIamges { get; set; }
        public DbSet<ContactUS> contactUS { get; set; }
        public DbSet<AboutUs> aboutUs { get; set; }
        public DbSet<GetOffer> getOffer { get; set; }
        public DbSet<Gallery> gallery { get; set; }
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

            base.OnModelCreating(builder);
        }
    }
}
