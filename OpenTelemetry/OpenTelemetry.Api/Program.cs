
using OpenTelemetry.Api.HealthChecks;
using System.Diagnostics.Metrics;

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
                .AddCheck<SampleHealthCheck>(nameof(SampleHealthCheck));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            var meter = new Meter("OpenTelemetry.Api", "1.0.0");
            var incrementRequests = meter.CreateCounter<int>("srv.increment-request.count", "requests", "Number of increment operations");
            var getRequests = meter.CreateCounter<int>("srv.get-request.count", "requests", "Number of get operations");

            app.Run();
        }
    }
}