using System.ComponentModel.DataAnnotations;

namespace Elagy.DTOs
{
    public class RegisterUserDTO
    {

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get;set; }
        [Required]
        public string Governorate { get; set; }
        [Required]
        public string Center { get; set; }

        [Required]
        public string Location { get; set; }


        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required, DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }


        [Required, DataType(DataType.Password)]
        public string Password { get; set; }


        [Required, DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Passwords don't match.")]
        public string ConfirmPassword { get; set; }
    }
}
