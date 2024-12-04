using backEnd.Models;
using backEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace backEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            try
            {
                var createdUser = await _userService.CreateUserAsync(user);
                return CreatedAtAction(nameof(ValidateUserCredentials), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/products")]
        public async Task<IActionResult> UpdateUserProducts(int id, [FromBody] List<string> newProducts)
        {
            try
            {
                var updatedUser = await _userService.UpdateUserProductsAsync(id, newProducts);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        public class UserCredentials
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateUserCredentials([FromBody] UserCredentials credentials)
        {
            if (string.IsNullOrEmpty(credentials.Username) || string.IsNullOrEmpty(credentials.Password))
            {
                return BadRequest(new { message = "El nombre de usuario y la contraseña son requeridos." });
            }

            try
            {
                // Llama al servicio para validar las credenciales
                var user = await _userService.ValidateUserCredentialsAsync(credentials.Username, credentials.Password);

                // Si se devuelve null, indica que las credenciales son inválidas
                if (user == null)
                {
                    return Unauthorized(new { message = "Usuario o contraseña incorrectos." });
                }

                // Si las credenciales son válidas, devuelve el usuario completo
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al validar credenciales: {ex.Message}" });
            }
        }
    }
}