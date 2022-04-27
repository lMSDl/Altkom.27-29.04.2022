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
        //public Task<IEnumerable<Order>> Get()
        public async Task<IActionResult> Get()
        {
            var entities = await _service.GetAsync();
            return Ok(entities); //zwracamy odpowiedź z ciałem (body) i kodem 200
        }

        //api/Orders/{id}
        //{} - w nawiasach klarmowych podajemy nazwę parametru - nawy parametrów fukcji dopasowane do nazw w nawiasach
        // : - po dwukropku możemy określić typ parametru dla routingu
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await _service.GetAsync(id);
            if (entity == null)
                return NotFound(); //zasób nieznaleziony
            
            return Ok(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Order order)
        {
            order = await _service.CreateAsync(order);

            return CreatedAtAction(nameof(Get), new { id = order.Id }, order); //zwraca wynik funkcji oraz dodaje do nagłowka odpowiedzi klucz Location z adresem spod którego można pobrać zasób
        }

        [HttpPut("{entityId}")]
        public async Task<IActionResult> Put(int entityId, Order order)
        {
            if (await _service.GetAsync(entityId) == null)
                return NotFound();

            await _service.UpdateAsync(entityId, order);
            return NoContent(); //odpowiedź 204 - przetwarzanie zakończone poprawnie, ale odpowiedź nie zawira ciała
        }

        [HttpDelete("{entityId}")]
        public async Task<IActionResult> Delete(int entityId)
        {
            if (await _service.GetAsync(entityId) == null)
                return NotFound();

            await _service.DeleteAsync(entityId);
            return NoContent();
        }


    }
}
