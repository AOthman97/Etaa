using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Etaa.Models
{
    public class ProjectsSocialBenefits
    {
        [Key]
        public int ProjectsSocialBenefitsId { get; set; }

        // Relationship between the project type assets and project type
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Projects? Projects { get; set; }
        // Relationship between the project type assets and project type
        public int ProjectSocialBenefitsId { get; set; }
        [ForeignKey("ProjectSocialBenefitsId")]
        public ProjectSocialBenefits? ProjectSocialBenefits { get; set; }
    }
}
