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
            _collection = faker.Generate(count).GroupBy(x => x.Id).Select(x => x.First()).ToList();
        }

        public Task<Order> CreateAsync(Order order)
        {
            order.Id = _collection.Max(x => x.Id) + 1;
            _collection.Add(order);
            return Task.FromResult(order);
        }

        public async Task DeleteAsync(int id)
        {
            _collection.Remove(await GetAsync(id));
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

        public async Task UpdateAsync(int id, Order order)
        {
            await DeleteAsync(id);
            order.Id = id;
            _collection.Add(order);
        }
    }
}
