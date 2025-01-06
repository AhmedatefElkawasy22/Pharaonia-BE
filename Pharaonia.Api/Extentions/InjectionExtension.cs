namespace Pharaonia.API.Extentions
{
    public static class InjectionExtension
    {
        public static void AddDependencyInjectionServices(this IServiceCollection services)
        {

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUrlHelperService, UrlHelperService>();
            services.AddScoped<IDestinationService, DestinationService>();
            services.AddScoped<IOfferService, OfferService>();
            services.AddScoped<IGalleryService, GalleryService>();
            services.AddScoped<IAboutUsService, AboutUsService>();
            services.AddHttpContextAccessor();
        }
    }
}
