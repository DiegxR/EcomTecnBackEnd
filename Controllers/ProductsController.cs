
using backEnd.Context;
using backEnd.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace backEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductServices _productService;
    public ProductsController(ProductServices productService)
    {
        _productService = productService;
    }

    [HttpGet("{category}")]
    public async Task<IActionResult> GetProducts(string category)
    {
        try
        {
            var products = await _productService.GetProductsAsync(category);
            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al obtener los productos: {ex.Message}");
        }
    }

    [HttpPost("byIds")]
    public async Task<IActionResult> GetProductsByIds([FromBody] List<string> ids)
    {
        if (ids == null || !ids.Any())
        {
            return BadRequest("Debe proporcionar una lista de IDs.");
        }

        try
        {
            var products = await _productService.GetProductsByIdsAsync(ids);
            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al obtener los productos por IDs: {ex.Message}");
        }
    }
}

