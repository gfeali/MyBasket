using FluentValidation;
using MediatR;
using MyBasket.Application.Models;
using MyBasket.Infrastructure.Extensions;
using MyBasket.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MyBasket.Application.Commands
{
    [DataContract]
    public class UpdateBasketCommand : IRequest
    {
        #region Ctor

        public UpdateBasketCommand()
        {
            _items = new List<BasketItemDto>();
        }

        public UpdateBasketCommand(IEnumerable<BasketItem> items, string customerId) : this()
        {
            this._items = items.Select(x => x.Map<BasketItemDto>()).ToList();
            this.CustomerId = customerId;
        }

        #endregion Ctor

        #region Properties

        [DataMember]
        public string CustomerId { get; private set; }

        [DataMember]
        private readonly List<BasketItemDto> _items;

        public IReadOnlyCollection<BasketItemDto> Items => _items;

        #endregion Properties
    }

    public class UpdateBasketCommandValidator : AbstractValidator<UpdateBasketCommand>
    {
        #region Ctor

        public UpdateBasketCommandValidator()
        {
            RuleFor(x => x.CustomerId).MustNotNullOrEmpty().MustGuid();
            RuleFor(x => x.Items).Must(x => x.NotNull() && x.Count > default(int)).WithMessage("There are not products in basket").DependentRules(() =>
            {
                RuleForEach(x => x.Items).ChildRules(quotation =>
                {
                    quotation.RuleFor(q => q.ProductId).GreaterThanDefault();
                    quotation.RuleFor(q => q.ProductName).MustNotNullOrEmpty();
                    quotation.RuleFor(q => q.Quantity).GreaterThanDefault();
                    quotation.RuleFor(q => q.Price).MustPositive();
                    quotation.RuleFor(q => q.OldPrice).MustPositive();
                    quotation.RuleFor(q => q.PictureUrl).MustNotNullOrEmpty();
                });
            });
        }

        #endregion Ctor
    }
}

public sealed class BasketItemDto
{
    #region Properties

    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public decimal OldPrice { get; set; }
    public int Quantity { get; set; }
    public string PictureUrl { get; set; }

    #endregion Properties
}