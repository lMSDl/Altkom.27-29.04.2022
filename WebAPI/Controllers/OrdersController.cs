﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class OrdersController : ApiController<Order>
    {
        public OrdersController(IOrdersService service) : base(service)
        {
        }

        //[HttpGet("{id}/Products")]
        //public async Task<IActionResult> GetProducts(int id)
        //{
        //    var order = await Service.GetAsync(id);
        //    if (order == null)
        //        return NotFound();
        //    return Ok(order.Products);
        //}
    }
}
