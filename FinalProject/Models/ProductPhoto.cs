namespace FinalProject.Model
{
    public class ProductPhoto : Entity<int>
    {
        public Guid ProductId { get; set; }
        public string ImageUrl { get; set; }

        public bool IsMain { get; set; }


        public Product Product { get; set; }
    }
}
