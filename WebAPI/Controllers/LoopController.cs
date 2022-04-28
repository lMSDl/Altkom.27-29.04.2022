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
            public string Name { get; set; }
            public Loop InsideLoop { get; set; }
        }

        // [route]?loop1Name=<value>&loop2Name=<value>
        public IActionResult Get([FromQuery]string loop1Name, [FromQuery] int loop2Name = 2)
        {
            var loop1 = new Loop() { Name = loop1Name };
            var loop2 = new Loop() { Name = loop2Name.ToString() };
            loop1.InsideLoop = loop2;
            loop2.InsideLoop = loop1;

            return Ok(loop1);
        }
    }
}
