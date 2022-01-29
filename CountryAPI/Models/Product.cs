using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CountryAPI.Models;

namespace CountryAPI.Models
{
    public class Product
    {

       
        public int Id { get; set; }
        [Required(ErrorMessage = "ref_assignto is required.")]
        public Guid ref_assignto { get; set; }
        public Guid ref_product { get; set; }

        [StringLength(20)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [NotMapped]
        public string img_profile { get; set; } = string.Empty;
       /* [ForeignKey("ref_product")]
        public Files? Files { get; set; }*/
    }


    public class ProductListDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "ref_assignto is required.")]
        public Guid ref_assignto { get; set; }

    }

    public class ProductDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "ref_assignto is required.")]
        public Guid ref_assignto { get; set; }
        [Required(ErrorMessage = "ref_product is required.")]
        public Guid ref_product { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;

    }
}
