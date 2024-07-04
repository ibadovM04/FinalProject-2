using FinalProject.Model;

namespace FinalProject.Models
{
    public class BigSaleProduct:Entity<int>
    {
        public string Title { get; set; }
        public string DiscountInfo { get; set; }
        public string? Description { get; set; }
        public string ImageURL { get; set; }
        public string Link { get; set; }
    }
}
