using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.FIlters
{
    public class LimiterFilter : IAsyncActionFilter
    {
        private int _limitPerMinute;
        private int _counter;

        public LimiterFilter(int limitPerMinute)
        {
            _limitPerMinute = limitPerMinute;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            if (_counter >= _limitPerMinute)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                return;
            }

            Interlocked.Increment(ref _counter);

            await next();

            _ = Task.Delay(60000).ContinueWith(x => Interlocked.Decrement(ref _counter));
        }
    }
}
