using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etaa.Models
{
    public class ProjectTypes
    {
        [Key]
        public int ProjectTypeId { get; set; }
        [Required(ErrorMessage = "Name(Ar) is Required!")]
        [Display(Name = "Name(Ar)")]
        public string NameAr { get; set; }
        public string? NameEn { get; set; }
        public bool? IsCanceled { get; set; }

        // Relationship between the project type and project domain
        public int ProjectDomainTypeId { get; set; }
        [ForeignKey("ProjectDomainTypeId")]
        public ProjectDomainTypes? ProjectDomainTypes { get; set; }

        // Relationship between the project type and project group
        public int ProjectGroupId { get; set; }
        [ForeignKey("ProjectGroupId")]
        public ProjectGroup? ProjectGroup { get; set; }
    }
}
