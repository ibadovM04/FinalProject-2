namespace FinalProject.Areas.Admin.Models
{
    public class ProductFilterModel
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }

        public int Page { get; set; } = 1;
    }
}
