
using System.ComponentModel.DataAnnotations;

namespace CountryAPI.Models
{
    public class SessionDto
    {
        public int Id { get; set; }
        public string token { get; set; } = string.Empty;
    }
}
