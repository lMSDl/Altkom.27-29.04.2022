using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Bogus.Fakes
{
    public class ProductFaker : EntityFaker<Product>
    {
        public ProductFaker(string locale = "pl") : base(locale)
        {
            RuleFor(x => x.Name, x => x.Commerce.Product());
        }
    }
}
