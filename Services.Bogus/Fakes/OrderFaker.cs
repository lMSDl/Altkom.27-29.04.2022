using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Bogus.Fakes
{
    public class OrderFaker : EntityFaker<Order>
    {
        public OrderFaker(string locale = "pl") : base(locale)
        {
            RuleFor(x => x.DateTime, x => x.Date.Recent());
            RuleFor(x => x.Currency, x => "PLN");
            RuleFor(x => x.Cost, x => (decimal)x.Random.Double(100, 1000));
            RuleFor(x => x.Products, x => new ProductFaker().Generate(x.Random.Int(1, 15)));
        }
    }
}
