using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserApplication.Models
{
    public class UserSocialURLs
    {
        [Key]
        public int UserSocialURLsId { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public string SocialMediaPlatformName { get; set; }
        [DataType(DataType.Url)]
        public string SocialMediaPlatformURL { get; set; }
    
        public User? User { get; set; }        

    }
}
