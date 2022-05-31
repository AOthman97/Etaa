using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etaa.Models
{
    public class PaymentVoucher
    {
        [Key]
        public int PaymentVoucherId { get; set; }
        public string? PaymentDocumentPath { get; set; }
        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public bool IsApprovedByManagement { get; set; }
        public bool IsCanceled { get; set; }

        // Relationship between the projects and families
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Projects? Projects { get; set; }
        // Relationship between the projects and families
        public int InstallmentsId { get; set; }
        [ForeignKey("InstallmentsId")]
        public Installments? Installments { get; set; }
        public string? UserId { get; set; }
        public string? ManagementUserId { get; set; }
    }
}
