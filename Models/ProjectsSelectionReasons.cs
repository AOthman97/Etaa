namespace Etaa.Models
{
    public class ProjectsSelectionReasons
    {
        [Key]
        public int ProjectsSelectionReasonsId { get; set; }

        // Relationship between the project type assets and project type
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Projects? Projects { get; set; }
        // Relationship between the project type assets and project type
        public int ProjectSelectionReasonsId { get; set; }
        [ForeignKey("ProjectSelectionReasonsId")]
        public ProjectSelectionReasons? ProjectSelectionReasons { get; set; }
    }
}