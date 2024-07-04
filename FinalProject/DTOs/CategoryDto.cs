namespace FinalProject.DTOs
{
	public class CategoryDto
	{
		public int CategoryId { get; set; }
		public string Name { get; set; }
		public int Priority { get; set; }
		
		public string Slogan { get; set; }
		public string ImageUrl { get; set; }
        public int? ParentId { get; set; }
        public List<CategoryDto> Children { get; set; } = new List<CategoryDto>();
    }
}
