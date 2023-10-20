##################################### V1 #####################################

##See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
#FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
#WORKDIR /src
#COPY ["OpenTelemetry.Api/OpenTelemetry.Api.csproj", "OpenTelemetry.Api/"]
#RUN dotnet restore "OpenTelemetry.Api/OpenTelemetry.Api.csproj"
#COPY . .
#WORKDIR "/src/OpenTelemetry.Api"
#RUN dotnet build "OpenTelemetry.Api.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "OpenTelemetry.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "OpenTelemetry.Api.dll"]

##################################### V2 #####################################

##See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
# >>>>> [CERTIFICADO] - OPCIONAL - Gera um certificado de desenvolvimento SSL, define o caminho para armazenar e a senha para usar.
ARG DEV_CERTIFICATE_PASSWORD_ENV=my_dev_certificate_password
RUN dotnet dev-certs https -ep /https/aspnetapp.pfx -p ${DEV_CERTIFICATE_PASSWORD_ENV}
RUN dotnet dev-certs https --trust
# <<<<<
WORKDIR /src
COPY ["OpenTelemetry.Api/OpenTelemetry.Api.csproj", "OpenTelemetry.Api/"]
RUN dotnet restore "OpenTelemetry.Api/OpenTelemetry.Api.csproj"
COPY . .
WORKDIR "/src/OpenTelemetry.Api"
RUN dotnet build "OpenTelemetry.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenTelemetry.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
## >>>>> [CERTIFICADO] - OPCIONAL -  No estagio final, copia o certificado para a pasta.
COPY --from=build /https/* /https/
## <<<<<
ENTRYPOINT ["dotnet", "OpenTelemetry.Api.dll"]