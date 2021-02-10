using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcService1
{
    public class LoadHandler
    {
        private readonly RequestDelegate _next;
        private readonly int maxCount;
        private readonly ILogger<LoadHandler> logger;
        private int count = 0;
        public LoadHandler(RequestDelegate next, int maxCount, ILogger<LoadHandler> logger)
        {
            this._next = next;
            this.maxCount = maxCount;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            logger.LogInformation($"total count = {count}");

            Interlocked.Increment(ref count);
            if (count < maxCount)
            {
                logger.LogInformation($"incremented count = {count}");
                await _next(context);
            }
            else
            {
                await Task.Run(() => context.Response.StatusCode = 503);
            }
            Interlocked.Decrement(ref count);
            logger.LogInformation($"decremented count = {count}");
        }
    }
}
