using Models;
using Services.Bogus.Fakes;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Bogus
{
    public class CrudService<T> : ICrudService<T> where T : Entity
    {
        protected ICollection<T> Collection { get; }

        public CrudService(EntityFaker<T> faker, int count = 1000)
        {
            Collection = faker.Generate(count).GroupBy(x => x.Id).Select(x => x.First()).ToList();
        }

        public Task<T> CreateAsync(T entity)
        {
            entity.Id = Collection.Max(x => x.Id) + 1;
            Collection.Add(entity);
            return Task.FromResult(entity);
        }

        public async Task DeleteAsync(int id)
        {
            Collection.Remove(await GetAsync(id));
        }

        public Task<IEnumerable<T>> GetAsync()
        {
            IEnumerable<T> result = Collection.ToList();
            return Task.FromResult(result);
        }

        public Task<T> GetAsync(int id)
        {
            return Task.FromResult(Collection.SingleOrDefault(x => x.Id == id));
        }

        public async Task UpdateAsync(int id, T entity)
        {
            await DeleteAsync(id);
            entity.Id = id;
            Collection.Add(entity);
        }
    }
}
