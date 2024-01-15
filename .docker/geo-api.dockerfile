# Base
FROM mcr-proxy.kontur.host/dotnet/aspnet:7.0-bullseye-slim AS base

# Build
FROM mcr-proxy.kontur.host/dotnet/sdk:7.0-bullseye-slim AS build
WORKDIR /app

## Install .NET Core global CLI diagnostic tools && dotTrace Command-Line Profiler tool
RUN dotnet tool install --tool-path /tools dotnet-trace  \
    && dotnet tool install --tool-path /tools dotnet-counters \
    && dotnet tool install --tool-path /tools dotnet-dump \
    && dotnet tool install --tool-path /tools dotnet-wtrace \
    && dotnet tool install --tool-path /tools JetBrains.dotTrace.GlobalTools

## Restore dependencies of .net core projects taking advantage of docker layer caching
COPY NuGet.config Directory.Build.targets *.sln ./*/*/*.csproj ./
RUN for file in $(ls ./*.csproj); do mkdir -p ${file%.*} && mv $file ${file%.*}; done
RUN dotnet restore Geo.Api/Geo.Api.csproj

## Copy everything else and build app
COPY . .
RUN dotnet publish Geo.Api/Geo.Api.csproj -c Release -o /app/publish

# Finalize
FROM base AS final

WORKDIR /tools
COPY --from=build /tools .

WORKDIR /app
COPY --from=build /app/publish .

## Setup the runtime environment
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_ENVIRONMENT=Production
ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false

## Run as a non-root user
RUN mkdir -p /home/ygolonac \
    && groupadd -r ygolonac \
    && useradd -r -g ygolonac -d /home/ygolonac -s /sbin/nologin ygolonac -u 1000 \
    && chown -R ygolonac:ygolonac /home/ygolonac \
    && chown -R ygolonac:ygolonac /app \
    && chown -R ygolonac:ygolonac /tools

USER ygolonac
ENTRYPOINT ["dotnet", "Kontur.IDevOps.Bokrug.Api.dll"]

