using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name).Length(5, 10).When(x => x.Name.Contains("ą"));
            RuleFor(x => x.Name).Must((obj, value) => value.Contains("ę") ? value.Length > 5 : value.Length <= 5);
        }

    }
}
