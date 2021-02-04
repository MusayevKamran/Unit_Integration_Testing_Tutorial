using APP.Repositorys;
using APP.Repositorys.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace APP.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IBlogRepository, BlogRepository>();       

            return services;
        }
    }
}
