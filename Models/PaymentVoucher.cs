using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etaa.Models
{
    public class PaymentVoucher
    {
        [Key]
        public int PaymentVoucherId { get; set; }
        [Display(Name = "مستند الدفع")]
        public string? PaymentDocumentPath { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "تاريخ الدفع")]
        public DateTime PaymentDate { get; set; }
        [Display(Name = "مبلغ الدفعية")]
        public decimal PaymentAmount { get; set; }
        [Display(Name = "هل تمت الموافقة من قبل الإدارة")]
        public bool IsApprovedByManagement { get; set; }
        [Display(Name = "هل تم الحذف")]
        public bool IsCanceled { get; set; }

        // Relationship between the projects and families
        [Display(Name = "المشروع")]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Projects? Projects { get; set; }
        // Relationship between the projects and families
        [Display(Name = "القسط")]
        public int InstallmentsId { get; set; }
        [ForeignKey("InstallmentsId")]
        public Installments? Installments { get; set; }
        [Display(Name = "إسم المستخدم")]
        public string? UserId { get; set; }
        [Display(Name = "إسم مستخدم الإدارة")]
        public string? ManagementUserId { get; set; }
    }
}
