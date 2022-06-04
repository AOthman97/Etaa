using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etaa.Models
{
    public class FinancialStatement
    {
        [Key]
        public int FinancialStatementId { get; set; }
        [Display(Name = "مستند الإقرار المالي")]
        public string? DocumentPath { get; set; }
        [Display(Name = "هل تمت الموافقة من قبل الإدارة")]
        public bool IsApprovedByManagement { get; set; }
        [Display(Name = "هل تم الحذف")]
        public bool IsCanceled { get; set; }

        // Relationship between the projects and families
        [Display(Name = "المشروع")]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Projects? Projects { get; set; }
        [Display(Name = "إسم المستخدم")]
        public string? UserId { get; set; }
        [Display(Name = "إسم مستخدم الإدارة")]
        public string? ManagementUserId { get; set; }
    }
}
