using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Hubs;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    //[NonController] //wyłączenie klasy jako kontroler (rejestracja w serwisach i endpoint)
    public class UsersController : ApiController<User>
    {
        private AuthService _authService;
        private IHubContext<UsersHub> _usersHub;

        public UsersController(ICrudService<User> service, AuthService authService, IHubContext<UsersHub> usersHub) : base(service)
        {
            _authService = authService;
            _usersHub = usersHub;
        }

        [ApiExplorerSettings(IgnoreApi = true)] //ukrycie akcji w api
        [NonAction] //wyłączenie metody z obsługi
        public override Task<IActionResult> Put(int entityId, User entity)
        {
            return base.Put(entityId, entity);
        }

        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public override Task<IActionResult> Get()
        {
            return base.Get();
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(User user)
        {
            var token = await _authService.AuthenticateAsync(user.Username, user.Password);
            if (token == null)
            {
                await _usersHub.Clients.Groups("LoginListener").SendAsync("LoginFailed", user.Username);
                return BadRequest();
            }
            await _usersHub.Clients.Groups("LoginListener").SendAsync("LoginSuceed", user.Username);

            return Ok(token);
        }
    }
}
