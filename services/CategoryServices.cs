using backEnd.Models;
using System.Text.Json;

namespace backEnd.services
{
    public class CategoryServices
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CategoryServices> _logger;

        public CategoryServices(HttpClient httpClient, ILogger<CategoryServices> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            try
            {
                _logger.LogInformation("Consultando productos de la API externa.");
                var response = await _httpClient.GetAsync("https://api.escuelajs.co/api/v1/categories?offset=0&limit=10");

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

                var categories = JsonSerializer.Deserialize<List<Category>>(content, options);

                _logger.LogInformation("Productos obtenidos exitosamente.");
                return categories!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos de la API externa.");
                throw;
            }
        }
    }
}
