namespace Pharaonia.Domain.Interfaces.IGenericRepository___IUOW
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<Destination> Destinations { get; }
        public IGenericRepository<Offer> Offers {  get; }
        public IGenericRepository<DestinationImages> DestinationImages {  get; }
        public IGenericRepository<OfferImages> OfferImages {  get; }
        public IGenericRepository<Gallery> Gallery { get; }
        public IGenericRepository<AboutUs> AboutUs { get; }
        Task<bool> SaveChangesAsync();
        bool SaveChanges();

    }
}
