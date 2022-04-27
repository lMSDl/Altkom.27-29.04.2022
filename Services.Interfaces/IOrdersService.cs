using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IOrdersService
    {
        Task<IEnumerable<Order>> GetAsync();
        Task<Order> GetAsync(int id);
    }
}
