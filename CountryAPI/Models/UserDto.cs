using System.ComponentModel.DataAnnotations;

namespace CountryAPI.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string User_name { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;


    }
}
