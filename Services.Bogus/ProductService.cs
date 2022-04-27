using Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Bogus
{
    public class ProductService : ICrdParentService<Product>
    {
        private ICrudService<Order> _parentService;

        public ProductService(ICrudService<Order> parentService)
        {
            _parentService = parentService;
        }

        public async Task<Product> CreateAsync(int parentId, Product entity)
        {
            var order = await _parentService.GetAsync(parentId);
            entity.Id = (await _parentService.GetAsync()).SelectMany(x => x.Products).Max(x => x.Id) + 1;
            order.Products.Add(entity);
            return entity;
        }

        public async Task DeleteAsync(int parentId, int id)
        {
            var order = await _parentService.GetAsync(parentId);
            order.Products.Remove(await GetAsync(parentId, id));
        }

        public async Task<Product> GetAsync(int parentId, int id)
        {
            return (await GetCollectionAsync(parentId)).SingleOrDefault(x => x.Id == id);
        }

        public async Task<IEnumerable<Product>> GetCollectionAsync(int parentId)
        {
            return (await _parentService.GetAsync(parentId)).Products;
        }
    }
}
