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
    public abstract class ApiController<T> : ControllerBase where T : Entity
    {
        private ICrudService<T> _service;
        public ApiController(ICrudService<T> service)
        {
            _service = service;
        }

        [HttpGet]
        //public Task<IEnumerable<T>> Get()
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
        public async Task<IActionResult> Post(T entity)
        {
            entity = await _service.CreateAsync(entity);

            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity); //zwraca wynik funkcji oraz dodaje do nagłowka odpowiedzi klucz Location z adresem spod którego można pobrać zasób
        }

        [HttpPut("{entityId}")]
        public virtual async Task<IActionResult> Put(int entityId, T entity)
        {
            if (await _service.GetAsync(entityId) == null)
                return NotFound();

            await _service.UpdateAsync(entityId, entity);
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
