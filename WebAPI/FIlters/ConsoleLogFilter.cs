using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.FIlters
{
    public class ConsoleLogFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("End of " + context.HttpContext.Request.Method);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("Begin of " + context.HttpContext.Request.Method);
        }
    }
}
