using System.ComponentModel.DataAnnotations;

namespace FinalProject.ServiceModels
{
    public class ProductReviewModel
    {
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
        public DateTime Accepted { get; set; }


        [Required]
        [MaxLength(30)]
        public string Message { get; set; }

    }
}
