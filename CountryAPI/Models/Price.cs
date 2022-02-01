using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CountryAPI.Models;
namespace CountryAPI.Models
{
    public class Price
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "ref_assignto is required.")]
        public Guid ref_assignto { get; set; }
        public Guid ref_price { get; set; }

        public int price { get; set; }
        public string currency { get; set; } = string.Empty;
    }
    public class PriceListDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "ref_assignto is required.")]
        public Guid ref_assignto { get; set; }

    }
    public class PriceDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "ref_assignto is required.")]
        public Guid ref_assignto { get; set; }

        [Required(ErrorMessage = "ref_price is required.")]
        public Guid ref_price { get; set; }

        public int price { get; set; }
        public string currency { get; set; } = string.Empty;
    }

}
