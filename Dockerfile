# Usa la imagen base de .NET SDK para compilar y ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copia el archivo .csproj y restaura las dependencias
COPY *.csproj .
RUN dotnet restore

# Copia el resto de los archivos del proyecto
COPY . .

# Instala dotnet-watch
RUN dotnet tool install --global dotnet-watch
ENV PATH="$PATH:/root/.dotnet/tools"

# Expone los puertos que usa la aplicación
EXPOSE 80
EXPOSE 443

# Define el comando para ejecutar la aplicación con dotnet watch
ENTRYPOINT ["dotnet", "watch", "run"]