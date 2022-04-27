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
    //[NonController] //wyłączenie klasy jako kontroler (rejestracja w serwisach i endpoint)
    public class UsersController : ApiController<User>
    {
        public UsersController(ICrudService<User> service) : base(service)
        {
        }

        [ApiExplorerSettings(IgnoreApi = true)] //ukrycie akcji w api
        [NonAction] //wyłączenie metody z obsługi
        public override Task<IActionResult> Put(int entityId, User entity)
        {
            return base.Put(entityId, entity);
        }
    }
}
