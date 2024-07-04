using FinalProject.Models;

namespace FinalProject.Model
{
    public class ProductOption : Entity<int>
    {
        public Guid ProductId { get; set; }
        public int OptionId { get; set; }
        public string Value { get; set; }

        public Option Option { get; set; }
        public Product Product { get; set; }


    }
}
