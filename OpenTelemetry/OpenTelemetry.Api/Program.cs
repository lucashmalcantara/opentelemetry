
using OpenTelemetry.Api.HealthChecks;
using Prometheus;

namespace OpenTelemetry.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHealthChecks()
                // Define a sample health check that always signals healthy state.
                .AddCheck<SampleHealthCheck>(nameof(SampleHealthCheck))
                // Report health check results in the metrics output.
                .ForwardToPrometheus();

            var app = builder.Build();

            // Capture metrics about all received HTTP requests.
            app.UseHttpMetrics();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            // Enable the /metrics page to export Prometheus metrics.
            // Open http://localhost:port/metrics to see the metrics.
            //
            // Metrics published in this sample:
            // * built-in process metrics giving basic information about the .NET runtime (enabled by default)
            // * metrics from .NET Event Counters (enabled by default, updated every 10 seconds)
            // * metrics from .NET Meters (enabled by default)
            // * metrics about requests made by registered HTTP clients
            // * metrics about requests handled by the web app
            // * ASP.NET health check statuses
            // * custom business logic metrics published
            app.MapMetrics();

            app.Run();
        }
    }
}