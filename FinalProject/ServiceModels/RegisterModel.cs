using System.ComponentModel.DataAnnotations;

namespace FinalProject.ServiceModels
{
    public class RegisterModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Surname { get; set; }
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(30)]
        public string Password { get; set; }
        [Required]
        [MaxLength(30)]
        [Compare("Password")]

        public string ConfirmPassword { get; set; }
    }

    }
