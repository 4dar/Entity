using System.ComponentModel.DataAnnotations;

namespace bankAccounts.Models
{
    public class RegisterViewModel : BaseEntity
    {
        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string Name { get; set; }
 
        [Required]
        [EmailAddress]
        public string Email { get; set; }
 
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
 
        [Required]
        [Compare("Password", ErrorMessage = "Password and confirmation must match.")]
        [DataType(DataType.Password)]
        public string pConfirm { get; set; }
    }

    public class LoginUser : BaseEntity
    {
        [Required]
        [EmailAddress]
        public string login_email {get;set;}

        [Required]
        [DataType(DataType.Password)]
        public string login_password {get;set;}
    }
}
