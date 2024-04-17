using System.ComponentModel.DataAnnotations;

namespace UserApplication.Models
{
    public class User
    {
        [Required]
        [Key]
        public int UserId { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? Contact { get; set; }
        [DataType(DataType.MultilineText)]
        public string? Address { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        public List<UserSocialURLs>? SocialMediaProfiles { get; set;}
    }
}
