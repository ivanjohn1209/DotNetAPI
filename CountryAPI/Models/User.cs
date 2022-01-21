using System.ComponentModel.DataAnnotations;

namespace CountryAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public Guid ref_profile { get; set; }
        public string User_name { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }

        public object user_obj() {
            return new { id=Id, ref_profile, user_name = User_name, email= Email };
        }
    }
}
