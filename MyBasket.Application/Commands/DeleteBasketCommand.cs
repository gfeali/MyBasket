using FluentValidation;
using MediatR;
using MyBasket.Infrastructure.Extensions;
using System.Runtime.Serialization;

namespace MyBasket.Application.Commands
{
    [DataContract]
    public class DeleteBasketCommand : IRequest
    {
        #region Ctor

        public DeleteBasketCommand(string customerId)
        {
            this.CustomerId = customerId;
        }

        #endregion Ctor

        #region Properties

        [DataMember]
        public string CustomerId { get; private set; }

        #endregion Properties
    }

    public class DeleteBasketCommandCommandValidator : AbstractValidator<DeleteBasketCommand>
    {
        #region Ctor

        public DeleteBasketCommandCommandValidator()
        {
            RuleFor(x => x.CustomerId).MustNotNullOrEmpty().MustGuid();
        }

        #endregion Ctor
    }
}