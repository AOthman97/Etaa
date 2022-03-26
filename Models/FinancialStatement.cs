using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etaa.Models
{
    public class FinancialStatement
    {
        [Key]
        public int FinancialStatementId { get; set; }
        public string DocumentPath { get; set; }
        public bool IsApprovedByManagement { get; set; }

        // Relationship between the projects and families
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Projects? Projects { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public int ManagementUserId { get; set; }
        [ForeignKey("UserId")]
        public Users? Users { get; set; }
    }
}
