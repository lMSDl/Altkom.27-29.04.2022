using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    //[NonController] //wyłączenie klasy jako kontroler (rejestracja w serwisach i endpoint)
    public class UsersController : ApiController<User>
    {
        private AuthService _authService;
        public UsersController(ICrudService<User> service, AuthService authService) : base(service)
        {
            _authService = authService;
        }

        [ApiExplorerSettings(IgnoreApi = true)] //ukrycie akcji w api
        [NonAction] //wyłączenie metody z obsługi
        public override Task<IActionResult> Put(int entityId, User entity)
        {
            return base.Put(entityId, entity);
        }

        [AllowAnonymous]
        public override Task<IActionResult> Get()
        {
            return base.Get();
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(User user)
        {
            var token = await _authService.AuthenticateAsync(user.Username, user.Password);
            if (token == null)
                return BadRequest();

            return Ok(token);
        }
    }
}
