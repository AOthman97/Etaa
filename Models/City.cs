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

        // Relationship between the city and the state
        public int StateId { get; set; }
        [ForeignKey("StateId")]
        public State? State { get; set; }

        // Relationship between the city and the district
        public ICollection<District>? Districts;
    }
}
