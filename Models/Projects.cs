using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etaa.Models
{
    public class Projects
    {
        [Key]
        public int ProjectId { get; set; }
        [Required(ErrorMessage = "Name(Ar) is Required!")]
        [Display(Name = "Name(Ar)")]
        public string NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? SignatureofApplicantPath { get; set; }
        public string? ProjectActivity { get; set; }
        public string? ProjectPurpose { get; set; }
        public decimal? Capital { get; set; }
        public decimal? MonthlyInstallmentAmount { get; set; }
        public int? NumberOfInstallments { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? WaiverPeriod { get; set; }
        public bool? IsApprovedByManagement { get; set; }

        // Relationship between the projects and families
        public int FamilyId { get; set; }
        [ForeignKey("FamilyId")]
        public Family? Family { get; set; }
        // Relationship between the projects and families
        public int NumberOfFundsId { get; set; }
        [ForeignKey("NumberOfFundsId")]
        public NumberOfFunds? NumberOfFunds { get; set; }
        // Relationship between the projects and families
        public int ProjectTypeId { get; set; }
        [ForeignKey("ProjectTypeId")]
        public ProjectTypes? ProjectTypes { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public int ManagementUserId { get; set; }
        [ForeignKey("UserId")]
        public Users? Users { get; set; }

        public ICollection<Family> Families { get; set; }
        public ICollection<ProjectsAssets> ProjectsAssets { get; set; }
        public ICollection<ProjectsSelectionReasons> ProjectsSelectionReasons { get; set; }
        public ICollection<ProjectsSocialBenefits> ProjectsSocialBenefits { get; set; }
    }
}
