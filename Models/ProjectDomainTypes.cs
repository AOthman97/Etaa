namespace Etaa.Models
{
    public class ProjectDomainTypes
    {
        [Key]
        public int ProjectDomainTypeId { get; set; }
        [Required(ErrorMessage = "الإسم عربي مطلوب !")]
        [Display(Name = "الإسم عربي")]
        public string NameAr { get; set; }
        public string? NameEn { get; set; }
        public bool? IsCanceled { get; set; }
    }
}
