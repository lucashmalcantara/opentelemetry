﻿using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace OpenTelemetry.Api.HealthChecks
{
    public class SampleHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            // All is well!
            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}