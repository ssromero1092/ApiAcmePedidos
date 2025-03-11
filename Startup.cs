using ApiAcmePedidos.Controllers;
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        // Configurar HttpClient
        services.AddHttpClient<PedidosController>(); // Esto permite inyectar HttpClient en el controlador
    }
}