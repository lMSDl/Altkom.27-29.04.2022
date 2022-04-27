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
    [Route("api/Orders/{parentId}/[controller]")]
    public class ProductsController : ApiParentController<Product, Order>
    {
        public ProductsController(ICrdParentService<Product> service, IOrdersService parentService) : base(service, parentService)
        {
        }
    }
}
