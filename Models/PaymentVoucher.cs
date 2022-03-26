using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etaa.Models
{
    public class PaymentVoucher
    {
        [Key]
        public int PaymentVoucherId { get; set; }
        public string PaymentDocumentPath { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public bool? IsApprovedByManagement { get; set; }
        public bool? IsCanceled { get; set; }

        // Relationship between the projects and families
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Projects? Projects { get; set; }
        // Relationship between the projects and families
        public int InstallmentsId { get; set; }
        [ForeignKey("InstallmentsId")]
        public Installments? Installments { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public int ManagementUserId { get; set; }
        [ForeignKey("UserId")]
        public Users? Users { get; set; }
    }
}
