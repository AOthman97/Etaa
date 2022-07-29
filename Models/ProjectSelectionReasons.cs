namespace Etaa.Models
{
    public class ProjectSelectionReasons
    {
        [Key]
        public int ProjectSelectionReasonsId { get; set; }
        [Required(ErrorMessage = "Name(Ar) is Required!")]
        [Display(Name = "Name(Ar)")]
        public string NameAr { get; set; }
        public string? NameEn { get; set; }
        public bool? IsCanceled { get; set; }
    }
}