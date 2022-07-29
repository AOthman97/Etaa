namespace Etaa.Models
{
    public class Clearance
    {
        [Key]
        public int ClearanceId { get; set; }
        [Display(Name = "مستند خلو الطرف")]
        public string? ClearanceDocumentPath { get; set; }
        [Display(Name = "التعليقات")]
        public string? Comments { get; set; }
        [Display(Name = "تاريخ خلو الطرف")]
        [DataType(DataType.Date)]
        public DateTime ClearanceDate { get; set; }
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
