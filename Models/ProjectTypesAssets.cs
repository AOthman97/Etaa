using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etaa.Models
{
    public class ProjectTypesAssets
    {
        [Key]
        public int ProjectTypesAssetsId { get; set; }
        [Required(ErrorMessage = "Name(Ar) is Required!")]
        [Display(Name = "Name(Ar)")]
        public string NameAr { get; set; }
        public string? NameEn { get; set; }
        public bool? IsCanceled { get; set; }

        // Relationship between the project type assets and project type
        public int ProjectTypeId { get; set; }
        [ForeignKey("ProjectTypeId")]
        public ProjectTypes? ProjectTypes { get; set; }
    }
}
