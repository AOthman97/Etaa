using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etaa.Models
{
    public class FinancialStatement
    {
        [Key]
        public int FinancialStatementId { get; set; }
        public string? DocumentPath { get; set; }
        public bool IsApprovedByManagement { get; set; }
        public bool IsCanceled { get; set; }

        // Relationship between the projects and families
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Projects? Projects { get; set; }
        public string? UserId { get; set; }
        public string? ManagementUserId { get; set; }
    }
}
