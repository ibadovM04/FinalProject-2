using FinalProject.DTOs;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels
{
    public class ProductDetailVm
    {
        public ProductDto? Product { get; set; }


        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        


        [Required]
        [MaxLength(30)]
        public string Message { get; set; }
    }
}
