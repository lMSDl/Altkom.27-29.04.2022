using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICrdParentService<T> where T : Entity
    {
        Task<T> GetAsync(int parentId, int id);
        Task<IEnumerable<T>> GetCollectionAsync(int parentId);
        Task DeleteAsync(int parentId, int id);
        Task<T> CreateAsync(int parentId, T entity);
    }
}
