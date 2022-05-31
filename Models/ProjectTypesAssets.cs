using System.ComponentModel.DataAnnotations;

namespace Etaa.Models
{
    public class ProjectTypesAssets
    {
        [Key]
        public int ProjectTypesAssetsId { get; set; }
        [Required(ErrorMessage = "!حقل الإسم عربي مطلوب")]
        [Display(Name = "حقل الإسم عربي")]
        public string NameAr { get; set; }
        [Required(ErrorMessage = "!حقل الإسم إنجليزي مطلوب")]
        [Display(Name = "حقل الإسم إنجليزي")]
        public string NameEn { get; set; }
        public bool IsCanceled { get; set; }

        // Relationship between the project type assets and project type, the relationship is represented in the main table
        public int ProjectTypeId { get; set; }
    }
}
