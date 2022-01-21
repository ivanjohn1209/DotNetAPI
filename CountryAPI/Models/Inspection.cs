using System.ComponentModel.DataAnnotations;
using CountryAPI.Models;

namespace CountryAPI.Models
{
    public class Inspection
    {
        public int Id { get; set; }
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;
        [StringLength(200)]
        public string Comments { get; set; } = string.Empty;
        public int InspectionTypeId { get; set; }
           
        public InspectionType? InspectionType { get; set; }
    }
}
