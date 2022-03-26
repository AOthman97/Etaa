using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etaa.Models
{
    public class FamilyMember
    {
        [Key]
        public int FamilyMemberId { get; set; }
        [Required(ErrorMessage = "Name(Ar) is Required!")]
        [Display(Name = "Name(Ar)")]
        public string NameAr { get; set; }
        public string? NameEn { get; set; }
        public int? Age { get; set; }
        public string? Note { get; set; }

        // Relationship between the family member and kinship
        public int KinshipId { get; set; }
        [ForeignKey("KinshipId")]
        public Kinship? Kinship { get; set; }

        // Relationship between the family member and gender
        public int GenderId { get; set; }
        [ForeignKey("GenderId")]
        public Gender? Gender { get; set; }

        // Relationship between the family member and educational status
        public int EducationalStatusId { get; set; }
        [ForeignKey("EducationalStatusId")]
        public EducationalStatus? EducationalStatus { get; set; }

        // Relationship between the family member and job
        public int JobId { get; set; }
        [ForeignKey("JobId")]
        public Job? Job { get; set; }
        // Relationship between the family member and family
        public int? FamilyId { get; set; }
        [ForeignKey("FamilyId")]
        public Family? Family { get; set; }
    }
}
