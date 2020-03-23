using AutoMapper;

namespace MyBasket.Infrastructure.AutoMapper
{
    internal static class AutoMapperConfiguration
    {
        #region Properties

        public static IMapper Mapper { get; private set; }

        public static MapperConfiguration MapperConfiguration { get; private set; }

        #endregion Properties

        #region Methods

        public static void Init(MapperConfiguration config)
        {
            MapperConfiguration = config;
            Mapper = config.CreateMapper();
        }

        #endregion Methods
    }
}