
namespace Pharaonia.Infrastructure.GenericRepository___UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public IGenericRepository<Destination> Destinations { get; set; }
        public IGenericRepository<Offer> Offers { get; set; }
        public IGenericRepository<DestinationImages> DestinationImages { get; }
        public IGenericRepository<OfferImages> OfferImages { get; }
        public IGenericRepository<Gallery> Gallery { get; }
        public IGenericRepository<AboutUs> AboutUs { get; }



        public UnitOfWork(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;

            Destinations = new GenericRepository<Destination>(_context);
            Offers = new GenericRepository<Offer>(_context);
            DestinationImages = new GenericRepository<DestinationImages>(_context);
            OfferImages = new GenericRepository<OfferImages>(_context);
            Gallery = new GenericRepository<Gallery>(_context);
            AboutUs = new GenericRepository<AboutUs>(_context);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0 ;
        }
        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
