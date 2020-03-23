using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyBasket.Application.Queries;
using MyBasket.Domain.BasketAggregate;
using MyBasket.Infrastructure;
using MyBasket.Infrastructure.Redis;

namespace MyBasket.Api.Infrastructure
{
    public class DependencyRegistrar : IDependency
    {
        #region Properties

        public int Order => 0;

        #endregion Properties

        #region Methods

        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRedisManager, RedisManager>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IBasketQueries, BasketQueries>();
        }

        #endregion Methods
    }
}