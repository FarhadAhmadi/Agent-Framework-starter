# syntax=docker/dockerfile:1.7

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy only the files needed for restore first to keep layer caching effective.
COPY global.json Directory.Build.props Directory.Packages.props WebAPI.slnx ./
COPY src/WebAPI.ServiceDefaults/WebAPI.ServiceDefaults.csproj src/WebAPI.ServiceDefaults/
COPY src/WebAPI.Web/WebAPI.Web.csproj src/WebAPI.Web/

RUN dotnet restore src/WebAPI.Web/WebAPI.Web.csproj

COPY src/ src/

RUN dotnet publish src/WebAPI.Web/WebAPI.Web.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Development \
    DOTNET_EnableDiagnostics=0

EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "WebAPI.Web.dll"]
