using MyBasket.Infrastructure.AutoMapper;
using MyBasket.Shared;
using System;

namespace MyBasket.Infrastructure.Extensions
{
    public static class AutoMapperExtensions
    {
        #region Methods

        public static TDestination Map<TDestination>(this object source)
        {
            Check.That(source.IsNull(), () => throw new ArgumentNullException(nameof(source)));

            return AutoMapperConfiguration.Mapper.Map<TDestination>(source);
        }

        #endregion Methods
    }
}