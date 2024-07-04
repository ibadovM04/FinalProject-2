using FinalProject.Model;

namespace FinalProject.Models
{
    public class FeaturedProduct:Entity<Guid>
    {
        public Guid ProductId { get; set; }
        public string? Slug { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string ImageURL { get; set; }
        public double Price { get; set; }
    }
}
