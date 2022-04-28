using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Validators
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(x => x.Cost).GreaterThan(0).WithMessage("Musi być > 0").LessThanOrEqualTo(1000).WithName("Koszt");
            RuleFor(x => x.Currency).IsEnumName(typeof(Currency)).WithMessage("Nie ma takiej waluty");
        }

    }
    public enum Currency
    {
        PLN,
        EUR,
        USD,
        GBP
    }
}
