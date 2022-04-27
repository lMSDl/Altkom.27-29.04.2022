using Models;
using Services.Bogus.Fakes;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Bogus
{
    public class OrdersService : IOrdersService
    {
        private ICollection<Order> _collection;

        public OrdersService(OrderFaker faker, int count = 1000)
        {
            _collection = faker.Generate(count);
        }

        public Task<IEnumerable<Order>> GetAsync()
        {
            IEnumerable<Order> result = _collection.ToList();
            return Task.FromResult(result);
        }

        public Task<Order> GetAsync(int id)
        {
            return Task.FromResult(_collection.SingleOrDefault(x => x.Id == id));
        }
    }
}
