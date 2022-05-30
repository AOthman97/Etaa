using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etaa.Models
{
    public class Clearance
    {
        [Key]
        public int ClearanceId { get; set; }
        public string? ClearanceDocumentPath { get; set; }
        public string? Comments { get; set; }
        [Display(Name = "تاريخ خلو الطرف")]
        [DataType(DataType.Date)]
        public DateTime ClearanceDate { get; set; }
        public bool? IsApprovedByManagement { get; set; }
        public bool? IsCanceled { get; set; }

        // Relationship between the projects and families
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Projects? Projects { get; set; }
        public string? UserId { get; set; }
        public string? ManagementUserId { get; set; }
    }
}
