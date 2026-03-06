using System.ComponentModel.DataAnnotations;

namespace Eat.ViewModels.Account
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Name is required")]

        public string Name { get; set; }
        [Required(ErrorMessage = "Surname is required")]

        public string Surname { get; set; }
        [Required(ErrorMessage = "UserName is required")]

        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]

        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage ="Password dont match")]
        public string ConfirmPassword { get; set; }


    }
}
