using System.ComponentModel.DataAnnotations;

namespace Etaa.Models
{
    public class State
    {
        [Key]
        public int StateId { get; set; }
        [Required(ErrorMessage = "Name(Ar) is Required!")]
        [Display(Name = "Name(Ar)")]
        public string NameAr { get; set; }
        public string? NameEn { get; set; }
    }
}
