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
    [ApiController]
    public abstract class ApiParentController<T, Tparent> : ControllerBase where T : Entity where Tparent : Entity
    {
        private ICrudService<Tparent> _parentService;
        private ICrdParentService<T> _service;
        public ApiParentController(ICrdParentService<T> service, ICrudService<Tparent> parentService)
        {
            _service = service;
            _parentService = parentService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int parentId)
        {
            if (await _parentService.GetAsync(parentId) == null)
                return NotFound();
            return Ok(await _service.GetCollectionAsync(parentId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int parentId, int id)
        {
            if (await _parentService.GetAsync(parentId) == null)
                return NotFound();
            var entity = await _service.GetAsync(parentId, id);
            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Post(int parentId, T entity)
        {
            if (await _parentService.GetAsync(parentId) == null)
                return NotFound();

            entity = await _service.CreateAsync(parentId, entity);

            return CreatedAtAction(nameof(Get), new { parentId = parentId, id = entity.Id }, entity);
        }

        [HttpDelete("{entityId}")]
        public async Task<IActionResult> Delete(int parentId, int entityId)
        {
            if (await _parentService.GetAsync(parentId) == null)
                return NotFound();
            if (await _service.GetAsync(parentId, entityId) == null)
                return NotFound();

            await _service.DeleteAsync(parentId, entityId);
            return NoContent();
        }

    }
}
