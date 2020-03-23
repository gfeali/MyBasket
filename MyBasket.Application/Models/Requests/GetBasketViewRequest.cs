using FluentValidation;

namespace MyBasket.Application.Models.Requests
{
    public class GetBasketViewRequest
    {
        #region Properties

        public string CustomerId { get; set; }

        #endregion Properties
    }

    public class GetBasketViewRequestValidator : AbstractValidator<GetBasketViewRequest>
    {
        #region Ctor

        public GetBasketViewRequestValidator()
        {
            RuleFor(x => x.CustomerId).NotNull().NotEmpty();
        }

        #endregion Ctor
    }
}