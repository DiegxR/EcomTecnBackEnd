# Etapa 1: Construcción
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copiar archivos del proyecto y restaurar dependencias
COPY *.csproj .
RUN dotnet restore

# Copiar el resto del código fuente y compilar
COPY . .
RUN dotnet publish -c Release -o out

# Etapa 2: Producción
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Copiar los archivos compilados desde la etapa de construcción
COPY --from=build /app/out .

# Exponer el puerto que usará la aplicación (Render asigna automáticamente uno)
EXPOSE 5432

# Comando para ejecutar la aplicación
ENTRYPOINT ["dotnet", "backEnd.dll"]
