using Models;
using Services.Bogus.Fakes;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Bogus
{
    public class OrdersService : IOrdersService
    {
        private ICollection<Order> _collection;

        public OrdersService(OrderFaker faker, int count)
        {
            _collection = faker.Generate(count);
        }

        public IEnumerable<Order> Get()
        {
            return _collection.ToList();
        }

        public Order Get(int id)
        {
            return _collection.SingleOrDefault(x => x.Id == id);
        }
    }
}
