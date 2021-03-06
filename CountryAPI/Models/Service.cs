using System.ComponentModel.DataAnnotations;

namespace CountryAPI.Models
{
    public class Service
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "ref_assignto is required.")]
        public Guid ref_assignto { get; set; }
        public Guid ref_service { get; set; }

        [StringLength(20)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }



    public class ServiceListDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "ref_assignto is required.")]
        public Guid ref_assignto { get; set; }

    }

    public class ServiceDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "ref_assignto is required.")]
        public Guid ref_assignto { get; set; }
        [Required(ErrorMessage = "ref_product is required.")]
        public Guid ref_service { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;

    }
}
