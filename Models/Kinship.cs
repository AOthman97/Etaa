using System.ComponentModel.DataAnnotations;

namespace Etaa.Models
{
    public class Kinship
    {
        [Key]
        public int KinshipId { get; set; }
        [Required(ErrorMessage = "Name(Ar) is Required!")]
        [Display(Name = "Name(Ar)")]
        public string NameAr { get; set; }
        public string? NameEn { get; set; }
        public bool? IsCanceled { get; set; }

        // Each kinship contains a number of family members, This property defines the relationship
        // between them
        public ICollection<FamilyMember>? FamilyMembers;
    }
}
