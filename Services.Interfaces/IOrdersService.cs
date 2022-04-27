using Models;
using System;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IOrdersService
    {
        IEnumerable<Order> Get();
        Order Get(int id);
    }
}
