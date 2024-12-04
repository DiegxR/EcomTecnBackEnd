namespace backEnd.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public DateTime CreationAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
