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



# REFERÊNCIAS



- https://stackoverflow.com/questions/69282468/using-dotnet-dev-certs-with-aspnet-docker-image

- https://stackoverflow.com/questions/62865631/docker-returning-response-only-for-http-port

- https://learn.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-3.1

- https://learn.microsoft.com/en-us/aspnet/core/security/docker-compose-https?view=aspnetcore-3.1
