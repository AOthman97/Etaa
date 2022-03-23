using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etaa.Models
{
    public class District
    {
        [Key]
        public int DistrictId { get; set; }
        [Required(ErrorMessage = "Name(Ar) is Required!")]
        [Display(Name = "Name(Ar)")]
        public string NameAr { get; set; }
        public string? NameEn { get; set; }

        // Relationship between the movie and the cinema
        public int CityNo { get; set; }
        [ForeignKey("CityId")]
        public City City { get; set; }
    }
}
