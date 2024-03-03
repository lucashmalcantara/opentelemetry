##############################################################################
# v1
##############################################################################

# FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
# WORKDIR /app
# EXPOSE 80
# EXPOSE 443

# FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
# # >>>>> [CERTIFICADO] - OPCIONAL - Gera um certificado de desenvolvimento SSL, define o caminho para armazenar e a senha para usar.
# ARG DEV_CERTIFICATE_PASSWORD_ENV=my_dev_certificate_password
# RUN dotnet dev-certs https -ep /https/aspnetapp.pfx -p ${DEV_CERTIFICATE_PASSWORD_ENV}
# RUN dotnet dev-certs https --trust
# # <<<<<
# WORKDIR /src
# COPY ["OpenTelemetry.Api/OpenTelemetry.Api.csproj", "OpenTelemetry.Api/"]
# RUN dotnet restore "OpenTelemetry.Api/OpenTelemetry.Api.csproj"
# COPY . .
# WORKDIR "/src/OpenTelemetry.Api"
# RUN dotnet build "OpenTelemetry.Api.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "OpenTelemetry.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ## >>>>> [CERTIFICADO] - OPCIONAL -  No estagio final, copia o certificado para a pasta.
# COPY --from=build /https/* /https/
# ## <<<<<
# ENTRYPOINT ["dotnet", "OpenTelemetry.Api.dll"]

##############################################################################
# v2
##############################################################################

#FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
#WORKDIR /app
## Directory for OpenTelemetry AI binaries
#ENV OTEL_DOTNET_AUTO_HOME=/otel-ai
#EXPOSE 8080
#
#ENV ASPNETCORE_URLS=http://+:8080
#
#FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
#WORKDIR /src
#COPY ["OpenTelemetry.Api/OpenTelemetry.Api.csproj", "OpenTelemetry.Api/"]
#RUN dotnet restore "./OpenTelemetry.Api/OpenTelemetry.Api.csproj"
#COPY . .
#WORKDIR "/src/OpenTelemetry.Api"
#RUN dotnet build "./OpenTelemetry.Api.csproj" -c Release -o /app/build
#
#FROM build AS publish
#ENV OTEL_DOTNET_AUTO_HOME=/otel-ai
#RUN dotnet publish "./OpenTelemetry.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false
## Helper script to intall the dependencies and instrument the application
#RUN curl -sSfL https://raw.githubusercontent.com/open-telemetry/opentelemetry-dotnet-instrumentation/v0.5.0/otel-dotnet-auto-install.sh -O
#RUN sh otel-dotnet-auto-install.sh
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
## Add OTEL binaries to published app
#COPY --from=publish $OTEL_DOTNET_AUTO_HOME $OTEL_DOTNET_AUTO_HOME
#ENTRYPOINT ["dotnet", "OpenTelemetry.Api.dll"]


##############################################################################
# v2
##############################################################################

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
# Directory for OpenTelemetry AI binaries
ENV OTEL_DOTNET_AUTO_HOME="/otel-dotnet-auto"
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY ["OpenTelemetry.Api/OpenTelemetry.Api.csproj", "OpenTelemetry.Api/"]
RUN dotnet restore "./OpenTelemetry.Api/OpenTelemetry.Api.csproj"
COPY . .
WORKDIR "/src/OpenTelemetry.Api"
RUN dotnet build "./OpenTelemetry.Api.csproj" -c Release -o /app/build

FROM build AS publish
ENV OTEL_DOTNET_AUTO_HOME="/otel-dotnet-auto"
RUN dotnet publish "./OpenTelemetry.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false
# install OpenTelemetry .NET Automatic Instrumentation
ARG OTEL_VERSION=1.4.0
RUN curl -sSfL https://github.com/open-telemetry/opentelemetry-dotnet-instrumentation/releases/download/v${OTEL_VERSION}/otel-dotnet-auto-install.sh -O
RUN sh otel-dotnet-auto-install.sh

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Add OTEL binaries to published app
COPY --from=publish $OTEL_DOTNET_AUTO_HOME $OTEL_DOTNET_AUTO_HOME
ENTRYPOINT ["dotnet", "OpenTelemetry.Api.dll"]


##############################################################################
# teste
##############################################################################

# FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
# WORKDIR /app
# EXPOSE 80
# EXPOSE 443

# FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
# ARG BUILD_CONFIGURATION=Release
# WORKDIR /src
# COPY ["OpenTelemetry.Api/OpenTelemetry.Api.csproj", "OpenTelemetry.Api/"]
# RUN dotnet restore "./OpenTelemetry.Api/OpenTelemetry.Api.csproj"
# COPY . .
# WORKDIR "/src/OpenTelemetry.Api"
# RUN dotnet build "./OpenTelemetry.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# FROM build AS publish
# ARG BUILD_CONFIGURATION=Release
# RUN dotnet publish "./OpenTelemetry.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "OpenTelemetry.Api.dll"]