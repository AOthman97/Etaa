using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etaa.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }
        [Required(ErrorMessage = "Name(Ar) is Required!")]
        [Display(Name = "Name(Ar)")]
        public string NameAr { get; set; }
        public string? NameEn { get; set; }

        // Relationship between the movie and the cinema
        public int StateNo { get; set; }
        [ForeignKey("StateId")]
        public State State { get; set; }
    }
}
