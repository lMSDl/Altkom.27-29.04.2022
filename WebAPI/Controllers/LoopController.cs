using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoopController : ControllerBase
    {

        public class Loop : Entity
        {
            public Loop InsideLoop { get; set; }
        }

        public IActionResult Get()
        {
            var loop1 = new Loop();
            var loop2 = new Loop();
            loop1.InsideLoop = loop2;
            loop2.InsideLoop = loop1;

            return Ok(loop1);
        }
    }
}
