using MyBasket.Application.Models.Responses;
using MyBasket.Domain.BasketAggregate;
using MyBasket.Infrastructure.AutoMapper;

namespace MyBasket.Api.Infrastructure
{
    public class AutoMapperConfiguration : AutoMapperProfile
    {
        #region Ctor

        public AutoMapperConfiguration()
        {
            BasketItem_To_GetBasketViewModel();
            BasketItem_To_BasketItemDto();
            BasketItemDto_To_BasketItem();
            BasketItem_To_BasketItem();
        }

        #endregion Ctor

        #region Properties

        public override int Order => 0;

        #endregion Properties

        #region Methods

        public void BasketItem_To_GetBasketViewModel()
        {
            CreateMap<Application.Models.BasketItem, GetBasketViewModel>();
        }

        public void BasketItem_To_BasketItemDto()
        {
            CreateMap<Application.Models.BasketItem, BasketItemDto>();
        }

        public void BasketItemDto_To_BasketItem()
        {
            CreateMap<BasketItemDto, BasketItem>();
        }

        public void BasketItem_To_BasketItem()
        {
            CreateMap<BasketItem, MyBasket.Application.Models.BasketItem>();
        }

        #endregion Methods
    }
}