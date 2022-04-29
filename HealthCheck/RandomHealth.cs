using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheck
{
    public class RandomHealth : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var value = new Random().Next();

            if(value % 2 == 0)
            {
                return Task.FromResult(HealthCheckResult.Healthy());
            }
            if(value % 3 == 0)
            {
                return Task.FromResult(HealthCheckResult.Degraded("I need help..."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy());


        }
    }
}
