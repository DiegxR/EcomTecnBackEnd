namespace backEnd.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; } 
        public string Description { get; set; } = string.Empty;
        public List<string> Images { get; set; } = null!; 
        public DateTime CreationAt { get; set; } 
        public DateTime UpdatedAt { get; set; } 
        public Category Category { get; set; } = null!; 
    }
}
