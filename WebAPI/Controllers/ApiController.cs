using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.FIlters;

namespace WebAPI.Controllers
{
    //Route - adnotacja pozwalająca na określenie adresu zasobów
    // [] - w nawiasach kwadratowych wstawiona zostanie nazwa kontrolera będąca nazwą zasobów
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ConsoleLogFilter))]
    [Authorize]
    public abstract class ApiController<T> : ControllerBase where T : Entity
    {
        protected ICrudService<T> Service { get; }
        public ApiController(ICrudService<T> service)
        {
            Service = service;
        }

        [HttpGet]
        [ServiceFilter(typeof(LimiterFilter))]
        //public Task<IEnumerable<T>> Get()
        [Authorize(Roles = nameof(Roles.Read))]
        public virtual async Task<IActionResult> Get()
        {
            var entities = await Service.GetAsync();
            return Ok(entities); //zwracamy odpowiedź z ciałem (body) i kodem 200
        }

        //api/Orders/{id}
        //{} - w nawiasach klarmowych podajemy nazwę parametru - nawy parametrów fukcji dopasowane do nazw w nawiasach
        // : - po dwukropku możemy określić typ parametru dla routingu
        [HttpGet("{id:int}")]
        [Authorize(Roles = nameof(Roles.Read))]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await Service.GetAsync(id);
            if (entity == null)
                return NotFound(); //zasób nieznaleziony

            return Ok(entity);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Roles.Write))]
        public async Task<IActionResult> Post(T entity)
        {
            if(entity.Id != 0)
            {
                //ręczne dodawanie błędów odnośnie modelu
                ModelState.AddModelError(nameof(Entity.Id), "Wartość musi być 0");
            }


            //przykład "ręcznego" sprawdzenia poprawności przesłanego modelu i odpowiedź informująca klienta o jego błędach
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            entity = await Service.CreateAsync(entity);

            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity); //zwraca wynik funkcji oraz dodaje do nagłowka odpowiedzi klucz Location z adresem spod którego można pobrać zasób
        }

        [HttpPut("{entityId}")]
        [Authorize(Roles = nameof(Roles.Update))]
        public virtual async Task<IActionResult> Put(int entityId, T entity)
        {
            if (await Service.GetAsync(entityId) == null)
                return NotFound();

            await Service.UpdateAsync(entityId, entity);
            return NoContent(); //odpowiedź 204 - przetwarzanie zakończone poprawnie, ale odpowiedź nie zawira ciała
        }

        [HttpDelete("{entityId}")]
        [Authorize(Roles = nameof(Roles.Delete))]
        public async Task<IActionResult> Delete(int entityId)
        {
            if (await Service.GetAsync(entityId) == null)
                return NotFound();

            await Service.DeleteAsync(entityId);
            return NoContent();
        }

    }
}
