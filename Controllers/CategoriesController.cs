using backEnd.services;
using Microsoft.AspNetCore.Mvc;

namespace backEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(CategoryServices categoryService) : ControllerBase

{
    public readonly CategoryServices _categotyService = categoryService;
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        try
        {
            var products = await _categotyService.GetCategoriesAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al obtener las categorias: {ex.Message}");
        }
    }
}
