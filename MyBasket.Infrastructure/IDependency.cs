using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyBasket.Infrastructure
{
    public interface IDependency
    {
        #region Properties

        int Order { get; }

        #endregion Properties

        #region Methods

        void Register(IServiceCollection services, IConfiguration configuration);

        #endregion Methods
    }
}