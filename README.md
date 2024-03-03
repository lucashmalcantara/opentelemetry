# opentelemetry

Experimentações sobre o tema OpenTelemetry (OTEL).



# COMO EXECUTAR

1 - O primeiro passo é configurar o ambiente Docker para fazermos o *build *do `Dockerfile` e execução do `docker-compose`. Uma das opções é instalar e configurar o [Podman](https://podman.io/). Será necessário configurar o `docker-compose`, conforme documentação do Podman ([Working with Compose](https://podman-desktop.io/docs/compose)). Se estiver usando o Podman Desktop for Windows, basta clicar em `Compose` na parte inferior da tela para instalar.

2 - Abrir o terminal na pasta `OpenTelemetry/`

3 - Executar o comando para criar a imagem da API de exemplo .NET.

```shell
podman build -t opentelemetry-dotnet-api:latest -f OpenTelemetry.Api/Dockerfile --build-arg DEV_CERTIFICATE_PASSWORD_ENV=my_dev_certificate_password .
```

4 - Ainda na pasta `OpenTelemetry/`, executar o comando:

```shell
docker-compose up
```



## Outras considerações

### Testar Dockerfile da API .NET isoladamente

Caso queira testar a imagem da API de forma isolada, sem `docker-compose`, siga as instruções abaixo.

1 - Abrir o terminal na pasta `OpenTelemetry/`

2 - Executar o comando abaixo:

Sem HTTPS:

```shell
podman run -d -p 5152:80 -e ASPNETCORE_ENVIRONMENT=Development -e ASPNETCORE_URLS="http://+:80" --name opentelemetry-dotnet-api opentelemetry-dotnet-api:latest
```

Com HTTPS:

```shell
podman run -d -p 5152:80 -p 7232:443 -e ASPNETCORE_ENVIRONMENT=Development -e ASPNETCORE_URLS="https://+:443;http://+:80" -e ASPNETCORE_HTTPS_PORT=7232 -e ASPNETCORE_Kestrel__Certificates__Default__Password=my_dev_certificate_password -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx --name opentelemetry-dotnet-api opentelemetry-dotnet-api:latest
```



3 - Testar no navegador.

HTTP: http://localhost:5152/WeatherForecast

HTTPS: https://localhost:7232/WeatherForecast



# PROCESSO DE CONFIGURAÇÃO DA API .NET



## Configuração do Prometheus na API

No GitHub da biblioteca do Promtheus para .NET, encontra-se um excelente exemplo de implementação que foi utilizado como base na implementação deste projeto: [Sample.Web](https://github.com/prometheus-net/prometheus-net/blob/master/Sample.Web/Program.cs)

1 - Instalar os pacotes do Prometheus para exportar métricas automaticamente.

- `prometheus-net.AspNetCore`

- `prometheus-net.AspNetCore.HealthChecks` (opicional; para publicar os resultados de health check)

2 - Adicionar Health Check.

```csharp
            builder.Services.AddHealthChecks()
                // Define a sample health check that always signals healthy state.
                .AddCheck<SampleHealthCheck>(nameof(SampleHealthCheck))
                // Report health check results in the metrics output.
                .ForwardToPrometheus();
```

3 - Adicionar `UseHttpMetrics` e `MapMetrics`.

```csharp
// ...

            // Capture metrics about all received HTTP requests.
            app.UseHttpMetrics();

// ...
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

// ..
```




```
docker-compose up --build --detach
```


# REFERÊNCIAS

- https://www.twilio.com/en-us/blog/automatic-instrumentation-of-containerized-dotnet-applications-with-opentelemetry
- https://opentelemetry.io/docs/languages/net/automatic/
    - https://github.com/open-telemetry/opentelemetry-dotnet-instrumentation/blob/main/examples/demo/docker-compose.yaml



- https://stackoverflow.com/questions/69282468/using-dotnet-dev-certs-with-aspnet-docker-image

- https://stackoverflow.com/questions/62865631/docker-returning-response-only-for-http-port

- https://learn.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-3.1

- https://learn.microsoft.com/en-us/aspnet/core/security/docker-compose-https?view=aspnetcore-3.1

- [Coletar métricas - .NET | Microsoft Learn](https://learn.microsoft.com/pt-br/dotnet/core/diagnostics/metrics-collection)

- [GitHub - prometheus-net](https://github.com/prometheus-net/prometheus-net)

# RASCUNHO

```shel
podman build -f ./OpenTelemetry.Api/Dockerfile . -t opentelemetry-api:latest
```