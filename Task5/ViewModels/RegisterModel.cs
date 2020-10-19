using System.ComponentModel.DataAnnotations;


namespace Task4.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email is required")]
		[DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't equal")]
        public string ConfirmPassword { get; set; }
    }
}
