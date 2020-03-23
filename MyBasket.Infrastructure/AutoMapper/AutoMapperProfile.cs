using AutoMapper;

namespace MyBasket.Infrastructure.AutoMapper
{
    public abstract class AutoMapperProfile : Profile
    {
        #region Properties

        public abstract int Order { get; }

        #endregion Properties
    }
}