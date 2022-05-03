using System.ComponentModel.DataAnnotations;

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
        public bool IsCanceled { get; set; }

        // Relationship between the project type assets and project type, the relationship is represented in the main table
        public int ProjectTypeId { get; set; }
    }
}
