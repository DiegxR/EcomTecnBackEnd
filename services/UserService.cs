using backEnd.Models;
using backEnd.Context;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(AppDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Crear usuario
        public async Task<User> CreateUserAsync(User user)
        {
            _logger.LogInformation("Creando un nuevo usuario.");
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos.
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el usuario: {ex.Message}");
                throw;
            }
        }

        // Actualizar productos del usuario
        public async Task<User> UpdateUserProductsAsync(int userId, List<string> newProducts)
        {
            _logger.LogInformation($"Actualizando productos para el usuario con ID {userId}.");
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            // Verificar cada producto en la lista nueva
            foreach (var product in newProducts)
            {
                if (user.Products.Contains(product))
                {
                    _logger.LogInformation($"El producto '{product}' ya existe en la lista. Será eliminado.");
                    user.Products.Remove(product);
                }
                else
                {
                    _logger.LogInformation($"El producto '{product}' no existe en la lista. Será agregado.");
                    user.Products.Add(product);
                }
            }

            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos.
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar productos del usuario: {ex.Message}");
                throw;
            }
        }

        // Obtener usuario por ID
        public async Task<User?> ValidateUserCredentialsAsync(string username, string password)
        {
            _logger.LogInformation($"Validando credenciales para el usuario {username}.");
            try
            {
                var user = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Name == username);

    
                if (user == null)
                {
                    _logger.LogWarning($"El usuario {username} no existe.");
                    throw new Exception("Usuario no encontrado.");
                }


                if (user.Password != password)
                {
                    _logger.LogWarning($"Contraseña incorrecta para el usuario {username}.");
                    throw new Exception("Contraseña incorrecta.");
                }

                _logger.LogInformation($"Credenciales válidas para el usuario {username}.");
                return user; 
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al validar credenciales: {ex.Message}");
                throw;
            }
        }
    }
}