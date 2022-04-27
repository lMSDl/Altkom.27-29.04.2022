using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    //Route - adnotacja pozwalająca na określenie adresu zasobów
    // [] - w nawiasach kwadratowych wstawiona zostanie nazwa kontrolera będąca nazwą zasobów
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IOrdersService _service;
        public OrdersController(IOrdersService service)
        {
            _service = service;
        }

        [HttpGet]
        public Task<IEnumerable<Order>> Get()
        {
            return _service.GetAsync();
        }

        //api/Orders/{id}
        //{} - w nawiasach klarmowych podajemy nazwę parametru - nawy parametrów fukcji dopasowane do nazw w nawiasach
        // : - po dwukropku możemy określić typ parametru dla routingu
        [HttpGet("{id:int}")]
        public Task<Order> Get(int id)
        {
            return _service.GetAsync(id);
        }
        
    }
}
