# Fase base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Fase de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# CORRECCIÓN AQUÍ:
# Copiamos el csproj a la raíz de /src
COPY ["AlmacenLP.csproj", "./"]

# Restauramos apuntando al archivo en la raíz, NO a una subcarpeta
RUN dotnet restore "./AlmacenLP.csproj"

# Copiamos todo el resto del código
COPY . .

# Construimos y Publicamos en un solo paso optimizado
RUN dotnet publish "./AlmacenLP.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Fase final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AlmacenLP.dll"]