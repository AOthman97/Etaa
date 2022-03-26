using System.ComponentModel.DataAnnotations;

namespace Etaa.Models
{
    public class ProjectGroup
    {
        [Key]
        public int ProjectGroupId { get; set; }
        [Required(ErrorMessage = "Name(Ar) is Required!")]
        [Display(Name = "Name(Ar)")]
        public string NameAr { get; set; }
        public string? NameEn { get; set; }
    }
}
