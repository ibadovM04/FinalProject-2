using FinalProject.Model;

namespace FinalProject.Models
{
    public class Cart:Entity<Guid>
    {
        public string Name { get; set; }
       
        public string? Barcode { get; set; }     
        public double SellAmount { get; set; }      
        public int Quantity { get; set; }     
        public double TotalPrice { get; set; }
    }
}
