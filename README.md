# ApiAcmePedidos

ApiAcmePedidos es una API desarrollada en ASP.NET Core 9.0 que permite enviar pedidos en formato JSON, transformarlos a XML y recibir respuestas en formato JSON desde un sistema de envíos de pedidos.

## Enunciado del Proyecto

### PRUEBA TÉCNICA .NET

La compañía ACME requiere implementar un servicio a través de una API REST para el ciclo de abastecimiento. La tienda Carrera 70 debe enviar la información de los pedidos a través de esta API con mensajería JSON, para que el servicio le retorne información del sistema de envío de pedidos.

**Requisitos:**

A. Se debe exponer una API REST con mensajería JSON.

B. Realizar transformación de la petición de JSON a XML, siguiendo esta tabla de mapeo:

| REST (JSON)       | SOAP (XML)  | Datos de Prueba       |
|-------------------|------------|-----------------------|
| numPedido        | pedido     | 75630275             |
| cantidadPedido   | Cantidad   | 1                     |
| codigoEAN        | EAN        | 00110000765191002104587 |
| nombreProducto   | Producto   | Armario INVAL         |
| numDocumento     | Cedula     | 1113987400            |
| direccion        | Direccion  | CR 72B 45 12 APT 301  |

**Endpoint de prueba:**  
`https://run.mocky.io/v3/19217075-6d4e-4818-98bc-416d1feb7b84`

C. Realizar transformación de la respuesta de XML a JSON, con la siguiente correspondencia:

| SOAP (XML) | REST (JSON)   | Datos de Prueba |
|------------|--------------|----------------|
| Codigo     | codigoEnvio  | 80375472       |
| Mensaje    | estado       | Entregado exitosamente al cliente |

D. Cargar y documentar el proyecto en un repositorio de Git para ejecución en contenedores de Docker y compartir la ruta.

---

## Configuración del Proyecto

### Archivos de Configuración

- `appsettings.json`: Configuración general de la aplicación.
- `appsettings.Development.json`: Configuración específica para el entorno de desarrollo.
- `.vscode/settings.json`: Configuración de Visual Studio Code.

### Dependencias

El proyecto utiliza las siguientes dependencias:

- `Microsoft.AspNetCore.OpenApi` (versión 9.0.2)

## Clonación y Ejecución del Proyecto

### Clonación del Repositorio

1. Clona el repositorio en tu equipo local:
   ```sh
   git clone https://github.com/ssromero1092/ApiAcmePedidos.git
   ```
2. Accede al directorio del proyecto:
   ```sh
   cd ApiAcmePedidos
   ```

### Requisitos Previos

- .NET SDK 9.0
- Docker (opcional, para ejecutar en contenedor)

### Ejecución Local

1. Restaura las dependencias:
   ```sh
   dotnet restore
   ```
2. Compila el proyecto:
   ```sh
   dotnet build
   ```
3. Ejecuta el proyecto:
   ```sh
   dotnet run
   ```

### Ejecución en Docker

1. Construye la imagen Docker:
   ```sh
   docker build -t apiacmepedidos .
   ```
2. Ejecuta el contenedor:
   ```sh
   docker run -v ${PWD}:/app -p 5288:80 -p 7231:443 -e ASPNETCORE_ENVIRONMENT=Development apiacmepedidos
   ```

## Endpoints

### Enviar Pedido

- **URL**: `/api/pedidos/enviarPedido`
- **Método**: `POST`
- **Cuerpo de la Solicitud**:
  ```json
  {
      "numPedido": "75630275",
      "cantidadPedido": "1",
      "codigoEAN": "00110000765191002104587",
      "nombreProducto": "Armario INVAL",
      "numDocumento": "1113987400",
      "direccion": "CR 72B 45 12 APT 301"
  }
  ```
- **Respuesta Exitosa**:
  ```json
  {
      "enviarPedidoRespuesta": {
          "codigoEnvio": "80375472",
          "estado": "Entregado exitosamente al cliente"
      }
  }
  ```

## Estructura del Código

### Program.cs
Configura y ejecuta la aplicación web.

### Startup.cs
Configura los servicios y el pipeline de la aplicación.

### PedidosControllers.cs
Controlador principal que maneja las solicitudes de envío de pedidos.

### Dockerfile
Archivo de configuración para construir y ejecutar la aplicación en un contenedor Docker.

## Contribuciones
Las contribuciones son bienvenidas. Por favor, abre un issue o un pull request para discutir cualquier cambio que te gustaría realizar.

## Licencia
Este proyecto está licenciado bajo la Licencia MIT. Consulta el archivo `LICENSE` para más detalles.

