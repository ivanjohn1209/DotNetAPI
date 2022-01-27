using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CountryAPI.Models
{
    public class Files
    {


        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } =  string.Empty;
        public string Extention { get; set; } =  string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;

        [NotMapped]
        public string Url
        {
            get
            {
                return "https://localhost:7247/api/Files/uploads?fileName=" + FileName;
            }
        }
        public Guid ref_assignto { get; set; }
        public Guid ref_file { get; set; }

        public DateTime? CreatedOn { get; set; }

    }

    public class FileUpload
    {
        public IFormFile files { get; set; }
        public Guid ref_assignto { get; set; }
    }
    public class FileListDbo
    {
        [Required(ErrorMessage = "ref_assignto is required.")]
        public Guid ref_assignto { get; set; }
    }
    public class FileDbo
    {
        [Required(ErrorMessage = "ref_assignto is required.")]
        public Guid ref_assignto { get; set; }
        [Required(ErrorMessage = "ref_file is required.")]
        public Guid ref_file { get; set; }

    }
}
