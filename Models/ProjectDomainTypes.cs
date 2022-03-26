using System.ComponentModel.DataAnnotations;

namespace Etaa.Models
{
    public class ProjectDomainTypes
    {
        [Key]
        public int ProjectDomainTypeId { get; set; }
        [Required(ErrorMessage = "Name(Ar) is Required!")]
        [Display(Name = "Name(Ar)")]
        public string NameAr { get; set; }
        public string? NameEn { get; set; }
        public bool? IsCanceled { get; set; }
    }
}
