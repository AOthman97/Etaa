﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etaa.Models
{
    public class Projects
    {
        [Key]
        public int ProjectId { get; set; }
        // Make the name an auto-generated field, how?
        // When saving or editing the project data it should concatenate the ProjectTypeName with the FamilyName, One for
        // NameAr, the other for NameEn
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? SignatureofApplicantPath { get; set; }
        [Display(Name = "Project Activity")]
        public string? ProjectActivity { get; set; }
        [Display(Name = "Project Purpose")]
        public string? ProjectPurpose { get; set; }
        public decimal? Capital { get; set; }
        [Display(Name = "Monthly Installment Amount")]
        public decimal? MonthlyInstallmentAmount { get; set; }
        [Display(Name = "Number of Installments")]
        public int? NumberOfInstallments { get; set; }
        public DateTime? Date { get; set; }
        [Display(Name = "Waifer Period")]
        public DateTime? WaiverPeriod { get; set; }
        public bool IsApprovedByManagement { get; set; }
        public bool IsCanceled { get; set; }

        // Relationship between the projects and families
        public int FamilyId { get; set; }
        public int NumberOfFundsId { get; set; }
        [NotMapped]
        public virtual ICollection<NumberOfFunds>? NumberOfFunds { get; set; }
        // Relationship between the projects and families
        public int ProjectTypeId { get; set; }
        [NotMapped]
        public virtual ICollection<ProjectTypes>? ProjectTypes { get; set; }
        public int UserId { get; set; }
        public int ManagementUserId { get; set; }

        [ForeignKey("ProjectId")]
        [NotMapped]
        public virtual ICollection<ProjectsAssets>? ProjectsAssets { get; set; }
        [ForeignKey("ProjectId")]
        [NotMapped]
        public virtual ICollection<ProjectsSelectionReasons>? ProjectsSelectionReasons { get; set; }
        [NotMapped]
        public virtual ICollection<MultiSelectList>? ProjectsSelectionReasonsList { get; set; }
        [ForeignKey("ProjectId")]
        [NotMapped]
        public virtual ICollection<ProjectsSocialBenefits>? ProjectsSocialBenefits { get; set; }
        [NotMapped]
        public virtual ICollection<MultiSelectList>? ProjectsSocialBenefitsList { get; set; }

        [NotMapped]
        public int ProjectSelectionReasonsId { get; set; }
        [NotMapped]
        public virtual ICollection<MultiSelectList>? ProjectSelectionReasons { get; set; }
        [NotMapped]
        public int ProjectSocialBenefitsId { get; set; }
        [NotMapped]
        public virtual ICollection<MultiSelectList>? ProjectSocialBenefits { get; set; }
    }
}