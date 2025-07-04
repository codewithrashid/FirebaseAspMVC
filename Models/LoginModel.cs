using System.ComponentModel.DataAnnotations;

namespace FirebaseAspMVC.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}
