# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiamos csproj y restauramos dependencias
COPY *.sln .
COPY AlmacenLP/*.csproj ./AlmacenLP/
COPY AlmacenLP.Infraestructura/*.csproj ./AlmacenLP.Infraestructura/
COPY AlmacenLP.Core/*.csproj ./AlmacenLP.Core/
RUN dotnet restore

# Copiamos todo y build
COPY . .
RUN dotnet publish -c Release -o out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "AlmacenLP.dll"]
