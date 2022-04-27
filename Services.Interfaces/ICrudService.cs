using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICrudService<T> where T : Entity
    {
        Task<IEnumerable<T>> GetAsync();
        Task<T> GetAsync(int id);
        Task DeleteAsync(int id);
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(int id, T entity);
    }
}
