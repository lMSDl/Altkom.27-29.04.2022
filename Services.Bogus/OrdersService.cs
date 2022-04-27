using Models;
using Services.Bogus.Fakes;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Bogus
{
    public class OrdersService : CrudService<Order>, IOrdersService
    {
        public OrdersService(OrderFaker faker, int count = 1000) : base(faker, count)
        {
        }
    }
}
