using backEnd.Models;
using System.Net.Http;
using System.Text.Json;
namespace backEnd.services;

    public class ProductServices
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProductServices> _logger;

    public ProductServices(HttpClient httpClient, ILogger<ProductServices> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<Product>> GetProductsAsync(string category)
    {
        try
        {
            _logger.LogInformation("Consultando productos de la API externa.");
            // Definir la URL base
            var baseUrl = "https://api.escuelajs.co/api/v1/products?offset=0&limit=10";

            // Verificar si se debe modificar la URL con el categoryId
            var url = category.ToLower() == "all" ? baseUrl : $"{baseUrl}&categoryId={category}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("La API externa devolvió un código de estado {StatusCode}.", response.StatusCode);
                throw new Exception($"Error al consultar la API externa: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignora mayúsculas/minúsculas
            };

            var products = JsonSerializer.Deserialize<List<Product>>(content, options);

            _logger.LogInformation("Productos obtenidos exitosamente.");
            return products!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener productos de la API externa.");
            throw;
        }
    }
    public async Task<List<Product>> GetProductsByIdsAsync(List<string> ids)
    {
        var products = new List<Product>();

        foreach (var id in ids)
        {
            try
            {
                _logger.LogInformation("Consultando producto con ID {Id} desde la API externa.", id);

                var response = await _httpClient.GetAsync($"https://api.escuelajs.co/api/v1/products/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Error al consultar el producto con ID {Id}. Código de estado: {StatusCode}", id, response.StatusCode);
                    continue; // Saltar al siguiente ID si hay un error
                }

                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var product = JsonSerializer.Deserialize<Product>(content, options);

                if (product != null)
                {
                    products.Add(product); // Agregar solo productos válidos
                }
                else
                {
                    _logger.LogWarning("No se pudo deserializar el producto con ID {Id}.", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar el producto con ID {Id}.", id);
            }
        }

        return products;
    }
}