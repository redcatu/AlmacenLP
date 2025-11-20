# Imagen base para ejecutar la app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Imagen para compilar la app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia todo el proyecto al contenedor
COPY . .

# Restaura dependencias
RUN dotnet restore "AlmacenLP.csproj"

# Compila la app
RUN dotnet build "AlmacenLP.csproj" -c Release -o /app/build

# Publica la app
FROM build AS publish
RUN dotnet publish "AlmacenLP.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagen final (producción)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AlmacenLP.dll"]
