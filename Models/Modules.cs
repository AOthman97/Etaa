namespace Etaa.Models
{
    public class Modules
    {
        [Key]
        public int ModuleId { get; set; }
        [Required(ErrorMessage = "Name(Ar) is Required!")]
        [Display(Name = "Name(Ar)")]
        public string NameAr { get; set; }
        public string? NameEn { get; set; }

        // Relationship between the Module and the log
        public ICollection<Log>? Logs;
    }
}
