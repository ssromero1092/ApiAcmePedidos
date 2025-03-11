
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml.Linq;
using System.Text.Json;

namespace ApiAcmePedidos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public PedidosController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        // POST: api/pedidos/enviarPedido
        [HttpPost("enviarPedido")]
        public async Task<ActionResult<string>> EnviarPedido([FromBody] PedidoRequest pedidoRequest)
        {
            if (pedidoRequest == null)
            {
                return BadRequest("La solicitud no puede estar vacía.");
            }
            var xmlRequest = TransformJsonToXml(pedidoRequest);
            try
            {
                var response = await _httpClient.PostAsync("https://run.mocky.io/v3/19217075-6d4e-4818-98bc-416d1feb7b84", new StringContent(xmlRequest, Encoding.UTF8, "application/xml"));

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    // Procesar la respuesta exitosa
                    return Ok(TransformXmlToJson(responseContent));
                }
                else
                {
                    // Mockear una respuesta en caso de error
                    var mockedResponse = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
                                <soapenv:Header/>
                                <soapenv:Body>
                                    <env:EnvioPedidoAcmeResponse xmlns:env=""http://WSDLs/EnvioPedidos/EnvioPedidosAcme"">
                                        <EnvioPedidoResponse>
                                            <Codigo>80375472</Codigo>
                                            <Mensaje>Entregado exitosamente al cliente</Mensaje>
                                        </EnvioPedidoResponse>
                                    </env:EnvioPedidoAcmeResponse>
                                </soapenv:Body>
                            </soapenv:Envelope>";

                    Console.WriteLine("Error en el servicio externo. Usando respuesta simulada.");
                    return Ok(TransformXmlToJson(mockedResponse));
                }
            }
            catch (HttpRequestException ex)
            {
                // Mockear una respuesta en caso de excepción de red
                var mockedResponse = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
                                <soapenv:Header/>
                                <soapenv:Body>
                                    <env:EnvioPedidoAcmeResponse xmlns:env=""http://WSDLs/EnvioPedidos/EnvioPedidosAcme"">
                                        <EnvioPedidoResponse>
                                            <Codigo>80375472</Codigo>
                                            <Mensaje>Entregado exitosamente al cliente</Mensaje>
                                        </EnvioPedidoResponse>
                                    </env:EnvioPedidoAcmeResponse>
                                </soapenv:Body>
                            </soapenv:Envelope>";


                Console.WriteLine($"Error de red: {ex.Message}. Usando respuesta simulada.");
                return Ok(TransformXmlToJson(mockedResponse));
            }

        }

        private string TransformJsonToXml(PedidoRequest pedidoRequest)
        {
            var soapenv = XNamespace.Get("http://schemas.xmlsoap.org/soap/envelope/");
            var env = XNamespace.Get("http://WSDLs/EnvioPedidos/EnvioPedidosAcme");

            var xml = new XElement(soapenv + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", soapenv),
                new XAttribute(XNamespace.Xmlns + "env", env),
                new XElement(soapenv + "Header"),
                new XElement(soapenv + "Body",
                    new XElement(env + "EnvioPedidoAcme",
                        new XElement("EnvioPedidoRequest",
                            new XElement("pedido", pedidoRequest.numPedido),
                            new XElement("Cantidad", pedidoRequest.cantidadPedido),
                            new XElement("EAN", pedidoRequest.codigoEAN),
                            new XElement("Producto", pedidoRequest.nombreProducto),
                            new XElement("Cedula", pedidoRequest.numDocumento),
                            new XElement("Direccion", pedidoRequest.direccion)
                        )
                    )
                )
            );

            return xml.ToString();
        }

        private string TransformXmlToJson(string xmlResponse)
        {
            var xmlDoc = XDocument.Parse(xmlResponse);
            var codigo = xmlDoc.Descendants("Codigo").FirstOrDefault()?.Value;
            var mensaje = xmlDoc.Descendants("Mensaje").FirstOrDefault()?.Value;

            var jsonResponse = new
            {
                enviarPedidoRespuesta = new
                {
                    codigoEnvio = codigo,
                    estado = mensaje
                }
            };

            return JsonSerializer.Serialize(jsonResponse); 
        }
    }

    public class PedidoRequest
    {
        public string numPedido { get; set; }
        public string cantidadPedido { get; set; }
        public string codigoEAN { get; set; }
        public string nombreProducto { get; set; }
        public string numDocumento { get; set; }
        public string direccion { get; set; }
    }
}