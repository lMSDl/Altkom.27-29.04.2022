using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Bogus.Fakes
{
    public abstract class EntityFaker<T> : Faker<T> where T : Models.Entity
    {
        protected EntityFaker(string locale = "pl") : base(locale)
        {
            StrictMode(true);
            RuleFor(x => x.Id, x => x.UniqueIndex);
        }
    }
}
