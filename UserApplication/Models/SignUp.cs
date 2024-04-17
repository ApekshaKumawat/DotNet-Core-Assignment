using System.ComponentModel.DataAnnotations;

namespace UserApplication.Models
{
    public class SignUp
    {
        [Required]
        [MinLength(5)]
        [MaxLength(30)]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Your password and confirm password do not match")]
        [MinLength(5)]
        [MaxLength(15)]
        public string ConfirmPassword { get; set; }
    }
}
