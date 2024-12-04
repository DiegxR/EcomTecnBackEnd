using System.ComponentModel.DataAnnotations;

namespace backEnd.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public List<string> Products { get; set; } = new();
    }
}